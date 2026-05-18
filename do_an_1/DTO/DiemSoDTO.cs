using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    public class DiemSoDTO
    {
        public int MaDiem { get; set; }
        public int MaCotDiem { get; set; }
        public decimal? GiaTri { get; set; }         // Có thể null
        public DateTime NgayNhap { get; set; }
        public string GhiChu { get; set; }
    }
}
