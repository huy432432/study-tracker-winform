using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.DTO
{
    public class MonHocDTO
    {
        public string MaMonHoc { get; set; }
        public int MaNguoiDung { get; set; }
        public string TenMonHoc { get; set; }
        public int SoTinChi { get; set; }
        public string TenGiangVien { get; set; }
        public string HinhThucThi { get; set; }
        public string HocKy { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
    }

    public class MonHocResult
    {
        public bool IsEdit { get; set; }
        public string OriginalMaMonHoc { get; set; } = string.Empty;
        public string MaMonHoc { get; set; } = string.Empty;
        public string TenMonHoc { get; set; } = string.Empty;
        public int SoTinChi { get; set; }
        public string TenGiangVien { get; set; } = string.Empty;
        public string HinhThucThi { get; set; } = string.Empty;
    }
}
