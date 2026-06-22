# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

WPF desktop app (.NET 8) for managing a vaccination clinic (Quản Lý Trung Tâm Tiêm Ngừa). UI language is Vietnamese.

## Commands

```bash
# Build
dotnet build

# Run
dotnet run

# Publish
dotnet publish -c Release
```

No test project exists.

## Environment Setup

App reads DB credentials from `.env` (loaded via `DotNetEnv`). Create `.env` in project root:

```
DB_HOST=localhost
DB_NAME=QuanLyKhachHang
DB_USER=root
DB_PASS=yourpassword
```

Run `Migration/QuanLyKhachHang.sql` against MySQL before first run.

## Architecture

MVC pattern, no MVVM/data-binding framework:

- **`Config/DbConnection.cs`** — `DatabaseConfig.GetConnection()` opens a `MySqlConnection` from `.env` vars. Every model method opens and closes its own connection.
- **`DTOs/`** — Plain data containers passed between layers.
- **`Models/`** — All SQL: parameterized queries, transactions for multi-table writes.
- **`Controllers/`** — Business logic and validation. Reads configurable rules from `THAMSO` table.
- **`Views/`** — WPF code-behind calls controllers directly. Single-window pattern currently.

## Full Requirements (7 features)

| # | Feature | Form | Rule | Sprint |
|---|---------|------|------|--------|
| 1 | Đăng ký thông tin khách hàng | BM1 | QĐ1 | 1 ✓ |
| 2 | Lập danh mục vắc-xin | BM2 | QĐ2 | 2 |
| 3 | Lập phiếu nhập vắc-xin | BM3 | QĐ3 | 3 |
| 4 | Tra cứu vắc-xin | BM4 | — | — |
| 5 | Đăng ký tiêm phòng | BM5 | QĐ5 | — |
| 6 | Lập báo cáo tháng | BM6 | — | — |
| 7 | Thay đổi quy định | — | QĐ7 | — |

## Business Rules

**QĐ1** — Customer under `THAMSO.SoTuoiCanGiamHo` (default 18) must have a guardian record. ✓ implemented.

**QĐ2** — Vaccine type (`LoaiVacXin`) must exist in `LOAIVACXIN` table. Default types: A, B, C. Configurable via QĐ7.

**QĐ3** — Cannot import a vaccine lot whose `NgayHetHan` is within `THAMSO.SoNgayHanNhap` days of `NgayNhap` (default 30). Check: `NgayHetHan - NgayNhap > SoNgayHanNhap`.

**QĐ5** — When registering vaccination:
- The vaccine lot must have stock remaining (`SoLuongTon > 0`)
- The lot must not be expired (`NgayHetHan >= NgayTiem`)
- Must satisfy the dose interval: `NgayTiem - LastDoseDate >= KhoangCachGiuaCacMui` (looked up from `VACXIN` via the customer's vaccination history)

**QĐ7** — User can change: QĐ1 (guardian age threshold), QĐ2 (add/remove vaccine types), QĐ3 (expiry day limit). All stored in `THAMSO` / `LOAIVACXIN`.

## Complete Database Schema

```sql
-- Configurable parameters
THAMSO(
  SoTuoiCanGiamHo INT DEFAULT 18,   -- QĐ1
  SoNgayHanNhap   INT DEFAULT 30    -- QĐ3
)

-- QĐ2: configurable vaccine types
LOAIVACXIN(
  MaLoaiVacXin  VARCHAR(10) PK,     -- 'A', 'B', 'C'
  TenLoaiVacXin VARCHAR(50)
)

-- Vaccine catalog (BM2)
VACXIN(
  MaVacXin             VARCHAR(20) PK,
  TenVacXin            VARCHAR(100),
  MaLoaiVacXin         VARCHAR(10) FK→LOAIVACXIN,
  KhoangCachGiuaCacMui INT          -- days between doses
)

-- Import slip header (BM3)
PHIEUNHAP(
  MaPhieuNhap VARCHAR(20) PK,
  NgayNhap    DATE,
  NhaCungCap  VARCHAR(200)
)

-- Vaccine lot / batch (BM3, BM4)
LOVACXIN(
  MaLo         VARCHAR(20) PK,
  MaVacXin     VARCHAR(20) FK→VACXIN,
  MaPhieuNhap  VARCHAR(20) FK→PHIEUNHAP,
  HangSanXuat  VARCHAR(100),
  NgayHetHan   DATE,
  SoLuongNhap  INT,
  SoLuongTon   INT,                  -- decremented on each vaccination
  DonGia       DECIMAL(18,2)
)

-- Vaccination slip header (BM5)
PHIEUTIEM(
  MaPhieuTiem    VARCHAR(20) PK,
  MaKH           VARCHAR(20) FK→KHACHHANG,
  NgayTiem       DATE,
  BacSiThucHien  VARCHAR(100),
  TongTien       DECIMAL(18,2),
  GhiChu         TEXT
)

-- Vaccination slip detail (BM5)
CHITIETTIEM(
  MaPhieuTiem  VARCHAR(20) FK→PHIEUTIEM,
  MaLo         VARCHAR(20) FK→LOVACXIN,
  SoLuong      INT,
  DonGia       DECIMAL(18,2),
  ThanhTien    DECIMAL(18,2),
  PRIMARY KEY (MaPhieuTiem, MaLo)
)

-- Already exists (Sprint 1)
NGUOIGIAMHO(MaGH PK, TenGH, SDT_GH, Email_GH, GioiTinh_GH, NgaySinh_GH, CCCD_GH, DiaChi_GH, QuanHe)
KHACHHANG(MaKH PK, TenKH, SDT, Email, GioiTinh, NgaySinh, CCCD, DiaChi, MaGH FK→NGUOIGIAMHO ON DELETE SET NULL)
```

## Reports (BM6)

**BM6.1** — Monthly stats by vaccine: `TenVacXin`, `LoaiVacXin`, total doses, revenue. Filter by month.

**BM6.2** — Monthly stats by month (for a year): month, total doses, % change vs previous month.

Both derived from `CHITIETTIEM` joined with `PHIEUTIEM` (for `NgayTiem`) and `LOVACXIN`→`VACXIN`.

## ID Generation Convention

- Customer: `"KH" + yyMMddHHmmss`
- Guardian: `"GH" + yyMMddHHmmss`
- Vaccine: `"VX" + yyMMddHHmmss` (follow same pattern)
- Import slip: `"PN" + yyMMddHHmmss`
- Lot: `"LO" + yyMMddHHmmss`
- Vaccination slip: `"PT" + yyMMddHHmmss`
