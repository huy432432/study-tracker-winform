// DAO/PomodoroDAO.cs
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using do_an_1.DTO;
using do_an_1.Utils;

namespace do_an_1.DAO
{
    public class PomodoroDAO
    {
        #region Phiên học Pomodoro

        /// <summary>
        /// Lưu phiên học mới vào database
        /// </summary>
        public int SaveStudySession(PomodoroSessionDTO session)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert phiên học
                        string query = @"
                            INSERT INTO PHIEN_HOC 
                            (MA_NGUOI_DUNG, MA_MON_HOC, THOI_GIAN_BAT_DAU, THOI_GIAN_KET_THUC, THOI_LUONG_PHUT)
                            VALUES 
                            (@MaNguoiDung, @MaMonHoc, @ThoiGianBatDau, @ThoiGianKetThuc, @ThoiLuongPhut);
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, conn, transaction);
                        cmd.Parameters.AddWithValue("@MaNguoiDung", session.MaNguoiDung);
                        cmd.Parameters.AddWithValue("@MaMonHoc", string.IsNullOrEmpty(session.MaMonHoc) ? DBNull.Value : (object)session.MaMonHoc);
                        cmd.Parameters.AddWithValue("@ThoiGianBatDau", session.ThoiGianBatDau);
                        cmd.Parameters.AddWithValue("@ThoiGianKetThuc", session.ThoiGianKetThuc ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ThoiLuongPhut", session.ThoiLuongPhut ?? (object)DBNull.Value);

                        int maPhien = Convert.ToInt32(cmd.ExecuteScalar());

                        // Nếu có nhiệm vụ được chọn, cập nhật trạng thái
                        if (session.MaNhiemVu.HasValue && session.MaNhiemVu.Value > 0)
                        {
                            string updateTaskQuery = @"
                                UPDATE NHIEM_VU 
                                SET TRANG_THAI = N'DA_HOAN_THANH',
                                    NGAY_HOAN_THANH = @NgayHoanThanh
                                WHERE MA_NHIEM_VU = @MaNhiemVu";

                            cmd = new SqlCommand(updateTaskQuery, conn, transaction);
                            cmd.Parameters.AddWithValue("@NgayHoanThanh", DateTime.Now);
                            cmd.Parameters.AddWithValue("@MaNhiemVu", session.MaNhiemVu.Value);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return maPhien;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Lấy danh sách phiên học theo người dùng
        /// </summary>
        public List<PomodoroSessionDTO> GetStudySessions(int maNguoiDung, DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<PomodoroSessionDTO> sessions = new List<PomodoroSessionDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"
                    SELECT ph.*, mh.TEN_MON_HOC
                    FROM PHIEN_HOC ph
                    LEFT JOIN MON_HOC mh ON ph.MA_MON_HOC = mh.MA_MON_HOC
                    WHERE ph.MA_NGUOI_DUNG = @MaNguoiDung";

                if (fromDate.HasValue)
                    query += " AND ph.THOI_GIAN_BAT_DAU >= @FromDate";
                if (toDate.HasValue)
                    query += " AND ph.THOI_GIAN_BAT_DAU <= @ToDate";

                query += " ORDER BY ph.THOI_GIAN_BAT_DAU DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                if (fromDate.HasValue)
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Value);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sessions.Add(MapToSessionDTO(reader));
                }
            }

