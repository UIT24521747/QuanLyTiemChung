#Requires -Version 5.1
$ErrorActionPreference = "Stop"

$Root      = $PSScriptRoot
$RelDir    = Join-Path $Root "Release"
$AppDir    = Join-Path $RelDir "app"
$MysqlDir  = Join-Path $RelDir "mysql"
$MigDir    = Join-Path $RelDir "Migration"

Write-Host "=== QuanLyTiemChung — Build Portable Release ===" -ForegroundColor Cyan

# ── 1. Clean ────────────────────────────────────────────────────────────────
if (Test-Path $RelDir) {
    Write-Host "Cleaning previous release..."
    Remove-Item $RelDir -Recurse -Force
}
New-Item -ItemType Directory -Path $AppDir, $MigDir | Out-Null

# ── 2. Publish .NET app (self-contained, single EXE) ────────────────────────
Write-Host "Publishing app..."
& dotnet publish "$Root\QuanLyKhachHang.csproj" `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -o $AppDir
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed." }

# ── 3. .env for portable MySQL (port 3307, no password) ─────────────────────
@"
DB_HOST=127.0.0.1
DB_PORT=3307
DB_NAME=QuanLyKhachHang
DB_USER=root
DB_PASS=
"@ | Set-Content (Join-Path $AppDir ".env") -Encoding utf8NoBOM

# ── 4. Migration SQL ─────────────────────────────────────────────────────────
Copy-Item (Join-Path $Root "Migration\QuanLyKhachHang.sql") $MigDir
Copy-Item (Join-Path $Root "Migration\SeedData.sql")        $MigDir

# ── 5. MySQL ZIP — download once, cache in project root ─────────────────────
# ── 5. MySQL — copy pre-extracted folder ────────────────────────────────────
$MysqlSrc = Join-Path $Root "mysql"
if (-not (Test-Path (Join-Path $MysqlSrc "bin\mysqld.exe"))) {
    throw "mysql\bin\mysqld.exe not found. Place the extracted MySQL folder at: $MysqlSrc"
}
Write-Host "Copying MySQL..."

Copy-Item $MysqlSrc $MysqlDir -Recurse

# ── 6. MySQL config ──────────────────────────────────────────────────────────
@"
[mysqld]
port=3307
character-set-server=utf8mb4
collation-server=utf8mb4_unicode_ci
"@ | Set-Content (Join-Path $MysqlDir "my.ini") -Encoding ascii

# ── 7. Start.bat ─────────────────────────────────────────────────────────────
@'
@echo off
setlocal EnableDelayedExpansion
SET ROOT=%~dp0
SET MYSQL_BIN=%ROOT%mysql\bin
SET DATA_DIR=%ROOT%data
SET INIT_FLAG=%ROOT%.initialized

:: Init MySQL data directory on first run
if not exist "%DATA_DIR%\mysql\" (
    echo [Setup] Initializing MySQL data directory...
    "%MYSQL_BIN%\mysqld.exe" --defaults-file="%ROOT%mysql\my.ini" --initialize-insecure --datadir="%DATA_DIR%"
    if errorlevel 1 ( echo ERROR: MySQL init failed. & pause & exit /b 1 )
)

:: Start MySQL in background
echo Starting MySQL on port 3307...
start /B "" "%MYSQL_BIN%\mysqld.exe" --defaults-file="%ROOT%mysql\my.ini" --datadir="%DATA_DIR%"

:: Wait until MySQL accepts connections (up to 30s)
set /a TRIES=0
:WAIT
timeout /t 1 /nobreak > nul
"%MYSQL_BIN%\mysqladmin.exe" -u root --protocol=TCP --port=3307 ping > nul 2>&1
if errorlevel 1 (
    set /a TRIES+=1
    if !TRIES! LSS 30 goto WAIT
    echo ERROR: MySQL did not start within 30 seconds.
    pause & exit /b 1
)

:: Create schema and seed data on first launch
if not exist "%INIT_FLAG%" (
    echo [Setup] Creating database...
    "%MYSQL_BIN%\mysql.exe" -u root --protocol=TCP --port=3307 -e "CREATE DATABASE IF NOT EXISTS QuanLyKhachHang CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;"
    echo [Setup] Running schema migration...
    "%MYSQL_BIN%\mysql.exe" -u root --protocol=TCP --port=3307 QuanLyKhachHang < "%ROOT%Migration\QuanLyKhachHang.sql"
    echo [Setup] Inserting seed data...
    "%MYSQL_BIN%\mysql.exe" -u root --protocol=TCP --port=3307 QuanLyKhachHang < "%ROOT%Migration\SeedData.sql"
    echo. > "%INIT_FLAG%"
    echo [Setup] Database ready.
)

echo Launching QuanLyTiemChung...
start /D "%ROOT%app" "" "%ROOT%app\QuanLyKhachHang.exe"
'@ | Set-Content (Join-Path $RelDir "Start.bat") -Encoding ascii

# ── 8. Stop.bat ──────────────────────────────────────────────────────────────
@'
@echo off
SET ROOT=%~dp0
echo Stopping MySQL...
"%ROOT%mysql\bin\mysqladmin.exe" -u root --protocol=TCP --port=3307 shutdown
if errorlevel 1 ( echo MySQL was not running. ) else ( echo MySQL stopped. )
'@ | Set-Content (Join-Path $RelDir "Stop.bat") -Encoding ascii

# ── Done ─────────────────────────────────────────────────────────────────────
Write-Host ""
Write-Host "Release built: $RelDir" -ForegroundColor Green
Write-Host ""
Write-Host "Contents:"
Write-Host "  app\           self-contained EXE + .env"
Write-Host "  mysql\         portable MySQL 8.0 (port 3307)"
Write-Host "  Migration\     schema + seed SQL"
Write-Host "  Start.bat      start MySQL, init DB on first run, launch app"
Write-Host "  Stop.bat       stop MySQL"
Write-Host ""
Write-Host "Run: Release\Start.bat" -ForegroundColor Yellow
