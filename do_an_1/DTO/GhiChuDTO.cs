using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    public class GhiChuDTO
    {
        public int MaGhiChu { get; set; }
        public int MaNguoiDung { get; set; }
        public string MaMonHoc { get; set; }         // Có thể null
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }          // NVARCHAR(MAX)
        public string TuKhoa { get; set; }
        public string LienKetTaiLieu { get; set; }   // NVARCHAR(MAX) - JSON string
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
    }
}
