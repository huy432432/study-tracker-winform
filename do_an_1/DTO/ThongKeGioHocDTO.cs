using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    /// <summary>
    /// DTO chứa dữ liệu so sánh giờ học chuẩn và thực tế
    /// Dùng cho Biểu đồ 2: Stacked Column (Giờ chuẩn vs Giờ thực tế)
    /// </summary>
    public class ThongKeGioHocDTO
    {
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public int SoTinChi { get; set; }

        /// <summary>
        /// Giờ tự học tiêu chuẩn = Số tín chỉ * 30 giờ (theo Bộ GD&ĐT)
        /// </summary>
        public double GioChuan { get; set; }

        /// <summary>
        /// Giờ học thực tế = Tổng thời lượng Pomodoro / 60 phút
        /// </summary>
        public double GioThucTe { get; set; }

        /// <summary>
        /// Tỷ lệ hoàn thành = (GioThucTe / GioChuan) * 100
        /// </summary>
        public double TyLeHoanThanh { get; set; }

        /// <summary>
        /// Đánh giá: "Xuất sắc", "Tốt", "Cần cố gắng"
        /// </summary>
        public string DanhGia { get; set; }
    }
}
