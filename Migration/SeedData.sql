-- Seed data for testing — run AFTER QuanLyKhachHang.sql
USE QuanLyKhachHang;
SET NAMES 'utf8mb4';
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
    ('LA', 'Loại A'),
    ('LB', 'Loại B'),
    ('LC', 'Loại C'),
    ('LD', 'Loại D'),
    ('LE', 'Loại E'),
    ('LF', 'Loại F'),
    ('LG', 'Loại G'),
    ('LH', 'Loại H'),
    ('LI', 'Loại I'),
    ('LJ', 'Loại J');

-- -------------------------------------------------------
-- Sprint 1: Customers + Guardians
-- IDs use yyMMddHHmmssfff format (milliseconds suffix 000)
-- -------------------------------------------------------
DELETE FROM CHITIETTIEM;
DELETE FROM PHIEUTIEM;
DELETE FROM KHACHHANG;
DELETE FROM NGUOIGIAMHO;

INSERT INTO NGUOIGIAMHO (MaGH, TenGH, SDT_GH, Email_GH, GioiTinh_GH, NgaySinh_GH, CCCD_GH, DiaChi_GH, QuanHe) VALUES
    ('GH250101000100001', 'Trần Văn Bình',   '0901234567', 'tvbinh@email.com',  'Nam', '1980-06-15', '079080001234', 'Quận 1, TP.HCM',      'Cha'),
    ('GH250101000200002', 'Nguyễn Thị Cúc',  '0912345678', 'ntcuc@email.com',   'Nữ',  '1978-03-22', '079078005678', 'Quận 3, TP.HCM',      'Mẹ');

INSERT INTO KHACHHANG (MaKH, TenKH, SDT, Email, GioiTinh, NgaySinh, CCCD, DiaChi, MaGH) VALUES
    ('KH250101000100001', 'Nguyễn Văn An',   '0909111222', 'nvan@email.com',    'Nam', '1990-05-15', '079090112233', 'Quận 1, TP.HCM',      NULL),
    ('KH250101000200002', 'Trần Thị Bích',   '0918222333', 'ttbich@email.com',  'Nữ',  '2012-08-20', '079012000111', 'Quận Bình Thạnh, TP.HCM', 'GH250101000100001'),
    ('KH250101000300003', 'Lê Văn Cường',    '0927333444', 'lvcuong@email.com', 'Nam', '1985-11-30', '079085334455', 'Quận 7, TP.HCM',      NULL),
    ('KH250101000400004', 'Phạm Thị Dung',   '0936444555', 'ptdung@email.com',  'Nữ',  '2010-03-10', '079010000222', 'Quận Gò Vấp, TP.HCM', 'GH250101000200002'),
    ('KH250101000500005', 'Hoàng Minh Đức',  '0945555666', 'hmduc@email.com',   'Nam', '1995-07-04', '079095556677', 'Quận 12, TP.HCM',     NULL);

-- -------------------------------------------------------
-- Sprint 2: Vaccine catalog
-- -------------------------------------------------------
DELETE FROM VACXIN;
INSERT INTO VACXIN (MaVacXin, TenVacXin, MaLoaiVacXin, SoMuiTiem, KhoangCachGiuaCacMui) VALUES
    ('VX250101000100001', 'COVID-19 (Pfizer)',        'LA', 2,  21),
    ('VX250101000200002', 'Cúm mùa',                 'LB', 1, 365),
    ('VX250101000300003', 'Viêm gan B',              'LC', 3,  30),
    ('VX250101000400004', 'Sởi - Quai bị - Rubella', 'LA', 2,  28),
    ('VX250101000500005', 'Uốn ván',                 'LB', 3,  60),
    ('VX250101000600006', 'Viêm não Nhật Bản',       'LC', 3,  14);

-- -------------------------------------------------------
-- Sprint 3: Import slips + lots
-- -------------------------------------------------------
DELETE FROM LOVACXIN;
DELETE FROM PHIEUNHAP;

INSERT INTO PHIEUNHAP (MaPhieuNhap, NgayNhap, NhaCungCap) VALUES
    ('PN250110000100001', '2025-01-10', 'Công ty Dược phẩm Việt Đức'),
    ('PN250301000100001', '2025-03-01', 'Công ty TNHH Y tế Hoàn Mỹ');

