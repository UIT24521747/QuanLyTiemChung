using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachHang.Controllers;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang
{
    public partial class MainForm : Form
    {
        private KhachHangController _controller = new KhachHangController();
        private bool _isEditMode = false;

        public MainForm()
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
            dpNgaySinh.Value = DateTime.Now;

            txtMaGH.Text = _controller.GenerateMaGH();
            txtTenGH.Text = "";
            txtSDT_GH.Text = "";
            txtEmail_GH.Text = "";
            txtCCCD_GH.Text = "";
            txtDiaChi_GH.Text = "";
            txtGioiTinh_GH.Text = "";
            dpNgaySinh_GH.Value = DateTime.Now;
            txtQuanHe.Text = "";

            lblError.Text = "";
            _isEditMode = false;
            btn1.Text = "Tiếp nhận";
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                if (string.IsNullOrWhiteSpace(txtTenKH.Text))
                {
                    lblError.Text = "Vui lòng nhập họ tên khách hàng!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    lblError.Text = "Vui lòng nhập số điện thoại!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtCCCD.Text))
                {
                    lblError.Text = "Vui lòng nhập CCCD!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
                {
                    lblError.Text = "Vui lòng nhập địa chỉ!";
                    return;
                }

                var kh = new KhachHangDTO
                {
                    MaKH = txtMaKH.Text ?? "",
                    TenKH = txtTenKH.Text ?? "",
                    SDT = txtSDT.Text ?? "",
                    Email = txtEmail.Text ?? "",
                    GioiTinh = txtGioiTinh.Text ?? "",
                    NgaySinh = dpNgaySinh.Value,
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
                        NgaySinh_GH = dpNgaySinh_GH.Value,
                        CCCD_GH = txtCCCD_GH.Text ?? "",
                        DiaChi_GH = txtDiaChi_GH.Text ?? "",
                        QuanHe = txtQuanHe.Text ?? ""
                    };
                }

                _controller.LuuKhachHang(kh, gh, _isEditMode);

                string msg = _isEditMode ? "Cập nhật khách hàng thành công!" : "Tiếp nhận khách hàng thành công!";
                MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetForm();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btn3_Click(object sender, EventArgs e)
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
                    MessageBox.Show($"Không tìm thấy khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _isEditMode = true;
                txtMaKH.Text = kh.MaKH ?? "";
                txtTenKH.Text = kh.TenKH ?? "";
                txtSDT.Text = kh.SDT ?? "";
                txtEmail.Text = kh.Email ?? "";
                txtCCCD.Text = kh.CCCD ?? "";
                txtDiaChi.Text = kh.DiaChi ?? "";
                dpNgaySinh.Value = kh.NgaySinh;
                txtGioiTinh.Text = kh.GioiTinh ?? "";

                txtMaGH.Text = !string.IsNullOrWhiteSpace(kh.MaGH) ? kh.MaGH : _controller.GenerateMaGH();
                txtTenGH.Text = gh?.TenGH ?? "";
                txtSDT_GH.Text = gh?.SDT_GH ?? "";
                txtEmail_GH.Text = gh?.Email_GH ?? "";
                txtCCCD_GH.Text = gh?.CCCD_GH ?? "";
                txtDiaChi_GH.Text = gh?.DiaChi_GH ?? "";
                txtGioiTinh_GH.Text = gh?.GioiTinh_GH ?? "";
                dpNgaySinh_GH.Value = gh?.NgaySinh_GH ?? DateTime.Now;
                txtQuanHe.Text = gh?.QuanHe ?? "";

                btn1.Text = "Cập nhật";
                lblError.Text = "Đã tìm thấy. Hãy chỉnh sửa và nhấn Cập nhật.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            try
            {
                string maKH = PromptForInput("Nhập mã khách hàng để xóa:");
                if (string.IsNullOrWhiteSpace(maKH))
                    return;

                var result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa khách hàng: {maKH}?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _controller.XoaKhachHang(maKH);
                    MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (!_isEditMode)
            {
                MessageBox.Show("Vui lòng tìm khách hàng trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            btn1_Click(this, EventArgs.Empty);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string PromptForInput(string prompt)
        {
            Form promptForm = new Form()
            {
                Width = 440,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Nhập thông tin",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };
            Label textLabel = new Label() { Left = 15, Top = 15, Width = 390, Height = 36, AutoEllipsis = true, Text = prompt };
            TextBox textBox = new TextBox() { Left = 15, Top = 60, Width = 390, Height = 30 };
            Button confirmation = new Button() { Text = "OK", Left = 250, Width = 70, Top = 105, DialogResult = DialogResult.OK };
            Button cancel = new Button() { Text = "Hủy", Left = 335, Width = 70, Top = 105, DialogResult = DialogResult.Cancel };
            
            confirmation.Click += (sender, e) => { promptForm.Close(); };
            cancel.Click += (sender, e) => { promptForm.Close(); };
            
            promptForm.Controls.Add(textBox);
            promptForm.Controls.Add(confirmation);
            promptForm.Controls.Add(cancel);
            promptForm.Controls.Add(textLabel);
            promptForm.AcceptButton = confirmation;
            promptForm.CancelButton = cancel;

            return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
