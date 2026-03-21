using QuanLyKhachHang.Config;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using QuanLyKhachHang.DTOs;

namespace QuanLyKhachHang.Models
{
    public class KhachHangModel
    {
        public int GetSoTuoiCanGiamHo()
        {
            using (var conn = DatabaseConfig.GetConnection())
            {
                conn.Open();
                string query = "SELECT SoTuoiCanGiamHo FROM THAMSO LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 18;
            }
        }

        public void InsertKhachHang(KhachHangDTO kh, NguoiGiamHoDTO? gh)
        {
            using (var conn = DatabaseConfig.GetConnection())
            {
                conn.Open();
                using (var tr = conn.BeginTransaction())
                {
                    try
                    {
                        if (gh != null)
                        {
                            string queryGH = @"INSERT INTO NGUOIGIAMHO (MaGH, TenGH, SDT_GH, Email_GH, GioiTinh_GH, NgaySinh_GH, CCCD_GH, DiaChi_GH, QuanHe) 
                                               VALUES (@MaGH, @TenGH, @SDT_GH, @Email_GH, @GioiTinh_GH, @NgaySinh_GH, @CCCD_GH, @DiaChi_GH, @QuanHe)";
                            MySqlCommand cmdGH = new MySqlCommand(queryGH, conn, tr);
                            cmdGH.Parameters.AddWithValue("@MaGH", gh.MaGH);
                            cmdGH.Parameters.AddWithValue("@TenGH", gh.TenGH);
                            cmdGH.Parameters.AddWithValue("@SDT_GH", gh.SDT_GH);
                            cmdGH.Parameters.AddWithValue("@Email_GH", gh.Email_GH);
                            cmdGH.Parameters.AddWithValue("@GioiTinh_GH", gh.GioiTinh_GH);
                            cmdGH.Parameters.AddWithValue("@NgaySinh_GH", gh.NgaySinh_GH);
                            cmdGH.Parameters.AddWithValue("@CCCD_GH", gh.CCCD_GH);
                            cmdGH.Parameters.AddWithValue("@DiaChi_GH", gh.DiaChi_GH);
                            cmdGH.Parameters.AddWithValue("@QuanHe", gh.QuanHe);
                            cmdGH.ExecuteNonQuery();

                            kh.MaGH = gh.MaGH;
                        }

                        string queryKH = @"INSERT INTO KHACHHANG (MaKH, TenKH, SDT, Email, GioiTinh, NgaySinh, CCCD, DiaChi, MaGH) 
                                           VALUES (@MaKH, @TenKH, @SDT, @Email, @GioiTinh, @NgaySinh, @CCCD, @DiaChi, @MaGH)";
                        MySqlCommand cmdKH = new MySqlCommand(queryKH, conn, tr);
                        cmdKH.Parameters.AddWithValue("@MaKH", kh.MaKH);
                        cmdKH.Parameters.AddWithValue("@TenKH", kh.TenKH);
                        cmdKH.Parameters.AddWithValue("@SDT", kh.SDT);
                        cmdKH.Parameters.AddWithValue("@Email", kh.Email);
                        cmdKH.Parameters.AddWithValue("@GioiTinh", kh.GioiTinh);
                        cmdKH.Parameters.AddWithValue("@NgaySinh", kh.NgaySinh);
                        cmdKH.Parameters.AddWithValue("@CCCD", kh.CCCD);
                        cmdKH.Parameters.AddWithValue("@DiaChi", kh.DiaChi);
                        cmdKH.Parameters.AddWithValue("@MaGH", string.IsNullOrEmpty(kh.MaGH) ? (object)DBNull.Value : kh.MaGH);
                        cmdKH.ExecuteNonQuery();

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteKhachHang(string maKH)
        {
            using (var conn = DatabaseConfig.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM KHACHHANG WHERE MaKH = @MaKH";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKH", maKH);
                cmd.ExecuteNonQuery();
            }
        }

        public List<KhachHangDTO> GetAllKhachHang()
        {
            List<KhachHangDTO> list = new List<KhachHangDTO>();
            using (var conn = DatabaseConfig.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaKH, TenKH, SDT, Email, GioiTinh, NgaySinh, CCCD, DiaChi, MaGH FROM KHACHHANG";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new KhachHangDTO
                        {
                            MaKH = reader["MaKH"].ToString(),
                            TenKH = reader["TenKH"].ToString(),
                            SDT = reader["SDT"].ToString(),
                            Email = reader["Email"].ToString(),
                            GioiTinh = reader["GioiTinh"].ToString(),
                            NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                            CCCD = reader["CCCD"].ToString(),
                            DiaChi = reader["DiaChi"].ToString(),
                            MaGH = reader["MaGH"]?.ToString()
                        });
                    }
                }
            }
            return list;
        }