            return sessions;
        }

        /// <summary>
        /// Lấy thống kê học tập tổng quan
        /// </summary>
        public StudyStatisticsDTO GetStudyStatistics(int maNguoiDung)
        {
            StudyStatisticsDTO stats = new StudyStatisticsDTO();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Tổng quan
                string overviewQuery = @"
                    SELECT 
                        COUNT(*) as TotalSessions,
                        ISNULL(SUM(THOI_LUONG_PHUT), 0) as TotalMinutes,
                        ISNULL(AVG(THOI_LUONG_PHUT), 0) as AvgMinutes,
                        MAX(THOI_GIAN_BAT_DAU) as LastStudyDate
                    FROM PHIEN_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung
                    AND THOI_LUONG_PHUT IS NOT NULL";

                SqlCommand cmd = new SqlCommand(overviewQuery, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    stats.TotalSessions = reader.GetInt32(0);
                    stats.TotalMinutes = reader.GetInt32(1);
                    stats.AverageSessionMinutes = Convert.ToDouble(reader[2]);
                    stats.LastStudyDate = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3);
                    stats.TotalPomodoros = stats.TotalSessions;
                }
                reader.Close();

                // Thống kê hôm nay
                string todayQuery = @"
                    SELECT 
                        COUNT(*) as TodaySessions,
                        ISNULL(SUM(THOI_LUONG_PHUT), 0) as TodayMinutes
                    FROM PHIEN_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung
                    AND CAST(THOI_GIAN_BAT_DAU AS DATE) = CAST(GETDATE() AS DATE)
                    AND THOI_LUONG_PHUT IS NOT NULL";

                cmd = new SqlCommand(todayQuery, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    stats.TodaySessions = reader.GetInt32(0);
                    stats.TodayMinutes = reader.GetInt32(1);
                    stats.TodayPomodoros = stats.TodaySessions;
                }
                reader.Close();

                // Thống kê tuần này
                string weekQuery = @"
                    SELECT 
                        COUNT(*) as WeekSessions,
                        ISNULL(SUM(THOI_LUONG_PHUT), 0) as WeekMinutes
                    FROM PHIEN_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung
                    AND THOI_GIAN_BAT_DAU >= DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)
                    AND THOI_LUONG_PHUT IS NOT NULL";

                cmd = new SqlCommand(weekQuery, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    stats.WeekSessions = reader.GetInt32(0);
                    stats.WeekMinutes = reader.GetInt32(1);
                }
                reader.Close();

                // Thống kê tháng này
                string monthQuery = @"
                    SELECT 
                        COUNT(*) as MonthSessions,
                        ISNULL(SUM(THOI_LUONG_PHUT), 0) as MonthMinutes
                    FROM PHIEN_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung
                    AND MONTH(THOI_GIAN_BAT_DAU) = MONTH(GETDATE())
                    AND YEAR(THOI_GIAN_BAT_DAU) = YEAR(GETDATE())
                    AND THOI_LUONG_PHUT IS NOT NULL";

                cmd = new SqlCommand(monthQuery, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    stats.MonthSessions = reader.GetInt32(0);
                    stats.MonthMinutes = reader.GetInt32(1);
                }
                reader.Close();

                // Chuỗi ngày học
                stats.CurrentStreakDays = CalculateCurrentStreak(maNguoiDung, conn);
                stats.BestStreakDays = CalculateBestStreak(maNguoiDung, conn);

                // Ngày học tốt nhất
                string bestDayQuery = @"
                    SELECT TOP 1 
                        CAST(THOI_GIAN_BAT_DAU AS DATE) as StudyDate,
                        ISNULL(SUM(THOI_LUONG_PHUT), 0) as TotalMinutes
                    FROM PHIEN_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung
                    AND THOI_LUONG_PHUT IS NOT NULL
                    GROUP BY CAST(THOI_GIAN_BAT_DAU AS DATE)
                    ORDER BY TotalMinutes DESC";

                cmd = new SqlCommand(bestDayQuery, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    stats.BestDayDate = reader.GetDateTime(0);
                    stats.BestDayMinutes = reader.GetInt32(1);
                }
            }

            return stats;
        }

        /// <summary>
        /// Lấy thống kê theo môn học
        /// </summary>
        public List<SubjectStudyStatsDTO> GetSubjectStatistics(int maNguoiDung)
        {
            List<SubjectStudyStatsDTO> stats = new List<SubjectStudyStatsDTO>();
            int totalMinutes = 0;

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Lấy tổng số phút học
                string totalQuery = @"
                    SELECT ISNULL(SUM(THOI_LUONG_PHUT), 0) 
                    FROM PHIEN_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung 
                    AND THOI_LUONG_PHUT IS NOT NULL";

                SqlCommand totalCmd = new SqlCommand(totalQuery, conn);
                totalCmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                totalMinutes = Convert.ToInt32(totalCmd.ExecuteScalar());

                // Lấy thống kê theo từng môn
                string query = @"
                    SELECT 
                        mh.MA_MON_HOC,
                        mh.TEN_MON_HOC,
                        COUNT(ph.MA_PHIEN) as TotalSessions,
                        ISNULL(SUM(ph.THOI_LUONG_PHUT), 0) as TotalMinutes,
                        ISNULL(AVG(ph.THOI_LUONG_PHUT), 0) as AvgMinutes,
                        MAX(ph.THOI_GIAN_BAT_DAU) as LastStudyDate
                    FROM MON_HOC mh
                    LEFT JOIN PHIEN_HOC ph ON mh.MA_MON_HOC = ph.MA_MON_HOC 
                        AND ph.MA_NGUOI_DUNG = @MaNguoiDung
                    WHERE mh.MA_NGUOI_DUNG = @MaNguoiDung
                    GROUP BY mh.MA_MON_HOC, mh.TEN_MON_HOC
                    ORDER BY TotalMinutes DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SubjectStudyStatsDTO stat = new SubjectStudyStatsDTO
                    {
                        MaMonHoc = reader["MA_MON_HOC"].ToString(),
                        TenMonHoc = reader["TEN_MON_HOC"].ToString(),
                        TotalSessions = reader.GetInt32(reader.GetOrdinal("TotalSessions")),
                        TotalMinutes = reader.GetInt32(reader.GetOrdinal("TotalMinutes")),
                        AverageMinutesPerSession = Convert.ToDouble(reader["AvgMinutes"]),
                        LastStudyDate = reader.IsDBNull(reader.GetOrdinal("LastStudyDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LastStudyDate")),
                        TotalPomodoros = reader.GetInt32(reader.GetOrdinal("TotalSessions")),
                        PercentageOfTotal = totalMinutes > 0
? (Convert.ToDouble(reader["TotalMinutes"]) / totalMinutes * 100)
: 0
                    };
                    stats.Add(stat);
                }
            }

            return stats;
        }

        /// <summary>
        /// Lấy thống kê theo ngày
        /// </summary>
        public List<DailyStudyStatsDTO> GetDailyStatistics(int maNguoiDung, int days = 30)
        {
            List<DailyStudyStatsDTO> dailyStats = new List<DailyStudyStatsDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        CAST(ph.THOI_GIAN_BAT_DAU AS DATE) as StudyDate,
                        COUNT(ph.MA_PHIEN) as SessionCount,
                        ISNULL(SUM(ph.THOI_LUONG_PHUT), 0) as TotalMinutes,
                        COUNT(DISTINCT ph.MA_MON_HOC) as SubjectCount,
                        (
                            SELECT TOP 1 mh.TEN_MON_HOC
                            FROM PHIEN_HOC ph2
                            JOIN MON_HOC mh ON ph2.MA_MON_HOC = mh.MA_MON_HOC
                            WHERE ph2.MA_NGUOI_DUNG = @MaNguoiDung
                            AND CAST(ph2.THOI_GIAN_BAT_DAU AS DATE) = CAST(ph.THOI_GIAN_BAT_DAU AS DATE)
                            GROUP BY mh.TEN_MON_HOC
                            ORDER BY COUNT(ph2.MA_PHIEN) DESC
                        ) as MostStudiedSubject
                    FROM PHIEN_HOC ph
                    WHERE ph.MA_NGUOI_DUNG = @MaNguoiDung
                    AND ph.THOI_GIAN_BAT_DAU >= DATEADD(DAY, -@Days, GETDATE())
                    AND ph.THOI_LUONG_PHUT IS NOT NULL
                    GROUP BY CAST(ph.THOI_GIAN_BAT_DAU AS DATE)
                    ORDER BY StudyDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                cmd.Parameters.AddWithValue("@Days", days);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dailyStats.Add(new DailyStudyStatsDTO
                    {
                        StudyDate = reader.GetDateTime(0),
                        SessionCount = reader.GetInt32(1),
                        TotalMinutes = reader.GetInt32(2),
                        SubjectCount = reader.GetInt32(3),
                        MostStudiedSubject = reader.IsDBNull(4) ? "Chưa có" : reader.GetString(4),
                        PomodoroCount = reader.GetInt32(1)
                    });
                }
            }

            return dailyStats;
        }

        #endregion

        #region Nhiệm vụ (Task) cho Pomodoro

        /// <summary>
        /// Lấy danh sách môn học của người dùng
        /// </summary>
        public DataTable GetSubjectsByUser(int maNguoiDung)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"
                    SELECT MA_MON_HOC, TEN_MON_HOC, SO_TIN_CHI, TEN_GIANG_VIEN
                    FROM MON_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung
                    ORDER BY TEN_MON_HOC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Lấy danh sách nhiệm vụ theo môn học
        /// </summary>
        public List<PomodoroTaskDTO> GetTasksBySubject(int maNguoiDung, string maMonHoc = null, bool includeCompleted = false)
        {
            List<PomodoroTaskDTO> tasks = new List<PomodoroTaskDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        nv.MA_NHIEM_VU,
                        nv.MA_NGUOI_DUNG,
                        nv.MA_MON_HOC,
                        mh.TEN_MON_HOC,
                        nv.TIEU_DE,
                        nv.MO_TA,
                        nv.THOI_HAN,
                        nv.TRANG_THAI,
                        nv.NGAY_HOAN_THANH
                    FROM NHIEM_VU nv
                    LEFT JOIN MON_HOC mh ON nv.MA_MON_HOC = mh.MA_MON_HOC
                    WHERE nv.MA_NGUOI_DUNG = @MaNguoiDung";

                if (!includeCompleted)
                    query += " AND nv.TRANG_THAI = N'CHƯA_HOAN_THANH'";

                if (!string.IsNullOrEmpty(maMonHoc))
                    query += " AND nv.MA_MON_HOC = @MaMonHoc";

                query += " ORDER BY nv.THOI_HAN ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                if (!string.IsNullOrEmpty(maMonHoc))
                    cmd.Parameters.AddWithValue("@MaMonHoc", maMonHoc);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PomodoroTaskDTO task = new PomodoroTaskDTO
                    {
                        MaNhiemVu = reader.GetInt32(0),
                        MaNguoiDung = reader.GetInt32(1),
                        MaMonHoc = reader.IsDBNull(2) ? null : reader.GetString(2),
                        TenMonHoc = reader.IsDBNull(3) ? "Không có môn" : reader.GetString(3),
                        TieuDe = reader.GetString(4),
                        MoTa = reader.IsDBNull(5) ? null : reader.GetString(5),
                        ThoiHan = reader.GetDateTime(6),
                        TrangThai = reader.GetString(7),
                        NgayHoanThanh = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8),
                        IsOverdue = reader.GetDateTime(6) < DateTime.Now && reader.GetString(7) == "CHƯA_HOAN_THANH",
                        DaysLeft = (int)(reader.GetDateTime(6) - DateTime.Now).TotalDays
                    };
                    tasks.Add(task);
                }
            }

            return tasks;
        }

        /// <summary>
        /// Thêm nhiệm vụ mới
        /// </summary>
        public int AddTask(PomodoroTaskDTO task)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO NHIEM_VU 
                    (MA_NGUOI_DUNG, MA_MON_HOC, TIEU_DE, MO_TA, THOI_HAN, TRANG_THAI)
                    VALUES 
                    (@MaNguoiDung, @MaMonHoc, @TieuDe, @MoTa, @ThoiHan, N'CHƯA_HOAN_THANH');
                    SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", task.MaNguoiDung);
                cmd.Parameters.AddWithValue("@MaMonHoc", string.IsNullOrEmpty(task.MaMonHoc) ? DBNull.Value : (object)task.MaMonHoc);
                cmd.Parameters.AddWithValue("@TieuDe", task.TieuDe);
                cmd.Parameters.AddWithValue("@MoTa", string.IsNullOrEmpty(task.MoTa) ? DBNull.Value : (object)task.MoTa);
                cmd.Parameters.AddWithValue("@ThoiHan", task.ThoiHan);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Cập nhật trạng thái nhiệm vụ
        /// </summary>
        public bool UpdateTaskStatus(int maNhiemVu, string trangThai)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"
                    UPDATE NHIEM_VU 
                    SET TRANG_THAI = @TrangThai,
                        NGAY_HOAN_THANH = CASE WHEN @TrangThai = N'DA_HOAN_THANH' THEN GETDATE() ELSE NULL END
                    WHERE MA_NHIEM_VU = @MaNhiemVu";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                cmd.Parameters.AddWithValue("@MaNhiemVu", maNhiemVu);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        #endregion

        #region Cảnh báo sức khỏe

        /// <summary>
        /// Kiểm tra cảnh báo sức khỏe
        /// </summary>
        public HealthWarningDTO CheckHealthWarning(int maNguoiDung)
        {
            HealthWarningDTO warning = new HealthWarningDTO();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Kiểm tra học liên tục trong 12 giờ qua
                string fatigueQuery = @"
                    SELECT 
                        COUNT(*) as SessionCount,
                        ISNULL(SUM(THOI_LUONG_PHUT), 0) as TotalMinutes,
                        MAX(THOI_GIAN_KET_THUC) as LastSessionEnd
                    FROM PHIEN_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung 
                    AND THOI_GIAN_BAT_DAU >= DATEADD(HOUR, -12, GETDATE())
                    AND THOI_LUONG_PHUT IS NOT NULL";

                SqlCommand cmd = new SqlCommand(fatigueQuery, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int sessionCount = reader.GetInt32(0);
                    int totalMinutes = reader.GetInt32(1);
                    warning.TodaySessionCount = sessionCount;
                    warning.TodayTotalMinutes = totalMinutes;
                    warning.ConsecutiveHours = totalMinutes / 60;

                    if (sessionCount > 8 || totalMinutes > 240) // Hơn 8 phiên hoặc 4 tiếng
                    {
                        warning.ShouldWarn = true;
                        warning.WarningType = "FATIGUE";
                        warning.WarningMessage = $"⚠️ Bạn đã học {warning.ConsecutiveHours} giờ với {sessionCount} phiên trong 12 giờ qua. Hãy nghỉ ngơi ít nhất 15 phút!";
                        warning.RecommendedBreakMinutes = 15;
                    }

                    if (!reader.IsDBNull(2))
                        warning.LastBreakTime = reader.GetDateTime(2);
                }
                reader.Close();

                // Kiểm tra học khuya (sau 23h)
                if (DateTime.Now.Hour >= 23)
                {
                    warning.ShouldWarn = true;
                    warning.WarningType = "LATE_NIGHT";
                    warning.WarningMessage = "🌙 Đã khuya rồi! Học tập hiệu quả cần giấc ngủ đủ. Hãy cân nhắc nghỉ ngơi.";
                    warning.RecommendedBreakMinutes = 480; // 8 tiếng ngủ
                }
            }

            return warning;
        }

        #endregion

        #region Private Helper Methods

        private PomodoroSessionDTO MapToSessionDTO(SqlDataReader reader)
        {
            return new PomodoroSessionDTO
            {
                MaPhien = reader.GetInt32(reader.GetOrdinal("MA_PHIEN")),
                MaNguoiDung = reader.GetInt32(reader.GetOrdinal("MA_NGUOI_DUNG")),
                MaMonHoc = reader.IsDBNull(reader.GetOrdinal("MA_MON_HOC")) ? null : reader.GetString(reader.GetOrdinal("MA_MON_HOC")),
                TenMonHoc = reader.IsDBNull(reader.GetOrdinal("TEN_MON_HOC")) ? null : reader.GetString(reader.GetOrdinal("TEN_MON_HOC")),
                ThoiGianBatDau = reader.GetDateTime(reader.GetOrdinal("THOI_GIAN_BAT_DAU")),
                ThoiGianKetThuc = reader.IsDBNull(reader.GetOrdinal("THOI_GIAN_KET_THUC")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("THOI_GIAN_KET_THUC")),
                ThoiLuongPhut = reader.IsDBNull(reader.GetOrdinal("THOI_LUONG_PHUT")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("THOI_LUONG_PHUT")),
                NgayTao = reader.GetDateTime(reader.GetOrdinal("NGAY_TAO"))
            };
        }

        private int CalculateCurrentStreak(int maNguoiDung, SqlConnection conn)
        {
            try
            {
                string query = @"
                    SELECT DISTINCT CAST(THOI_GIAN_BAT_DAU AS DATE) as StudyDate
                    FROM PHIEN_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung
                    AND THOI_LUONG_PHUT IS NOT NULL
                    ORDER BY StudyDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                SqlDataReader reader = cmd.ExecuteReader();
                List<DateTime> studyDates = new List<DateTime>();

                while (reader.Read())
                {
                    studyDates.Add(reader.GetDateTime(0));
                }
                reader.Close();

                if (studyDates.Count == 0) return 0;

                int streak = 0;
                DateTime currentDate = DateTime.Now.Date;

                foreach (DateTime date in studyDates)
                {
                    if (date == currentDate)
                    {
                        streak++;
                        currentDate = currentDate.AddDays(-1);
                    }
                    else if (date == currentDate)
                    {
                        streak++;
                        currentDate = currentDate.AddDays(-1);
                    }
                    else
                    {
                        break;
                    }
                }

                return streak;
            }
            catch
            {
                return 0;
            }
        }

        private int CalculateBestStreak(int maNguoiDung, SqlConnection conn)
        {
            try
            {
                string query = @"
                    SELECT DISTINCT CAST(THOI_GIAN_BAT_DAU AS DATE) as StudyDate
                    FROM PHIEN_HOC 
                    WHERE MA_NGUOI_DUNG = @MaNguoiDung
                    AND THOI_LUONG_PHUT IS NOT NULL
                    ORDER BY StudyDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);

                SqlDataReader reader = cmd.ExecuteReader();
                List<DateTime> studyDates = new List<DateTime>();

                while (reader.Read())
                {
                    studyDates.Add(reader.GetDateTime(0));
                }
                reader.Close();

                if (studyDates.Count == 0) return 0;

                int bestStreak = 1;
                int currentStreak = 1;

                for (int i = 1; i < studyDates.Count; i++)
                {
                    if ((studyDates[i] - studyDates[i - 1]).Days == 1)
                    {
                        currentStreak++;
                        bestStreak = Math.Max(bestStreak, currentStreak);
                    }
                    else
                    {
                        currentStreak = 1;
                    }
                }

                return bestStreak;
            }
            catch
            {
                return 0;
            }
        }

        #endregion
    }
}