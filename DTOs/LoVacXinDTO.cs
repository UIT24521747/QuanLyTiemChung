using System;

namespace QuanLyKhachHang.DTOs
{
    public class LoVacXinDTO
    {
        public string? MaLo { get; set; }
        public string? MaVacXin { get; set; }
        public string? TenVacXin { get; set; }
        public string? MaPhieuNhap { get; set; }
        public string? HangSanXuat { get; set; }
        public DateTime NgayHetHan { get; set; }
        public int SoLuongNhap { get; set; }
        public int SoLuongTon { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuongNhap * DonGia;
    }
}
