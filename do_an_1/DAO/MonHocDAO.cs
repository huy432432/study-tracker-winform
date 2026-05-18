using System;
using System.Collections.Generic;
using System.Data;
using do_an_1.DTO;
using do_an_1.DTO;
using do_an_1.Utils;
using Microsoft.Data.SqlClient;

namespace do_an_1.DAO
{
    public class MonHocDAO
    {
        #region [1] Lấy danh sách môn học của user
        /// <summary>
        /// Lấy tất cả môn học của 1 user, bao gồm điểm hiện tại và trọng số
        /// </summary>
        public List<MonHocDiemDTO> GetDanhSachMonHoc(int maNguoiDung)
        {
            List<MonHocDiemDTO> list = new List<MonHocDiemDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetDanhSachMonHoc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MonHocDiemDTO dto = new MonHocDiemDTO
                        {
                            MaMonHoc = reader["MA_MON_HOC"].ToString(),
                            MaNguoiDung = Convert.ToInt32(reader["MA_NGUOI_DUNG"]),
                            TenMonHoc = reader["TEN_MON_HOC"].ToString(),
                            SoTinChi = Convert.ToInt32(reader["SO_TIN_CHI"]),
                            TenGiangVien = reader["TEN_GIANG_VIEN"]?.ToString() ?? "",
                            HinhThucThi = reader["HINH_THUC_THI"]?.ToString() ?? "",
                            HocKy = reader["HOC_KY"]?.ToString() ?? "",
                            NgayTao = Convert.ToDateTime(reader["NGAY_TAO"]),
                            NgayCapNhat = Convert.ToDateTime(reader["NGAY_CAP_NHAT"]),
                            DiemHienTai = reader["DIEM_HIEN_TAI"] != DBNull.Value ? Convert.ToDecimal(reader["DIEM_HIEN_TAI"]) : 0,
                            TrongSoConLai = reader["TONG_TRONG_SO"] != DBNull.Value ?
                                Convert.ToDecimal(reader["TONG_TRONG_SO"]) - Convert.ToDecimal(reader["TRONG_SO_DA_CO"]) : 0
                        };
                        list.Add(dto);
                    }
                }
            }
            return list;
        }

        public List<MonHocDiemDTO> GetDanhSachMonHoc(int maNguoiDung, string keyword = "")
        {
            List<MonHocDiemDTO> list = new List<MonHocDiemDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetDanhSachMonHoc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                // Truyền thêm keyword, nếu trống thì SQL sẽ hiểu là lấy hết
                cmd.Parameters.AddWithValue("@TenMon", string.IsNullOrEmpty(keyword) ? (object)DBNull.Value : keyword);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MonHocDiemDTO dto = new MonHocDiemDTO
                        {
                            MaMonHoc = reader["MA_MON_HOC"].ToString(),
                            MaNguoiDung = Convert.ToInt32(reader["MA_NGUOI_DUNG"]),
                            TenMonHoc = reader["TEN_MON_HOC"].ToString(),
                            SoTinChi = Convert.ToInt32(reader["SO_TIN_CHI"]),
                            TenGiangVien = reader["TEN_GIANG_VIEN"]?.ToString() ?? "",
                            HinhThucThi = reader["HINH_THUC_THI"]?.ToString() ?? "",
                            HocKy = reader["HOC_KY"]?.ToString() ?? "",
                            NgayTao = Convert.ToDateTime(reader["NGAY_TAO"]),
                            NgayCapNhat = Convert.ToDateTime(reader["NGAY_CAP_NHAT"]),
                            DiemHienTai = reader["DIEM_HIEN_TAI"] != DBNull.Value ? Convert.ToDecimal(reader["DIEM_HIEN_TAI"]) : 0,
                            TrongSoConLai = reader["TONG_TRONG_SO"] != DBNull.Value ?
                                Convert.ToDecimal(reader["TONG_TRONG_SO"]) - Convert.ToDecimal(reader["TRONG_SO_DA_CO"]) : 0
                        };
                        list.Add(dto);
                    }
                }
            }
            return list;
        }
        #endregion

        #region [2] Lấy 1 môn học theo mã
        /// <summary>
        /// Lấy thông tin 1 môn học theo MA_MON_HOC
        /// </summary>
        public MonHocDTO GetMonHocById(string maMonHoc)
        {
            MonHocDTO dto = null;

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetMonHocById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dto = new MonHocDTO
                        {
                            MaMonHoc = reader["MA_MON_HOC"].ToString(),
                            MaNguoiDung = Convert.ToInt32(reader["MA_NGUOI_DUNG"]),
                            TenMonHoc = reader["TEN_MON_HOC"].ToString(),
                            SoTinChi = Convert.ToInt32(reader["SO_TIN_CHI"]),
                            TenGiangVien = reader["TEN_GIANG_VIEN"]?.ToString() ?? "",
                            HinhThucThi = reader["HINH_THUC_THI"]?.ToString() ?? "",
                            HocKy = reader["HOC_KY"]?.ToString() ?? "",
                            NgayTao = Convert.ToDateTime(reader["NGAY_TAO"]),
                            NgayCapNhat = Convert.ToDateTime(reader["NGAY_CAP_NHAT"])
                        };
                    }
                }
            }
            return dto;
        }
        #endregion

        #region [3] Thêm mới môn học
        /// <summary>
        /// Thêm mới môn học, tự động tạo 2 cột điểm mặc định (Giữa kỳ 40%, Cuối kỳ 60%)
        /// </summary>
        public bool InsertMonHoc(MonHocDTO mon)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertMonHoc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", mon.MaMonHoc);
                cmd.Parameters.AddWithValue("@MaNguoiDung", mon.MaNguoiDung);
                cmd.Parameters.AddWithValue("@TenMonHoc", mon.TenMonHoc);
                cmd.Parameters.AddWithValue("@SoTinChi", mon.SoTinChi);
                cmd.Parameters.AddWithValue("@TenGiangVien", mon.TenGiangVien ?? "");
                cmd.Parameters.AddWithValue("@HinhThucThi", mon.HinhThucThi);
                cmd.Parameters.AddWithValue("@HocKy", mon.HocKy ?? "2025-2026");

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region [4] Cập nhật môn học
        /// <summary>
        /// Cập nhật thông tin môn học
        /// </summary>
        public bool UpdateMonHoc(MonHocDTO mon)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateMonHoc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", mon.MaMonHoc);
                cmd.Parameters.AddWithValue("@TenMonHoc", mon.TenMonHoc);
                cmd.Parameters.AddWithValue("@SoTinChi", mon.SoTinChi);
                cmd.Parameters.AddWithValue("@TenGiangVien", mon.TenGiangVien ?? "");
                cmd.Parameters.AddWithValue("@HinhThucThi", mon.HinhThucThi);
                cmd.Parameters.AddWithValue("@HocKy", mon.HocKy ?? "2025-2026");

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region [5] Xóa môn học
        /// <summary>
        /// Xóa môn học + xóa cascade các cột điểm và điểm số liên quan
        /// </summary>
        public bool DeleteMonHoc(string maMonHoc)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteMonHoc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region [6] Lấy danh sách cột điểm + điểm số của 1 môn
        /// <summary>
        /// Lấy tất cả cột điểm và điểm số (nếu có) của 1 môn học
        /// </summary>
        public List<CotDiemHienThiDTO> GetCotDiemVaDiemSo(string maMonHoc)
        {
            List<CotDiemHienThiDTO> list = new List<CotDiemHienThiDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetCotDiemVaDiemSo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CotDiemHienThiDTO dto = new CotDiemHienThiDTO
                        {
                            MaCotDiem = Convert.ToInt32(reader["MA_COT_DIEM"]),
                            MaMonHoc = reader["MA_MON_HOC"].ToString(),
                            TenCotDiem = reader["TEN_COT_DIEM"].ToString(),
                            TrongSo = Convert.ToDecimal(reader["TRONG_SO"]),
                            ThuTu = Convert.ToInt32(reader["THU_TU"]),
                            NgayTao = Convert.ToDateTime(reader["NGAY_TAO"]),
                            MaDiem = reader["MA_DIEM"] != DBNull.Value ? Convert.ToInt32(reader["MA_DIEM"]) : (int?)null,
                            GiaTri = reader["GIA_TRI"] != DBNull.Value ? Convert.ToDecimal(reader["GIA_TRI"]) : (decimal?)null,
                            GhiChu = reader["GHI_CHU"]?.ToString() ?? ""
                        };
                        list.Add(dto);
                    }
                }
            }
            return list;
        }
        #endregion

        #region [7] Thêm cột điểm mới
        /// <summary>
        /// Thêm 1 cột điểm mới cho môn học, trả về MA_COT_DIEM vừa tạo
        /// </summary>
        public int InsertCotDiem(CotDiemDTO cot)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertCotDiem", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", cot.MaMonHoc);
                cmd.Parameters.AddWithValue("@TenCotDiem", cot.TenCotDiem);
                cmd.Parameters.AddWithValue("@TrongSo", cot.TrongSo);
                cmd.Parameters.AddWithValue("@ThuTu", cot.ThuTu);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
        #endregion

        #region [8] Xóa cột điểm
        /// <summary>
        /// Xóa 1 cột điểm và điểm số liên quan
        /// </summary>
        public bool DeleteCotDiem(int maCotDiem)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteCotDiem", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaCotDiem", maCotDiem);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region [9] Cập nhật hoặc thêm điểm số (UPSERT)
        /// <summary>
        /// Nếu đã có điểm thì UPDATE, chưa có thì INSERT
        /// </summary>
        public bool UpsertDiemSo(int maCotDiem, decimal giaTri, string ghiChu = null)
        {
            try
            {
                // Sử dụng kết nối SQL của bạn
                using (SqlConnection conn =DatabaseConnection.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_UpsertDiemSo", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Khớp chính xác tham số với Proc trong SQL của bạn
                    cmd.Parameters.AddWithValue("@MaCotDiem", maCotDiem);
                    cmd.Parameters.AddWithValue("@GiaTri", giaTri);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)ghiChu ?? DBNull.Value);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi UpsertDiemSo: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region [10] Lấy GPA Summary của user
        /// <summary>
        /// Lấy thông tin tổng hợp GPA: GPA hiện tại, tỷ lệ hoàn thành, số cột trống...
        /// </summary>
        public GpaSummaryDTO GetGpaSummary(int maNguoiDung)
        {
            GpaSummaryDTO dto = new GpaSummaryDTO();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetGpaSummary", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dto.GpaHienTai = reader["GPA_HIEN_TAI"] != DBNull.Value ? Convert.ToDecimal(reader["GPA_HIEN_TAI"]) : 0;
                        dto.SoMonHoc = reader["SO_MON_HOC"] != DBNull.Value ? Convert.ToInt32(reader["SO_MON_HOC"]) : 0;
                        dto.TongTinChi = reader["TONG_TIN_CHI"] != DBNull.Value ? Convert.ToInt32(reader["TONG_TIN_CHI"]) : 0;
                        dto.TyLeHoanThanh = reader["TY_LE_HOAN_THANH"] != DBNull.Value ? Convert.ToDecimal(reader["TY_LE_HOAN_THANH"]) : 0;
                        dto.SoCotDiemTrong = reader["SO_COT_DIEM_TRONG"] != DBNull.Value ? Convert.ToInt32(reader["SO_COT_DIEM_TRONG"]) : 0;
                        dto.TongCotDiem = reader["TONG_COT_DIEM"] != DBNull.Value ? Convert.ToInt32(reader["TONG_COT_DIEM"]) : 0;
                    }
                }
            }
            return dto;
        }
        #endregion

        #region [11] Lấy GPA mục tiêu của user
        /// <summary>
        /// Lấy GPA_MUC_TIEU từ bảng NGUOI_DUNG
        /// </summary>
        public decimal GetGpaMucTieu(int maNguoiDung)
        {
            decimal gpaMucTieu = 3.60m; // Mặc định

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetThongTinNguoiDung", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        gpaMucTieu = reader["GPA_MUC_TIEU"] != DBNull.Value ? Convert.ToDecimal(reader["GPA_MUC_TIEU"]) : 3.60m;
                    }
                }
            }
            return gpaMucTieu;
        }
        #endregion

        #region [12] Kiểm tra mã môn học trùng
        /// <summary>
        /// Trả về true nếu mã môn học đã tồn tại
        /// </summary>
        public bool CheckDuplicateMaMonHoc(string maMonHoc, int maNguoiDung)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CheckDuplicateMaMonHoc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
        #region [13] Cập nhật trọng số cột điểm
        /// <summary>
        /// Cập nhật trọng số cho một cột điểm cụ thể
        /// </summary>
        public bool UpdateTrongSo(int maCotDiem, decimal trongSoMoi)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                // Bạn có thể viết trực tiếp SQL hoặc tạo Proc sp_UpdateTrongSo
                string sql = "UPDATE COT_DIEM SET TRONG_SO = @TrongSo WHERE MA_COT_DIEM = @MaCot";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaCot", maCotDiem);
                cmd.Parameters.AddWithValue("@TrongSo", trongSoMoi);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #endregion
    }
}