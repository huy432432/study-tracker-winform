using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient; // Sử dụng thư viện mới bạn đã cài
using do_an_1.DTO;
using do_an_1.Utils; // Link tới class DatabaseConnection bạn vừa gửi

namespace do_an_1.DAO
{
    public class DeadlineDAO
    {
        // Hàm dùng chung để lấy 1 giá trị số (cho Proc 4, 5)
        public int GetCountByProc(string procName, int maND)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(procName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maND);
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch { return 0; }
        }

        // Lấy danh sách nhiệm vụ cho Ma trận Eisenhower (Proc 8)
        public List<NhiemVuEisenhowerDTO> GetNhiemVuChoEisenhower(int maND)
        {
            List<NhiemVuEisenhowerDTO> list = new List<NhiemVuEisenhowerDTO>();
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_GetNhiemVuChoEisenhower", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maND);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new NhiemVuEisenhowerDTO
                            {
                                MaNhiemVu = (int)dr["MA_NHIEM_VU"],
                                TieuDe = dr["TIEU_DE"].ToString(),
                                ThoiHan = (DateTime)dr["THOI_HAN"],
                                SoTinChi = dr["SO_TIN_CHI"] != DBNull.Value ? (int)dr["SO_TIN_CHI"] : 0,
                                TrangThai = dr["TRANG_THAI"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { /* Log lỗi nếu cần */ }
            return list;

        }

        public List<NhiemVuEisenhowerDTO> GetAllDeadline(int maND)
        {
            List<NhiemVuEisenhowerDTO> list = new List<NhiemVuEisenhowerDTO>();
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                // Lấy tất cả, ưu tiên việc CHƯA_HOAN_THANH lên đầu, sau đó đến hạn gần nhất
                string sql = @"
            SELECT nv.*, mh.SO_TIN_CHI 
            FROM NHIEM_VU nv
            LEFT JOIN MON_HOC mh ON nv.MA_MON_HOC = mh.MA_MON_HOC
            WHERE nv.MA_NGUOI_DUNG = @maND
            ORDER BY CASE WHEN nv.TRANG_THAI = 'CHƯA_HOAN_THANH' THEN 0 ELSE 1 END, 
                     nv.THOI_HAN ASC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@maND", maND);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new NhiemVuEisenhowerDTO
                    {
                        MaNhiemVu = (int)dr["MA_NHIEM_VU"],
                        TieuDe = dr["TIEU_DE"].ToString(),
                        ThoiHan = (DateTime)dr["THOI_HAN"],
                        TrangThai = dr["TRANG_THAI"].ToString(),
                        MaMonHoc = dr["MA_MON_HOC"]?.ToString(),
                        SoTinChi = dr["SO_TIN_CHI"] != DBNull.Value ? (int)dr["SO_TIN_CHI"] : 0
                    });
                }
            }
            return list;
        }
        public bool AddDeadline(string tieuDe, object maMonHoc, DateTime thoiHan, int maND)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                // TRANG_THAI phải là N'CHƯA_HOAN_THANH' mới đúng Constraint của bạn
                string sql = @"INSERT INTO NHIEM_VU (MA_NGUOI_DUNG, MA_MON_HOC, TIEU_DE, THOI_HAN, TRANG_THAI) 
                       VALUES (@maND, @maMonHoc, @tieuDe, @thoiHan, N'CHƯA_HOAN_THANH')";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tieuDe", tieuDe);
                cmd.Parameters.AddWithValue("@maMonHoc", maMonHoc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@thoiHan", thoiHan);
                cmd.Parameters.AddWithValue("@maND", maND);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 1. Hàm lấy danh sách môn học để đổ vào ComboBox
        public DataTable GetDanhSachMonHoc(int maND)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                // Sử dụng UNION để chắc chắn luôn có ít nhất 1 dòng dữ liệu
                string sql = @"
            SELECT NULL AS MA_MON_HOC, N'-- unknow --' AS DISPLAY_NAME
            UNION ALL
            SELECT MA_MON_HOC, TEN_MON_HOC
            FROM MON_HOC 
            WHERE MA_NGUOI_DUNG = @maND";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@maND", maND);
                da.Fill(dt);
            }
            return dt;
        }
            
        // 2. Hàm cập nhật nhanh một trường dữ liệu bất kỳ
        public bool UpdateNhiemVuNhanh(int maNV, string column, object value)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    // Lưu ý: column phải được kiểm soát để tránh SQL Injection
                    string sql = $"UPDATE NHIEM_VU SET {column} = @val WHERE MA_NHIEM_VU = @ma";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@val", value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ma", maNV);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch { return false; }
        }

        // 3. Hàm xóa nhiệm vụ
        public bool DeleteNhiemVu(int maNV)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    string sql = "DELETE FROM NHIEM_VU WHERE MA_NHIEM_VU = @ma";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maNV);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch { return false; }
        }
    }
}