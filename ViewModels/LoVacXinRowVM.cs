using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyKhachHang.ViewModels
{
    public class LoVacXinRowVM : INotifyPropertyChanged
    {
        private int _rowNum;
        private string? _maVacXin;
        private string? _hangSanXuat;
        private DateTime? _ngayHetHan;
        private int _soLuongNhap;
        private decimal _donGia;

        public int RowNum { get => _rowNum; set { _rowNum = value; Notify(); } }

        public string? MaVacXin
        {
            get => _maVacXin;
            set { _maVacXin = value; Notify(); }
        }

        public string? HangSanXuat
        {
            get => _hangSanXuat;
            set { _hangSanXuat = value; Notify(); }
        }

        public DateTime? NgayHetHan
        {
            get => _ngayHetHan;
            set { _ngayHetHan = value; Notify(); }
        }

        public int SoLuongNhap
        {
            get => _soLuongNhap;
            set { _soLuongNhap = value; Notify(); Notify(nameof(ThanhTien)); }
        }

        public decimal DonGia
        {
            get => _donGia;
            set { _donGia = value; Notify(); Notify(nameof(ThanhTien)); }
        }

        public decimal ThanhTien => SoLuongNhap * DonGia;

        public bool IsEmpty => string.IsNullOrWhiteSpace(MaVacXin);

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
