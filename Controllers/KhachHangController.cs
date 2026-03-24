using System;
using System.Collections.Generic;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.Models;

namespace QuanLyKhachHang.Controllers
{
    public class KhachHangController
    {
        private KhachHangModel _model = new KhachHangModel();

        public string GenerateMaKH() => "KH" + DateTime.Now.ToString("yyMMddHHmmss");
        public string GenerateMaGH() => "GH" + DateTime.Now.ToString("yyMMddHHmmss");

        public void LuuKhachHang(KhachHangDTO kh, NguoiGiamHoDTO? gh, bool isUpdate = false)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(kh.TenKH))
                throw new Exception("Họ tên khách hàng không được để trống!");
            if (string.IsNullOrWhiteSpace(kh.SDT))
                throw new Exception("Số điện thoại không được để trống!");
            if (string.IsNullOrWhiteSpace(kh.CCCD))
                throw new Exception("CCCD không được để trống!");
            if (string.IsNullOrWhiteSpace(kh.DiaChi))
                throw new Exception("Địa chỉ không được để trống!");

            int age = DateTime.Now.Year - kh.NgaySinh.Year;
            if (kh.NgaySinh.Date > DateTime.Now.AddYears(-age)) age--;

            int tuoiQuyDinh = _model.GetSoTuoiCanGiamHo();

            if (age < tuoiQuyDinh)
            {
                if (gh == null || string.IsNullOrWhiteSpace(gh.TenGH))
                    throw new Exception($"Quy định: Khách hàng dưới {tuoiQuyDinh} tuổi bắt buộc phải có thông tin người giám hộ!");

                if (string.IsNullOrWhiteSpace(gh.MaGH))
                    gh.MaGH = kh.MaGH;

                kh.MaGH = gh.MaGH;
            }
            else
            {
                gh = null;
                kh.MaGH = null;
            }

            if (isUpdate)
            {
                _model.UpdateKhachHang(kh, gh);
            }
            else
            {
                _model.InsertKhachHang(kh, gh);
            }
        }

        public void XoaKhachHang(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new Exception("Mã khách hàng không được để trống!");
            _model.DeleteKhachHang(maKH);
        }

        public List<KhachHangDTO> GetAllKhachHang()
        {
            return _model.GetAllKhachHang();
        }

        public KhachHangDTO? GetKhachHangById(string maKH)
        {
            return _model.GetKhachHangById(maKH);
        }

        public (KhachHangDTO? KhachHang, NguoiGiamHoDTO? NguoiGiamHo) GetKhachHangWithNguoiGiamHoById(string maKH)
        {
            return _model.GetKhachHangWithNguoiGiamHoById(maKH);
        }
    }
}