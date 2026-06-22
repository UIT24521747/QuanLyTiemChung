-- Seed data for testing — run AFTER QuanLyKhachHang.sql
USE QuanLyKhachHang;
SET FOREIGN_KEY_CHECKS = 0;

-- -------------------------------------------------------
-- THAMSO: guardian age = 18, import expiry limit = 30 days
-- -------------------------------------------------------
DELETE FROM THAMSO;
INSERT INTO THAMSO (SoTuoiCanGiamHo, SoNgayHanNhap) VALUES (18, 30);

-- -------------------------------------------------------
-- LOAIVACXIN
-- -------------------------------------------------------
DELETE FROM LOAIVACXIN;
INSERT INTO LOAIVACXIN VALUES
    ('A', 'Loại A'),
    ('B', 'Loại B'),
    ('C', 'Loại C');

-- -------------------------------------------------------
-- Sprint 1: Customers + Guardians
-- Customer 1: adult (born 1990) — no guardian needed
-- Customer 2: minor (born 2012) — guardian required (QĐ1)
-- Customer 3: adult (born 1985) — no guardian needed
-- -------------------------------------------------------
DELETE FROM KHACHHANG;
DELETE FROM NGUOIGIAMHO;

INSERT INTO NGUOIGIAMHO (MaGH, TenGH, SDT_GH, Email_GH, GioiTinh_GH, NgaySinh_GH, CCCD_GH, DiaChi_GH, QuanHe) VALUES
    ('GH2501010001', 'Trần Văn Bình',   '0901234567', 'tvbinh@email.com',  'Nam', '1980-06-15', '079080001234', 'Quận 1, TP.HCM',      'Cha'),
    ('GH2501010002', 'Nguyễn Thị Cúc',  '0912345678', 'ntcuc@email.com',   'Nữ',  '1978-03-22', '079078005678', 'Quận 3, TP.HCM',      'Mẹ');

INSERT INTO KHACHHANG (MaKH, TenKH, SDT, Email, GioiTinh, NgaySinh, CCCD, DiaChi, MaGH) VALUES
    ('KH2501010001', 'Nguyễn Văn An',   '0909111222', 'nvan@email.com',    'Nam', '1990-05-15', '079090112233', 'Quận 1, TP.HCM',      NULL),
    ('KH2501010002', 'Trần Thị Bích',   '0918222333', 'ttbich@email.com',  'Nữ',  '2012-08-20', '079012000111', 'Quận Bình Thạnh, TP.HCM', 'GH2501010001'),
    ('KH2501010003', 'Lê Văn Cường',    '0927333444', 'lvcuong@email.com', 'Nam', '1985-11-30', '079085334455', 'Quận 7, TP.HCM',      NULL),
    ('KH2501010004', 'Phạm Thị Dung',   '0936444555', 'ptdung@email.com',  'Nữ',  '2010-03-10', '079010000222', 'Quận Gò Vấp, TP.HCM', 'GH2501010002'),
    ('KH2501010005', 'Hoàng Minh Đức',  '0945555666', 'hmduc@email.com',   'Nam', '1995-07-04', '079095556677', 'Quận 12, TP.HCM',     NULL);

-- -------------------------------------------------------
-- Sprint 2: Vaccine catalog
-- -------------------------------------------------------
DELETE FROM VACXIN;
INSERT INTO VACXIN (MaVacXin, TenVacXin, MaLoaiVacXin, SoMuiTiem, KhoangCachGiuaCacMui) VALUES
    ('VX2501010001', 'COVID-19 (Pfizer)',      'A', 2,  21),
    ('VX2501010002', 'Cúm mùa',                'B', 1, 365),
    ('VX2501010003', 'Viêm gan B',             'C', 3,  30),
    ('VX2501010004', 'Sởi - Quai bị - Rubella','A', 2,  28),
    ('VX2501010005', 'Uốn ván',                'B', 3,  60),
    ('VX2501010006', 'Viêm não Nhật Bản',      'C', 3,  14);

-- -------------------------------------------------------
-- Sprint 3: Import slips + lots
-- Phieu 1: imported 2025-01-10 (all lots expire well after 30-day limit)
-- Phieu 2: imported 2025-03-01
-- -------------------------------------------------------
DELETE FROM LOVACXIN;
DELETE FROM PHIEUNHAP;

INSERT INTO PHIEUNHAP (MaPhieuNhap, NgayNhap, NhaCungCap) VALUES
    ('PN2501100001', '2025-01-10', 'Công ty Dược phẩm Việt Đức'),
    ('PN2503010001', '2025-03-01', 'Công ty TNHH Y tế Hoàn Mỹ');

INSERT INTO LOVACXIN (MaLo, MaVacXin, MaPhieuNhap, HangSanXuat, NgayHetHan, SoLuongNhap, SoLuongTon, DonGia) VALUES
    -- Phieu 1 lots
    ('LO2501100001', 'VX2501010001', 'PN2501100001', 'Pfizer (Mỹ)',          '2026-06-30', 100, 100, 350000.00),
    ('LO2501100002', 'VX2501010002', 'PN2501100001', 'Sanofi (Pháp)',        '2026-01-15',  50,  50, 250000.00),
    ('LO2501100003', 'VX2501010003', 'PN2501100001', 'Merck Sharp (Đức)',    '2027-03-31',  80,  80, 200000.00),
    ('LO2501100004', 'VX2501010004', 'PN2501100001', 'GlaxoSmithKline (Anh)','2026-09-30',  60,  60, 420000.00),
    -- Phieu 2 lots
    ('LO2503010001', 'VX2501010005', 'PN2503010001', 'Bio Farma (Indonesia)','2027-06-30', 120, 120, 180000.00),
    ('LO2503010002', 'VX2501010006', 'PN2503010001', 'Vabiotech (Việt Nam)', '2026-12-31',  70,  70, 290000.00),
    ('LO2503010003', 'VX2501010001', 'PN2503010001', 'Moderna (Mỹ)',         '2026-08-31',  90,  90, 380000.00);

SET FOREIGN_KEY_CHECKS = 1;

-- -------------------------------------------------------
-- Verify
-- -------------------------------------------------------
SELECT 'THAMSO'      AS Bang, COUNT(*) AS SoBan FROM THAMSO
UNION ALL
SELECT 'LOAIVACXIN',  COUNT(*) FROM LOAIVACXIN
UNION ALL
SELECT 'NGUOIGIAMHO', COUNT(*) FROM NGUOIGIAMHO
UNION ALL
SELECT 'KHACHHANG',   COUNT(*) FROM KHACHHANG
UNION ALL
SELECT 'VACXIN',      COUNT(*) FROM VACXIN
UNION ALL
SELECT 'PHIEUNHAP',   COUNT(*) FROM PHIEUNHAP
UNION ALL
SELECT 'LOVACXIN',    COUNT(*) FROM LOVACXIN;
