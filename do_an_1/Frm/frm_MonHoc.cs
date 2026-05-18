using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using do_an_1.DAO;
using do_an_1.DTO;
using do_an_1.DTO;

namespace do_an_1.Frm
{
    public partial class frm_MonHoc : Form
    {
        private int maNguoiDung;
        private MonHocDAO dao;

        public frm_MonHoc(int maND)
        {
            InitializeComponent();
            this.maNguoiDung = maND;
            dao = new MonHocDAO();
        }

        private void frm_MonHoc_Load(object sender, EventArgs e)
        {
            LoadDanhSachMonHoc();
        }

        private void lblMoKhoTriThuc_Click(object sender, EventArgs e)
        {
        }

        #region phương thức

        private void PnlSubjectColumnName_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pnlSubjectColumeName.BackColor);

            // Border top & bottom
            Color borderColor = Color.FromArgb(210, 215, 225);
            Pen borderPen = new Pen(borderColor, 1f);
            g.DrawLine(borderPen, 0, 0, pnlSubjectColumeName.Width, 0);
            g.DrawLine(borderPen, 0, pnlSubjectColumeName.Height - 1, pnlSubjectColumeName.Width, pnlSubjectColumeName.Height - 1);

            // Font và màu
            Font font = new Font("Segoe UI", 9f, FontStyle.Bold);
            Color color = Color.FromArgb(74, 107, 191);
            SolidBrush brush = new SolidBrush(color);

            int panelWidth = pnlSubjectColumeName.Width;
            float[] colRatios = { 9f, 22f, 6f, 13f, 12f, 11f, 10f, 9f, 8f };
            string[] colNames = {
                "Mã môn", "Tên môn học", "Tín chỉ", "Giảng viên",
                "Hình thức thi", "Điểm hiện tại", "Dự báo", "Tiến độ", "Thao tác"
            };

            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            sfCenter.LineAlignment = StringAlignment.Center;

            float x = 0;
            float panelHeight = pnlSubjectColumeName.Height;

            for (int i = 0; i < colNames.Length; i++)
            {
                float colWidth = (panelWidth * colRatios[i]) / 100f;
                RectangleF rect = new RectangleF(x, 0, colWidth, panelHeight);
                g.DrawString(colNames[i], font, brush, rect, sfCenter);
                x += colWidth;
            }

