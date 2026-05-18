using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    public class NguoiDungDTO
    {
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string MatKhauMaHoa { get; set; }
        public string AnhDaiDien { get; set; }
        public decimal GpaMucTieu { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime LanCapNhatCuoi { get; set; }
    }
}
