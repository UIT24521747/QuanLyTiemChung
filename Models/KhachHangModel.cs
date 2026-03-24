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

        public (KhachHangDTO? KhachHang, NguoiGiamHoDTO? NguoiGiamHo) GetKhachHangWithNguoiGiamHoById(string maKH)
        {
            using (var conn = DatabaseConfig.GetConnection())
            {
                conn.Open();
                string query = @"SELECT 
                                    KH.MaKH, KH.TenKH, KH.SDT, KH.Email, KH.GioiTinh, KH.NgaySinh, KH.CCCD, KH.DiaChi, KH.MaGH,
                                    GH.TenGH, GH.SDT_GH, GH.Email_GH, GH.GioiTinh_GH, GH.NgaySinh_GH, GH.CCCD_GH, GH.DiaChi_GH, GH.QuanHe
                                 FROM KHACHHANG KH
                                 LEFT JOIN NGUOIGIAMHO GH ON KH.MaGH = GH.MaGH
                                 WHERE KH.MaKH = @MaKH";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKH", maKH);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return (null, null);
                    }

                    var kh = new KhachHangDTO
                    {
                        MaKH = reader["MaKH"].ToString(),
                        TenKH = reader["TenKH"].ToString(),
                        SDT = reader["SDT"].ToString(),
                        Email = reader["Email"].ToString(),
                        GioiTinh = reader["GioiTinh"].ToString(),
                        NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                        CCCD = reader["CCCD"].ToString(),
                        DiaChi = reader["DiaChi"].ToString(),
                        MaGH = reader["MaGH"] == DBNull.Value ? null : reader["MaGH"].ToString()
                    };

                    NguoiGiamHoDTO? gh = null;
                    if (reader["MaGH"] != DBNull.Value)
                    {
                        gh = new NguoiGiamHoDTO
                        {
                            MaGH = reader["MaGH"].ToString(),
                            TenGH = reader["TenGH"] == DBNull.Value ? null : reader["TenGH"].ToString(),
                            SDT_GH = reader["SDT_GH"] == DBNull.Value ? null : reader["SDT_GH"].ToString(),
                            Email_GH = reader["Email_GH"] == DBNull.Value ? null : reader["Email_GH"].ToString(),
                            GioiTinh_GH = reader["GioiTinh_GH"] == DBNull.Value ? null : reader["GioiTinh_GH"].ToString(),
                            NgaySinh_GH = reader["NgaySinh_GH"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgaySinh_GH"]),
                            CCCD_GH = reader["CCCD_GH"] == DBNull.Value ? null : reader["CCCD_GH"].ToString(),
                            DiaChi_GH = reader["DiaChi_GH"] == DBNull.Value ? null : reader["DiaChi_GH"].ToString(),
                            QuanHe = reader["QuanHe"] == DBNull.Value ? null : reader["QuanHe"].ToString()
                        };
                    }

                    return (kh, gh);
                }
            }
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
                        if (gh != null)
                        {
                            string queryCheckGH = "SELECT COUNT(1) FROM NGUOIGIAMHO WHERE MaGH = @MaGH";
                            MySqlCommand cmdCheckGH = new MySqlCommand(queryCheckGH, conn, tr);
                            cmdCheckGH.Parameters.AddWithValue("@MaGH", gh.MaGH);
                            int ghExists = Convert.ToInt32(cmdCheckGH.ExecuteScalar());

                            if (ghExists > 0)
                            {
                                string queryUpdateGH = @"UPDATE NGUOIGIAMHO SET TenGH=@TenGH, SDT_GH=@SDT_GH, Email_GH=@Email_GH, GioiTinh_GH=@GioiTinh_GH, NgaySinh_GH=@NgaySinh_GH, CCCD_GH=@CCCD_GH, DiaChi_GH=@DiaChi_GH, QuanHe=@QuanHe WHERE MaGH=@MaGH";
                                MySqlCommand cmdUpdateGH = new MySqlCommand(queryUpdateGH, conn, tr);
                                cmdUpdateGH.Parameters.AddWithValue("@MaGH", gh.MaGH);
                                cmdUpdateGH.Parameters.AddWithValue("@TenGH", gh.TenGH);
                                cmdUpdateGH.Parameters.AddWithValue("@SDT_GH", gh.SDT_GH);
                                cmdUpdateGH.Parameters.AddWithValue("@Email_GH", gh.Email_GH);
                                cmdUpdateGH.Parameters.AddWithValue("@GioiTinh_GH", gh.GioiTinh_GH);
                                cmdUpdateGH.Parameters.AddWithValue("@NgaySinh_GH", gh.NgaySinh_GH);
                                cmdUpdateGH.Parameters.AddWithValue("@CCCD_GH", gh.CCCD_GH);
                                cmdUpdateGH.Parameters.AddWithValue("@DiaChi_GH", gh.DiaChi_GH);
                                cmdUpdateGH.Parameters.AddWithValue("@QuanHe", gh.QuanHe);
                                cmdUpdateGH.ExecuteNonQuery();
                            }
                            else
                            {
                                string queryInsertGH = @"INSERT INTO NGUOIGIAMHO (MaGH, TenGH, SDT_GH, Email_GH, GioiTinh_GH, NgaySinh_GH, CCCD_GH, DiaChi_GH, QuanHe)
                                                         VALUES (@MaGH, @TenGH, @SDT_GH, @Email_GH, @GioiTinh_GH, @NgaySinh_GH, @CCCD_GH, @DiaChi_GH, @QuanHe)";
                                MySqlCommand cmdInsertGH = new MySqlCommand(queryInsertGH, conn, tr);
                                cmdInsertGH.Parameters.AddWithValue("@MaGH", gh.MaGH);
                                cmdInsertGH.Parameters.AddWithValue("@TenGH", gh.TenGH);
                                cmdInsertGH.Parameters.AddWithValue("@SDT_GH", gh.SDT_GH);
                                cmdInsertGH.Parameters.AddWithValue("@Email_GH", gh.Email_GH);
                                cmdInsertGH.Parameters.AddWithValue("@GioiTinh_GH", gh.GioiTinh_GH);
                                cmdInsertGH.Parameters.AddWithValue("@NgaySinh_GH", gh.NgaySinh_GH);
                                cmdInsertGH.Parameters.AddWithValue("@CCCD_GH", gh.CCCD_GH);
                                cmdInsertGH.Parameters.AddWithValue("@DiaChi_GH", gh.DiaChi_GH);
                                cmdInsertGH.Parameters.AddWithValue("@QuanHe", gh.QuanHe);
                                cmdInsertGH.ExecuteNonQuery();
                            }

                            kh.MaGH = gh.MaGH;
                        }

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