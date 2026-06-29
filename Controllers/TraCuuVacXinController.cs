using System.Collections.Generic;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.Models;

namespace QuanLyKhachHang.Controllers
{
    public class TraCuuVacXinController
    {
        private readonly TraCuuVacXinModel _model = new();

        public List<LoaiVacXinDTO> GetAllLoaiVacXin() => _model.GetAllLoaiVacXin();

        public Dictionary<string, string> GetMaLoHangSanXuatMap() => _model.GetMaLoHangSanXuatMap();

        public List<TraCuuVacXinDTO> TraCuu(TraCuuVacXinParams p) => _model.TraCuu(p);
    }
}
