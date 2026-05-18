using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    /// <summary>
    /// DTO chứa dữ liệu thống kê theo tuần
    /// Dùng cho Biểu đồ 1: Cột (Tổng nhiệm vụ) + Đường (Chỉ số Stress)
    /// </summary>
    public class ThongKeTuanDTO
    {
        public int SoTuan { get; set; }              // Số thứ tự tuần trong năm (1-52)
        public DateTime NgayBatDauTuan { get; set; } // Ngày đầu tiên của tuần
        public int TongNhiemVu { get; set; }          // Tổng số nhiệm vụ được giao trong tuần
        public int NhiemVuGanHan { get; set; }        // Số nhiệm vụ CHƯA hoàn thành và còn hạn <= 3 ngày
        public double StressIndex { get; set; }       // Chỉ số stress = (NhiemVuGanHan / TongNhiemVu) * 100
        public string TenTuan { get; set; }           // Nhãn hiển thị: "Tuần 1 (01/01-07/01)"
    }
}