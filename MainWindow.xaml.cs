using System;
using System.Windows;
using System.Windows.Controls;
using QuanLyKhachHang.Controllers;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang
{
    public partial class MainWindow : Window
    {
        private KhachHangController _controller = new KhachHangController();
        private bool _isEditMode = false;

        public MainWindow()
        {
            InitializeComponent();
            ResetForm();
        }

        private void ResetForm()
        {
            txtMaKH.Text = _controller.GenerateMaKH();
            txtTenKH.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            txtCCCD.Text = "";
            txtDiaChi.Text = "";
            txtGioiTinh.Text = "";
            dpNgaySinh.SelectedDate = DateTime.Now;

            txtMaGH.Text = _controller.GenerateMaGH();
            txtTenGH.Text = "";
            txtSDT_GH.Text = "";
            txtEmail_GH.Text = "";
            txtCCCD_GH.Text = "";
            txtDiaChi_GH.Text = "";
            txtGioiTinh_GH.Text = "";
            dpNgaySinh_GH.SelectedDate = DateTime.Now;

            txtError.Text = "";
            _isEditMode = false;
            btn1.Content = "Tiếp nhận";
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtError.Text = "";

                if (string.IsNullOrWhiteSpace(txtTenKH.Text))
                {
                    txtError.Text = "Vui lòng nhập họ tên khách hàng!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    txtError.Text = "Vui lòng nhập số điện thoại!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtCCCD.Text))
                {
                    txtError.Text = "Vui lòng nhập CCCD!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
                {
                    txtError.Text = "Vui lòng nhập địa chỉ!";
                    return;
                }

                var kh = new KhachHangDTO
                {
                    MaKH = txtMaKH.Text ?? "",
                    TenKH = txtTenKH.Text ?? "",
                    SDT = txtSDT.Text ?? "",
                    Email = txtEmail.Text ?? "",
                    GioiTinh = txtGioiTinh.Text ?? "",
                    NgaySinh = dpNgaySinh.SelectedDate ?? DateTime.Now,
                    CCCD = txtCCCD.Text ?? "",
                    DiaChi = txtDiaChi.Text ?? "",
                    MaGH = string.IsNullOrWhiteSpace(txtMaGH.Text) ? null : txtMaGH.Text
                };

                NguoiGiamHoDTO? gh = null;
                if (!string.IsNullOrWhiteSpace(txtTenGH.Text))
                {
                    gh = new NguoiGiamHoDTO
                    {
                        MaGH = txtMaGH.Text,
                        TenGH = txtTenGH.Text,
                        SDT_GH = txtSDT_GH.Text ?? "",
                        Email_GH = txtEmail_GH.Text ?? "",
                        GioiTinh_GH = txtGioiTinh_GH.Text ?? "",
                        NgaySinh_GH = dpNgaySinh_GH.SelectedDate ?? DateTime.Now,
                        CCCD_GH = txtCCCD_GH.Text ?? "",
                        DiaChi_GH = txtDiaChi_GH.Text ?? ""
                    };
                }

                _controller.LuuKhachHang(kh, gh, _isEditMode);

                string msg = _isEditMode ? "Cập nhật khách hàng thành công!" : "Tiếp nhận khách hàng thành công!";
                MessageBox.Show(msg, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                ResetForm();
            }
            catch (Exception ex)
            {
                txtError.Text = ex.Message;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string maKH = PromptForInput("Nhập mã khách hàng để tìm kiếm:");
                if (string.IsNullOrWhiteSpace(maKH))
                    return;

                var result = _controller.GetKhachHangWithNguoiGiamHoById(maKH);
                var kh = result.KhachHang;
                var gh = result.NguoiGiamHo;
                if (kh == null)
                {
                    MessageBox.Show($"Không tìm thấy khách hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                _isEditMode = true;
                txtMaKH.Text = kh.MaKH ?? "";
                txtTenKH.Text = kh.TenKH ?? "";
                txtSDT.Text = kh.SDT ?? "";
                txtEmail.Text = kh.Email ?? "";
                txtCCCD.Text = kh.CCCD ?? "";
                txtDiaChi.Text = kh.DiaChi ?? "";
                dpNgaySinh.SelectedDate = kh.NgaySinh;
                txtGioiTinh.Text = kh.GioiTinh ?? "";

                txtMaGH.Text = !string.IsNullOrWhiteSpace(kh.MaGH) ? kh.MaGH : _controller.GenerateMaGH();
                txtTenGH.Text = gh?.TenGH ?? "";
                txtSDT_GH.Text = gh?.SDT_GH ?? "";
                txtEmail_GH.Text = gh?.Email_GH ?? "";
                txtCCCD_GH.Text = gh?.CCCD_GH ?? "";
                txtDiaChi_GH.Text = gh?.DiaChi_GH ?? "";
                txtGioiTinh_GH.Text = gh?.GioiTinh_GH ?? "";
                dpNgaySinh_GH.SelectedDate = gh?.NgaySinh_GH ?? DateTime.Now;

                btn1.Content = "Cập nhật";
                txtError.Text = "Đã tìm thấy. Hãy chỉnh sửa và nhấn Cập nhật.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string maKH = PromptForInput("Nhập mã khách hàng để xóa:");
                if (string.IsNullOrWhiteSpace(maKH))
                    return;

                var result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa khách hàng: {maKH}?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _controller.XoaKhachHang(maKH);
                    MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            if (!_isEditMode)
            {
                MessageBox.Show("Vui lòng tìm khách hàng trước!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Btn1_Click(this, new RoutedEventArgs());
        }

        private void Btn6_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private string PromptForInput(string prompt)
        {
            var dialog = new Window
            {
                Title = "Nhập thông tin",
                Width = 420,
                MinHeight = 190,
                SizeToContent = SizeToContent.Height,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                Background = System.Windows.Media.Brushes.White
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var label = new TextBlock { Text = prompt, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(15, 10, 15, 10) };
            Grid.SetRow(label, 0);
            grid.Children.Add(label);

            var textBox = new TextBox { Margin = new Thickness(15, 0, 15, 10), Padding = new Thickness(8), Height = 35 };
            Grid.SetRow(textBox, 1);
            grid.Children.Add(textBox);

            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(15, 10, 15, 10) };
            Grid.SetRow(btnPanel, 2);

            var okBtn = new Button { Content = "OK", Width = 70, Margin = new Thickness(5), Padding = new Thickness(8) };
            okBtn.Click += (s, e) => dialog.DialogResult = true;

            var cancelBtn = new Button { Content = "Hủy", Width = 70, Margin = new Thickness(5), Padding = new Thickness(8) };
            cancelBtn.Click += (s, e) => dialog.DialogResult = false;

            btnPanel.Children.Add(okBtn);
            btnPanel.Children.Add(cancelBtn);
            grid.Children.Add(btnPanel);

            dialog.Content = grid;
            return dialog.ShowDialog() == true ? textBox.Text : "";
        }

    }
}
