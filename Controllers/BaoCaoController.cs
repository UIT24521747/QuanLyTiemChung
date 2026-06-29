using System.Collections.Generic;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.Models;

namespace QuanLyKhachHang.Controllers
{
    public class BaoCaoController
    {
        private readonly BaoCaoModel _model = new();

        public List<BaoCaoVacXinDTO> GetBaoCaoVacXin(int nam, int thang) =>
            _model.GetBaoCaoVacXin(nam, thang);

        public List<BaoCaoThangDTO> GetBaoCaoThang(int nam) =>
            _model.GetBaoCaoThang(nam);
    }
}
