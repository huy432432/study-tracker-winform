using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace do_an_1.DTO
    {
        /// <summary>
        /// DTO dùng để hiển thị cột điểm + giá trị điểm (nếu có)
        /// Dùng trong form frmMonHoc khi expand chi tiết môn học
        /// </summary>
        public class CotDiemHienThiDTO
        {
            public int MaCotDiem { get; set; }
            public string MaMonHoc { get; set; }
            public string TenCotDiem { get; set; }
            public decimal TrongSo { get; set; }
            public int ThuTu { get; set; }
            public DateTime NgayTao { get; set; }

            // Trường từ DIEM_SO (có thể null nếu chưa có điểm)
            public int? MaDiem { get; set; }
            public decimal? GiaTri { get; set; }
            public string GhiChu { get; set; }
        }
    }
