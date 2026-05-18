using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    public class CotDiemDTO
    {
        public int MaCotDiem { get; set; }
        public string MaMonHoc { get; set; }
        public string TenCotDiem { get; set; }
        public decimal TrongSo { get; set; }
        public int ThuTu { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
