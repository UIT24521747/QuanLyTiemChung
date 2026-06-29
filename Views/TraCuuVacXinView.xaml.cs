using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QuanLyKhachHang.Controllers;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Views
{
    public partial class TraCuuVacXinView : UserControl
    {
        private readonly TraCuuVacXinController _controller = new();
        private Dictionary<string, string> _maLoHangMap = new();

        // sentinel item for "Tất cả" selection
        private static readonly LoaiVacXinDTO TatCaLoai = new() { MaLoaiVacXin = null, TenLoaiVacXin = "Tất cả" };
        private const string TatCaMaLo = "Tất cả";

        public TraCuuVacXinView()
        {
            InitializeComponent();
            Loaded += (_, _) => LoadCombos();
        }

        // Process 1: load LoaiVacXin; Process 2: load MaLo
        private void LoadCombos()
        {
            try
            {
                var loais = new List<LoaiVacXinDTO> { TatCaLoai };
                loais.AddRange(_controller.GetAllLoaiVacXin());
                cboLoaiVacXin.ItemsSource = loais;
                cboLoaiVacXin.SelectedIndex = 0;

                _maLoHangMap = _controller.GetMaLoHangSanXuatMap();
                var maLoList = new List<string> { TatCaMaLo };
                maLoList.AddRange(_maLoHangMap.Keys);
                cboMaLo.ItemsSource = maLoList;
                cboMaLo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Process 3: auto-fill HangSanXuat when MaLo selected
        private void CboMaLo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboMaLo.SelectedItem is string maLo && maLo != TatCaMaLo)
            {
                if (_maLoHangMap.TryGetValue(maLo, out var hang))
                    txtHangSanXuat.Text = hang;
            }
            else
            {
                txtHangSanXuat.Text = "";
            }
        }

        // Process 4: Tra cứu
        private void BtnTraCuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loai = cboLoaiVacXin.SelectedItem as LoaiVacXinDTO;
                var maLo = cboMaLo.SelectedItem as string;

                var p = new TraCuuVacXinParams
                {
                    MaVacXin      = txtMaVacXin.Text.Trim().NullIfEmpty(),
                    TenVacXin     = txtTenVacXin.Text.Trim().NullIfEmpty(),
                    MaLoaiVacXin  = (loai?.MaLoaiVacXin),
                    MaLo          = (maLo == TatCaMaLo ? null : maLo),
                    HangSanXuat   = txtHangSanXuat.Text.Trim().NullIfEmpty(),
                    NhaCungCap    = txtNhaCungCap.Text.Trim().NullIfEmpty(),
                    MaPhieuNhapTu = txtMPNTu.Text.Trim().NullIfEmpty(),
                    MaPhieuNhapDen= txtMPNDen.Text.Trim().NullIfEmpty(),

                    KhoangCachTu  = ParseInt(txtKCTu.Text),
                    KhoangCachDen = ParseInt(txtKCDen.Text),
                    SoLuongTonTu  = ParseInt(txtSLTonTu.Text),
                    SoLuongTonDen = ParseInt(txtSLTonDen.Text),
                    SoLuongNhapTu = ParseInt(txtSLNhapTu.Text),
                    SoLuongNhapDen= ParseInt(txtSLNhapDen.Text),
                    DonGiaTu      = ParseDecimal(txtDonGiaTu.Text),
                    DonGiaDen     = ParseDecimal(txtDonGiaDen.Text),

                    NgayHetHanTu  = dpNHHTu.SelectedDate,
                    NgayHetHanDen = dpNHHDen.SelectedDate,
                    NgayNhapTu    = dpNgayNhapTu.SelectedDate,
                    NgayNhapDen   = dpNgayNhapDen.SelectedDate,
                };

                var results = _controller.TraCuu(p);

                // Wrap with STT
                dgResults.ItemsSource = results
                    .Select((r, i) => new TraCuuResultRow(i + 1, r))
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tra cứu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Process 5: Thoát
        private void BtnThoat_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private static int? ParseInt(string s) =>
            int.TryParse(s.Trim(), out var v) ? v : null;

        private static decimal? ParseDecimal(string s) =>
            decimal.TryParse(s.Trim(), out var v) ? v : null;
    }

    // View-layer row wrapper that adds STT
    internal class TraCuuResultRow
    {
        public int Stt { get; }
        public string? TenVacXin { get; }
        public string? TenLoaiVacXin { get; }
        public int KhoangCachGiuaCacMui { get; }
        public int SoLuongTon { get; }

        public TraCuuResultRow(int stt, TraCuuVacXinDTO d)
        {
            Stt = stt;
            TenVacXin = d.TenVacXin;
            TenLoaiVacXin = d.TenLoaiVacXin;
            KhoangCachGiuaCacMui = d.KhoangCachGiuaCacMui;
            SoLuongTon = d.SoLuongTon;
        }
    }

    internal static class StringExt
    {
        public static string? NullIfEmpty(this string s) =>
            string.IsNullOrEmpty(s) ? null : s;
    }
}
