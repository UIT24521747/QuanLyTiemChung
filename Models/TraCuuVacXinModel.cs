using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using QuanLyKhachHang.Config;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Models
{
    public class TraCuuVacXinModel
    {
        public List<LoaiVacXinDTO> GetAllLoaiVacXin()
        {
            var list = new List<LoaiVacXinDTO>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT MaLoaiVacXin, TenLoaiVacXin FROM LOAIVACXIN ORDER BY MaLoaiVacXin", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new LoaiVacXinDTO { MaLoaiVacXin = r.GetString(0), TenLoaiVacXin = r.GetString(1) });
            return list;
        }

        public Dictionary<string, string> GetMaLoHangSanXuatMap()
        {
            var map = new Dictionary<string, string>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT MaLo, HangSanXuat FROM LOVACXIN ORDER BY MaLo", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                map[r.GetString(0)] = r.IsDBNull(1) ? "" : r.GetString(1);
            return map;
        }

        public List<TraCuuVacXinDTO> TraCuu(TraCuuVacXinParams p)
        {
            var conds = new List<string>();
            var prms = new List<(string, object?)> ();

            void Add(string cond, string name, object? val) { conds.Add(cond); prms.Add((name, val)); }

            if (!string.IsNullOrWhiteSpace(p.MaVacXin))    Add("v.MaVacXin LIKE @MaVacXin",           "@MaVacXin",      $"%{p.MaVacXin!.Trim()}%");
            if (!string.IsNullOrWhiteSpace(p.TenVacXin))   Add("v.TenVacXin LIKE @TenVacXin",         "@TenVacXin",     $"%{p.TenVacXin!.Trim()}%");
            if (!string.IsNullOrWhiteSpace(p.MaLoaiVacXin))Add("v.MaLoaiVacXin = @MaLoaiVacXin",      "@MaLoaiVacXin",  p.MaLoaiVacXin);
            if (p.KhoangCachTu.HasValue)                    Add("v.KhoangCachGiuaCacMui >= @KCTu",      "@KCTu",          p.KhoangCachTu.Value);
            if (p.KhoangCachDen.HasValue)                   Add("v.KhoangCachGiuaCacMui <= @KCDen",     "@KCDen",         p.KhoangCachDen.Value);
            if (!string.IsNullOrWhiteSpace(p.MaLo))        Add("lo.MaLo = @MaLo",                     "@MaLo",          p.MaLo);
            if (!string.IsNullOrWhiteSpace(p.HangSanXuat)) Add("lo.HangSanXuat LIKE @HangSanXuat",     "@HangSanXuat",   $"%{p.HangSanXuat!.Trim()}%");
            if (p.SoLuongNhapTu.HasValue)                   Add("lo.SoLuongNhap >= @SLNhapTu",         "@SLNhapTu",      p.SoLuongNhapTu.Value);
            if (p.SoLuongNhapDen.HasValue)                  Add("lo.SoLuongNhap <= @SLNhapDen",        "@SLNhapDen",     p.SoLuongNhapDen.Value);
            if (p.DonGiaTu.HasValue)                        Add("lo.DonGia >= @DonGiaTu",              "@DonGiaTu",      p.DonGiaTu.Value);
            if (p.DonGiaDen.HasValue)                       Add("lo.DonGia <= @DonGiaDen",             "@DonGiaDen",     p.DonGiaDen.Value);
            if (p.NgayHetHanTu.HasValue)                    Add("lo.NgayHetHan >= @NHHTu",             "@NHHTu",         p.NgayHetHanTu.Value.Date);
            if (p.NgayHetHanDen.HasValue)                   Add("lo.NgayHetHan <= @NHHDen",            "@NHHDen",        p.NgayHetHanDen.Value.Date);
            if (p.SoLuongTonTu.HasValue)                    Add("lo.SoLuongTon >= @SLTonTu",           "@SLTonTu",       p.SoLuongTonTu.Value);
            if (p.SoLuongTonDen.HasValue)                   Add("lo.SoLuongTon <= @SLTonDen",          "@SLTonDen",      p.SoLuongTonDen.Value);
            if (!string.IsNullOrWhiteSpace(p.NhaCungCap))   Add("pn.NhaCungCap LIKE @NhaCungCap",      "@NhaCungCap",    $"%{p.NhaCungCap!.Trim()}%");
            if (!string.IsNullOrWhiteSpace(p.MaPhieuNhapTu))Add("pn.MaPhieuNhap >= @MPNTu",           "@MPNTu",         p.MaPhieuNhapTu);
            if (!string.IsNullOrWhiteSpace(p.MaPhieuNhapDen))Add("pn.MaPhieuNhap <= @MPNDen",         "@MPNDen",        p.MaPhieuNhapDen);
            if (p.NgayNhapTu.HasValue)                      Add("pn.NgayNhap >= @NgayNhapTu",          "@NgayNhapTu",    p.NgayNhapTu.Value.Date);
            if (p.NgayNhapDen.HasValue)                     Add("pn.NgayNhap <= @NgayNhapDen",         "@NgayNhapDen",   p.NgayNhapDen.Value.Date);

            var sql = new StringBuilder(@"
SELECT v.MaVacXin, v.TenVacXin, lv.MaLoaiVacXin, lv.TenLoaiVacXin,
       v.KhoangCachGiuaCacMui,
       lo.MaLo, lo.HangSanXuat, lo.NgayHetHan, lo.SoLuongNhap, lo.SoLuongTon, lo.DonGia,
       pn.MaPhieuNhap, pn.NgayNhap, pn.NhaCungCap
FROM VACXIN v
LEFT JOIN LOAIVACXIN lv ON v.MaLoaiVacXin = lv.MaLoaiVacXin
LEFT JOIN LOVACXIN lo   ON v.MaVacXin = lo.MaVacXin
LEFT JOIN PHIEUNHAP pn  ON lo.MaPhieuNhap = pn.MaPhieuNhap");

            if (conds.Count > 0)
                sql.Append(" WHERE " + string.Join(" AND ", conds));

            sql.Append(" ORDER BY v.TenVacXin, lo.MaLo");

            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql.ToString(), conn);
            foreach (var (name, val) in prms)
                cmd.Parameters.AddWithValue(name, val ?? DBNull.Value);

            var list = new List<TraCuuVacXinDTO>();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new TraCuuVacXinDTO
                {
                    MaVacXin             = r.IsDBNull(0)  ? null : r.GetString(0),
                    TenVacXin            = r.IsDBNull(1)  ? null : r.GetString(1),
                    MaLoaiVacXin         = r.IsDBNull(2)  ? null : r.GetString(2),
                    TenLoaiVacXin        = r.IsDBNull(3)  ? null : r.GetString(3),
                    KhoangCachGiuaCacMui = r.IsDBNull(4)  ? 0    : r.GetInt32(4),
                    MaLo                 = r.IsDBNull(5)  ? null : r.GetString(5),
                    HangSanXuat          = r.IsDBNull(6)  ? null : r.GetString(6),
                    NgayHetHan           = r.IsDBNull(7)  ? null : r.GetDateTime(7),
                    SoLuongNhap          = r.IsDBNull(8)  ? 0    : r.GetInt32(8),
                    SoLuongTon           = r.IsDBNull(9)  ? 0    : r.GetInt32(9),
                    DonGia               = r.IsDBNull(10) ? 0    : r.GetDecimal(10),
                    MaPhieuNhap          = r.IsDBNull(11) ? null : r.GetString(11),
                    NgayNhap             = r.IsDBNull(12) ? null : r.GetDateTime(12),
                    NhaCungCap           = r.IsDBNull(13) ? null : r.GetString(13),
                });
            }
            return list;
        }
    }
}
