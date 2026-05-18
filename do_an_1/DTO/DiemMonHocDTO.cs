using System;

namespace do_an_1.DTO
{
    /// <summary>
    /// DTO chứa điểm tổng hợp từng môn học
    /// Dùng cho bảng điểm trong PDF báo cáo
    /// </summary>
    public class DiemMonHocDTO
    {
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public int SoTinChi { get; set; }
        public string HocKy { get; set; }

        /// <summary>Điểm trung bình có trọng số = SUM(Điểm × TrongSo%) </summary>
        public double DiemTrungBinh { get; set; }

        /// <summary>Số cột điểm đã nhập / tổng số cột</summary>
        public int SoCotDaNhap { get; set; }
        public int TongSoCot { get; set; }

        /// <summary>
        /// Xếp loại theo thang điểm 10:
        /// >= 9.0 → Xuất sắc | >= 8.0 → Giỏi | >= 7.0 → Khá
        /// >= 5.0 → Trung bình | &lt; 5.0 → Yếu | chưa đủ cột → Chưa hoàn thành
        /// </summary>
        public string XepLoai
        {
            get
            {
                if (SoCotDaNhap < TongSoCot) return "Chưa hoàn thành";
                if (DiemTrungBinh >= 9.0) return "Xuất sắc";
                if (DiemTrungBinh >= 8.0) return "Giỏi";
                if (DiemTrungBinh >= 7.0) return "Khá";
                if (DiemTrungBinh >= 5.0) return "Trung bình";
                return "Yếu";
            }
        }

        /// <summary>Điểm chữ hệ 4 (theo Bộ GD&ĐT)</summary>
        public string DiemChu
        {
            get
            {
                if (SoCotDaNhap < TongSoCot) return "--";
                if (DiemTrungBinh >= 9.0) return "A+";
                if (DiemTrungBinh >= 8.5) return "A";
                if (DiemTrungBinh >= 8.0) return "B+";
                if (DiemTrungBinh >= 7.0) return "B";
                if (DiemTrungBinh >= 6.5) return "C+";
                if (DiemTrungBinh >= 5.5) return "C";
                if (DiemTrungBinh >= 5.0) return "D+";
                if (DiemTrungBinh >= 4.0) return "D";
                return "F";
            }
        }

        /// <summary>Điểm hệ 4</summary>
        public double DiemHe4
        {
            get
            {
                if (SoCotDaNhap < TongSoCot) return 0;
                if (DiemTrungBinh >= 9.0) return 4.0;
                if (DiemTrungBinh >= 8.5) return 3.7;
                if (DiemTrungBinh >= 8.0) return 3.5;
                if (DiemTrungBinh >= 7.0) return 3.0;
                if (DiemTrungBinh >= 6.5) return 2.5;
                if (DiemTrungBinh >= 5.5) return 2.0;
                if (DiemTrungBinh >= 5.0) return 1.5;
                if (DiemTrungBinh >= 4.0) return 1.0;
                return 0;
            }
        }
    }
}