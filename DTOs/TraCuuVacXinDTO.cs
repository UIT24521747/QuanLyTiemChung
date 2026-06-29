using System;

namespace QuanLyKhachHang.DTOs
{
    public class TraCuuVacXinDTO
    {
        public string? MaVacXin { get; set; }
        public string? TenVacXin { get; set; }
        public string? MaLoaiVacXin { get; set; }
        public string? TenLoaiVacXin { get; set; }
        public int KhoangCachGiuaCacMui { get; set; }
        public string? MaLo { get; set; }
        public string? HangSanXuat { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public int SoLuongNhap { get; set; }
        public int SoLuongTon { get; set; }
        public decimal DonGia { get; set; }
        public string? MaPhieuNhap { get; set; }
        public DateTime? NgayNhap { get; set; }
        public string? NhaCungCap { get; set; }
    }

    public class TraCuuVacXinParams
    {
        public string? MaVacXin { get; set; }
        public string? TenVacXin { get; set; }
        public string? MaLoaiVacXin { get; set; }
        public int? KhoangCachTu { get; set; }
        public int? KhoangCachDen { get; set; }
        public int? SoLuongTonTu { get; set; }
        public int? SoLuongTonDen { get; set; }
        public string? MaLo { get; set; }
        public string? HangSanXuat { get; set; }
        public int? SoLuongNhapTu { get; set; }
        public int? SoLuongNhapDen { get; set; }
        public decimal? DonGiaTu { get; set; }
        public decimal? DonGiaDen { get; set; }
        public DateTime? NgayHetHanTu { get; set; }
        public DateTime? NgayHetHanDen { get; set; }
        public string? NhaCungCap { get; set; }
        public string? MaPhieuNhapTu { get; set; }
        public string? MaPhieuNhapDen { get; set; }
        public DateTime? NgayNhapTu { get; set; }
        public DateTime? NgayNhapDen { get; set; }
    }
}
