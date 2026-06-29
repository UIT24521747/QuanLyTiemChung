using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QuanLyKhachHang.Config;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Models
{
    public class BaoCaoModel
    {
        public List<BaoCaoVacXinDTO> GetBaoCaoVacXin(int nam, int thang)
        {
            var list = new List<BaoCaoVacXinDTO>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            const string sql = @"
SELECT v.TenVacXin, lv.TenLoaiVacXin,
       SUM(ct.SoLuong)   AS TongLuotTiem,
       SUM(ct.ThanhTien) AS DoanhThu
FROM CHITIETTIEM ct
JOIN PHIEUTIEM  pt ON ct.MaPhieuTiem  = pt.MaPhieuTiem
JOIN LOVACXIN   lo ON ct.MaLo         = lo.MaLo
JOIN VACXIN     v  ON lo.MaVacXin     = v.MaVacXin
JOIN LOAIVACXIN lv ON v.MaLoaiVacXin  = lv.MaLoaiVacXin
WHERE YEAR(pt.NgayTiem) = @Nam AND MONTH(pt.NgayTiem) = @Thang
GROUP BY v.MaVacXin, v.TenVacXin, lv.TenLoaiVacXin
ORDER BY DoanhThu DESC";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nam",   nam);
            cmd.Parameters.AddWithValue("@Thang", thang);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new BaoCaoVacXinDTO
                {
                    TenVacXin     = r.GetString(0),
                    TenLoaiVacXin = r.GetString(1),
                    TongLuotTiem  = r.GetInt32(2),
                    DoanhThu      = r.GetDecimal(3),
                });
            return list;
        }

        public List<BaoCaoDoanhSoThangDTO> GetDoanhSoTheoThang(int nam)
        {
            var map = new Dictionary<int, decimal>(); // Thang → DoanhThu
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            const string sql = @"
SELECT MONTH(pt.NgayTiem) AS Thang, SUM(ct.ThanhTien) AS DoanhThu
FROM CHITIETTIEM ct
JOIN PHIEUTIEM pt ON ct.MaPhieuTiem = pt.MaPhieuTiem
WHERE YEAR(pt.NgayTiem) = @Nam
GROUP BY MONTH(pt.NgayTiem)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nam", nam);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                map[r.GetInt32(0)] = r.GetDecimal(1);

            var list = new List<BaoCaoDoanhSoThangDTO>();
            for (int t = 1; t <= 12; t++)
            {
                decimal cur  = map.GetValueOrDefault(t, 0);
                decimal prev = map.GetValueOrDefault(t - 1, 0);
                string soSanh = "—";
                if (t > 1 && prev > 0)
                    soSanh = $"{(double)(cur / prev) * 100:F1}%";
                list.Add(new BaoCaoDoanhSoThangDTO
                {
                    Thang           = t,
                    DoanhThu        = cur,
                    SoVoiThangTruoc = soSanh,
                });
            }
            return list;
        }
    }
}
