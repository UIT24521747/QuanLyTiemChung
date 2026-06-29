using System;

namespace QuanLyKhachHang.DTOs
{
    public class PhieuTiemDTO
    {
        public string? MaPhieuTiem { get; set; }
        public string? MaKH { get; set; }
        public DateTime NgayTiem { get; set; }
        public string? BacSiThucHien { get; set; }
        public decimal TongTien { get; set; }
        public string? GhiChu { get; set; }
    }

    public class ChiTietTiemDTO
    {
        public string? MaPhieuTiem { get; set; }
        public string? MaLo { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }

    public class LoVacXinForTiemDTO
    {
        public string? MaLo { get; set; }
        public string? MaVacXin { get; set; }
        public string? TenVacXin { get; set; }
        public string? TenLoaiVacXin { get; set; }
        public string? HangSanXuat { get; set; }
        public DateTime NgayHetHan { get; set; }
        public int SoLuongTon { get; set; }
        public decimal DonGia { get; set; }
        public int KhoangCachGiuaCacMui { get; set; }
    }
}
