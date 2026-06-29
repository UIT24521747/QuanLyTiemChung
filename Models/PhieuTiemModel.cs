using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QuanLyKhachHang.Config;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Models
{
    public class PhieuTiemModel
    {
        public List<KhachHangDTO> GetAllKhachHang()
        {
            var list = new List<KhachHangDTO>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                "SELECT MaKH, TenKH FROM KHACHHANG ORDER BY TenKH", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new KhachHangDTO { MaKH = r.GetString(0), TenKH = r.GetString(1) });
            return list;
        }

        public List<LoVacXinForTiemDTO> GetAllLoVacXin()
        {
            var list = new List<LoVacXinForTiemDTO>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            const string sql = @"
SELECT lo.MaLo, v.MaVacXin, v.TenVacXin, lv.TenLoaiVacXin,
       lo.HangSanXuat, lo.NgayHetHan, lo.SoLuongTon, lo.DonGia,
       v.KhoangCachGiuaCacMui
FROM LOVACXIN lo
JOIN VACXIN v      ON lo.MaVacXin     = v.MaVacXin
JOIN LOAIVACXIN lv ON v.MaLoaiVacXin  = lv.MaLoaiVacXin
ORDER BY lo.MaLo";
            using var cmd = new MySqlCommand(sql, conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new LoVacXinForTiemDTO
                {
                    MaLo                 = r.GetString(0),
                    MaVacXin             = r.GetString(1),
                    TenVacXin            = r.GetString(2),
                    TenLoaiVacXin        = r.GetString(3),
                    HangSanXuat          = r.IsDBNull(4) ? null : r.GetString(4),
                    NgayHetHan           = r.GetDateTime(5),
                    SoLuongTon           = r.GetInt32(6),
                    DonGia               = r.GetDecimal(7),
                    KhoangCachGiuaCacMui = r.GetInt32(8),
                });
            return list;
        }

        // Returns the most recent NgayTiem for (maKH, maVacXin) strictly before ngayTiem
        public DateTime? GetLastDoseDate(string maKH, string maVacXin, DateTime ngayTiem)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            const string sql = @"
SELECT MAX(pt.NgayTiem)
FROM CHITIETTIEM ct
JOIN PHIEUTIEM  pt ON ct.MaPhieuTiem = pt.MaPhieuTiem
JOIN LOVACXIN   lo ON ct.MaLo        = lo.MaLo
WHERE pt.MaKH     = @MaKH
  AND lo.MaVacXin = @MaVacXin
  AND pt.NgayTiem < @NgayTiem";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaKH",     maKH);
            cmd.Parameters.AddWithValue("@MaVacXin", maVacXin);
            cmd.Parameters.AddWithValue("@NgayTiem", ngayTiem.Date);
            var val = cmd.ExecuteScalar();
            return (val == null || val == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(val);
        }

        public void InsertPhieuTiem(PhieuTiemDTO pt, List<ChiTietTiemDTO> chiTiets)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                using (var cmd = new MySqlCommand(
                    "INSERT INTO PHIEUTIEM (MaPhieuTiem,MaKH,NgayTiem,BacSiThucHien,TongTien,GhiChu) " +
                    "VALUES (@MaPT,@MaKH,@NgayTiem,@BacSi,@TongTien,@GhiChu)", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@MaPT",     pt.MaPhieuTiem);
                    cmd.Parameters.AddWithValue("@MaKH",     pt.MaKH);
                    cmd.Parameters.AddWithValue("@NgayTiem", pt.NgayTiem.Date);
                    cmd.Parameters.AddWithValue("@BacSi",    pt.BacSiThucHien ?? "");
                    cmd.Parameters.AddWithValue("@TongTien", pt.TongTien);
                    cmd.Parameters.AddWithValue("@GhiChu",   (object?)pt.GhiChu ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }

                foreach (var ct in chiTiets)
                {
                    using var ins = new MySqlCommand(
                        "INSERT INTO CHITIETTIEM (MaPhieuTiem,MaLo,SoLuong,DonGia,ThanhTien) " +
                        "VALUES (@MaPT,@MaLo,@SL,@DG,@TT)", conn, tx);
                    ins.Parameters.AddWithValue("@MaPT", ct.MaPhieuTiem);
                    ins.Parameters.AddWithValue("@MaLo", ct.MaLo);
                    ins.Parameters.AddWithValue("@SL",   ct.SoLuong);
                    ins.Parameters.AddWithValue("@DG",   ct.DonGia);
                    ins.Parameters.AddWithValue("@TT",   ct.ThanhTien);
                    ins.ExecuteNonQuery();

                    using var upd = new MySqlCommand(
                        "UPDATE LOVACXIN SET SoLuongTon = SoLuongTon - @SL WHERE MaLo = @MaLo",
                        conn, tx);
                    upd.Parameters.AddWithValue("@SL",   ct.SoLuong);
                    upd.Parameters.AddWithValue("@MaLo", ct.MaLo);
                    upd.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}
