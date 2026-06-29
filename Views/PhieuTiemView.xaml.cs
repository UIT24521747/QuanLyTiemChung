using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QuanLyKhachHang.Controllers;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.ViewModels;

namespace QuanLyKhachHang.Views
{
    public partial class PhieuTiemView : UserControl
    {
        private readonly PhieuTiemController _controller = new();
        private readonly ObservableCollection<ChiTietTiemRowVM> _rows = new();

        public List<LoVacXinForTiemDTO> LoVacXinList { get; private set; } = new();

        public PhieuTiemView()
        {
            InitializeComponent();
            dgChiTiet.ItemsSource = _rows;

            _rows.CollectionChanged += (_, e) =>
            {
                if (e.NewItems != null)
                    foreach (ChiTietTiemRowVM r in e.NewItems)
                        r.PropertyChanged += (_, _) => UpdateTongTien();
                RefreshRowNumbers();
                UpdateTongTien();
            };

            LoadCombos();
            ResetPhieu();
        }

        private void LoadCombos()
        {
            try
            {
                cboKhachHang.ItemsSource = _controller.GetAllKhachHang();
                LoVacXinList = _controller.GetAllLoVacXin();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetPhieu()
        {
            txtMaPT.Text = _controller.GenerateMaPhieuTiem();
            dpNgayTiem.SelectedDate = DateTime.Today;
            cboKhachHang.SelectedIndex = -1;
            txtBacSi.Text = "";
            txtGhiChu.Text = "";
            _rows.Clear();
            for (int i = 0; i < 4; i++) _rows.Add(new ChiTietTiemRowVM());
            txtTongTien.Text = "";
            txtError.Text = "";
        }

        private void RefreshRowNumbers()
        {
            for (int i = 0; i < _rows.Count; i++)
                _rows[i].RowNum = i + 1;
        }

        private void UpdateTongTien()
        {
            decimal tong = _rows.Sum(r => r.ThanhTien);
            txtTongTien.Text = tong > 0 ? tong.ToString("N0") : "";
        }

        private void DgChiTiet_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Dispatcher.InvokeAsync(UpdateTongTien);
        }

        private void BtnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is ChiTietTiemRowVM row)
            {
                _rows.Remove(row);
                RefreshRowNumbers();
                UpdateTongTien();
            }
        }

        private void BtnLapPhieu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtError.Text = "";
                dgChiTiet.CommitEdit(DataGridEditingUnit.Row, true);

                var kh = cboKhachHang.SelectedItem as KhachHangDTO;
                if (kh == null) throw new Exception("Phải chọn khách hàng!");
                if (dpNgayTiem.SelectedDate == null) throw new Exception("Phải chọn ngày tiêm!");

                var activeRows = _rows.Where(r => !r.IsEmpty).ToList();
                if (activeRows.Count == 0) throw new Exception("Phải nhập ít nhất một lô vắc-xin!");

                var pt = new PhieuTiemDTO
                {
                    MaPhieuTiem   = txtMaPT.Text.Trim(),
                    MaKH          = kh.MaKH,
                    NgayTiem      = dpNgayTiem.SelectedDate.Value,
                    BacSiThucHien = txtBacSi.Text.Trim(),
                    GhiChu        = txtGhiChu.Text.Trim().Length > 0 ? txtGhiChu.Text.Trim() : null,
                    TongTien      = activeRows.Sum(r => r.ThanhTien),
                };

                var chiTiets = activeRows.Select(r => new ChiTietTiemDTO
                {
                    MaPhieuTiem = pt.MaPhieuTiem,
                    MaLo        = r.MaLo,
                    SoLuong     = r.SoLuong,
                    DonGia      = r.DonGia,
                    ThanhTien   = r.ThanhTien,
                }).ToList();

                _controller.LuuPhieuTiem(pt, chiTiets, LoVacXinList);
                MessageBox.Show("Lập phiếu đăng ký tiêm phòng thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Reload LoVacXin (stock changed) then reset
                LoVacXinList = _controller.GetAllLoVacXin();
                ResetPhieu();
            }
            catch (Exception ex)
            {
                txtError.Text = ex.Message;
            }
        }

        private void BtnLamMoi_Click(object sender, RoutedEventArgs e) => ResetPhieu();

        private void BtnThoat_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
