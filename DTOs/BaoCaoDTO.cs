namespace QuanLyKhachHang.DTOs
{
    public class BaoCaoVacXinDTO
    {
        public string? TenVacXin { get; set; }
        public string? TenLoaiVacXin { get; set; }
        public int TongLuotTiem { get; set; }
        public decimal DoanhThu { get; set; }
        public double TiLe { get; set; }       // % of total revenue
    }

    public class BaoCaoDoanhSoThangDTO
    {
        public int Thang { get; set; }
        public decimal DoanhThu { get; set; }
        public string SoVoiThangTruoc { get; set; } = "—";
    }
}
