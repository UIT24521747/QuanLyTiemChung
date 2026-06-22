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
    public partial class PhieuNhapView : UserControl
    {
        private readonly PhieuNhapController _controller = new PhieuNhapController();
        private readonly ObservableCollection<LoVacXinRowVM> _rows = new();
        private List<VacXinDTO> _vacXinList = new();

        public PhieuNhapView()
        {
            InitializeComponent();
            dgLots.ItemsSource = _rows;

            // Subscribe: collection add/remove → refresh row numbers
            // Each row's PropertyChanged → only recompute TongTien (NOT row numbers — would loop)
            _rows.CollectionChanged += (_, e) =>
            {
                if (e.NewItems != null)
                    foreach (LoVacXinRowVM r in e.NewItems)
                        r.PropertyChanged += (_, _) => UpdateTongTien();
                RefreshRowNumbers();
                UpdateTongTien();
            };

            // Process 2,7: Nạp danh sách Vắc-xin on load
            LoadVacXin();
            // Process 1,5: auto codes + today's date
            ResetPhieu();
        }

        private void LoadVacXin()
        {
            try
            {
                _vacXinList = _controller.GetAllVacXin();
                colVacXin.ItemsSource = _vacXinList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục vắc-xin: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetPhieu()
        {
            txtMaPN.Text = _controller.GenerateMaPhieuNhap();  // Process 1
            dpNgayNhap.SelectedDate = DateTime.Today;           // Process 5
            txtNhaCungCap.Text = "";
            _rows.Clear();
            for (int i = 0; i < 4; i++) _rows.Add(new LoVacXinRowVM());
            txtTongTien.Text = "";
            txtPnError.Text = "";
        }

        private void RefreshRowNumbers()
        {
            for (int i = 0; i < _rows.Count; i++)
                _rows[i].RowNum = i + 1;
        }

        // Process 6: Tổng giá trị = sum(ThanhTien) — auto recomputed
        private void UpdateTongTien()
        {
            decimal tong = _rows.Sum(r => r.ThanhTien);
            txtTongTien.Text = tong > 0 ? tong.ToString("N0") : "";
        }

        // Trigger TongTien update after each cell edit (Process 10 → 6 chain)
        private void DgLots_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Dispatcher.InvokeAsync(UpdateTongTien);
        }

        // Delete row button
        private void BtnDeleteLotRow_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is LoVacXinRowVM row)
            {
                _rows.Remove(row);
                RefreshRowNumbers();
                UpdateTongTien();
            }
        }

        // Process 11: Lập phiếu nhập Vắc-xin (MAIN — nghiệp vụ)
        private void BtnLapPhieu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtPnError.Text = "";
                dgLots.CommitEdit(DataGridEditingUnit.Row, true);

                if (dpNgayNhap.SelectedDate == null) throw new Exception("Phải chọn ngày nhập!");

                var pn = new PhieuNhapDTO
                {
                    MaPhieuNhap = txtMaPN.Text.Trim(),
                    NgayNhap    = dpNgayNhap.SelectedDate.Value,
                    NhaCungCap  = txtNhaCungCap.Text.Trim()
                };

                var lots = new List<LoVacXinDTO>();
                var vacLookup = _vacXinList.ToDictionary(v => v.MaVacXin ?? "");

                foreach (var row in _rows.Where(r => !r.IsEmpty))
                {
                    if (row.NgayHetHan == null) throw new Exception($"Hàng {row.RowNum}: chưa nhập ngày hết hạn!");
                    if (row.SoLuongNhap <= 0)   throw new Exception($"Hàng {row.RowNum}: số lượng phải > 0!");
                    if (row.DonGia < 0)          throw new Exception($"Hàng {row.RowNum}: đơn giá không hợp lệ!");

                    string tenVacXin = vacLookup.TryGetValue(row.MaVacXin ?? "", out var vx) ? vx.TenVacXin ?? "" : "";

                    lots.Add(new LoVacXinDTO
                    {
                        MaLo        = _controller.GenerateMaLo(),
                        MaVacXin    = row.MaVacXin,
                        TenVacXin   = tenVacXin,
                        NgayHetHan  = row.NgayHetHan.Value,
                        HangSanXuat = row.HangSanXuat ?? "",
                        SoLuongNhap = row.SoLuongNhap,
                        SoLuongTon  = row.SoLuongNhap,
                        DonGia      = row.DonGia
                    });
                }

                _controller.LuuPhieuNhap(pn, lots);
                MessageBox.Show("Lập phiếu nhập thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetPhieu();
            }
            catch (Exception ex)
            {
                txtPnError.Text = ex.Message;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Process 12: Tra cứu Vắc-xin — shows existing vaccines in popup
        private void BtnTraCuuVX_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var popup = new Window
                {
                    Title = "Tra cứu vắc-xin",
                    Width = 680, Height = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Window.GetWindow(this)
                };
                var dg = new DataGrid { AutoGenerateColumns = false, IsReadOnly = true, ItemsSource = _vacXinList, Margin = new Thickness(10) };
                dg.Columns.Add(new DataGridTextColumn { Header = "Mã VX",    Binding = new System.Windows.Data.Binding("MaVacXin"),             Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                dg.Columns.Add(new DataGridTextColumn { Header = "Tên",       Binding = new System.Windows.Data.Binding("TenVacXin"),            Width = new DataGridLength(2, DataGridLengthUnitType.Star) });
                dg.Columns.Add(new DataGridTextColumn { Header = "Loại",      Binding = new System.Windows.Data.Binding("TenLoaiVacXin"),        Width = new DataGridLength(80) });
                dg.Columns.Add(new DataGridTextColumn { Header = "Số mũi",    Binding = new System.Windows.Data.Binding("SoMuiTiem"),            Width = new DataGridLength(70) });
                dg.Columns.Add(new DataGridTextColumn { Header = "KC (ngày)", Binding = new System.Windows.Data.Binding("KhoangCachGiuaCacMui"), Width = new DataGridLength(90) });
                popup.Content = dg;
                popup.ShowDialog();
            }
            catch (Exception ex) { txtPnError.Text = ex.Message; }
        }

        // Làm mới
        private void BtnLamMoi_Click(object sender, RoutedEventArgs e) => ResetPhieu();

        // Process 13: Thoát
        private void BtnThoat_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