        public KhachHangDTO? GetKhachHangById(string maKH)
        {
            using (var conn = DatabaseConfig.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaKH, TenKH, SDT, Email, GioiTinh, NgaySinh, CCCD, DiaChi, MaGH FROM KHACHHANG WHERE MaKH = @MaKH";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKH", maKH);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new KhachHangDTO
                        {
                            MaKH = reader["MaKH"].ToString(),
                            TenKH = reader["TenKH"].ToString(),
                            SDT = reader["SDT"].ToString(),
                            Email = reader["Email"].ToString(),
                            GioiTinh = reader["GioiTinh"].ToString(),
                            NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                            CCCD = reader["CCCD"].ToString(),
                            DiaChi = reader["DiaChi"].ToString(),
                            MaGH = reader["MaGH"]?.ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void UpdateKhachHang(KhachHangDTO kh, NguoiGiamHoDTO? gh)
        {
            using (var conn = DatabaseConfig.GetConnection())
            {
                conn.Open();
                using (var tr = conn.BeginTransaction())
                {
                    try
                    {
                        string queryKH = @"UPDATE KHACHHANG SET TenKH=@TenKH, SDT=@SDT, Email=@Email, GioiTinh=@GioiTinh, NgaySinh=@NgaySinh, CCCD=@CCCD, DiaChi=@DiaChi, MaGH=@MaGH WHERE MaKH=@MaKH";
                        MySqlCommand cmdKH = new MySqlCommand(queryKH, conn, tr);
                        cmdKH.Parameters.AddWithValue("@MaKH", kh.MaKH);
                        cmdKH.Parameters.AddWithValue("@TenKH", kh.TenKH);
                        cmdKH.Parameters.AddWithValue("@SDT", kh.SDT);
                        cmdKH.Parameters.AddWithValue("@Email", kh.Email);
                        cmdKH.Parameters.AddWithValue("@GioiTinh", kh.GioiTinh);
                        cmdKH.Parameters.AddWithValue("@NgaySinh", kh.NgaySinh);
                        cmdKH.Parameters.AddWithValue("@CCCD", kh.CCCD);
                        cmdKH.Parameters.AddWithValue("@DiaChi", kh.DiaChi);
                        cmdKH.Parameters.AddWithValue("@MaGH", string.IsNullOrEmpty(kh.MaGH) ? (object)DBNull.Value : kh.MaGH);
                        cmdKH.ExecuteNonQuery();

                        if (gh != null)
                        {
                            string queryGH = @"UPDATE NGUOIGIAMHO SET TenGH=@TenGH, SDT_GH=@SDT_GH, Email_GH=@Email_GH, GioiTinh_GH=@GioiTinh_GH, NgaySinh_GH=@NgaySinh_GH, CCCD_GH=@CCCD_GH, DiaChi_GH=@DiaChi_GH, QuanHe=@QuanHe WHERE MaGH=@MaGH";
                            MySqlCommand cmdGH = new MySqlCommand(queryGH, conn, tr);
                            cmdGH.Parameters.AddWithValue("@MaGH", gh.MaGH);
                            cmdGH.Parameters.AddWithValue("@TenGH", gh.TenGH);
                            cmdGH.Parameters.AddWithValue("@SDT_GH", gh.SDT_GH);
                            cmdGH.Parameters.AddWithValue("@Email_GH", gh.Email_GH);
                            cmdGH.Parameters.AddWithValue("@GioiTinh_GH", gh.GioiTinh_GH);
                            cmdGH.Parameters.AddWithValue("@NgaySinh_GH", gh.NgaySinh_GH);
                            cmdGH.Parameters.AddWithValue("@CCCD_GH", gh.CCCD_GH);
                            cmdGH.Parameters.AddWithValue("@DiaChi_GH", gh.DiaChi_GH);
                            cmdGH.Parameters.AddWithValue("@QuanHe", gh.QuanHe);
                            cmdGH.ExecuteNonQuery();
                        }

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}