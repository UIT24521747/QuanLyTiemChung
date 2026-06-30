using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QuanLyKhachHang.Controllers;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Views
{
    public partial class ThayDoiQuyDinhView : UserControl
    {
        private readonly ThamSoController _ctrl = new();
        private List<LoaiVacXinStatusDTO> _allLoai = new();
        private List<LoaiVacXinStatusDTO> _filtered = new();
        private int _currentPage = 1;
        private const int PageSize = 4;

        public ThayDoiQuyDinhView()
        {
            InitializeComponent();
            ShowLoaiVacXin();
            LoadLoaiVacXin();
        }

        // ── NAV ─────────────────────────────────────────────────────────────

        private void NavLoai_Click(object sender, RoutedEventArgs e) => ShowLoaiVacXin();
        private void NavThamSo_Click(object sender, RoutedEventArgs e) => ShowThamSo();

        private void ShowLoaiVacXin()
        {
            panelLoaiVacXin.Visibility = Visibility.Visible;
            panelThamSo.Visibility     = Visibility.Collapsed;
            navBorderLoai.Background   = new SolidColorBrush(Color.FromRgb(0x2D, 0x4D, 0x6B));
            navBorderThamSo.Background = Brushes.Transparent;
            SetNavTextColor(btnNavLoai,   "White");
            SetNavTextColor(btnNavThamSo, "#9AB3CB");
        }

        private void ShowThamSo()
        {
            panelLoaiVacXin.Visibility = Visibility.Collapsed;
            panelThamSo.Visibility     = Visibility.Visible;
            navBorderThamSo.Background = new SolidColorBrush(Color.FromRgb(0x2D, 0x4D, 0x6B));
            navBorderLoai.Background   = Brushes.Transparent;
            SetNavTextColor(btnNavThamSo, "White");
            SetNavTextColor(btnNavLoai,   "#9AB3CB");
            LoadThamSo();
        }

        private static void SetNavTextColor(Button btn, string colorHex)
        {
            if (btn.Content is StackPanel sp)
                foreach (var tb in sp.Children.OfType<TextBlock>())
                    tb.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString(colorHex));
        }

        // ── LOẠI VẮC-XIN ────────────────────────────────────────────────────

        private void LoadLoaiVacXin()
        {
            _allLoai = _ctrl.GetAllLoaiVacXinWithStatus();
            _currentPage = 1;
            RefreshGrid(_allLoai);

            var plain = _allLoai.Cast<LoaiVacXinDTO>().ToList();
            cboSuaLoai.ItemsSource  = plain;
            cboXoaLoai.ItemsSource  = plain;
        }

        private void RefreshGrid(List<LoaiVacXinStatusDTO> source)
        {
            _filtered    = source;
            _currentPage = Math.Max(1, Math.Min(_currentPage, TotalPages()));
            ApplyPage();
        }

        private int TotalPages() => Math.Max(1, (int)Math.Ceiling(_filtered.Count / (double)PageSize));

        private void ApplyPage()
        {
            int total = TotalPages();
            var page  = _filtered.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();
            int baseN = (_currentPage - 1) * PageSize + 1;
            int stt   = baseN;
            dgLoaiVacXin.ItemsSource = page.Select(r => new
            {
                Stt           = stt++,
                r.MaLoaiVacXin,
                r.TenLoaiVacXin
            }).ToList();

            txtPageInfo.Text    = $"Trang {_currentPage} / {total}";
            int from = _filtered.Count == 0 ? 0 : baseN;
            int to   = Math.Min(_currentPage * PageSize, _filtered.Count);
            txtPageSummary.Text = $"Hiện {from}–{to} trong {_filtered.Count} loại vắc-xin";
            btnPrevPage.IsEnabled = _currentPage > 1;
            btnNextPage.IsEnabled = _currentPage < total;
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1) { _currentPage--; ApplyPage(); }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < TotalPages()) { _currentPage++; ApplyPage(); }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = txtSearch.Text.Trim().ToLower();
            var filtered = string.IsNullOrEmpty(q)
                ? _allLoai
                : _allLoai.Where(l =>
                    (l.MaLoaiVacXin?.ToLower().Contains(q) == true) ||
                    (l.TenLoaiVacXin?.ToLower().Contains(q) == true)).ToList();
            _currentPage = 1;
            RefreshGrid(filtered);
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            ClearLoaiMsgs();
            try
            {
                _ctrl.ThemLoaiVacXin(txtTenLoaiAdd.Text);
                txtTenLoaiAdd.Text = "";
                SetMsg(txtAddMsg, "Đã thêm thành công.", "#166534");
                LoadLoaiVacXin();
            }
            catch (Exception ex) { SetMsg(txtAddMsg, ex.Message, "#B91C1C"); }
        }

        private void BtnHuyAdd_Click(object sender, RoutedEventArgs e)
        {
            txtTenLoaiAdd.Text = "";
            ClearLoaiMsgs();
        }

        private void BtnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            ClearLoaiMsgs();
            try
            {
                if (cboSuaLoai.SelectedItem is not LoaiVacXinDTO selected)
                    throw new Exception("Chưa chọn loại vắc-xin cần sửa!");
                _ctrl.SuaLoaiVacXin(selected.MaLoaiVacXin!, txtTenLoaiEdit.Text);
                txtTenLoaiEdit.Text = "";
                cboSuaLoai.SelectedItem = null;
                SetMsg(txtEditMsg, "Đã cập nhật thành công.", "#166534");
                LoadLoaiVacXin();
            }
            catch (Exception ex) { SetMsg(txtEditMsg, ex.Message, "#B91C1C"); }
        }

        private void BtnHuyEdit_Click(object sender, RoutedEventArgs e)
        {
            cboSuaLoai.SelectedItem = null;
            txtTenLoaiEdit.Text = "";
            ClearLoaiMsgs();
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            ClearLoaiMsgs();
            try
            {
                if (cboXoaLoai.SelectedItem is not LoaiVacXinDTO selected)
                    throw new Exception("Chưa chọn loại vắc-xin cần xóa!");
                _ctrl.XoaLoaiVacXin(selected.MaLoaiVacXin!);
                cboXoaLoai.SelectedItem = null;
                SetMsg(txtDeleteMsg, "Đã xóa thành công.", "#166534");
                LoadLoaiVacXin();
            }
            catch (Exception ex) { SetMsg(txtDeleteMsg, ex.Message, "#B91C1C"); }
        }

        private void BtnHuyDel_Click(object sender, RoutedEventArgs e)
        {
            cboXoaLoai.SelectedItem = null;
            ClearLoaiMsgs();
        }

        private void ClearLoaiMsgs()
        {
            txtAddMsg.Text    = "";
            txtEditMsg.Text   = "";
            txtDeleteMsg.Text = "";
            txtLoaiError.Text = "";
        }

        // ── THAM SỐ ─────────────────────────────────────────────────────────

        private void LoadThamSo()
        {
            var dto = _ctrl.GetThamSo();
            txtTuoi.Text    = dto.SoTuoiCanGiamHo.ToString();
            txtNgayHan.Text = dto.SoNgayHanNhap.ToString();
            txtThamSoMsg.Text   = "";
            txtThamSoError.Text = "";
        }

        private void BtnLuuThamSo_Click(object sender, RoutedEventArgs e)
        {
            txtThamSoMsg.Text   = "";
            txtThamSoError.Text = "";
            try
            {
                if (!int.TryParse(txtTuoi.Text.Trim(), out int tuoi))
                    throw new Exception("Tuổi phải là số nguyên!");
                if (!int.TryParse(txtNgayHan.Text.Trim(), out int ngayHan))
                    throw new Exception("Số ngày phải là số nguyên!");
                _ctrl.LuuThamSo(new ThamSoDTO { SoTuoiCanGiamHo = tuoi, SoNgayHanNhap = ngayHan });
                txtThamSoMsg.Text = "Đã cập nhật thành công.";
            }
            catch (Exception ex) { txtThamSoError.Text = ex.Message; }
        }

        private void BtnDatLai_Click(object sender, RoutedEventArgs e) => LoadThamSo();

        private void BtnTuoiUp_Click(object sender, RoutedEventArgs e)   => Increment(txtTuoi, +1);
        private void BtnTuoiDown_Click(object sender, RoutedEventArgs e) => Increment(txtTuoi, -1);
        private void BtnNgayHanUp_Click(object sender, RoutedEventArgs e)   => Increment(txtNgayHan, +1);
        private void BtnNgayHanDown_Click(object sender, RoutedEventArgs e) => Increment(txtNgayHan, -1);

        private static void Increment(TextBox tb, int delta)
        {
            if (int.TryParse(tb.Text, out int v))
                tb.Text = Math.Max(1, v + delta).ToString();
        }

        private static void SetMsg(TextBlock tb, string text, string colorHex)
        {
            tb.Text       = text;
            tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex));
        }
    }
}
