using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    /// <summary>
    /// DTO tổng hợp thông tin GPA để hiển thị 3 card trên cùng form frmMonHoc
    /// </summary>
    public class GpaSummaryDTO
    {
        // GPA Card 1: Hiện tại
        public decimal GpaHienTai { get; set; }          // GPA thang 4
        public int SoMonHoc { get; set; }                // Tổng số môn
        public int TongTinChi { get; set; }              // Tổng tín chỉ

        // GPA Card 2: Mục tiêu
        public decimal GpaMucTieu { get; set; }          // Từ NGUOI_DUNG
        public decimal ChenhLech { get; set; }           // GpaMucTieu - GpaHienTai
        public string TrangThaiGap { get; set; }         // "warning", "success", "danger"

        // GPA Card 3: Tỷ lệ hoàn thành
        public decimal TyLeHoanThanh { get; set; }       // % trọng số đã có điểm
        public int SoCotDiemTrong { get; set; }          // Số cột chưa có điểm
        public int TongCotDiem { get; set; }             // Tổng số cột điểm
    }
}