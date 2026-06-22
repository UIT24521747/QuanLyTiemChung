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
    $out = & cmd /c "`"$mysqlExe`" -u $DB_USER -p$DB_PASS -P $TEST_PORT < `"$file`" 2>&1"
    $errors = $out | Where-Object { $_ -match "^ERROR" }
    if ($errors) { Write-Warning ($errors -join "`n") }
    $out | Where-Object { $_ -notmatch "Warning" -and $_.Trim() } |
        ForEach-Object { Write-Host "  $_" }
    return ($errors.Count -eq 0)
}

# ── 1. Check port not already in use ─────────────────────────────────────
try {
    $chk = New-Object System.Net.Sockets.TcpClient
    $chk.Connect("127.0.0.1", $TEST_PORT)
    $chk.Close()
    Write-Error "Port $TEST_PORT is already in use. Stop that process first."
    exit 1
} catch { <# port is free #> }

# ── 2. Start OUR mysqld on TEST_PORT ─────────────────────────────────────
Write-Host "Starting test MySQL on port $TEST_PORT..." -ForegroundColor Cyan
$proc = Start-Process -FilePath $mysqldExe `
    -ArgumentList "--datadir=`"$DATA_DIR`" --port=$TEST_PORT" `
    -PassThru -WindowStyle Hidden

# ── 3. Wait for TEST_PORT ─────────────────────────────────────────────────
Write-Host "Waiting for port $TEST_PORT..." -ForegroundColor Cyan
if (-not (Wait-Port $TEST_PORT 30)) {
    Write-Error "MySQL did not start within 30 s."
    Stop-Process -Id $proc.Id -Force -ErrorAction SilentlyContinue
    exit 1
}
Write-Host "MySQL ready (PID $($proc.Id))." -ForegroundColor Green

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
            Write-Warning "Test mysqld (PID $($proc.Id)) exited unexpectedly — restarting..."
            $proc = Start-Process -FilePath $mysqldExe `
                -ArgumentList "--datadir=`"$DATA_DIR`" --port=$TEST_PORT" `
                -PassThru -WindowStyle Hidden
            Start-Sleep -Seconds 3
        }
    }
} finally {
    Write-Host "`nStopping test MySQL (PID $($proc.Id))..." -ForegroundColor Yellow
    Stop-Process -Id $proc.Id -Force -ErrorAction SilentlyContinue
    Write-Host "Done. Existing MySQL on 3306 untouched." -ForegroundColor Yellow
}
