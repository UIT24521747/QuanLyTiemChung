using System.ComponentModel;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.ViewModels
{
    public class ChiTietTiemRowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify(string n) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private int _rowNum;
        public int RowNum { get => _rowNum; set { _rowNum = value; Notify(nameof(RowNum)); } }

        private LoVacXinForTiemDTO? _selectedLo;
        public LoVacXinForTiemDTO? SelectedLoVacXin
        {
            get => _selectedLo;
            set
            {
                _selectedLo = value;
                Notify(nameof(SelectedLoVacXin));
                Notify(nameof(MaLo));
                Notify(nameof(TenVacXin));
                Notify(nameof(TenLoaiVacXin));
                Notify(nameof(HangSanXuat));
                Notify(nameof(DonGia));
                Notify(nameof(ThanhTien));
            }
        }

        public string? MaLo         => _selectedLo?.MaLo;
        public string? TenVacXin    => _selectedLo?.TenVacXin;
        public string? TenLoaiVacXin=> _selectedLo?.TenLoaiVacXin;
        public string? HangSanXuat  => _selectedLo?.HangSanXuat;
        public decimal DonGia       => _selectedLo?.DonGia ?? 0;

        private int _soLuong;
        public int SoLuong
        {
            get => _soLuong;
            set { _soLuong = value; Notify(nameof(SoLuong)); Notify(nameof(ThanhTien)); }
        }

        public decimal ThanhTien => SoLuong * DonGia;

        public bool IsEmpty => _selectedLo == null;
    }
}
