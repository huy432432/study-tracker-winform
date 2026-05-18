using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    /// <summary>
    /// DTO chứa dữ liệu phân bổ thời gian học
    /// Dùng cho Biểu đồ 3: Pie Chart
    /// </summary>
    public class PhanBoThoiGianDTO
    {
        public string TenMonHoc { get; set; }
        public double TongGio { get; set; }        // Tổng giờ học thực tế
        public double TyLePhanTram { get; set; }   // Phần trăm trên tổng thời gian
    }
}
