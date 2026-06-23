using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.ViewModels
{
    public class LoVacXinRowVM : INotifyPropertyChanged
    {
        private int _rowNum;
        private VacXinDTO? _selectedVacXin;
        private string? _hangSanXuat;
        private DateTime? _ngayHetHan;
        private int _soLuongNhap;
        private decimal _donGia;

        public int RowNum { get => _rowNum; set { _rowNum = value; Notify(); } }

        public VacXinDTO? SelectedVacXin
        {
            get => _selectedVacXin;
            set
            {
                _selectedVacXin = value;
                Notify();
                Notify(nameof(MaVacXin));
                Notify(nameof(TenVacXin));
            }
        }

        public string? MaVacXin => _selectedVacXin?.MaVacXin;
        public string? TenVacXin => _selectedVacXin?.TenVacXin;

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

        public bool IsEmpty => _selectedVacXin == null;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