INSERT INTO LOVACXIN (MaLo, MaVacXin, MaPhieuNhap, HangSanXuat, NgayHetHan, SoLuongNhap, SoLuongTon, DonGia) VALUES
    -- Phieu 1 lots
    ('LO250110000100001', 'VX250101000100001', 'PN250110000100001', 'Pfizer (Mỹ)',           '2029-06-30', 100, 100, 350000.00),
    ('LO250110000200002', 'VX250101000200002', 'PN250110000100001', 'Sanofi (Pháp)',         '2029-01-15',  50,  50, 250000.00),
    ('LO250110000300003', 'VX250101000300003', 'PN250110000100001', 'Merck Sharp (Đức)',     '2029-03-31',  80,  80, 200000.00),
    ('LO250110000400004', 'VX250101000400004', 'PN250110000100001', 'GlaxoSmithKline (Anh)', '2029-09-30',  60,  60, 420000.00),
    -- Phieu 2 lots
    ('LO250301000100001', 'VX250101000500005', 'PN250301000100001', 'Bio Farma (Indonesia)', '2029-06-30', 120, 120, 180000.00),
    ('LO250301000200002', 'VX250101000600006', 'PN250301000100001', 'Vabiotech (Việt Nam)',  '2029-12-31',  70,  70, 290000.00),
    ('LO250301000300003', 'VX250101000100001', 'PN250301000100001', 'Moderna (Mỹ)',          '2029-08-31',  90,  90, 380000.00);

-- -------------------------------------------------------
-- Sprint 5: Vaccination slips — all 12 months of 2026 (BM6 testing)
-- Lots: LO1=COVID Pfizer 350k, LO2=Cúm mùa 250k, LO3=Viêm gan B 200k,
--       LO4=MMR 420k, LO5=Uốn ván 180k, LO6=Viêm não NB 290k, LO7=COVID Moderna 380k
-- -------------------------------------------------------
INSERT INTO PHIEUTIEM (MaPhieuTiem, MaKH, NgayTiem, BacSiThucHien, TongTien, GhiChu) VALUES
    ('PT260105000100001', 'KH250101000100001', '2026-01-05', 'BS. Nguyễn Văn A',  350000.00, NULL),
    ('PT260210000100001', 'KH250101000300003', '2026-02-10', 'BS. Nguyễn Văn A',  250000.00, NULL),
    ('PT260315000100001', 'KH250101000500005', '2026-03-15', 'BS. Trần Thị B',    620000.00, NULL),
    ('PT260405000100001', 'KH250101000100001', '2026-04-05', 'BS. Nguyễn Văn A',  180000.00, NULL),
    ('PT260510000100001', 'KH250101000300003', '2026-05-10', 'BS. Trần Thị B',    290000.00, NULL),
    ('PT260601000100001', 'KH250101000100001', '2026-06-01', 'BS. Nguyễn Văn A',  350000.00, NULL),
    ('PT260601000200002', 'KH250101000300003', '2026-06-01', 'BS. Nguyễn Văn A',  250000.00, NULL),
    ('PT260615000100001', 'KH250101000500005', '2026-06-15', 'BS. Trần Thị B',    970000.00, NULL),
    ('PT260710000100001', 'KH250101000500005', '2026-07-10', 'BS. Nguyễn Văn A',  380000.00, NULL),
    ('PT260805000100001', 'KH250101000100001', '2026-08-05', 'BS. Trần Thị B',    380000.00, NULL),
    ('PT260910000100001', 'KH250101000300003', '2026-09-10', 'BS. Nguyễn Văn A',  420000.00, NULL),
    ('PT261005000100001', 'KH250101000500005', '2026-10-05', 'BS. Trần Thị B',    290000.00, NULL),
    ('PT261110000100001', 'KH250101000100001', '2026-11-10', 'BS. Nguyễn Văn A',  730000.00, NULL),
    ('PT261215000100001', 'KH250101000300003', '2026-12-15', 'BS. Trần Thị B',    430000.00, NULL);

