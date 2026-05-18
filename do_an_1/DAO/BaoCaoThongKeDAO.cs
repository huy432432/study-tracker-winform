using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using do_an_1.DTO;
using do_an_1.Utils;

namespace do_an_1.DAO
{
    /// <summary>
    /// DAO xử lý tất cả truy vấn thống kê cho Module Báo cáo
    /// Áp dụng quy chế tín chỉ Bộ GD&ĐT và Hiệu ứng Zeigarnik
    /// </summary>
    public class BaoCaoThongKeDAO
    {
        /// <summary>
        /// Lấy danh sách học kỳ của người dùng (không trùng lặp)
        /// </summary>
        public List<string> GetHocKyByUser(int maNguoiDung)
        {
            List<string> list = new List<string>();

            string query = @"
                SELECT DISTINCT HOC_KY 
                FROM MON_HOC 
                WHERE MA_NGUOI_DUNG = @MaNguoiDung 
                  AND HOC_KY IS NOT NULL 
                ORDER BY HOC_KY DESC";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["HOC_KY"].ToString());
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Lấy danh sách môn học của người dùng, có thể lọc theo học kỳ
        /// </summary>
        public List<MonHocDTO> GetMonHocByUser(int maNguoiDung, string hocKy = null)
        {
            List<MonHocDTO> list = new List<MonHocDTO>();

            string query = @"
                SELECT MA_MON_HOC, TEN_MON_HOC, SO_TIN_CHI, HOC_KY 
                FROM MON_HOC 
                WHERE MA_NGUOI_DUNG = @MaNguoiDung";

            if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
            {
                query += " AND HOC_KY = @HocKy";
            }

            query += " ORDER BY TEN_MON_HOC";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                    if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
                    {
                        cmd.Parameters.AddWithValue("@HocKy", hocKy);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new MonHocDTO
                            {
                                MaMonHoc = reader["MA_MON_HOC"].ToString(),
                                TenMonHoc = reader["TEN_MON_HOC"].ToString(),
                                SoTinChi = Convert.ToInt32(reader["SO_TIN_CHI"]),
                                HocKy = reader["HOC_KY"]?.ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// [BIỂU ĐỒ 1] Thống kê Stress & Nhiệm vụ theo tuần
        /// Áp dụng Hiệu ứng Zeigarnik: Stress = (Nhiệm vụ chưa hoàn thành gần hạn / Tổng nhiệm vụ) * 100
        /// </summary>
        public List<ThongKeTuanDTO> GetThongKeTheoTuan(int maNguoiDung, string hocKy = null, string maMonHoc = null)
        {
            List<ThongKeTuanDTO> list = new List<ThongKeTuanDTO>();

            // Query SQL Aggregate: GROUP BY theo tuần, sử dụng DATEPART
            string query = @"
                WITH NhiemVuTheoTuan AS (
                    SELECT 
                        DATEPART(WEEK, THOI_HAN) AS SoTuan,
                        DATEADD(DAY, 1 - DATEPART(WEEKDAY, THOI_HAN), THOI_HAN) AS NgayBatDauTuan,
                        COUNT(*) AS TongNhiemVu,
                        SUM(CASE 
                            WHEN TRANG_THAI = N'CHƯA_HOAN_THANH' 
                             AND DATEDIFF(DAY, GETDATE(), THOI_HAN) BETWEEN 0 AND 3 
                            THEN 1 ELSE 0 
                        END) AS NhiemVuGanHan
                    FROM NHIEM_VU
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung
                      AND THOI_HAN IS NOT NULL";

            // Lọc động theo môn học
            if (!string.IsNullOrEmpty(maMonHoc) && maMonHoc != "Tất cả")
            {
                query += " AND MA_MON_HOC = @MaMonHoc";
            }

            // Lọc động theo học kỳ (qua bảng MON_HOC)
            if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
            {
                query += @" AND MA_MON_HOC IN (
                                SELECT MA_MON_HOC FROM MON_HOC 
                                WHERE HOC_KY = @HocKy AND MA_NGUOI_DUNG = @MaNguoiDung
                            )";
            }

            query += @"
                    GROUP BY 
                        DATEPART(WEEK, THOI_HAN),
                        DATEADD(DAY, 1 - DATEPART(WEEKDAY, THOI_HAN), THOI_HAN)
                )
                SELECT 
                    SoTuan,
                    NgayBatDauTuan,
                    TongNhiemVu,
                    NhiemVuGanHan,
                    CASE 
                        WHEN TongNhiemVu > 0 
                        THEN CAST(NhiemVuGanHan AS FLOAT) / CAST(TongNhiemVu AS FLOAT) * 100.0 
                        ELSE 0 
                    END AS StressIndex
                FROM NhiemVuTheoTuan
                ORDER BY NgayBatDauTuan";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                    if (!string.IsNullOrEmpty(maMonHoc) && maMonHoc != "Tất cả")
                    {
                        cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);
                    }

                    if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
                    {
                        cmd.Parameters.AddWithValue("@HocKy", hocKy);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime ngayBatDau = Convert.ToDateTime(reader["NgayBatDauTuan"]);
                            int soTuan = Convert.ToInt32(reader["SoTuan"]);
                            int tongNhiemVu = Convert.ToInt32(reader["TongNhiemVu"]);
                            double stressIndex = Convert.ToDouble(reader["StressIndex"]);

                            list.Add(new ThongKeTuanDTO
                            {
                                SoTuan = soTuan,
                                NgayBatDauTuan = ngayBatDau,
                                TongNhiemVu = tongNhiemVu,
                                NhiemVuGanHan = Convert.ToInt32(reader["NhiemVuGanHan"]),
                                StressIndex = Math.Round(stressIndex, 1),
                                TenTuan = $"Tuần {soTuan} ({ngayBatDau:dd/MM})"
                            });
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// [BIỂU ĐỒ 2] Thống kê giờ học: So sánh Giờ chuẩn vs Giờ thực tế
        /// Căn cứ Bộ GD&ĐT: 1 tín chỉ = 30 giờ tự học
        /// </summary>
        public List<ThongKeGioHocDTO> GetThongKeGioHoc(int maNguoiDung, string hocKy = null, string maMonHoc = null)
        {
            List<ThongKeGioHocDTO> list = new List<ThongKeGioHocDTO>();

            // Query SQL Aggregate: LEFT JOIN MON_HOC với PHIEN_HOC, GROUP BY môn học
            string query = @"
                SELECT 
                    MH.MA_MON_HOC,
                    MH.TEN_MON_HOC,
                    MH.SO_TIN_CHI,
                    -- Giờ chuẩn = Số tín chỉ * 30 (theo Bộ GD&ĐT)
                    CAST(MH.SO_TIN_CHI * 30 AS FLOAT) AS GioChuan,
                    -- Giờ thực tế = Tổng thời lượng Pomodoro / 60 phút
                    ISNULL(SUM(PH.THOI_LUONG_PHUT), 0) / 60.0 AS GioThucTe
                FROM MON_HOC MH
                LEFT JOIN PHIEN_HOC PH 
                    ON MH.MA_MON_HOC = PH.MA_MON_HOC 
                    AND PH.MA_NGUOI_DUNG = @MaNguoiDung
                WHERE MH.MA_NGUOI_DUNG = @MaNguoiDung";

            if (!string.IsNullOrEmpty(maMonHoc) && maMonHoc != "Tất cả")
            {
                query += " AND MH.MA_MON_HOC = @MaMonHoc";
            }

            if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
            {
                query += " AND MH.HOC_KY = @HocKy";
            }

            query += @"
                GROUP BY MH.MA_MON_HOC, MH.TEN_MON_HOC, MH.SO_TIN_CHI
                ORDER BY MH.TEN_MON_HOC";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                    if (!string.IsNullOrEmpty(maMonHoc) && maMonHoc != "Tất cả")
                    {
                        cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);
                    }

                    if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
                    {
                        cmd.Parameters.AddWithValue("@HocKy", hocKy);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string maMH = reader["MA_MON_HOC"].ToString();
                            string tenMH = reader["TEN_MON_HOC"].ToString();
                            int soTinChi = Convert.ToInt32(reader["SO_TIN_CHI"]);
                            double gioChuan = Convert.ToDouble(reader["GioChuan"]);
                            double gioThucTe = Math.Round(Convert.ToDouble(reader["GioThucTe"]), 1);

                            // Tính tỷ lệ hoàn thành
                            double tyLeHoanThanh = gioChuan > 0
                                ? Math.Round((gioThucTe / gioChuan) * 100.0, 1)
                                : 0;

                            // Đánh giá theo tiêu chí
                            string danhGia;
                            if (gioThucTe >= gioChuan)
                                danhGia = "Xuất sắc";
                            else if (tyLeHoanThanh >= 50)
                                danhGia = "Tốt";
                            else
                                danhGia = "Cần cố gắng";

                            list.Add(new ThongKeGioHocDTO
                            {
                                MaMonHoc = maMH,
                                TenMonHoc = tenMH,
                                SoTinChi = soTinChi,
                                GioChuan = gioChuan,
                                GioThucTe = gioThucTe,
                                TyLeHoanThanh = tyLeHoanThanh,
                                DanhGia = danhGia
                            });
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// [BIỂU ĐỒ 3] Thống kê phân bổ thời gian học giữa các môn
        /// Pie Chart: Hiển thị tỷ lệ % thời gian thực tế cho từng môn
        /// </summary>
        public List<PhanBoThoiGianDTO> GetPhanBoThoiGian(int maNguoiDung, string hocKy = null)
        {
            List<PhanBoThoiGianDTO> list = new List<PhanBoThoiGianDTO>();

            string query = @"
                SELECT 
                    MH.TEN_MON_HOC,
                    ISNULL(SUM(PH.THOI_LUONG_PHUT), 0) / 60.0 AS TongGio
                FROM MON_HOC MH
                LEFT JOIN PHIEN_HOC PH 
                    ON MH.MA_MON_HOC = PH.MA_MON_HOC 
                    AND PH.MA_NGUOI_DUNG = @MaNguoiDung
                WHERE MH.MA_NGUOI_DUNG = @MaNguoiDung";

            if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
            {
                query += " AND MH.HOC_KY = @HocKy";
            }

            query += @"
                GROUP BY MH.TEN_MON_HOC
                HAVING ISNULL(SUM(PH.THOI_LUONG_PHUT), 0) > 0
                ORDER BY TongGio DESC";

            // Trước tiên lấy tổng giờ để tính tỷ lệ %
            double tongGioTatCa = 0;
            List<Tuple<string, double>> tempList = new List<Tuple<string, double>>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                    if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
                    {
                        cmd.Parameters.AddWithValue("@HocKy", hocKy);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tenMH = reader["TEN_MON_HOC"].ToString();
                            double gio = Math.Round(Convert.ToDouble(reader["TongGio"]), 1);
                            tempList.Add(new Tuple<string, double>(tenMH, gio));
                            tongGioTatCa += gio;
                        }
                    }
                }
            }

            // Tính tỷ lệ phần trăm cho từng môn
            foreach (var item in tempList)
            {
                double tyLe = tongGioTatCa > 0
                    ? Math.Round((item.Item2 / tongGioTatCa) * 100.0, 1)
                    : 0;

                list.Add(new PhanBoThoiGianDTO
                {
                    TenMonHoc = item.Item1,
                    TongGio = item.Item2,
                    TyLePhanTram = tyLe
                });
            }

            return list;
        }

        // ================================================================
        // THÊM HÀM NÀY VÀO CUỐI CLASS BaoCaoThongKeDAO
        // (trước dấu đóng ngoặc } cuối cùng của class)
        // ================================================================

        /// <summary>
        /// [BẢNG ĐIỂM PDF] Lấy điểm trung bình có trọng số từng môn
        /// Công thức: SUM(GiaTriDiem × TrongSo / 100)
        /// Lọc theo học kỳ và/hoặc môn học cụ thể
        /// </summary>
        public List<DiemMonHocDTO> GetDiemMonHoc(int maNguoiDung,
                                                   string hocKy = null,
                                                   string maMonHoc = null)
        {
            var list = new List<DiemMonHocDTO>();

            string query = @"
                SELECT
                    MH.MA_MON_HOC,
                    MH.TEN_MON_HOC,
                    MH.SO_TIN_CHI,
                    MH.HOC_KY,

                    -- Điểm trung bình có trọng số
                    -- Chỉ tính các cột đã có điểm (DS.GIA_TRI IS NOT NULL)
                    ISNULL(
                        SUM(CASE WHEN DS.GIA_TRI IS NOT NULL
                                 THEN DS.GIA_TRI * CD.TRONG_SO / 100.0
                                 ELSE 0 END),
                        0
                    ) AS DiemTrungBinh,

                    -- Số cột đã nhập điểm
                    COUNT(CASE WHEN DS.GIA_TRI IS NOT NULL THEN 1 END) AS SoCotDaNhap,

                    -- Tổng số cột của môn đó
                    COUNT(CD.MA_COT_DIEM) AS TongSoCot

                FROM MON_HOC MH
                LEFT JOIN COT_DIEM CD ON MH.MA_MON_HOC  = CD.MA_MON_HOC
                LEFT JOIN DIEM_SO  DS ON CD.MA_COT_DIEM = DS.MA_COT_DIEM

                WHERE MH.MA_NGUOI_DUNG = @MaNguoiDung";

            if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
                query += " AND MH.HOC_KY = @HocKy";

            if (!string.IsNullOrEmpty(maMonHoc) && maMonHoc != "Tất cả")
                query += " AND MH.MA_MON_HOC = @MaMonHoc";

            query += @"
                GROUP BY MH.MA_MON_HOC, MH.TEN_MON_HOC, MH.SO_TIN_CHI, MH.HOC_KY
                ORDER BY MH.HOC_KY DESC, MH.TEN_MON_HOC";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                    if (!string.IsNullOrEmpty(hocKy) && hocKy != "Tất cả")
                        cmd.Parameters.AddWithValue("@HocKy", hocKy);

                    if (!string.IsNullOrEmpty(maMonHoc) && maMonHoc != "Tất cả")
                        cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DiemMonHocDTO
                            {
                                MaMonHoc = reader["MA_MON_HOC"].ToString(),
                                TenMonHoc = reader["TEN_MON_HOC"].ToString(),
                                SoTinChi = Convert.ToInt32(reader["SO_TIN_CHI"]),
                                HocKy = reader["HOC_KY"]?.ToString(),
                                DiemTrungBinh = Math.Round(Convert.ToDouble(reader["DiemTrungBinh"]), 2),
                                SoCotDaNhap = Convert.ToInt32(reader["SoCotDaNhap"]),
                                TongSoCot = Convert.ToInt32(reader["TongSoCot"])
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}