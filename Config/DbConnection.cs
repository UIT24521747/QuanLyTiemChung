using MySql.Data.MySqlClient;
using System;
using DotNetEnv; 

namespace QuanLyKhachHang.Config
{
    public class DatabaseConfig
    {
        public static MySqlConnection GetConnection()
        {
            Env.Load();
            string? host = Environment.GetEnvironmentVariable("DB_HOST");
            string? db = Environment.GetEnvironmentVariable("DB_NAME");
            string? user = Environment.GetEnvironmentVariable("DB_USER");
            string? pass = Environment.GetEnvironmentVariable("DB_PASS"); 
            string connString = $"Server={host};Database={db};Uid={user};Pwd={pass};CharSet=utf8;";
            
            return new MySqlConnection(connString);
        }
    }
}