INSERT INTO CHITIETTIEM (MaPhieuTiem, MaLo, SoLuong, DonGia, ThanhTien) VALUES
    -- Jan: COVID Pfizer
    ('PT260105000100001', 'LO250110000100001', 1, 350000.00, 350000.00),
    -- Feb: Cúm mùa
    ('PT260210000100001', 'LO250110000200002', 1, 250000.00, 250000.00),
    -- Mar: Viêm gan B + MMR
    ('PT260315000100001', 'LO250110000300003', 1, 200000.00, 200000.00),
    ('PT260315000100001', 'LO250110000400004', 1, 420000.00, 420000.00),
    -- Apr: Uốn ván
    ('PT260405000100001', 'LO250301000100001', 1, 180000.00, 180000.00),
    -- May: Viêm não NB
    ('PT260510000100001', 'LO250301000200002', 1, 290000.00, 290000.00),
    -- Jun 01 KH1: COVID Pfizer
    ('PT260601000100001', 'LO250110000100001', 1, 350000.00, 350000.00),
    -- Jun 01 KH3: Cúm mùa
    ('PT260601000200002', 'LO250110000200002', 1, 250000.00, 250000.00),
    -- Jun 15 KH5: COVID Pfizer + Viêm gan B + MMR
    ('PT260615000100001', 'LO250110000100001', 1, 350000.00, 350000.00),
    ('PT260615000100001', 'LO250110000300003', 1, 200000.00, 200000.00),
    ('PT260615000100001', 'LO250110000400004', 1, 420000.00, 420000.00),
    -- Jul: COVID Moderna
    ('PT260710000100001', 'LO250301000300003', 1, 380000.00, 380000.00),
    -- Aug: Viêm gan B + Uốn ván
    ('PT260805000100001', 'LO250110000300003', 1, 200000.00, 200000.00),
    ('PT260805000100001', 'LO250301000100001', 1, 180000.00, 180000.00),
    -- Sep: MMR
    ('PT260910000100001', 'LO250110000400004', 1, 420000.00, 420000.00),
    -- Oct: Viêm não NB
    ('PT261005000100001', 'LO250301000200002', 1, 290000.00, 290000.00),
    -- Nov: COVID Pfizer + COVID Moderna
    ('PT261110000100001', 'LO250110000100001', 1, 350000.00, 350000.00),
    ('PT261110000100001', 'LO250301000300003', 1, 380000.00, 380000.00),
    -- Dec: Cúm mùa + Uốn ván
    ('PT261215000100001', 'LO250110000200002', 1, 250000.00, 250000.00),
    ('PT261215000100001', 'LO250301000100001', 1, 180000.00, 180000.00);

-- Update SoLuongTon: starting quantities minus seeded vaccinations
-- LO1 (COVID Pfizer):   100 - 4 (Jan+Jun01+Jun15+Nov)  = 96
-- LO2 (Cúm mùa):        50 - 3 (Feb+Jun01+Dec)         = 47
-- LO3 (Viêm gan B):     80 - 3 (Mar+Jun15+Aug)         = 77
-- LO4 (MMR):             60 - 3 (Mar+Jun15+Sep)         = 57
-- LO5 (Uốn ván):       120 - 3 (Apr+Aug+Dec)           = 117
-- LO6 (Viêm não NB):    70 - 2 (May+Oct)               = 68
-- LO7 (COVID Moderna):  90 - 2 (Jul+Nov)               = 88
UPDATE LOVACXIN SET SoLuongTon = 96  WHERE MaLo = 'LO250110000100001';
UPDATE LOVACXIN SET SoLuongTon = 47  WHERE MaLo = 'LO250110000200002';
UPDATE LOVACXIN SET SoLuongTon = 77  WHERE MaLo = 'LO250110000300003';
UPDATE LOVACXIN SET SoLuongTon = 57  WHERE MaLo = 'LO250110000400004';
UPDATE LOVACXIN SET SoLuongTon = 117 WHERE MaLo = 'LO250301000100001';
UPDATE LOVACXIN SET SoLuongTon = 68  WHERE MaLo = 'LO250301000200002';
UPDATE LOVACXIN SET SoLuongTon = 88  WHERE MaLo = 'LO250301000300003';

SET FOREIGN_KEY_CHECKS = 1;

-- -------------------------------------------------------
-- Verify
-- -------------------------------------------------------
SELECT 'THAMSO'      AS Bang, COUNT(*) AS SoBan FROM THAMSO
UNION ALL SELECT 'LOAIVACXIN',   COUNT(*) FROM LOAIVACXIN
UNION ALL SELECT 'NGUOIGIAMHO',  COUNT(*) FROM NGUOIGIAMHO
UNION ALL SELECT 'KHACHHANG',    COUNT(*) FROM KHACHHANG
UNION ALL SELECT 'VACXIN',       COUNT(*) FROM VACXIN
UNION ALL SELECT 'PHIEUNHAP',    COUNT(*) FROM PHIEUNHAP
UNION ALL SELECT 'LOVACXIN',     COUNT(*) FROM LOVACXIN
UNION ALL SELECT 'PHIEUTIEM',    COUNT(*) FROM PHIEUTIEM
UNION ALL SELECT 'CHITIETTIEM',  COUNT(*) FROM CHITIETTIEM;
