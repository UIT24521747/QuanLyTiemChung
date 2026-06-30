using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QuanLyKhachHang.Config;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Models
{
    public class PhieuNhapModel
    {
        public int GetSoNgayHanNhap()
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT SoNgayHanNhap FROM THAMSO LIMIT 1", conn);
            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 30;
        }

        public void InsertPhieuNhap(PhieuNhapDTO pn, List<LoVacXinDTO> lots)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var tr = conn.BeginTransaction();
            try
            {
                var cmdPN = new MySqlCommand(
                    "INSERT INTO PHIEUNHAP (MaPhieuNhap, NgayNhap, NhaCungCap) VALUES (@Ma, @Ngay, @NCC)",
                    conn, tr);
                cmdPN.Parameters.AddWithValue("@Ma", pn.MaPhieuNhap);
                cmdPN.Parameters.AddWithValue("@Ngay", pn.NgayNhap.Date);
                cmdPN.Parameters.AddWithValue("@NCC", pn.NhaCungCap);
                cmdPN.ExecuteNonQuery();

                foreach (var lot in lots)
                {
                    var cmdLo = new MySqlCommand(
                        @"INSERT INTO LOVACXIN (MaLo, MaVacXin, MaPhieuNhap, HangSanXuat, NgayHetHan, SoLuongNhap, SoLuongTon, DonGia)
                          VALUES (@MaLo, @MaVX, @MaPN, @HSX, @NHH, @SLN, @SLT, @DG)",
                        conn, tr);
                    cmdLo.Parameters.AddWithValue("@MaLo", lot.MaLo);
                    cmdLo.Parameters.AddWithValue("@MaVX", lot.MaVacXin);
                    cmdLo.Parameters.AddWithValue("@MaPN", pn.MaPhieuNhap);
                    cmdLo.Parameters.AddWithValue("@HSX", lot.HangSanXuat);
                    cmdLo.Parameters.AddWithValue("@NHH", lot.NgayHetHan.Date);
                    cmdLo.Parameters.AddWithValue("@SLN", lot.SoLuongNhap);
                    cmdLo.Parameters.AddWithValue("@SLT", lot.SoLuongNhap); // tồn = nhập lúc đầu
                    cmdLo.Parameters.AddWithValue("@DG", lot.DonGia);
                    cmdLo.ExecuteNonQuery();
                }

                tr.Commit();
            }
            catch
            {
                tr.Rollback();
                throw;
            }
        }

        public bool MaLoExists(string maLo)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT COUNT(*) FROM LOVACXIN WHERE MaLo=@MaLo", conn);
            cmd.Parameters.AddWithValue("@MaLo", maLo);
            return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
        }

        public List<PhieuNhapDTO> GetAllPhieuNhap()
        {
            var list = new List<PhieuNhapDTO>();
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(
                "SELECT MaPhieuNhap, NgayNhap, NhaCungCap FROM PHIEUNHAP ORDER BY NgayNhap DESC",
                conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(new PhieuNhapDTO
                {
                    MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                    NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                    NhaCungCap = reader["NhaCungCap"].ToString()
                });
            return list;
        }
    }
}
