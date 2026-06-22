-- Full schema for Quan Ly Trung Tam Tiem Ngua
-- Run this on a fresh MySQL instance.
-- For existing Sprint-1 databases, see upgrade comments below.

CREATE DATABASE IF NOT EXISTS QuanLyKhachHang CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE QuanLyKhachHang;

-- -------------------------------------------------------
-- THAMSO: configurable rules
-- Upgrade: ALTER TABLE THAMSO ADD COLUMN SoNgayHanNhap INT DEFAULT 30;
--          UPDATE THAMSO SET SoNgayHanNhap = 30 WHERE SoNgayHanNhap IS NULL;
-- -------------------------------------------------------
CREATE TABLE IF NOT EXISTS THAMSO (
    SoTuoiCanGiamHo INT DEFAULT 18,   -- QD1: guardian age threshold
    SoNgayHanNhap   INT DEFAULT 30    -- QD3: min days before expiry on import
);
INSERT INTO THAMSO (SoTuoiCanGiamHo, SoNgayHanNhap) VALUES (18, 30);

-- -------------------------------------------------------
-- Sprint 1: Customer registration
-- -------------------------------------------------------
CREATE TABLE IF NOT EXISTS NGUOIGIAMHO (
    MaGH        VARCHAR(20)  PRIMARY KEY,
    TenGH       VARCHAR(100),
    SDT_GH      VARCHAR(15),
    Email_GH    VARCHAR(100),
    GioiTinh_GH VARCHAR(10),
    NgaySinh_GH DATE,
    CCCD_GH     VARCHAR(20),
    DiaChi_GH   VARCHAR(200),
    QuanHe      VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS KHACHHANG (
    MaKH     VARCHAR(20)  PRIMARY KEY,
    TenKH    VARCHAR(100),
    SDT      VARCHAR(15),
    Email    VARCHAR(100),
    GioiTinh VARCHAR(10),
    NgaySinh DATE,
    CCCD     VARCHAR(20),
    DiaChi   VARCHAR(200),
    MaGH     VARCHAR(20),
    FOREIGN KEY (MaGH) REFERENCES NGUOIGIAMHO(MaGH) ON DELETE SET NULL
);

-- -------------------------------------------------------
-- Sprint 2: Vaccine catalog
-- -------------------------------------------------------
CREATE TABLE IF NOT EXISTS LOAIVACXIN (
    MaLoaiVacXin  VARCHAR(10)  PRIMARY KEY,
    TenLoaiVacXin VARCHAR(50)
);
INSERT IGNORE INTO LOAIVACXIN VALUES ('A', 'Loại A'), ('B', 'Loại B'), ('C', 'Loại C');

CREATE TABLE IF NOT EXISTS VACXIN (
    MaVacXin             VARCHAR(20)  PRIMARY KEY,
    TenVacXin            VARCHAR(100),
    MaLoaiVacXin         VARCHAR(10),
    SoMuiTiem            INT DEFAULT 1,
    KhoangCachGiuaCacMui INT DEFAULT 0,
    FOREIGN KEY (MaLoaiVacXin) REFERENCES LOAIVACXIN(MaLoaiVacXin)
);

-- -------------------------------------------------------
-- Sprint 3: Vaccine import slip
-- -------------------------------------------------------
CREATE TABLE IF NOT EXISTS PHIEUNHAP (
    MaPhieuNhap VARCHAR(20)  PRIMARY KEY,
    NgayNhap    DATE,
    NhaCungCap  VARCHAR(200)
);

CREATE TABLE IF NOT EXISTS LOVACXIN (
    MaLo        VARCHAR(20)   PRIMARY KEY,
    MaVacXin    VARCHAR(20),
    MaPhieuNhap VARCHAR(20),
    HangSanXuat VARCHAR(100),
    NgayHetHan  DATE,
    SoLuongNhap INT,
    SoLuongTon  INT,
    DonGia      DECIMAL(18,2),
    FOREIGN KEY (MaVacXin)    REFERENCES VACXIN(MaVacXin),
    FOREIGN KEY (MaPhieuNhap) REFERENCES PHIEUNHAP(MaPhieuNhap)
);
