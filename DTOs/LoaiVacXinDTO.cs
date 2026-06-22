namespace QuanLyKhachHang.DTOs
{
    public class LoaiVacXinDTO
    {
        public string? MaLoaiVacXin { get; set; }
        public string? TenLoaiVacXin { get; set; }
        public override string ToString() => TenLoaiVacXin ?? MaLoaiVacXin ?? "";
    }
}
