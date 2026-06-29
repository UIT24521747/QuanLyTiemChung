using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QuanLyKhachHang.Controllers;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Views
{
    public partial class BaoCaoView : UserControl
    {
        private readonly BaoCaoController _controller = new();

        public BaoCaoView()
        {
            InitializeComponent();

            // Process 1 & 2: default year and month to current
            txtNamVX.Text    = DateTime.Today.Year.ToString();
            txtNamThang.Text = DateTime.Today.Year.ToString();

            cboThang.ItemsSource   = Enumerable.Range(1, 12).ToList();
            cboThang.SelectedItem  = DateTime.Today.Month;
        }

        // BM6.1
        private void BtnBaoCaoVX_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtNamVX.Text.Trim(), out int nam) || nam < 2000)
                    throw new Exception("Năm không hợp lệ!");
                if (cboThang.SelectedItem is not int thang)
                    throw new Exception("Phải chọn tháng!");

                var rows = _controller.GetBaoCaoVacXin(nam, thang);
                int stt = 1;
                dgVacXin.ItemsSource = rows.Select(r => new
                {
                    Stt           = stt++,
                    r.TenVacXin,
                    r.TenLoaiVacXin,
                    r.TongLuotTiem,
                    r.DoanhThu,
                }).ToList();

                int tongLuot = rows.Sum(r => r.TongLuotTiem);
                txtTongLuotVX.Text = tongLuot > 0 ? tongLuot.ToString("N0") : "0";

                decimal tongDoanhSo = rows.Sum(r => r.DoanhThu);
                txtTongDoanhSo.Text = tongDoanhSo.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // BM6.2
        private void BtnBaoCaoThang_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtNamThang.Text.Trim(), out int nam) || nam < 2000)
                    throw new Exception("Năm không hợp lệ!");

                var rows = _controller.GetBaoCaoThang(nam);
                dgThang.ItemsSource = rows;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e) =>
            Application.Current.Shutdown();
    }
}
