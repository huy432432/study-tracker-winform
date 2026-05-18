using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using do_an_1.DTO;
using do_an_1.Utils;
using Microsoft.Data.SqlClient;

namespace do_an_1.DAO
{
    public class DashboardDAO
    {
        /// <summary>
        /// Method 1: Lấy thông tin người dùng
        /// </summary>
        public NguoiDungDTO GetThongTinNguoiDung(int maNguoiDung)
        {
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
                        return new NguoiDungDTO
                        {
                            MaNguoiDung = Convert.ToInt32(reader["MA_NGUOI_DUNG"]),
                            HoTen = reader["HO_TEN"] != DBNull.Value
                                ? reader["HO_TEN"].ToString() : null,
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
        /// Method 2: Lấy tổng phút học hôm nay
        /// </summary>
        public int GetTongPhutHocHomNay(int maNguoiDung)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetTongPhutHocHomNay", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Method 3: Đếm số môn học của user
        /// </summary>
        public int GetSoMonHoc(int maNguoiDung)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetSoMonHoc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Method 4: Đếm số deadline chưa hoàn thành
        /// </summary>
        public int GetSoDeadline(int maNguoiDung)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetSoDeadline", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Method 5: Đếm số deadline đỏ (quá hạn, chưa xong)
        /// </summary>
        public int GetSoDeadlineDo(int maNguoiDung)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetSoDeadlineDo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Method 6: Đếm số phiên Pomodoro hôm nay
        /// </summary>
        public int GetSoPhienPomodoroHomNay(int maNguoiDung)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetSoPhienPomodoroHomNay", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Method 7: Đếm số ghi chú của user
        /// </summary>
        public int GetSoGhiChu(int maNguoiDung)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetSoGhiChu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Method 8: Lấy danh sách nhiệm vụ chưa hoàn thành (cho ma trận Eisenhower)
        /// </summary>
        public List<NhiemVuEisenhowerDTO> GetNhiemVuChoEisenhower(int maNguoiDung)
        {
            List<NhiemVuEisenhowerDTO> list = new List<NhiemVuEisenhowerDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetNhiemVuChoEisenhower", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NhiemVuEisenhowerDTO nv = new NhiemVuEisenhowerDTO
                        {
                            MaNhiemVu = Convert.ToInt32(reader["MA_NHIEM_VU"]),
                            MaNguoiDung = Convert.ToInt32(reader["MA_NGUOI_DUNG"]),
                            MaMonHoc = reader["MA_MON_HOC"] != DBNull.Value
                                ? reader["MA_MON_HOC"].ToString() : null,
                            TieuDe = reader["TIEU_DE"].ToString(),
                            MoTa = reader["MO_TA"] != DBNull.Value
                                ? reader["MO_TA"].ToString() : null,
                            ThoiHan = Convert.ToDateTime(reader["THOI_HAN"]),
                            TrangThai = reader["TRANG_THAI"].ToString(),
                            NgayTao = Convert.ToDateTime(reader["NGAY_TAO"]),
                            NgayHoanThanh = reader["NGAY_HOAN_THANH"] != DBNull.Value
                                ? Convert.ToDateTime(reader["NGAY_HOAN_THANH"]) : null,
                            SoTinChi = Convert.ToInt32(reader["SO_TIN_CHI"])
                        };

                        list.Add(nv);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Method 9: Lấy danh sách môn học kèm điểm hiện tại
        /// </summary>
        public List<MonHocDiemDTO> GetMonHocVaDiem(int maNguoiDung)
        {
            List<MonHocDiemDTO> list = new List<MonHocDiemDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetMonHocVaDiem", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new MonHocDiemDTO
                        {
                            MaMonHoc = reader["MA_MON_HOC"].ToString(),
                            MaNguoiDung = Convert.ToInt32(reader["MA_NGUOI_DUNG"]),
                            TenMonHoc = reader["TEN_MON_HOC"].ToString(),
                            SoTinChi = Convert.ToInt32(reader["SO_TIN_CHI"]),
                            TenGiangVien = reader["TEN_GIANG_VIEN"].ToString(),
                            HinhThucThi = reader["HINH_THUC_THI"].ToString(),
                            HocKy = reader["HOC_KY"] != DBNull.Value
                                ? reader["HOC_KY"].ToString() : null,
                            DiemHienTai = reader["DIEM_HIEN_TAI"] != DBNull.Value
                                ? Convert.ToDecimal(reader["DIEM_HIEN_TAI"]) : null,
                            TrongSoConLai = reader["TRONG_SO_CON_LAI"] != DBNull.Value
                                ? Convert.ToDecimal(reader["TRONG_SO_CON_LAI"]) : null
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Method 10: Lấy dữ liệu heatmap theo tháng
        /// </summary>
        public Dictionary<int, int> GetDuLieuHeatmap(int maNguoiDung, int thang, int nam)
        {
            Dictionary<int, int> data = new Dictionary<int, int>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetDuLieuHeatmap", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ngay = Convert.ToInt32(reader["NGAY"]);
                        int phut = Convert.ToInt32(reader["TONG_PHUT"]);
                        data[ngay] = phut;
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Method 11: Lấy 5 ghi chú gần đây nhất
        /// </summary>
        public List<GhiChuDTO> GetGhiChuGanDay(int maNguoiDung)
        {
            List<GhiChuDTO> list = new List<GhiChuDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetGhiChuGanDay", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new GhiChuDTO
                        {
                            MaGhiChu = Convert.ToInt32(reader["MA_GHI_CHU"]),
                            MaNguoiDung = Convert.ToInt32(reader["MA_NGUOI_DUNG"]),
                            MaMonHoc = reader["MA_MON_HOC"] != DBNull.Value
                                ? reader["MA_MON_HOC"].ToString() : null,
                            TieuDe = reader["TIEU_DE"].ToString(),
                            NoiDung = reader["NOI_DUNG"] != DBNull.Value
                                ? reader["NOI_DUNG"].ToString() : null,
                            TuKhoa = reader["TU_KHOA"] != DBNull.Value
                                ? reader["TU_KHOA"].ToString() : null,
                            NgayTao = Convert.ToDateTime(reader["NGAY_TAO"]),
                            NgayCapNhat = Convert.ToDateTime(reader["NGAY_CAP_NHAT"])
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Method 12: Tính GPA hiện tại (thang 4, làm tròn 2 số)
        /// </summary>
        public decimal GetGPAHienTai(int maNguoiDung)
        {
            decimal tongDiem4xTC = 0;
            int tongTinChiDaCoDiem = 0;

            List<MonHocDiemDTO> dsMon = GetMonHocVaDiem(maNguoiDung);

            foreach (MonHocDiemDTO mon in dsMon)
            {
                // Chỉ tính những môn đã bắt đầu có điểm (Trọng số đã có > 0)
                if (mon.DiemHienTai.HasValue && mon.TrongSoDaCo > 0)
                {
                    // 1. Tính điểm hệ 10 thực tế: (2.8 / 40) * 100 = 7.0
                    decimal diem10ThucTe = (mon.DiemHienTai.Value / mon.TrongSoDaCo) * 100;

                    // 2. Quy đổi con số 7.0 này sang hệ 4
                    decimal diem4 = QuyDoiThang10SangThang4(diem10ThucTe);

                    // 3. Cộng dồn để tính trung bình trọng số theo tín chỉ
                    tongDiem4xTC += diem4 * mon.SoTinChi;
                    tongTinChiDaCoDiem += mon.SoTinChi;
                }
            }

            if (tongTinChiDaCoDiem == 0) return 0;
            return Math.Round(tongDiem4xTC / tongTinChiDaCoDiem, 2);
        }

        /// <summary>
        /// Hàm quy đổi thang 10 → thang 4
        /// </summary>
        private decimal QuyDoiThang10SangThang4(decimal diem10)
        {
            if (diem10 >= 9.0m) return 4.0m;
            if (diem10 >= 8.0m) return 3.5m;
            if (diem10 >= 7.0m) return 3.0m;
            if (diem10 >= 6.0m) return 2.5m;
            if (diem10 >= 5.0m) return 2.0m;
            if (diem10 >= 4.0m) return 1.5m;
            return 0.0m;
        }
    }
}
