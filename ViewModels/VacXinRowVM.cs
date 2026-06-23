using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.ViewModels
{
    public class VacXinRowVM : INotifyPropertyChanged
    {
        private int _rowNum;
        private LoaiVacXinDTO? _selectedLoai;
        private int _khoangCach = 0;

        public int RowNum { get => _rowNum; set { _rowNum = value; Notify(); } }

        public LoaiVacXinDTO? SelectedLoaiVacXin
        {
            get => _selectedLoai;
            set
            {
                _selectedLoai = value;
                Notify();
                Notify(nameof(MaLoaiVacXin));
                Notify(nameof(TenLoaiVacXin));
            }
        }

        public string? MaLoaiVacXin => _selectedLoai?.MaLoaiVacXin;
        public string? TenLoaiVacXin => _selectedLoai?.TenLoaiVacXin;

        private string? _tenVacXin;
        public string? TenVacXin { get => _tenVacXin; set { _tenVacXin = value; Notify(); } }

        public int KhoangCach { get => _khoangCach; set { _khoangCach = value; Notify(); } }

        public bool IsEmpty => string.IsNullOrWhiteSpace(TenVacXin);

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
