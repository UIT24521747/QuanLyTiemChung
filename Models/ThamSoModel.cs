using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QuanLyKhachHang.Config;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Models
{
    public class ThamSoModel
    {
        public ThamSoDTO GetThamSo()
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT SoTuoiCanGiamHo, SoNgayHanNhap FROM THAMSO LIMIT 1", conn);
            using var r = cmd.ExecuteReader();
            if (r.Read())
                return new ThamSoDTO
                {
                    SoTuoiCanGiamHo = r.GetInt32("SoTuoiCanGiamHo"),
                    SoNgayHanNhap   = r.GetInt32("SoNgayHanNhap")
                };
            return new ThamSoDTO();
        }

        public void UpdateThamSo(ThamSoDTO dto)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(
                "UPDATE THAMSO SET SoTuoiCanGiamHo = @Tuoi, SoNgayHanNhap = @NgayHan", conn);
            cmd.Parameters.AddWithValue("@Tuoi",    dto.SoTuoiCanGiamHo);
            cmd.Parameters.AddWithValue("@NgayHan", dto.SoNgayHanNhap);
            cmd.ExecuteNonQuery();
        }

        public List<LoaiVacXinStatusDTO> GetAllLoaiVacXinWithStatus()
        {
            var list = new List<LoaiVacXinStatusDTO>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            const string sql = @"
                SELECT l.MaLoaiVacXin, l.TenLoaiVacXin,
                       EXISTS (SELECT 1 FROM VACXIN v WHERE v.MaLoaiVacXin = l.MaLoaiVacXin) AS DangSuDung
                FROM LOAIVACXIN l
                ORDER BY l.MaLoaiVacXin";
            using var r = new MySqlCommand(sql, conn).ExecuteReader();
            while (r.Read())
                list.Add(new LoaiVacXinStatusDTO
                {
                    MaLoaiVacXin  = r.GetString("MaLoaiVacXin"),
                    TenLoaiVacXin = r.GetString("TenLoaiVacXin"),
                    DangSuDung    = r.GetBoolean("DangSuDung")
                });
            return list;
        }

        public List<LoaiVacXinDTO> GetAllLoaiVacXin()
        {
            var list = new List<LoaiVacXinDTO>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var r = new MySqlCommand(
                "SELECT MaLoaiVacXin, TenLoaiVacXin FROM LOAIVACXIN ORDER BY MaLoaiVacXin", conn).ExecuteReader();
            while (r.Read())
                list.Add(new LoaiVacXinDTO
                {
                    MaLoaiVacXin  = r.GetString("MaLoaiVacXin"),
                    TenLoaiVacXin = r.GetString("TenLoaiVacXin")
                });
            return list;
        }

        public void InsertLoaiVacXin(LoaiVacXinDTO dto)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(
                "INSERT INTO LOAIVACXIN (MaLoaiVacXin, TenLoaiVacXin) VALUES (@Ma, @Ten)", conn);
            cmd.Parameters.AddWithValue("@Ma",  dto.MaLoaiVacXin);
            cmd.Parameters.AddWithValue("@Ten", dto.TenLoaiVacXin);
            cmd.ExecuteNonQuery();
        }

        public void UpdateLoaiVacXin(string ma, string newTen)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(
                "UPDATE LOAIVACXIN SET TenLoaiVacXin = @Ten WHERE MaLoaiVacXin = @Ma", conn);
            cmd.Parameters.AddWithValue("@Ten", newTen);
            cmd.Parameters.AddWithValue("@Ma",  ma);
            cmd.ExecuteNonQuery();
        }

        public void DeleteLoaiVacXin(string ma)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM LOAIVACXIN WHERE MaLoaiVacXin = @Ma", conn);
            cmd.Parameters.AddWithValue("@Ma", ma);
            cmd.ExecuteNonQuery();
        }

        public string GenerateMaLoaiVacXin()
        {
            var existing = new HashSet<string>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var r = new MySqlCommand("SELECT MaLoaiVacXin FROM LOAIVACXIN", conn).ExecuteReader();
            while (r.Read()) existing.Add(r.GetString(0));

            for (char c = 'A'; c <= 'Z'; c++)
            {
                string s = c.ToString();
                if (!existing.Contains(s)) return s;
            }
            for (char c1 = 'A'; c1 <= 'Z'; c1++)
                for (char c2 = 'A'; c2 <= 'Z'; c2++)
                {
                    string s = $"{c1}{c2}";
                    if (!existing.Contains(s)) return s;
                }
            return Guid.NewGuid().ToString("N")[..4].ToUpper();
        }
    }
}
