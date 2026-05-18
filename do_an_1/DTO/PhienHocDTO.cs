// DTO/PomodoroDTO.cs
using System;
using System.Collections.Generic;

namespace do_an_1.DTO
{
    /// <summary>
    /// DTO cho phiên học Pomodoro
    /// </summary>
    public class PomodoroSessionDTO
    {
        public int MaPhien { get; set; }
        public int MaNguoiDung { get; set; }
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public int? ThoiLuongPhut { get; set; }
        public DateTime NgayTao { get; set; }

        // Thông tin cấu hình phiên
        public int WorkMinutes { get; set; }
        public int BreakMinutes { get; set; }
        public bool IsWorkSession { get; set; }
        public int CompletedPomodoros { get; set; }

        // Thông tin nhiệm vụ
        public int? MaNhiemVu { get; set; }
        public string TenNhiemVu { get; set; }

        public PomodoroSessionDTO()
        {
            IsWorkSession = true;
            WorkMinutes = 25;
            BreakMinutes = 5;
            CompletedPomodoros = 0;
            ThoiGianBatDau = DateTime.Now;
        }
    }

    /// <summary>
    /// DTO cho thống kê học tập tổng quan
    /// </summary>
    public class StudyStatisticsDTO
    {
        // Thống kê tổng quan
        public int TotalSessions { get; set; }
        public int TotalMinutes { get; set; }
        public double AverageSessionMinutes { get; set; }
        public int TotalPomodoros { get; set; }

        // Thống kê hôm nay
        public int TodaySessions { get; set; }
        public int TodayMinutes { get; set; }
        public int TodayPomodoros { get; set; }

        // Thống kê tuần này
        public int WeekSessions { get; set; }
        public int WeekMinutes { get; set; }

        // Thống kê tháng này
        public int MonthSessions { get; set; }
        public int MonthMinutes { get; set; }

        // Chuỗi ngày học
        public int CurrentStreakDays { get; set; }
        public int BestStreakDays { get; set; }
        public DateTime LastStudyDate { get; set; }

        // Thành tích tốt nhất
        public int BestDayMinutes { get; set; }
        public DateTime BestDayDate { get; set; }
    }

    /// <summary>
    /// DTO cho thống kê theo môn học
    /// </summary>
    public class SubjectStudyStatsDTO
    {
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public int TotalSessions { get; set; }
        public int TotalMinutes { get; set; }
        public double AverageMinutesPerSession { get; set; }
        public int TotalPomodoros { get; set; }
        public DateTime LastStudyDate { get; set; }
        public double PercentageOfTotal { get; set; }
    }

    /// <summary>
    /// DTO cho thống kê theo ngày
    /// </summary>
    public class DailyStudyStatsDTO
    {
        public DateTime StudyDate { get; set; }
        public int SessionCount { get; set; }
        public int TotalMinutes { get; set; }
        public int PomodoroCount { get; set; }
        public int SubjectCount { get; set; }
        public string MostStudiedSubject { get; set; }
    }

    /// <summary>
    /// DTO cho cảnh báo sức khỏe
    /// </summary>
    public class HealthWarningDTO
    {
        public bool ShouldWarn { get; set; }
        public string WarningType { get; set; } // FATIGUE, LONG_SESSION, LATE_NIGHT
        public string WarningMessage { get; set; }
        public int ConsecutiveHours { get; set; }
        public int TodaySessionCount { get; set; }
        public int TodayTotalMinutes { get; set; }
        public DateTime LastBreakTime { get; set; }
        public int RecommendedBreakMinutes { get; set; }
    }

    /// <summary>
    /// DTO cho nhiệm vụ Pomodoro
    /// </summary>
    public class PomodoroTaskDTO
    {
        public int MaNhiemVu { get; set; }
        public int MaNguoiDung { get; set; }
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; }
        public DateTime ThoiHan { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayHoanThanh { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysLeft { get; set; }
        public int EstimatedPomodoros { get; set; }
        public int CompletedPomodoros { get; set; }
        public double ProgressPercentage { get; set; }
    }
}