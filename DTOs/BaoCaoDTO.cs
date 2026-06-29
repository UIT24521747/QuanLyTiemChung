namespace QuanLyKhachHang.DTOs
{
    public class BaoCaoVacXinDTO
    {
        public string? TenVacXin { get; set; }
        public string? TenLoaiVacXin { get; set; }
        public int TongLuotTiem { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class BaoCaoThangDTO
    {
        public int Thang { get; set; }
        public int TongLuotTiem { get; set; }
        public string SoVoiThangTruoc { get; set; } = "—";
    }
}
