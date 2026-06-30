using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QuanLyKhachHang.Controllers;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.ViewModels;

namespace QuanLyKhachHang.Views
{
    public partial class VacXinView : UserControl
    {
        private readonly VacXinController _controller = new VacXinController();
        private readonly ObservableCollection<VacXinRowVM> _rows = new();
        public List<LoaiVacXinDTO> LoaiVacXinList { get; private set; } = new();

        public VacXinView()
        {
            InitializeComponent();
            dgVacXin.ItemsSource = _rows;
            _rows.CollectionChanged += (_, _) => RefreshRowNumbers();

            // Process 1,2,3,4: load data on screen open
            LoadLoaiVacXin();
            // Seed 4 empty rows matching spec
            for (int i = 0; i < 4; i++) AddEmptyRow();
        }

        private void LoadLoaiVacXin()
        {
            try { LoaiVacXinList = _controller.GetAllLoaiVacXin(); }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải loại vắc-xin: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddEmptyRow()
        {
            var row = new VacXinRowVM { RowNum = _rows.Count + 1 };
            _rows.Add(row);
        }

        private void RefreshRowNumbers()
        {
            for (int i = 0; i < _rows.Count; i++)
                _rows[i].RowNum = i + 1;
        }

        // Process 5: Xóa per-row (chất lượng)
        private void BtnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is VacXinRowVM row)
            {
                _rows.Remove(row);
                RefreshRowNumbers();
            }
        }

        // Process 6: Thêm danh sách Vắc-xin (MAIN — nghiệp vụ)
        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtVxError.Text = "";
                // Commit any pending edit
                dgVacXin.CommitEdit(DataGridEditingUnit.Row, true);

                var toSave = _rows.Where(r => !r.IsEmpty).ToList();
                if (toSave.Count == 0) { txtVxError.Text = "Chưa nhập thông tin vắc-xin!"; return; }

                var dupNames = toSave
                    .Where(r => !string.IsNullOrWhiteSpace(r.TenVacXin) && _controller.IsTenVacXinDuplicate(r.TenVacXin))
                    .Select(r => r.TenVacXin)
                    .ToList();
                if (dupNames.Count > 0)
                {
                    string list = string.Join("\n  • ", dupNames);
                    var result = MessageBox.Show(
                        $"Các vắc-xin sau đã tồn tại trong hệ thống:\n  • {list}\n\nVẫn tiếp tục thêm?",
                        "Tên trùng", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No) return;
                }

                int saved = 0, skipped = 0;
                foreach (var row in toSave)
                {
                    try
                    {
                        _controller.ThemVacXin(new VacXinDTO
                        {
                            MaVacXin             = _controller.GenerateMaVacXin(),
                            TenVacXin            = row.TenVacXin,
                            MaLoaiVacXin         = row.MaLoaiVacXin,
                            SoMuiTiem            = 1,
                            KhoangCachGiuaCacMui = row.KhoangCach,
                            TenLoaiVacXin        = row.TenLoaiVacXin
                        });
                        saved++;
                    }
                    catch (Exception ex)
                    {
                        skipped++;
                        txtVxError.Text += $"• {row.TenVacXin}: {ex.Message}\n";
                    }
                }

                string msg = skipped == 0
                    ? $"Đã thêm {saved} vắc-xin thành công!"
                    : $"Đã thêm {saved} vắc-xin. {skipped} dòng lỗi (xem bên dưới).";
                MessageBox.Show(msg, "Kết quả", MessageBoxButton.OK,
                    skipped == 0 ? MessageBoxImage.Information : MessageBoxImage.Warning);

                if (saved > 0) ResetGrid();
            }
            catch (Exception ex) { txtVxError.Text = ex.Message; }
        }

        private void ResetGrid()
        {
            _rows.Clear();
            for (int i = 0; i < 4; i++) AddEmptyRow();
            txtVxError.Text = "";
        }

        // Process 7: Tra cứu Vắc-xin (chất lượng) — shows existing vaccines in popup
        private void BtnTraCuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var all = _controller.GetAllVacXin();
                var popup = new Window
                {
                    Title = "Danh sách vắc-xin hiện có",
                    Width = 700, Height = 420,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Window.GetWindow(this)
                };
                var dg = new DataGrid
                {
                    AutoGenerateColumns = false,
                    IsReadOnly = true,
                    ItemsSource = all,
                    Margin = new Thickness(10)
                };
                dg.Columns.Add(new DataGridTextColumn { Header = "Mã VX",    Binding = new System.Windows.Data.Binding("MaVacXin"),             Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                dg.Columns.Add(new DataGridTextColumn { Header = "Tên",       Binding = new System.Windows.Data.Binding("TenVacXin"),            Width = new DataGridLength(2, DataGridLengthUnitType.Star) });
                dg.Columns.Add(new DataGridTextColumn { Header = "Loại",      Binding = new System.Windows.Data.Binding("TenLoaiVacXin"),        Width = new DataGridLength(80) });
                dg.Columns.Add(new DataGridTextColumn { Header = "KC (ngày)", Binding = new System.Windows.Data.Binding("KhoangCachGiuaCacMui"), Width = new DataGridLength(90) });
                popup.Content = dg;
                popup.ShowDialog();
            }
            catch (Exception ex) { txtVxError.Text = ex.Message; }
        }

        // Process 8: Thoát
        private void BtnThoat_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
