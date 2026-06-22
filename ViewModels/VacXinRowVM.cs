using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyKhachHang.ViewModels
{
    public class VacXinRowVM : INotifyPropertyChanged
    {
        private int _rowNum;
        private string? _tenVacXin;
        private string? _maLoaiVacXin;
        private int _soMuiTiem = 0;
        private int _khoangCach = 0;

        public int RowNum { get => _rowNum; set { _rowNum = value; Notify(); } }
        public string? TenVacXin { get => _tenVacXin; set { _tenVacXin = value; Notify(); } }
        public string? MaLoaiVacXin { get => _maLoaiVacXin; set { _maLoaiVacXin = value; Notify(); } }
        public int SoMuiTiem { get => _soMuiTiem; set { _soMuiTiem = value; Notify(); } }
        public int KhoangCach { get => _khoangCach; set { _khoangCach = value; Notify(); } }

        public bool IsEmpty => string.IsNullOrWhiteSpace(TenVacXin);

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
