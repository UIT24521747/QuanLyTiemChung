using System.Windows.Forms;

namespace QuanLyKhachHang
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.mainPanel = new QuanLyKhachHang.RoundedPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            
            // Customer Fields
            this.lblMaKH = new System.Windows.Forms.Label();
            this.txtMaKH = new QuanLyKhachHang.RoundedTextBox();
            this.lblTenKH = new System.Windows.Forms.Label();
            this.txtTenKH = new QuanLyKhachHang.RoundedTextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtSDT = new QuanLyKhachHang.RoundedTextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new QuanLyKhachHang.RoundedTextBox();
            this.lblGioiTinh = new System.Windows.Forms.Label();
            this.txtGioiTinh = new QuanLyKhachHang.RoundedTextBox();
            this.lblNgaySinh = new System.Windows.Forms.Label();
            this.dpNgaySinh = new QuanLyKhachHang.RoundedDateTimePicker();
            this.lblCCCD = new System.Windows.Forms.Label();
            this.txtCCCD = new QuanLyKhachHang.RoundedTextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new QuanLyKhachHang.RoundedTextBox();

            // Guardian Section
            this.lblGuardianTitle = new System.Windows.Forms.Label();
            this.pnlGuardianInfo = new QuanLyKhachHang.RoundedPanel();
            this.lblGuardianInfo = new System.Windows.Forms.Label();
            
            this.lblMaGH = new System.Windows.Forms.Label();
            this.txtMaGH = new QuanLyKhachHang.RoundedTextBox();
            this.lblTenGH = new System.Windows.Forms.Label();
            this.txtTenGH = new QuanLyKhachHang.RoundedTextBox();
            this.lblSDT_GH = new System.Windows.Forms.Label();
            this.txtSDT_GH = new QuanLyKhachHang.RoundedTextBox();
            this.lblEmail_GH = new System.Windows.Forms.Label();
            this.txtEmail_GH = new QuanLyKhachHang.RoundedTextBox();
            this.lblGioiTinh_GH = new System.Windows.Forms.Label();
            this.txtGioiTinh_GH = new QuanLyKhachHang.RoundedTextBox();
            this.lblNgaySinh_GH = new System.Windows.Forms.Label();
            this.dpNgaySinh_GH = new QuanLyKhachHang.RoundedDateTimePicker();
            this.lblCCCD_GH = new System.Windows.Forms.Label();
            this.txtCCCD_GH = new QuanLyKhachHang.RoundedTextBox();
            this.lblDiaChi_GH = new System.Windows.Forms.Label();
            this.txtDiaChi_GH = new QuanLyKhachHang.RoundedTextBox();
            this.lblQuanHe = new System.Windows.Forms.Label();
            this.txtQuanHe = new QuanLyKhachHang.RoundedTextBox();

            this.lblError = new System.Windows.Forms.Label();

            // Buttons
            this.pnlButtons = new QuanLyKhachHang.RoundedPanel();
            this.btn1 = new QuanLyKhachHang.ModernButton();
            this.btn2 = new QuanLyKhachHang.ModernButton();
            this.btn3 = new QuanLyKhachHang.ModernButton();
            this.btn4 = new QuanLyKhachHang.ModernButton();
            this.btn5 = new QuanLyKhachHang.ModernButton();
            this.btn6 = new QuanLyKhachHang.ModernButton();

            this.pnlHeader.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.pnlGuardianInfo.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(20, 15);
            this.lblHeader.Text = "HỆ THỐNG QUẢN LÝ KHÁCH HÀNG";

            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.White;
            this.mainPanel.BorderRadius = 24;
            this.mainPanel.Location = new System.Drawing.Point(40, 80);
            this.mainPanel.Size = new System.Drawing.Size(900, 750);
            this.mainPanel.AutoScroll = true;
            this.mainPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Title
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Text = "Phiếu đăng ký thông tin khách hàng";
            this.mainPanel.Controls.Add(this.lblTitle);

            int startY = 90;
            int col1 = 30;
            int col2 = 320;
            int col3 = 610;
            int inputWidth1 = 270;
            int inputWidth2 = 560; // Span 2 cols

            // Row 1
            this.lblMaKH.Text = "Mã khách hàng *";
            this.lblMaKH.Location = new System.Drawing.Point(col1, startY);
            this.lblMaKH.AutoSize = true;
            this.txtMaKH.Location = new System.Drawing.Point(col1, startY + 25);
            this.txtMaKH.Size = new System.Drawing.Size(inputWidth1, 35);
            this.txtMaKH.ReadOnly = true;
            this.txtMaKH.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);

            this.lblTenKH.Text = "Họ tên *";
            this.lblTenKH.Location = new System.Drawing.Point(col2, startY);
            this.lblTenKH.AutoSize = true;
            this.txtTenKH.Location = new System.Drawing.Point(col2, startY + 25);
            this.txtTenKH.Size = new System.Drawing.Size(inputWidth2, 35);

            // Row 2
            startY += 75;
            this.lblSDT.Text = "SĐT *";
            this.lblSDT.Location = new System.Drawing.Point(col1, startY);
            this.lblSDT.AutoSize = true;
            this.txtSDT.Location = new System.Drawing.Point(col1, startY + 25);
            this.txtSDT.Size = new System.Drawing.Size(inputWidth1, 35);

            this.lblEmail.Text = "Email";
            this.lblEmail.Location = new System.Drawing.Point(col2, startY);
            this.lblEmail.AutoSize = true;
            this.txtEmail.Location = new System.Drawing.Point(col2, startY + 25);
            this.txtEmail.Size = new System.Drawing.Size(inputWidth2, 35);

            // Row 3
            startY += 75;
            this.lblGioiTinh.Text = "Giới tính *";
            this.lblGioiTinh.Location = new System.Drawing.Point(col1, startY);
            this.lblGioiTinh.AutoSize = true;
            this.txtGioiTinh.Location = new System.Drawing.Point(col1, startY + 25);
            this.txtGioiTinh.Size = new System.Drawing.Size(inputWidth1, 35);

            this.lblNgaySinh.Text = "Ngày sinh *";
            this.lblNgaySinh.Location = new System.Drawing.Point(col2, startY);
            this.lblNgaySinh.AutoSize = true;
            this.dpNgaySinh.Location = new System.Drawing.Point(col2, startY + 25);
            this.dpNgaySinh.Size = new System.Drawing.Size(inputWidth1, 35);
            this.dpNgaySinh.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dpNgaySinh.Format = DateTimePickerFormat.Short;

            this.lblCCCD.Text = "CCCD *";
            this.lblCCCD.Location = new System.Drawing.Point(col3, startY);
            this.lblCCCD.AutoSize = true;
            this.txtCCCD.Location = new System.Drawing.Point(col3, startY + 25);
            this.txtCCCD.Size = new System.Drawing.Size(inputWidth1, 35);

            // Row 4
            startY += 75;
            this.lblDiaChi.Text = "Địa chỉ *";
            this.lblDiaChi.Location = new System.Drawing.Point(col1, startY);
            this.lblDiaChi.AutoSize = true;
            this.txtDiaChi.Location = new System.Drawing.Point(col1, startY + 25);
            this.txtDiaChi.Size = new System.Drawing.Size(850, 35);

            // Guardian Title
            startY += 80;
            this.lblGuardianTitle.AutoSize = true;
            this.lblGuardianTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblGuardianTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblGuardianTitle.Location = new System.Drawing.Point(30, startY);
            this.lblGuardianTitle.Text = "Thông tin người giám hộ";

            // Guardian Info Box
            startY += 40;
            this.pnlGuardianInfo.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlGuardianInfo.BorderRadius = 12;
            this.pnlGuardianInfo.Location = new System.Drawing.Point(30, startY);
            this.pnlGuardianInfo.Size = new System.Drawing.Size(850, 40);
            
            this.lblGuardianInfo.Text = "ℹ️ Bắt buộc với người dưới 18 tuổi (trên 18 tuổi có thể bỏ qua).";
            this.lblGuardianInfo.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblGuardianInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGuardianInfo.AutoSize = true;
            this.lblGuardianInfo.Location = new System.Drawing.Point(10, 10);
            this.pnlGuardianInfo.Controls.Add(this.lblGuardianInfo);

            // Guardian Row 1
            startY += 60;
            this.lblMaGH.Text = "Mã người giám hộ";
            this.lblMaGH.Location = new System.Drawing.Point(col1, startY);
            this.lblMaGH.AutoSize = true;
            this.txtMaGH.Location = new System.Drawing.Point(col1, startY + 25);
            this.txtMaGH.Size = new System.Drawing.Size(inputWidth1, 35);
            this.txtMaGH.ReadOnly = true;
            this.txtMaGH.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);

            this.lblTenGH.Text = "Họ tên người giám hộ";
            this.lblTenGH.Location = new System.Drawing.Point(col2, startY);
            this.lblTenGH.AutoSize = true;
            this.txtTenGH.Location = new System.Drawing.Point(col2, startY + 25);
            this.txtTenGH.Size = new System.Drawing.Size(inputWidth2, 35);

            // Guardian Row 2
            startY += 75;
            this.lblSDT_GH.Text = "SĐT";
            this.lblSDT_GH.Location = new System.Drawing.Point(col1, startY);
            this.lblSDT_GH.AutoSize = true;
            this.txtSDT_GH.Location = new System.Drawing.Point(col1, startY + 25);
            this.txtSDT_GH.Size = new System.Drawing.Size(inputWidth1, 35);

            this.lblEmail_GH.Text = "Email";
            this.lblEmail_GH.Location = new System.Drawing.Point(col2, startY);
            this.lblEmail_GH.AutoSize = true;
            this.txtEmail_GH.Location = new System.Drawing.Point(col2, startY + 25);
            this.txtEmail_GH.Size = new System.Drawing.Size(inputWidth2, 35);

            // Guardian Row 3
            startY += 75;
            this.lblGioiTinh_GH.Text = "Giới tính";
            this.lblGioiTinh_GH.Location = new System.Drawing.Point(col1, startY);
            this.lblGioiTinh_GH.AutoSize = true;
            this.txtGioiTinh_GH.Location = new System.Drawing.Point(col1, startY + 25);
            this.txtGioiTinh_GH.Size = new System.Drawing.Size(inputWidth1, 35);

            this.lblNgaySinh_GH.Text = "Ngày sinh";
            this.lblNgaySinh_GH.Location = new System.Drawing.Point(col2, startY);
            this.lblNgaySinh_GH.AutoSize = true;
            this.dpNgaySinh_GH.Location = new System.Drawing.Point(col2, startY + 25);
            this.dpNgaySinh_GH.Size = new System.Drawing.Size(inputWidth1, 35);
            this.dpNgaySinh_GH.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dpNgaySinh_GH.Format = DateTimePickerFormat.Short;

            this.lblCCCD_GH.Text = "CCCD";
            this.lblCCCD_GH.Location = new System.Drawing.Point(col3, startY);
            this.lblCCCD_GH.AutoSize = true;
            this.txtCCCD_GH.Location = new System.Drawing.Point(col3, startY + 25);
            this.txtCCCD_GH.Size = new System.Drawing.Size(inputWidth1, 35);

            // Guardian Row 4
            startY += 75;
            this.lblDiaChi_GH.Text = "Địa chỉ";
            this.lblDiaChi_GH.Location = new System.Drawing.Point(col1, startY);
            this.lblDiaChi_GH.AutoSize = true;
            this.txtDiaChi_GH.Location = new System.Drawing.Point(col1, startY + 25);
            this.txtDiaChi_GH.Size = new System.Drawing.Size(850, 35);

            // Guardian Row 5
            startY += 75;
            this.lblQuanHe.Text = "Quan hệ với khách hàng";
            this.lblQuanHe.Location = new System.Drawing.Point(col1, startY);
            this.lblQuanHe.AutoSize = true;
            this.txtQuanHe.Location = new System.Drawing.Point(col1, startY + 25);
            this.txtQuanHe.Size = new System.Drawing.Size(850, 35);

            // Error Label
            startY += 70;
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(30, startY);

            // Buttons Panel
            startY += 30;
            this.pnlButtons.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlButtons.BorderRadius = 12;
            this.pnlButtons.Location = new System.Drawing.Point(30, startY);
            this.pnlButtons.Size = new System.Drawing.Size(850, 70);

            this.btn1.Text = "Tiếp nhận";
            this.btn1.Location = new System.Drawing.Point(15, 15);
            this.btn1.Size = new System.Drawing.Size(160, 40);
            this.btn1.Click += new System.EventHandler(this.btn1_Click);

            this.btn2.Text = "Khách hàng mới";
            this.btn2.Location = new System.Drawing.Point(185, 15);
            this.btn2.Size = new System.Drawing.Size(130, 40);
            this.btn2.Click += new System.EventHandler(this.btn2_Click);

            this.btn3.Text = "Tìm";
            this.btn3.Location = new System.Drawing.Point(325, 15);
            this.btn3.Size = new System.Drawing.Size(130, 40);
            this.btn3.Click += new System.EventHandler(this.btn3_Click);

            this.btn4.Text = "Xóa";
            this.btn4.Location = new System.Drawing.Point(465, 15);
            this.btn4.Size = new System.Drawing.Size(130, 40);
            this.btn4.Click += new System.EventHandler(this.btn4_Click);

            this.btn5.Text = "Cập nhật";
            this.btn5.Location = new System.Drawing.Point(605, 15);
            this.btn5.Size = new System.Drawing.Size(150, 40);
            this.btn5.Click += new System.EventHandler(this.btn5_Click);

            this.btn6.Text = "Thoát";
            this.btn6.Location = new System.Drawing.Point(765, 15);
            this.btn6.Size = new System.Drawing.Size(70, 40);
            this.btn6.Click += new System.EventHandler(this.btn6_Click);

            this.pnlButtons.Controls.Add(this.btn1);
            this.pnlButtons.Controls.Add(this.btn2);
            this.pnlButtons.Controls.Add(this.btn3);
            this.pnlButtons.Controls.Add(this.btn4);
            this.pnlButtons.Controls.Add(this.btn5);
            this.pnlButtons.Controls.Add(this.btn6);

            // Add controls to main panel
            this.mainPanel.Controls.Add(this.lblMaKH);
            this.mainPanel.Controls.Add(this.txtMaKH);
            this.mainPanel.Controls.Add(this.lblTenKH);
            this.mainPanel.Controls.Add(this.txtTenKH);
            this.mainPanel.Controls.Add(this.lblSDT);
            this.mainPanel.Controls.Add(this.txtSDT);
            this.mainPanel.Controls.Add(this.lblEmail);
            this.mainPanel.Controls.Add(this.txtEmail);
            this.mainPanel.Controls.Add(this.lblGioiTinh);
            this.mainPanel.Controls.Add(this.txtGioiTinh);
            this.mainPanel.Controls.Add(this.lblNgaySinh);
            this.mainPanel.Controls.Add(this.dpNgaySinh);
            this.mainPanel.Controls.Add(this.lblCCCD);
            this.mainPanel.Controls.Add(this.txtCCCD);
            this.mainPanel.Controls.Add(this.lblDiaChi);
            this.mainPanel.Controls.Add(this.txtDiaChi);

            this.mainPanel.Controls.Add(this.lblGuardianTitle);
            this.mainPanel.Controls.Add(this.pnlGuardianInfo);
            this.mainPanel.Controls.Add(this.lblMaGH);
            this.mainPanel.Controls.Add(this.txtMaGH);
            this.mainPanel.Controls.Add(this.lblTenGH);
            this.mainPanel.Controls.Add(this.txtTenGH);
            this.mainPanel.Controls.Add(this.lblSDT_GH);
            this.mainPanel.Controls.Add(this.txtSDT_GH);
            this.mainPanel.Controls.Add(this.lblEmail_GH);
            this.mainPanel.Controls.Add(this.txtEmail_GH);
            this.mainPanel.Controls.Add(this.lblGioiTinh_GH);
            this.mainPanel.Controls.Add(this.txtGioiTinh_GH);
            this.mainPanel.Controls.Add(this.lblNgaySinh_GH);
            this.mainPanel.Controls.Add(this.dpNgaySinh_GH);
            this.mainPanel.Controls.Add(this.lblCCCD_GH);
            this.mainPanel.Controls.Add(this.txtCCCD_GH);
            this.mainPanel.Controls.Add(this.lblDiaChi_GH);
            this.mainPanel.Controls.Add(this.txtDiaChi_GH);
            this.mainPanel.Controls.Add(this.lblQuanHe);
            this.mainPanel.Controls.Add(this.txtQuanHe);

            this.mainPanel.Controls.Add(this.lblError);
            this.mainPanel.Controls.Add(this.pnlButtons);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(980, 850);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.pnlHeader);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Khách Hàng";
            
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.pnlGuardianInfo.ResumeLayout(false);
            this.pnlGuardianInfo.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private QuanLyKhachHang.RoundedPanel mainPanel;
        private System.Windows.Forms.Label lblTitle;
        
        private System.Windows.Forms.Label lblMaKH;
        private QuanLyKhachHang.RoundedTextBox txtMaKH;
        private System.Windows.Forms.Label lblTenKH;
        private QuanLyKhachHang.RoundedTextBox txtTenKH;
        private System.Windows.Forms.Label lblSDT;
        private QuanLyKhachHang.RoundedTextBox txtSDT;
        private System.Windows.Forms.Label lblEmail;
        private QuanLyKhachHang.RoundedTextBox txtEmail;
        private System.Windows.Forms.Label lblGioiTinh;
        private QuanLyKhachHang.RoundedTextBox txtGioiTinh;
        private System.Windows.Forms.Label lblNgaySinh;
        private QuanLyKhachHang.RoundedDateTimePicker dpNgaySinh;
        private System.Windows.Forms.Label lblCCCD;
        private QuanLyKhachHang.RoundedTextBox txtCCCD;
        private System.Windows.Forms.Label lblDiaChi;
        private QuanLyKhachHang.RoundedTextBox txtDiaChi;

        private System.Windows.Forms.Label lblGuardianTitle;
        private QuanLyKhachHang.RoundedPanel pnlGuardianInfo;
        private System.Windows.Forms.Label lblGuardianInfo;
        
        private System.Windows.Forms.Label lblMaGH;
        private QuanLyKhachHang.RoundedTextBox txtMaGH;
        private System.Windows.Forms.Label lblTenGH;
        private QuanLyKhachHang.RoundedTextBox txtTenGH;
        private System.Windows.Forms.Label lblSDT_GH;
        private QuanLyKhachHang.RoundedTextBox txtSDT_GH;
        private System.Windows.Forms.Label lblEmail_GH;
        private QuanLyKhachHang.RoundedTextBox txtEmail_GH;
        private System.Windows.Forms.Label lblGioiTinh_GH;
        private QuanLyKhachHang.RoundedTextBox txtGioiTinh_GH;
        private System.Windows.Forms.Label lblNgaySinh_GH;
        private QuanLyKhachHang.RoundedDateTimePicker dpNgaySinh_GH;
        private System.Windows.Forms.Label lblCCCD_GH;
        private QuanLyKhachHang.RoundedTextBox txtCCCD_GH;
        private System.Windows.Forms.Label lblDiaChi_GH;
        private QuanLyKhachHang.RoundedTextBox txtDiaChi_GH;
        private System.Windows.Forms.Label lblQuanHe;
        private QuanLyKhachHang.RoundedTextBox txtQuanHe;

        private System.Windows.Forms.Label lblError;

        private QuanLyKhachHang.RoundedPanel pnlButtons;
        private QuanLyKhachHang.ModernButton btn1;
        private QuanLyKhachHang.ModernButton btn2;
        private QuanLyKhachHang.ModernButton btn3;
        private QuanLyKhachHang.ModernButton btn4;
        private QuanLyKhachHang.ModernButton btn5;
        private QuanLyKhachHang.ModernButton btn6;
    }
}
