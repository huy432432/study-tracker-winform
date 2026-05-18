using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    public class MonHocDiemDTO : MonHocDTO
    {
        public decimal? DiemHienTai { get; set; }   // Điểm trung bình đã có
        public decimal? TrongSoConLai { get; set; }  // % trọng số chưa có điểm
        public decimal TrongSoDaCo => 100 - (TrongSoConLai ?? 0);
    }
}
