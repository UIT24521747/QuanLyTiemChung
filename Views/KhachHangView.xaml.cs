using System;
using System.Windows;
using System.Windows.Controls;
using QuanLyKhachHang.Controllers;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Views
{
    public partial class KhachHangView : UserControl
    {
        private readonly KhachHangController _controller = new KhachHangController();
        private readonly ThamSoController _thamSoCtrl = new();
        private bool _isEditMode = false;

        public KhachHangView()
        {
            InitializeComponent();
            IsVisibleChanged += (_, e) => { if ((bool)e.NewValue) LoadThamSo(); };
            LoadThamSo();
            ResetForm();
        }

        private void LoadThamSo()
        {
            try
            {
                int age = _thamSoCtrl.GetThamSo().SoTuoiCanGiamHo;
                txtGiamHoHint.Text = $"[i] Bắt buộc khi khách hàng dưới {age} tuổi (độ tuổi quy định).";
            }
            catch { txtGiamHoHint.Text = "[i] Bắt buộc khi khách hàng dưới độ tuổi quy định."; }
        }

        private void ResetForm()
        {
            txtMaKH.Text     = _controller.GenerateMaKH();
            txtTenKH.Text    = "";
            txtGioiTinh.Text = "";
            txtCCCD.Text     = "";
            txtSDT.Text      = "";
            txtEmail.Text    = "";
            txtDiaChi.Text   = "";
            dpNgaySinh.SelectedDate = null;

            txtMaGH.Text        = _controller.GenerateMaGH();
            txtTenGH.Text       = "";
            txtQuanHe_GH.Text   = "";
            txtCCCD_GH.Text     = "";
            txtSDT_GH.Text      = "";
            txtDiaChi_GH.Text   = "";
            txtGioiTinh_GH.Text = "";
            txtEmail_GH.Text    = "";

            txtError.Text = "";
            _isEditMode   = false;
            btn1.Content  = "Tiếp nhận khách hàng";
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtError.Text = "";
                if (string.IsNullOrWhiteSpace(txtTenKH.Text))  { txtError.Text = "Vui lòng nhập họ tên!"; return; }
                if (string.IsNullOrWhiteSpace(txtSDT.Text))    { txtError.Text = "Vui lòng nhập SĐT!"; return; }
                if (string.IsNullOrWhiteSpace(txtCCCD.Text))   { txtError.Text = "Vui lòng nhập CCCD!"; return; }
                if (string.IsNullOrWhiteSpace(txtDiaChi.Text)) { txtError.Text = "Vui lòng nhập địa chỉ!"; return; }
                if (dpNgaySinh.SelectedDate == null)            { txtError.Text = "Vui lòng chọn ngày sinh!"; return; }

                var kh = new KhachHangDTO
                {
                    MaKH     = txtMaKH.Text,
                    TenKH    = txtTenKH.Text,
                    SDT      = txtSDT.Text,
                    Email    = txtEmail.Text,
                    GioiTinh = txtGioiTinh.Text,
                    NgaySinh = dpNgaySinh.SelectedDate!.Value,
                    CCCD     = txtCCCD.Text,
                    DiaChi   = txtDiaChi.Text,
                    MaGH     = string.IsNullOrWhiteSpace(txtMaGH.Text) ? null : txtMaGH.Text
                };

                NguoiGiamHoDTO? gh = null;
                if (!string.IsNullOrWhiteSpace(txtTenGH.Text))
                    gh = new NguoiGiamHoDTO
                    {
                        MaGH        = txtMaGH.Text,
                        TenGH       = txtTenGH.Text,
                        SDT_GH      = txtSDT_GH.Text,
                        Email_GH    = txtEmail_GH.Text,
                        GioiTinh_GH = txtGioiTinh_GH.Text,
                        NgaySinh_GH = DateTime.Now,
                        CCCD_GH     = txtCCCD_GH.Text,
                        DiaChi_GH   = txtDiaChi_GH.Text,
                        QuanHe      = txtQuanHe_GH.Text
                    };

                _controller.LuuKhachHang(kh, gh, _isEditMode);
                MessageBox.Show(_isEditMode ? "Cập nhật thành công!" : "Tiếp nhận thành công!",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetForm();
            }
            catch (Exception ex) { txtError.Text = ex.Message; }
        }

        private void Btn2_Click(object sender, RoutedEventArgs e) => ResetForm();

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string maKH = PromptForInput("Nhập mã khách hàng để tra cứu:");
                if (string.IsNullOrWhiteSpace(maKH)) return;

                var (kh, gh) = _controller.GetKhachHangWithNguoiGiamHoById(maKH);
                if (kh == null) { MessageBox.Show("Không tìm thấy!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information); return; }

                _isEditMode       = true;
                txtMaKH.Text      = kh.MaKH ?? "";
                txtTenKH.Text     = kh.TenKH ?? "";
                txtSDT.Text       = kh.SDT ?? "";
                txtEmail.Text     = kh.Email ?? "";
                txtCCCD.Text      = kh.CCCD ?? "";
                txtDiaChi.Text    = kh.DiaChi ?? "";
                txtGioiTinh.Text  = kh.GioiTinh ?? "";
                dpNgaySinh.SelectedDate = kh.NgaySinh;

                txtMaGH.Text        = !string.IsNullOrWhiteSpace(kh.MaGH) ? kh.MaGH : _controller.GenerateMaGH();
                txtTenGH.Text       = gh?.TenGH ?? "";
                txtSDT_GH.Text      = gh?.SDT_GH ?? "";
                txtEmail_GH.Text    = gh?.Email_GH ?? "";
                txtCCCD_GH.Text     = gh?.CCCD_GH ?? "";
                txtDiaChi_GH.Text   = gh?.DiaChi_GH ?? "";
                txtGioiTinh_GH.Text = gh?.GioiTinh_GH ?? "";
                txtQuanHe_GH.Text   = gh?.QuanHe ?? "";

                btn1.Content  = "Cập nhật khách hàng";
                txtError.Text = "Đã tìm thấy — chỉnh sửa và nhấn Cập nhật.";
            }
            catch (Exception ex) { txtError.Text = ex.Message; }
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string maKH = PromptForInput("Nhập mã khách hàng để xóa:");
                if (string.IsNullOrWhiteSpace(maKH)) return;
                if (MessageBox.Show($"Xóa khách hàng {maKH}?", "Xác nhận",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                _controller.XoaKhachHang(maKH);
                MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetForm();
            }
            catch (Exception ex) { txtError.Text = ex.Message; }
        }

        private void Btn6_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private string PromptForInput(string prompt)
        {
            var pw = Window.GetWindow(this);
            var dlg = new Window
            {
                Title = "Tra cứu", Width = 380, SizeToContent = SizeToContent.Height,
                ResizeMode = ResizeMode.NoResize, WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = pw, Background = System.Windows.Media.Brushes.White
            };
            var g = new Grid();
            g.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            g.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            g.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var lbl = new TextBlock { Text = prompt, Margin = new Thickness(14, 12, 14, 6), TextWrapping = TextWrapping.Wrap };
            Grid.SetRow(lbl, 0); g.Children.Add(lbl);
            var tb = new TextBox { Margin = new Thickness(14, 0, 14, 10), Padding = new Thickness(6), Height = 34 };
            Grid.SetRow(tb, 1); g.Children.Add(tb);
            var row = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(14, 0, 14, 12) };
            Grid.SetRow(row, 2);
            var ok = new Button { Content = "OK", Width = 68, Margin = new Thickness(4), Padding = new Thickness(6) };
            ok.Click += (_, _) => dlg.DialogResult = true;
            var no = new Button { Content = "Hủy", Width = 68, Margin = new Thickness(4), Padding = new Thickness(6) };
            no.Click += (_, _) => dlg.DialogResult = false;
            row.Children.Add(ok); row.Children.Add(no); g.Children.Add(row);
            dlg.Content = g;
            return dlg.ShowDialog() == true ? tb.Text : "";
        }
    }
}
