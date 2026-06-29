using System.Collections.Generic;
using System.Linq;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.Models;

namespace QuanLyKhachHang.Controllers
{
    public class BaoCaoController
    {
        private readonly BaoCaoModel _model = new();

        public List<BaoCaoVacXinDTO> GetBaoCaoVacXin(int nam, int thang)
        {
            var list = _model.GetBaoCaoVacXin(nam, thang);
            decimal tong = list.Sum(x => x.DoanhThu);
            foreach (var row in list)
                row.TiLe = tong > 0 ? (double)(row.DoanhThu / tong) * 100 : 0;
            return list;
        }

        public List<BaoCaoDoanhSoThangDTO> GetDoanhSoTheoThang(int nam) =>
            _model.GetDoanhSoTheoThang(nam);
    }
}
