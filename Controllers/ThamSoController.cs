using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.Models;

namespace QuanLyKhachHang.Controllers
{
    public class ThamSoController
    {
        private readonly ThamSoModel _model = new();

        public ThamSoDTO GetThamSo() => _model.GetThamSo();

        public void LuuThamSo(ThamSoDTO dto)
        {
            if (dto.SoTuoiCanGiamHo <= 0)
                throw new Exception("Tuổi cần người giám hộ phải là số nguyên dương!");
            if (dto.SoNgayHanNhap <= 0)
                throw new Exception("Số ngày hạn nhập phải là số nguyên dương!");

            _model.UpdateThamSo(dto);
        }

        public (int violateKH, int violateLo) CheckThamSoViolations(ThamSoDTO dto) =>
            (_model.CountKhachHangViolateGiamHo(dto.SoTuoiCanGiamHo),
             _model.CountLoVacXinViolateHanNhap(dto.SoNgayHanNhap));

        public List<LoaiVacXinStatusDTO> GetAllLoaiVacXinWithStatus() =>
            _model.GetAllLoaiVacXinWithStatus();

        public List<LoaiVacXinDTO> GetAllLoaiVacXin() => _model.GetAllLoaiVacXin();

        public void ThemLoaiVacXin(string tenLoai)
        {
            if (string.IsNullOrWhiteSpace(tenLoai))
                throw new Exception("Tên loại vắc-xin không được để trống!");
            string ma = _model.GenerateMaLoaiVacXin();
            _model.InsertLoaiVacXin(new LoaiVacXinDTO { MaLoaiVacXin = ma, TenLoaiVacXin = tenLoai.Trim() });
        }

        public void SuaLoaiVacXin(string ma, string newTen)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new Exception("Chưa chọn loại vắc-xin để sửa!");
            if (string.IsNullOrWhiteSpace(newTen))
                throw new Exception("Tên loại vắc-xin mới không được để trống!");
            _model.UpdateLoaiVacXin(ma, newTen.Trim());
        }

        public void XoaLoaiVacXin(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new Exception("Chưa chọn loại vắc-xin để xóa!");
            var status = _model.GetAllLoaiVacXinWithStatus().FirstOrDefault(l => l.MaLoaiVacXin == ma);
            if (status?.DangSuDung == true)
                throw new Exception("Không thể xóa: loại vắc-xin này đang có vắc-xin thuộc loại. Hãy chuyển hoặc xóa các vắc-xin đó trước!");
            _model.DeleteLoaiVacXin(ma);
        }
    }
}
