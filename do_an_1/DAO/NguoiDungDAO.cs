using System;
using System.Data;
using do_an_1.DTO;
using do_an_1.Utils;
using Microsoft.Data.SqlClient;

namespace do_an_1.DAO
{
    public class NguoiDungDAO
    {
        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        public bool DangKy(NguoiDungDTO nd)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DangKy", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HoTen", nd.HoTen);
                cmd.Parameters.AddWithValue("@Email", nd.Email);
                cmd.Parameters.AddWithValue("@MatKhauMaHoa", nd.MatKhauMaHoa);
                cmd.Parameters.AddWithValue("@GpaMucTieu", nd.GpaMucTieu);
                conn.Open();

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        /// <summary>
        /// Đăng nhập - trả về thông tin user nếu thành công
        /// </summary>
        public NguoiDungDTO DangNhap(string email, string matKhauMaHoa)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DangNhap", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@MatKhauMaHoa", matKhauMaHoa);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new NguoiDungDTO
                        {
                            MaNguoiDung = Convert.ToInt32(reader["MA_NGUOI_DUNG"]),
                            HoTen = reader["HO_TEN"].ToString(),
                            Email = reader["EMAIL"].ToString(),
                            GpaMucTieu = reader["GPA_MUC_TIEU"] != DBNull.Value
                                ? Convert.ToDecimal(reader["GPA_MUC_TIEU"]) : 3.60m
                        };
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa
        /// </summary>
        public bool KiemTraEmailTonTai(string email)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_KiemTraEmailTonTai", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result) > 0;
            }
        }
    }
}