using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyKhachHang.DTOs;
using QuanLyKhachHang.Models;

namespace QuanLyKhachHang.Controllers
{
    public class PhieuTiemController
    {
        private readonly PhieuTiemModel _model = new();

        public string GenerateMaPhieuTiem() => "PT" + DateTime.Now.ToString("yyMMddHHmmssfff");

        public List<KhachHangDTO> GetAllKhachHang() => _model.GetAllKhachHang();

        public List<LoVacXinForTiemDTO> GetAllLoVacXin() => _model.GetAllLoVacXin();

        public void LuuPhieuTiem(PhieuTiemDTO pt, List<ChiTietTiemDTO> chiTiets,
                                  List<LoVacXinForTiemDTO> loDetails)
        {
            if (string.IsNullOrWhiteSpace(pt.MaKH))
                throw new Exception("Phải chọn khách hàng!");
            if (chiTiets.Count == 0)
                throw new Exception("Phải có ít nhất một lô vắc-xin!");

            // Build lookup from MaLo → detail
            var lookup = loDetails.ToDictionary(l => l.MaLo!);

            // Merge duplicate MaLo rows by summing SoLuong
            var merged = chiTiets
                .GroupBy(ct => ct.MaLo)
                .Select(g =>
                {
                    var totalQty = g.Sum(ct => ct.SoLuong);
                    var lo = lookup.TryGetValue(g.Key!, out var d) ? d : null;
                    return new ChiTietTiemDTO
                    {
                        MaPhieuTiem = pt.MaPhieuTiem,
                        MaLo        = g.Key,
                        SoLuong     = totalQty,
                        DonGia      = lo?.DonGia ?? 0,
                        ThanhTien   = totalQty * (lo?.DonGia ?? 0)
                    };
                })
                .ToList();

            foreach (var ct in merged)
            {
                if (!lookup.TryGetValue(ct.MaLo!, out var lo))
                    throw new Exception($"Lô '{ct.MaLo}' không tồn tại!");

                if (ct.SoLuong <= 0)
                    throw new Exception($"Lô {ct.MaLo}: số lượng phải > 0!");

                if (ct.SoLuong > lo.SoLuongTon)
                    throw new Exception(
                        $"Lô {ct.MaLo}: số lượng đăng ký ({ct.SoLuong}) vượt tồn kho ({lo.SoLuongTon})!");

                if (pt.NgayTiem.Date > lo.NgayHetHan.Date)
                    throw new Exception(
                        $"Lô {ct.MaLo}: ngày tiêm ({pt.NgayTiem:dd/MM/yyyy}) sau ngày hết hạn ({lo.NgayHetHan:dd/MM/yyyy})!");

                // Dose interval check
                var lastDose = _model.GetLastDoseDate(pt.MaKH!, lo.MaVacXin!, pt.NgayTiem);
                if (lastDose.HasValue)
                {
                    int daysSince = (pt.NgayTiem.Date - lastDose.Value.Date).Days;
                    if (daysSince < lo.KhoangCachGiuaCacMui)
                        throw new Exception(
                            $"Lô {ct.MaLo} ({lo.TenVacXin}): khoảng cách mũi tiêm chưa đủ " +
                            $"(cần {lo.KhoangCachGiuaCacMui} ngày, còn {lo.KhoangCachGiuaCacMui - daysSince} ngày nữa)!");
                }
            }

            _model.InsertPhieuTiem(pt, merged);
        }
    }
}