            borderPen.Dispose();
            font.Dispose();
            brush.Dispose();
            sfCenter.Dispose();
        }

        private void LoadDanhSachMonHoc(string keyword = "")
        {
            flpSubjects.SuspendLayout();
            flpSubjects.Controls.Clear();
            List<MonHocDiemDTO> dsMon = dao.GetDanhSachMonHoc(maNguoiDung, keyword);
            decimal gpaMucTieu = dao.GetGpaMucTieu(maNguoiDung);
            decimal diemMucTieuThang10 = gpaMucTieu / 4.0m * 10.0m;

            if (dsMon == null || dsMon.Count == 0)
            {
                HienThiPlaceholder();
                CapNhatGPASummary();
                return;
            }

            AnPlaceholder();

            foreach (MonHocDiemDTO mon in dsMon)
            {
                // Lấy cột điểm + điểm số từ DAO
                List<CotDiemHienThiDTO> dsCotDiemHienThi = dao.GetCotDiemVaDiemSo(mon.MaMonHoc);

                // Tách ra 2 list riêng cho SubjectRowControl
                List<CotDiemDTO> dsCotDiem = new List<CotDiemDTO>();
                List<DiemSoDTO> dsDiemSo = new List<DiemSoDTO>();

                if (dsCotDiemHienThi != null)
                {
                    foreach (var item in dsCotDiemHienThi)
                    {
                        dsCotDiem.Add(new CotDiemDTO
                        {
                            MaCotDiem = item.MaCotDiem,
                            MaMonHoc = item.MaMonHoc,
                            TenCotDiem = item.TenCotDiem,
                            TrongSo = item.TrongSo,
                            ThuTu = item.ThuTu,
                            NgayTao = item.NgayTao
                        });

                        if (item.MaDiem != null)
                        {
                            dsDiemSo.Add(new DiemSoDTO
                            {
                                MaDiem = item.MaDiem.Value,
                                MaCotDiem = item.MaCotDiem,
                                GiaTri = item.GiaTri,
                                GhiChu = item.GhiChu
                            });
                        }
                    }
                }

                SubjectRowControl row = new SubjectRowControl(this.maNguoiDung);
                row.MonHoc = mon;
                row.DsCotDiem = dsCotDiem;
                row.DsDiemSo = dsDiemSo;
                row.DiemMucTieuThang10 = diemMucTieuThang10;
                row.Width = flpSubjects.Width - 5;
                row.SuaClick += Row_OnSuaClick;
                row.XoaClick += Row_OnXoaClick;


                flpSubjects.Controls.Add(row);
            }

            CapNhatGPASummary();
            flpSubjects.ResumeLayout();
        }
        private void Row_OnSuaClick(object sender, EventArgs e)
        {
            // Lấy ra chính cái UserControl vừa được nhấn nút Sửa
            SubjectRowControl clickedRow = (SubjectRowControl)sender;
            var mon = clickedRow.MonHoc;

            // Mở form add với Constructor chế độ SỬA (truyền các tham số)
            using (frm_addMonHoc frm = new frm_addMonHoc(
                maNguoiDung,
                mon.MaMonHoc,
                mon.TenMonHoc,
                mon.SoTinChi,
                mon.TenGiangVien,
                mon.HinhThucThi))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadDanhSachMonHoc(); // Load lại toàn bộ khi lưu thành công
                }
            }
        }
        private void Row_OnXoaClick(object sender, EventArgs e)
        {
            // 1. Xác định row nào vừa được nhấn Xóa
            SubjectRowControl clickedRow = (SubjectRowControl)sender;
            string maMon = clickedRow.MonHoc.MaMonHoc;
            string tenMon = clickedRow.MonHoc.TenMonHoc;

            // 2. Hiện thông báo xác nhận (đã có trong SubjectRowControl nhưng viết ở đây cho chắc)
            var confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa môn [{tenMon}] không?\nToàn bộ điểm số liên quan sẽ bị xóa vĩnh viễn!",
                "Xác nhận xóa môn học",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                // 3. Gọi DAO để xóa trong Database
                bool isSuccess = dao.DeleteMonHoc(maMon);

                if (isSuccess)
                {
                    // 4. Load lại danh sách để cập nhật giao diện
                    LoadDanhSachMonHoc();
                    // Cập nhật lại các con số GPA tổng quát
                    CapNhatGPASummary();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại! Vui lòng thử lại sau.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void HienThiPlaceholder()
        {
            foreach (Control ctrl in flpSubjects.Controls)
            {
                if (ctrl.Name == "lblPlaceholderMonHoc") return;
            }

            Label lbl = new Label()
            {
                Name = "lblPlaceholderMonHoc",
                Text = "📖 Bạn chưa thêm môn học nào.\n\nNhấn nút ➕ Thêm môn học mới để bắt đầu!",
                Font = new Font("Segoe UI", 12f),
                ForeColor = Color.FromArgb(139, 156, 192),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(flpSubjects.Width - 20, 100),
                Location = new Point(10, 50)
            };
            flpSubjects.Controls.Add(lbl);
        }

        private void AnPlaceholder()
        {
            // Duyệt ngược để tránh lỗi khi gỡ bỏ item khỏi Controls
            for (int i = flpSubjects.Controls.Count - 1; i >= 0; i--)
            {
                if (flpSubjects.Controls[i].Name == "lblPlaceholderMonHoc")
                {
                    Control ctrl = flpSubjects.Controls[i];
                    flpSubjects.Controls.Remove(ctrl);
                    ctrl.Dispose();
                }
            }
        }

        public void CapNhatGPASummary()
        {
            // Dùng DashboardDAO để đồng bộ với Dashboard
            DashboardDAO dashboardDAO = new DashboardDAO();
            decimal gpaHienTai = dashboardDAO.GetGPAHienTai(maNguoiDung);

            GpaSummaryDTO summary = dao.GetGpaSummary(maNguoiDung);
            summary.GpaHienTai = gpaHienTai; // Ghi đè bằng giá trị từ Dashboard
            decimal gpaMucTieu = dao.GetGpaMucTieu(maNguoiDung);

            // GPA Hiện tại
            lblGPAValue.Text = summary.GpaHienTai > 0 ? summary.GpaHienTai.ToString("0.00") : "--";
            lblGpaDetail.Text = $"{summary.SoMonHoc} môn học · {summary.TongTinChi} tín chỉ";

            // GPA Mục tiêu
            lblGpaTargetValue.Text = gpaMucTieu.ToString("0.00");

            if (summary.GpaHienTai > 0)
            {
                decimal gap = gpaMucTieu - summary.GpaHienTai;
                if (gap > 0)
                {
                    lblGapText.Text = $"⚠️ Còn thiếu: {gap:0.00}";
                    pnlGPAGap.BackColor = Color.FromArgb(255, 152, 0);
                }
                else
                {
                    lblGapText.Text = "✅ Đã đạt mục tiêu!";
                    pnlGPAGap.BackColor = Color.FromArgb(76, 175, 80);
                }
            }
            else
            {
                lblGapText.Text = "Chưa có điểm";
                pnlGPAGap.BackColor = Color.FromArgb(200, 200, 200);
            }

            // Tỷ lệ hoàn thành
            lblCompletionValue.Text = $"{summary.TyLeHoanThanh:F0}%";
            lblCompletionDetail.Text = $"Còn {summary.SoCotDiemTrong} cột điểm trống";
            this.Refresh(); // Buộc form vẽ lại toàn bộ các nhãn điểm số mới
        }

        private void btnThemMonHoc_Click(object sender, EventArgs e)
        {
            using (frm_addMonHoc frm = new frm_addMonHoc(maNguoiDung))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadDanhSachMonHoc(); // Load lại danh sách sau khi thêm thành công
                }
            }
        }

        #endregion

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Khai báo biến keyword ở đây
            string keyword = txtSearch.Text.Trim();

            // Truyền nó vào hàm Load
            LoadDanhSachMonHoc(keyword);
        }

        private void btnMenuDashboard_Click(object sender, EventArgs e)
        {
            // Mở form Dashboard
            frm_Dashboard dashboardForm = new frm_Dashboard(maNguoiDung);
            dashboardForm.Show();

            this.Close(); // Đóng form môn học sau khi mở Dashboard
        }

        private void btnBackToDashboard_Click(object sender, EventArgs e)
        {
            //giống hàm trên nhưng đặt tên khác cho dễ hiểu
            frm_Dashboard dashboardForm = new frm_Dashboard(maNguoiDung);
            dashboardForm.Show();
            this.Close();
        }

        private void pnlSubjectList_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}