using System;
using System.Collections.Generic;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.Models;

namespace QuanLyKhachHang.Controllers
{
    public class PhieuNhapController
    {
        private readonly PhieuNhapModel _model = new PhieuNhapModel();
        private readonly VacXinModel _vacXinModel = new VacXinModel();

        public string GenerateMaPhieuNhap() => "PN" + DateTime.Now.ToString("yyMMddHHmmss");

        public string GenerateMaLo() => "LO" + DateTime.Now.ToString("yyMMddHHmmssfff");

        public List<VacXinDTO> GetAllVacXin() => _vacXinModel.GetAllVacXin();

        public List<PhieuNhapDTO> GetAllPhieuNhap() => _model.GetAllPhieuNhap();

        public void LuuPhieuNhap(PhieuNhapDTO pn, List<LoVacXinDTO> lots)
        {
            if (string.IsNullOrWhiteSpace(pn.NhaCungCap))
                throw new Exception("Nhà cung cấp không được để trống!");
            if (lots.Count == 0)
                throw new Exception("Phải có ít nhất một lô vắc-xin!");

            int soNgayHan = _model.GetSoNgayHanNhap();
            foreach (var lot in lots)
            {
                int diff = (lot.NgayHetHan.Date - pn.NgayNhap.Date).Days;
                if (diff <= soNgayHan)
                    throw new Exception(
                        $"Lô {lot.MaLo} ({lot.TenVacXin}): ngày hết hạn phải cách ngày nhập hơn {soNgayHan} ngày (hiện tại: {diff} ngày)!");
            }

            _model.InsertPhieuNhap(pn, lots);
        }
    }
}
