namespace QuanLyKhachHang.DTOs
{
    public class VacXinDTO
    {
        public string? MaVacXin { get; set; }
        public string? TenVacXin { get; set; }
        public string? MaLoaiVacXin { get; set; }
        public string? TenLoaiVacXin { get; set; }
        public int SoMuiTiem { get; set; }
        public int KhoangCachGiuaCacMui { get; set; }
        public override string ToString() => TenVacXin ?? MaVacXin ?? "";
    }
}
