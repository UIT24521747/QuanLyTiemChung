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

            txtNamVX.Text = DateTime.Today.Year.ToString();

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
                    r.TiLe,
                }).ToList();

                decimal tongDoanhSo = rows.Sum(r => r.DoanhThu);
                txtTongDoanhSoThang.Text = tongDoanhSo > 0 ? tongDoanhSo.ToString("N0") : "0";
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
