using System;
using System.Collections.Generic;
using System.Threading;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.Models;

namespace QuanLyKhachHang.Controllers
{
    public class VacXinController
    {
        private readonly VacXinModel _model = new VacXinModel();

        private static int _vxSeq = 0;
        public string GenerateMaVacXin() =>
            "VX" + DateTime.Now.ToString("yyMMddHHmmss") + Interlocked.Increment(ref _vxSeq).ToString("D3");

        public bool IsTenVacXinDuplicate(string ten) => _model.ExistsByTenVacXin(ten);

        public List<LoaiVacXinDTO> GetAllLoaiVacXin() => _model.GetAllLoaiVacXin();

        public List<VacXinDTO> GetAllVacXin() => _model.GetAllVacXin();

        public void ThemVacXin(VacXinDTO vx)
        {
            if (string.IsNullOrWhiteSpace(vx.TenVacXin))
                throw new Exception("Tên vắc-xin không được để trống!");
            if (string.IsNullOrWhiteSpace(vx.MaLoaiVacXin))
                throw new Exception("Phải chọn loại vắc-xin!");
            if (vx.SoMuiTiem <= 0)
                throw new Exception("Số mũi tiêm phải là số nguyên dương!");
            if (vx.KhoangCachGiuaCacMui < 0)
                throw new Exception("Khoảng cách giữa các mũi không hợp lệ!");
            _model.InsertVacXin(vx);
        }

        public void SuaVacXin(VacXinDTO vx)
        {
            if (string.IsNullOrWhiteSpace(vx.MaVacXin))
                throw new Exception("Chưa chọn vắc-xin để sửa!");
            if (string.IsNullOrWhiteSpace(vx.TenVacXin))
                throw new Exception("Tên vắc-xin không được để trống!");
            if (string.IsNullOrWhiteSpace(vx.MaLoaiVacXin))
                throw new Exception("Phải chọn loại vắc-xin!");
            if (vx.SoMuiTiem <= 0)
                throw new Exception("Số mũi tiêm phải là số nguyên dương!");
            if (vx.KhoangCachGiuaCacMui < 0)
                throw new Exception("Khoảng cách giữa các mũi không hợp lệ!");
            _model.UpdateVacXin(vx);
        }

        public void XoaVacXin(string maVacXin)
        {
            if (string.IsNullOrWhiteSpace(maVacXin))
                throw new Exception("Chưa chọn vắc-xin để xóa!");
            _model.DeleteVacXin(maVacXin);
        }
    }
}
