# start-test-db.ps1
# Starts a dedicated test MySQL 8.x instance on port 3307.
# Never touches any existing MySQL running on 3306.
# On Ctrl+C / exit: kills ONLY the process this script spawned.

$TEST_PORT = 3307
$DATA_DIR  = "C:\MySQLData"
$PROJECT_ROOT = Split-Path $PSScriptRoot -Parent
$MIGRATION    = Join-Path $PROJECT_ROOT "Migration"
$ENV_FILE     = Join-Path $PROJECT_ROOT ".env"
$DB_USER      = "root"
$DB_PASS      = "root"

# ── Locate MySQL 8.x bin ─────────────────────────────────────────────────
function Find-MySQLBin {
    $inPath = Get-Command mysqld -ErrorAction SilentlyContinue
    if ($inPath) { return Split-Path $inPath.Source }

    $candidates = Get-ChildItem "C:\Program Files\MySQL" -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -match "MySQL Server 8\." } |
        Sort-Object Name -Descending

    foreach ($dir in $candidates) {
        $bin = Join-Path $dir.FullName "bin"
        if (Test-Path (Join-Path $bin "mysqld.exe")) { return $bin }
    }

    foreach ($path in @("C:\mysql\bin","C:\mysql8\bin","$env:ProgramW6432\MySQL\bin")) {
        if (Test-Path (Join-Path $path "mysqld.exe")) { return $path }
    }
    return $null
}

$MYSQL_BIN = Find-MySQLBin
if (-not $MYSQL_BIN) {
    Write-Error "MySQL 8.x not found. Install MySQL 8 or add its bin to PATH."
    exit 1
}

$mysqldExe = Join-Path $MYSQL_BIN "mysqld.exe"
$mysqlExe  = Join-Path $MYSQL_BIN "mysql.exe"

$ver = & "$mysqlExe" --version 2>&1 | Select-Object -First 1
Write-Host "Found : $ver" -ForegroundColor DarkGray
Write-Host "Bin   : $MYSQL_BIN" -ForegroundColor DarkGray
Write-Host "Port  : $TEST_PORT  (existing MySQL on 3306 is untouched)" -ForegroundColor DarkGray

# ── Cleanup: kill by port so we never accidentally touch 3306 ────────────
$global:_testPort = $TEST_PORT   # accessible from engine-event scope

function Drop-TestDB([int]$port) {
    try {
        & cmd /c "chcp 65001 >nul && `"$mysqlExe`" -u $DB_USER -p$DB_PASS -P $port -e `"DROP DATABASE IF EXISTS QuanLyKhachHang;`"" 2>&1 | Out-Null
        Write-Host "Database dropped." -ForegroundColor Yellow
    } catch {}
}

function Stop-TestMySQL {
    param([string]$source = "finally")
    Write-Host "`n[$source] Stopping test MySQL on port $TEST_PORT..." -ForegroundColor Yellow

    # Drop DB while MySQL is still alive
    Drop-TestDB $TEST_PORT

    # Primary: kill by owning PID of the test port
    try {
        $conn = Get-NetTCPConnection -LocalPort $TEST_PORT -State Listen -ErrorAction SilentlyContinue |
                Select-Object -First 1
        if ($conn) { Stop-Process -Id $conn.OwningProcess -Force -ErrorAction SilentlyContinue }
    } catch {}

    # Secondary: kill the tracked proc object directly
    if ($null -ne $script:proc -and -not $script:proc.HasExited) {
        Stop-Process -Id $script:proc.Id -Force -ErrorAction SilentlyContinue
    }

    Write-Host "Done. Port 3306 untouched." -ForegroundColor Yellow
}

# Backup for window-close / engine shutdown (fires before PowerShell process exits)
Register-EngineEvent PowerShell.Exiting -Action {
    $port = $global:_testPort
    try {
        & cmd /c "chcp 65001 >nul && `"$mysqlExe`" -u $DB_USER -p$DB_PASS -P $port -e `"DROP DATABASE IF EXISTS QuanLyKhachHang;`"" 2>&1 | Out-Null
    } catch {}
    $conn = Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue |
            Select-Object -First 1
    if ($conn) { Stop-Process -Id $conn.OwningProcess -Force -ErrorAction SilentlyContinue }
} | Out-Null

# ── Helpers ──────────────────────────────────────────────────────────────
function Wait-Port([int]$port, [int]$timeoutSec = 30) {
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    while ($sw.Elapsed.TotalSeconds -lt $timeoutSec) {
        try {
            $tcp = New-Object System.Net.Sockets.TcpClient
            $tcp.Connect("127.0.0.1", $port)
            $tcp.Close()
            return $true
        } catch { Start-Sleep -Milliseconds 400 }
    }
    return $false
}

function Run-SQL([string]$file) {
    # Write a clean UTF-8-without-BOM temp file, then use chcp 65001 (UTF-8 codepage)
    # so cmd.exe < redirection reads bytes correctly — avoids PowerShell pipeline encoding issues
    $tmp = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), [System.IO.Path]::GetRandomFileName() + ".sql")
    try {
        $content = [System.IO.File]::ReadAllText($file, [System.Text.Encoding]::UTF8)
        [System.IO.File]::WriteAllText($tmp, $content, (New-Object System.Text.UTF8Encoding $false))

        $out = & cmd /c "chcp 65001 >nul && `"$mysqlExe`" -u $DB_USER -p$DB_PASS -P $TEST_PORT --default-character-set=utf8mb4 < `"$tmp`"" 2>&1

        $errors = $out | Where-Object { ([string]$_) -match "^ERROR" }
        if ($errors) { Write-Warning ($errors -join "`n") }
        $out | Where-Object { ([string]$_) -notmatch "Warning" -and ([string]$_).Trim() } |
            ForEach-Object { Write-Host "  $_" }
        return ($errors.Count -eq 0)
    } finally {
        Remove-Item $tmp -Force -ErrorAction SilentlyContinue
    }
}

