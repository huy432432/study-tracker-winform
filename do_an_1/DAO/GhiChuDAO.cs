using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using do_an_1.DTO;
using do_an_1.Utils;

namespace do_an_1.DAO
{
    public class GhiChuDAO
    {
        // ============================================================
        // 1. LẤY DANH SÁCH GHI CHÚ
        //    Có thể lọc theo môn học (null = lấy tất cả)
        //    Có thể tìm theo từ khóa trong tiêu đề / nội dung / tag
        // ============================================================
        public List<GhiChuDTO> GetDanhSach(int maNguoiDung,
                                            string maMonHoc = null,
                                            string tuKhoaTimKiem = null)
        {
            var list = new List<GhiChuDTO>();

            string query = @"
                SELECT 
                    MA_GHI_CHU,
                    MA_NGUOI_DUNG,
                    MA_MON_HOC,
                    TIEU_DE,
                    NOI_DUNG,
                    TU_KHOA,
                    LIEN_KET_TAI_LIEU,
                    NGAY_TAO,
                    NGAY_CAP_NHAT
                FROM GHI_CHU
                WHERE MA_NGUOI_DUNG = @MaNguoiDung";

            // Lọc theo môn học
            if (!string.IsNullOrEmpty(maMonHoc) && maMonHoc != "Tất cả")
                query += " AND MA_MON_HOC = @MaMonHoc";

            // Tìm kiếm full-text trong 3 cột
            if (!string.IsNullOrEmpty(tuKhoaTimKiem))
                query += @" AND (
                    TIEU_DE          LIKE N'%' + @TuKhoa + '%' OR
                    NOI_DUNG         LIKE N'%' + @TuKhoa + '%' OR
                    TU_KHOA          LIKE N'%' + @TuKhoa + '%'
                )";

            query += " ORDER BY NGAY_CAP_NHAT DESC";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                    if (!string.IsNullOrEmpty(maMonHoc) && maMonHoc != "Tất cả")
                        cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);

                    if (!string.IsNullOrEmpty(tuKhoaTimKiem))
                        cmd.Parameters.AddWithValue("@TuKhoa", tuKhoaTimKiem);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(MapRow(reader));
                    }
                }
            }

            return list;
        }

        // ============================================================
        // 2. LẤY 1 GHI CHÚ THEO ID
        // ============================================================
        public GhiChuDTO GetById(int maGhiChu)
        {
            const string query = @"
                SELECT 
                    MA_GHI_CHU, MA_NGUOI_DUNG, MA_MON_HOC,
                    TIEU_DE, NOI_DUNG, TU_KHOA,
                    LIEN_KET_TAI_LIEU, NGAY_TAO, NGAY_CAP_NHAT
                FROM GHI_CHU
                WHERE MA_GHI_CHU = @MaGhiChu";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaGhiChu", maGhiChu);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapRow(reader);
                    }
                }
            }

            return null;
        }

        // ============================================================
        // 3. THÊM MỚI GHI CHÚ
        //    Trả về MA_GHI_CHU vừa tạo (SCOPE_IDENTITY)
        // ============================================================
        public int Insert(GhiChuDTO dto)
        {
            const string query = @"
                INSERT INTO GHI_CHU 
                    (MA_NGUOI_DUNG, MA_MON_HOC, TIEU_DE, NOI_DUNG,
                     TU_KHOA, LIEN_KET_TAI_LIEU, NGAY_TAO, NGAY_CAP_NHAT)
                VALUES
                    (@MaNguoiDung, @MaMonHoc, @TieuDe, @NoiDung,
                     @TuKhoa, @LienKet, GETDATE(), GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    BindParams(cmd, dto);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        // ============================================================
        // 4. CẬP NHẬT GHI CHÚ
        //    Tự động cập nhật NGAY_CAP_NHAT = GETDATE()
        // ============================================================
        public bool Update(GhiChuDTO dto)
        {
            const string query = @"
                UPDATE GHI_CHU SET
                    MA_MON_HOC         = @MaMonHoc,
                    TIEU_DE            = @TieuDe,
                    NOI_DUNG           = @NoiDung,
                    TU_KHOA            = @TuKhoa,
                    LIEN_KET_TAI_LIEU  = @LienKet,
                    NGAY_CAP_NHAT      = GETDATE()
                WHERE MA_GHI_CHU = @MaGhiChu
                  AND MA_NGUOI_DUNG = @MaNguoiDung";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    BindParams(cmd, dto);
                    cmd.Parameters.AddWithValue("@MaGhiChu", dto.MaGhiChu);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ============================================================
        // 5. XÓA GHI CHÚ
        //    Kiểm tra MA_NGUOI_DUNG để tránh xóa nhầm của người khác
        // ============================================================
        public bool Delete(int maGhiChu, int maNguoiDung)
        {
            const string query = @"
                DELETE FROM GHI_CHU
                WHERE MA_GHI_CHU   = @MaGhiChu
                  AND MA_NGUOI_DUNG = @MaNguoiDung";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaGhiChu", maGhiChu);
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ============================================================
        // HELPER: Map SqlDataReader → GhiChuDTO
        // ============================================================
        private GhiChuDTO MapRow(SqlDataReader r)
        {
            return new GhiChuDTO
            {
                MaGhiChu = Convert.ToInt32(r["MA_GHI_CHU"]),
                MaNguoiDung = Convert.ToInt32(r["MA_NGUOI_DUNG"]),
                MaMonHoc = r["MA_MON_HOC"] == DBNull.Value ? null : r["MA_MON_HOC"].ToString(),
                TieuDe = r["TIEU_DE"].ToString(),
                NoiDung = r["NOI_DUNG"] == DBNull.Value ? null : r["NOI_DUNG"].ToString(),
                TuKhoa = r["TU_KHOA"] == DBNull.Value ? null : r["TU_KHOA"].ToString(),
                LienKetTaiLieu = r["LIEN_KET_TAI_LIEU"] == DBNull.Value ? null : r["LIEN_KET_TAI_LIEU"].ToString(),
                NgayTao = Convert.ToDateTime(r["NGAY_TAO"]),
                NgayCapNhat = Convert.ToDateTime(r["NGAY_CAP_NHAT"])
            };
        }

        // ============================================================
        // HELPER: Bind params chung cho Insert & Update
        // ============================================================
        private void BindParams(SqlCommand cmd, GhiChuDTO dto)
        {
            cmd.Parameters.AddWithValue("@MaNguoiDung", dto.MaNguoiDung);

            // MA_MON_HOC có thể null (ghi chú chung không gắn môn)
            if (string.IsNullOrEmpty(dto.MaMonHoc))
                cmd.Parameters.AddWithValue("@MaMonHoc", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@MaMonHoc", dto.MaMonHoc);

            cmd.Parameters.AddWithValue("@TieuDe", dto.TieuDe);

            // NOI_DUNG, TU_KHOA, LIEN_KET đều nullable
            cmd.Parameters.AddWithValue("@NoiDung",
                string.IsNullOrEmpty(dto.NoiDung) ? (object)DBNull.Value : dto.NoiDung);

            cmd.Parameters.AddWithValue("@TuKhoa",
                string.IsNullOrEmpty(dto.TuKhoa) ? (object)DBNull.Value : dto.TuKhoa);

            cmd.Parameters.AddWithValue("@LienKet",
                string.IsNullOrEmpty(dto.LienKetTaiLieu) ? (object)DBNull.Value : dto.LienKetTaiLieu);
        }
    }
}