using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QuanLyKhachHang.Config;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Models
{
    public class VacXinModel
    {
        public List<LoaiVacXinDTO> GetAllLoaiVacXin()
        {
            var list = new List<LoaiVacXinDTO>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT MaLoaiVacXin, TenLoaiVacXin FROM LOAIVACXIN ORDER BY MaLoaiVacXin", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(new LoaiVacXinDTO
                {
                    MaLoaiVacXin = reader["MaLoaiVacXin"].ToString(),
                    TenLoaiVacXin = reader["TenLoaiVacXin"].ToString()
                });
            return list;
        }

        public List<VacXinDTO> GetAllVacXin()
        {
            var list = new List<VacXinDTO>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            string query = @"SELECT v.MaVacXin, v.TenVacXin, v.MaLoaiVacXin,
                                    l.TenLoaiVacXin, v.SoMuiTiem, v.KhoangCachGiuaCacMui
                             FROM VACXIN v
                             LEFT JOIN LOAIVACXIN l ON v.MaLoaiVacXin = l.MaLoaiVacXin
                             ORDER BY v.TenVacXin";
            var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(new VacXinDTO
                {
                    MaVacXin = reader["MaVacXin"].ToString(),
                    TenVacXin = reader["TenVacXin"].ToString(),
                    MaLoaiVacXin = reader["MaLoaiVacXin"].ToString(),
                    TenLoaiVacXin = reader["TenLoaiVacXin"].ToString(),
                    SoMuiTiem = Convert.ToInt32(reader["SoMuiTiem"]),
                    KhoangCachGiuaCacMui = Convert.ToInt32(reader["KhoangCachGiuaCacMui"])
                });
            return list;
        }

        public void InsertVacXin(VacXinDTO vx)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(
                "INSERT INTO VACXIN (MaVacXin, TenVacXin, MaLoaiVacXin, SoMuiTiem, KhoangCachGiuaCacMui) VALUES (@Ma, @Ten, @Loai, @Mui, @KC)",
                conn);
            cmd.Parameters.AddWithValue("@Ma", vx.MaVacXin);
            cmd.Parameters.AddWithValue("@Ten", vx.TenVacXin);
            cmd.Parameters.AddWithValue("@Loai", vx.MaLoaiVacXin);
            cmd.Parameters.AddWithValue("@Mui", vx.SoMuiTiem);
            cmd.Parameters.AddWithValue("@KC", vx.KhoangCachGiuaCacMui);
            cmd.ExecuteNonQuery();
        }

        public void UpdateVacXin(VacXinDTO vx)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(
                "UPDATE VACXIN SET TenVacXin=@Ten, MaLoaiVacXin=@Loai, SoMuiTiem=@Mui, KhoangCachGiuaCacMui=@KC WHERE MaVacXin=@Ma",
                conn);
            cmd.Parameters.AddWithValue("@Ma", vx.MaVacXin);
            cmd.Parameters.AddWithValue("@Ten", vx.TenVacXin);
            cmd.Parameters.AddWithValue("@Loai", vx.MaLoaiVacXin);
            cmd.Parameters.AddWithValue("@Mui", vx.SoMuiTiem);
            cmd.Parameters.AddWithValue("@KC", vx.KhoangCachGiuaCacMui);
            cmd.ExecuteNonQuery();
        }

        public void DeleteVacXin(string maVacXin)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM VACXIN WHERE MaVacXin=@Ma", conn);
            cmd.Parameters.AddWithValue("@Ma", maVacXin);
            cmd.ExecuteNonQuery();
        }
    }
}