# ── 1. Check port not already in use ──────────────────────────────────────
try {
    $chk = New-Object System.Net.Sockets.TcpClient
    $chk.Connect("127.0.0.1", $TEST_PORT)
    $chk.Close()
    Write-Error "Port $TEST_PORT is already in use. Stop that process first."
    exit 1
} catch { <# port is free #> }

$LOG_FILE = Join-Path $env:TEMP "test-mysqld-$TEST_PORT.log"

# ── 2. Initialize data dir if needed ──────────────────────────────────────
$mysqlPrivate = Join-Path $DATA_DIR "mysql"
$justInitialized = $false
if (-not (Test-Path $mysqlPrivate)) {
    Write-Host "Data dir not initialized — running --initialize-insecure..." -ForegroundColor Cyan
    if (-not (Test-Path $DATA_DIR)) { New-Item -ItemType Directory -Force $DATA_DIR | Out-Null }
    $initOut = & "$mysqldExe" "--datadir=$DATA_DIR" "--initialize-insecure" "--user=root" 2>&1
    $initOut | ForEach-Object { Write-Host "  $_" -ForegroundColor DarkGray }
    Write-Host "Initialization done." -ForegroundColor Green
    $justInitialized = $true
}

# ── 3. Start OUR mysqld on TEST_PORT ─────────────────────────────────────
Write-Host "Starting test MySQL on port $TEST_PORT..." -ForegroundColor Cyan
Remove-Item $LOG_FILE -Force -ErrorAction SilentlyContinue
$script:proc = Start-Process -FilePath $mysqldExe `
    -ArgumentList "--datadir=`"$DATA_DIR`" --port=$TEST_PORT --log-error=`"$LOG_FILE`"" `
    -PassThru -WindowStyle Hidden
$proc = $script:proc

# ── 4. Wait for TEST_PORT ─────────────────────────────────────────────────
Write-Host "Waiting for port $TEST_PORT..." -ForegroundColor Cyan
if (-not (Wait-Port $TEST_PORT 30)) {
    Write-Error "MySQL did not start within 30 s."
    if (Test-Path $LOG_FILE) {
        Write-Host "`nmysqld error log:" -ForegroundColor Red
        Get-Content $LOG_FILE | Select-Object -Last 20 | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
    }
    Stop-Process -Id $proc.Id -Force -ErrorAction SilentlyContinue
    exit 1
}
Write-Host "MySQL ready (PID $($proc.Id))." -ForegroundColor Green

# ── 4b. Set root password after fresh init (initialize-insecure = no password) ──
if ($justInitialized) {
    Write-Host "Setting root password..." -ForegroundColor Cyan
    $setPwd = "ALTER USER 'root'@'localhost' IDENTIFIED BY '$DB_PASS'; FLUSH PRIVILEGES;"
    & cmd /c "chcp 65001 >nul && `"$mysqlExe`" -u root --connect-expired-password -P $TEST_PORT -e `"$setPwd`"" 2>&1 | Out-Null
    Write-Host "Root password set." -ForegroundColor Green
}

# ── 4. Schema ─────────────────────────────────────────────────────────────
Write-Host "Applying schema..." -ForegroundColor Cyan
$ok = Run-SQL (Join-Path $MIGRATION "QuanLyKhachHang.sql")
Write-Host $(if ($ok) {"Schema OK."} else {"Schema had errors."}) `
    -ForegroundColor $(if ($ok) {"Green"} else {"Red"})

# ── 5. Seed ───────────────────────────────────────────────────────────────
Write-Host "Seeding data..." -ForegroundColor Cyan
$ok = Run-SQL (Join-Path $MIGRATION "SeedData.sql")
Write-Host $(if ($ok) {"Seed OK."} else {"Seed had errors."}) `
    -ForegroundColor $(if ($ok) {"Green"} else {"Red"})

# ── 6. Write .env (port 3307) ─────────────────────────────────────────────
@"
DB_HOST=localhost
DB_PORT=$TEST_PORT
DB_NAME=QuanLyKhachHang
DB_USER=$DB_USER
DB_PASS=$DB_PASS
"@ | Set-Content -Path $ENV_FILE -Encoding UTF8 -NoNewline
Write-Host ".env written (port $TEST_PORT)." -ForegroundColor Green

# ── 7. Keep alive; kill ONLY our process on exit ──────────────────────────
Write-Host ""
Write-Host "Test database ready on port $TEST_PORT. Press Ctrl+C to stop." -ForegroundColor Green

try {
    while ($true) {
        Start-Sleep -Seconds 2
        if ($proc.HasExited) {
            Write-Warning "Test mysqld exited unexpectedly — restarting..."
            $script:proc = Start-Process -FilePath $mysqldExe `
                -ArgumentList "--datadir=`"$DATA_DIR`" --port=$TEST_PORT --log-error=`"$LOG_FILE`"" `
                -PassThru -WindowStyle Hidden
            $proc = $script:proc
            Start-Sleep -Seconds 3
        }
    }
} finally {
    Stop-TestMySQL "finally"
}
