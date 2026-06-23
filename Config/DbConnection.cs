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
            string? db   = Environment.GetEnvironmentVariable("DB_NAME");
            string? user = Environment.GetEnvironmentVariable("DB_USER");
            string? pass = Environment.GetEnvironmentVariable("DB_PASS");
            string? port = Environment.GetEnvironmentVariable("DB_PORT");
            string connString = $"Server={host};Port={port ?? "3306"};Database={db};Uid={user};Pwd={pass};CharSet=utf8mb4;";
            
            return new MySqlConnection(connString);
        }
    }
}