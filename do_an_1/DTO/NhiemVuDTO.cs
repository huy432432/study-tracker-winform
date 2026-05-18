using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    public class NhiemVuDTO
    {
        public int MaNhiemVu { get; set; }
        public int MaNguoiDung { get; set; }
        public string MaMonHoc { get; set; }         // Có thể null
        public string TieuDe { get; set; }
        public string MoTa { get; set; }             // NVARCHAR(MAX) = string
        public DateTime ThoiHan { get; set; }
        public string TrangThai { get; set; }        // CHƯA_HOAN_THANH | DA_HOAN_THANH
        public DateTime NgayTao { get; set; }
        public DateTime? NgayHoanThanh { get; set; } // Có thể null
    }
}
