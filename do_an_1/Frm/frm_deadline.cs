using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using do_an_1.DAO;
using do_an_1.DTO;
using Guna.UI2.WinForms;

namespace do_an_1.Frm
{
    public partial class frm_deadline : Form
    {
        private int maNguoiDung;
        private DeadlineDAO deadlineDAO = new DeadlineDAO();

        public frm_deadline(int maND)
        {
            InitializeComponent();
            this.maNguoiDung = maND;

            // Tối ưu hóa UI: Chống giật lag khi cuộn
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        private void frm_deadline_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            LoadThongKe();
            LoadEisenhowerMatrix();
            InitTableHeaders();

            flpDeadLine.Controls.Clear();
            // Quan trọng: Dùng hàm lấy TẤT CẢ deadline
            List<NhiemVuEisenhowerDTO> ds = deadlineDAO.GetAllDeadline(maNguoiDung);

            if (ds != null)
            {
                foreach (var nv in ds)
                {
                    flpDeadLine.Controls.Add(CreateDeadlineRow(nv));
                }
            }


        }

        private void LoadThongKe()
        {
            int tong = deadlineDAO.GetCountByProc("sp_GetSoDeadline", maNguoiDung);
            int doHan = deadlineDAO.GetCountByProc("sp_GetSoDeadlineDo", maNguoiDung);

            // Gán giá trị lên nhãn HTML (Guna2HtmlLabel hỗ trợ định dạng tốt)
            lbl_tongDeadlineValue.Text = tong.ToString();
            lbl_tongDeadlineDoValue.Text = doHan.ToString();
            lbl_TongDeadlineDenHanValue.Text = tong.ToString();

            // Logic màu sắc áp lực
            if (doHan > 5)
            {
                lbl_mucDoApLuc.Text = "Cực kỳ áp lực";
                lbl_mucDoApLuc.ForeColor = Color.Crimson;
            }
            else if (doHan > 0)
            {
                lbl_mucDoApLuc.Text = "Áp lực cao";
                lbl_mucDoApLuc.ForeColor = Color.OrangeRed;
            }
            else
            {
                lbl_mucDoApLuc.Text = "Thoải mái";
                lbl_mucDoApLuc.ForeColor = Color.MediumSeaGreen;
            }
        }

        private void LoadEisenhowerMatrix()
        {
            // Xóa sạch các control cũ trong 4 ô ma trận
            flpQ1.Controls.Clear(); flpQ2.Controls.Clear();
            flpQ3.Controls.Clear(); flpQ4.Controls.Clear();

            List<NhiemVuEisenhowerDTO> dsNhiemVu = deadlineDAO.GetNhiemVuChoEisenhower(maNguoiDung);

            foreach (var nv in dsNhiemVu)
            {
                // 1. Tính toán thời gian
                TimeSpan timeRemaining = nv.ThoiHan - DateTime.Now;
                double totalDays = timeRemaining.TotalDays;

                // QUY TẮC MỚI: Gấp nếu còn <= 2 ngày HOẶC đã quá hạn (totalDays < 0)
                bool isUrgent = totalDays <= 2;
                bool isImportant = nv.SoTinChi >= 3;

                // 2. Xác định Panel và Màu sắc
                Color qColor;
                FlowLayoutPanel targetPanel;

                if (isImportant && isUrgent) { qColor = Color.FromArgb(239, 68, 68); targetPanel = flpQ1; } // Đỏ
                else if (isImportant) { qColor = Color.FromArgb(245, 158, 11); targetPanel = flpQ2; }      // Cam
                else if (isUrgent) { qColor = Color.FromArgb(59, 130, 246); targetPanel = flpQ3; }         // Xanh dương
                else { qColor = Color.FromArgb(156, 163, 175); targetPanel = flpQ4; }                      // Xám

                // 3. Tạo chuỗi hiển thị thời gian (Xử lý logic hiển thị)
                string timeText = "";
                if (totalDays < 0)
                {
                    timeText = "Quá hạn!";
                }
                else if (totalDays < 1) // Trong ngày hôm nay
                {
                    timeText = "Hạn: " + nv.ThoiHan.ToString("HH:mm");
                }
                else
                {
                    timeText = $"Còn {(int)Math.Ceiling(totalDays)} ngày";
                }

                // 4. Thêm Item vào UI (Truyền thêm biến timeText vào hàm tạo)
                targetPanel.Controls.Add(CreateTaskItem(nv, qColor, timeText));
            }
        }

        private Control CreateTaskItem(NhiemVuEisenhowerDTO nv, Color accentColor, string time)
        {
            // 1. Tính toán thời gian thực tế
            TimeSpan timeRemaining = nv.ThoiHan - DateTime.Now;
            double totalDays = timeRemaining.TotalDays;

            string displayTime = "";
            Color timeTextColor = Color.DimGray; // Mặc định là xám

            // 2. Logic hiển thị nội dung và màu sắc thời gian
            if (totalDays < 0)
            {
                displayTime = "⚠ Quá hạn!";
                timeTextColor = Color.Red; // Quá hạn thì cho chữ đỏ rực
            }
            else if (totalDays <= 1)
            {
                // Hạn trong ngày: Hiện giờ phút
                displayTime = "Hạn: Hôm nay " + nv.ThoiHan.ToString("HH:mm");
                timeTextColor = accentColor; // Đổi sang màu của phân loại (Đỏ/Cam...)
            }
            else
            {
                // Hạn còn nhiều ngày: Hiện số ngày còn lại
                displayTime = "Còn " + (int)Math.Ceiling(totalDays) + " ngày nữa";
                timeTextColor = Color.DimGray;
            }

            // --- UI SETUP ---
            Guna2Panel pnl = new Guna2Panel();
            pnl.Size = new Size(flpQ1.Width - 25, 55);
            pnl.Margin = new Padding(8, 4, 8, 4);
            pnl.FillColor = Color.White;
            pnl.BorderRadius = 8;

            // Tạo border trái dày để phân biệt loại
            pnl.CustomBorderThickness = new Padding(5, 0, 0, 0);
            pnl.CustomBorderColor = accentColor;

            // Bóng đổ nhẹ
            pnl.ShadowDecoration.Enabled = true;
            pnl.ShadowDecoration.Depth = 5;
            pnl.ShadowDecoration.Color = Color.FromArgb(200, 200, 200);

            // Tiêu đề
            Label lblTitle = new Label();
            lblTitle.Text = nv.TieuDe;
            lblTitle.Font = new Font("Segoe UI Semibold", 9.5F);
            lblTitle.Location = new Point(15, 8);
            lblTitle.AutoSize = true;

            // Thời gian (SỬ DỤNG LOGIC MỚI)
            Label lblTime = new Label();
            lblTime.Text = displayTime;
            lblTime.Font = new Font("Segoe UI Semibold", 8F); // Cho đậm lên chút cho dễ nhìn
            lblTime.ForeColor = timeTextColor;
            lblTime.Location = new Point(15, 30);
            lblTime.AutoSize = true;

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblTime);

            // Hiệu ứng Click
            pnl.Cursor = Cursors.Hand;
            pnl.Click += (s, e) =>
            {
                MessageBox.Show($"Nhiệm vụ: {nv.TieuDe}\nHạn chót: {nv.ThoiHan:dd/MM/yyyy HH:mm}", "Thông tin chi tiết");
            };

            return pnl;
        }

        private void InitTableHeaders()
        {
            pnlDeadlineColumeName.Controls.Clear();
            pnlDeadlineColumeName.Height = 40;
            pnlDeadlineColumeName.FillColor = Color.FromArgb(242, 245, 250); // Màu nền xám nhạt chuyên nghiệp
            pnlDeadlineColumeName.BorderRadius = 8;

            // Định nghĩa các cột: Tên - Tỉ lệ % độ rộng
            var columns = new[] {
        new { Name = "Tiêu đề",  Width = 0.30f },
        new { Name = "Môn học",  Width = 0.15f },
        new { Name = "Thời hạn",  Width = 0.15f },
        new { Name = "Trạng thái", Width = 0.15f },
        new { Name = "Phân loại", Width = 0.15f },
        new { Name = "Thao tác",  Width = 0.10f }
    };

            int currentX = 20; // Lề trái
            int totalWidth = pnlDeadlineColumeName.Width - 40;

            foreach (var col in columns)
            {
                int colWidth = (int)(totalWidth * col.Width);
                Label lbl = new Label
                {
                    Text = col.Name.ToUpper(),
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(125, 137, 149),
                    AutoSize = false,
                    Size = new Size(colWidth, pnlDeadlineColumeName.Height),
                    Location = new Point(currentX, 0),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                if (col.Name == "Thao tác") lbl.TextAlign = ContentAlignment.MiddleCenter;

                pnlDeadlineColumeName.Controls.Add(lbl);
                currentX += colWidth;
            }
        }
        // Hàm phân loại Eisenhower (dùng lại logic đã có)
        private string PhanLoaiEisenhower(DateTime thoiHan, int soTinChi)
        {
            double daysRemaining = (thoiHan - DateTime.Now).TotalDays;
            bool isUrgent = daysRemaining <= 2; // Gấp: hạn trong 2 ngày
            bool isImportant = soTinChi >= 3; // Quan trọng: môn >= 3 tín chỉ
            if (isImportant && isUrgent) return "Q1";
            else if (isImportant) return "Q2";
            else if (isUrgent) return "Q3";
            else return "Q4";
        }



        public void RefreshData()
        {
            // Gọi các hàm đã làm ở bước trước
            LoadThongKe();
            LoadEisenhowerMatrix();

            // 1. Vẽ tiêu đề bảng
            InitTableHeaders();

            // 2. Đổ dữ liệu vào bảng danh sách
            flpDeadLine.Controls.Clear();
            List<NhiemVuEisenhowerDTO> ds = deadlineDAO.GetNhiemVuChoEisenhower(maNguoiDung);

            foreach (var nv in ds)
            {
                flpDeadLine.Controls.Add(CreateDeadlineRow(nv));
            }
        }




        private Guna2Panel CreateDeadlineRow(NhiemVuEisenhowerDTO nv)
        {
            // ... (Giữ nguyên phần khởi tạo Guna2Panel row) ...
            Guna2Panel row = new Guna2Panel
            {
                Size = new Size(flpDeadLine.Width - 25, 55),
                Margin = new Padding(0, 0, 0, 6),
                FillColor = Color.White,
                BorderRadius = 10,
            };

            int totalW = row.Width - 40;
            int currentX = 20;

            // Biến chặn sự kiện (Flag)
            bool isLoading = true;

            // --- 1. CỘT TIÊU ĐỀ ---
            Guna2TextBox txtTieuDe = new Guna2TextBox
            {
                Text = nv.TieuDe,
                Size = new Size((int)(totalW * 0.30f) - 10, 36),
                Location = new Point(currentX, 9),
                BorderThickness = 0,
                FillColor = Color.White
            };

            // Hiệu ứng gạch ngang
            if (nv.TrangThai == "DA_HOAN_THANH")
            {
                txtTieuDe.Font = new Font("Segoe UI", 9F, FontStyle.Strikeout);
                txtTieuDe.ForeColor = Color.Gray;
            }
            else
            {
                txtTieuDe.Font = new Font("Segoe UI Semibold", 9F);
                txtTieuDe.ForeColor = Color.Black;
            }

            txtTieuDe.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    deadlineDAO.UpdateNhiemVuNhanh(nv.MaNhiemVu, "TIEU_DE", txtTieuDe.Text);
                    row.Focus();
                }
            };
            currentX += (int)(totalW * 0.30f);


            // --- CỘT MÔN HỌC ---
            Guna2ComboBox cboMonHoc = new Guna2ComboBox
            {
                Size = new Size((int)(totalW * 0.15f) - 10, 32),
                Location = new Point(currentX, 11),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // [QUAN TRỌNG NHẤT] Ép khởi tạo BindingContext trước khi gán DataSource
            // Để WinForms biết mà nạp Items ngay lập tức
            cboMonHoc.BindingContext = new BindingContext();

            DataTable dtMon = deadlineDAO.GetDanhSachMonHoc(maNguoiDung);

            // Kiểm tra kỹ bảng dữ liệu
            if (dtMon != null && dtMon.Rows.Count > 0)
            {
                cboMonHoc.DataSource = dtMon;

                // Sửa DISPLAY_NAME thành TEN_MON_HOC cho khớp với cột trong Database
                cboMonHoc.DisplayMember = "DISPLAY_NAME";
                cboMonHoc.ValueMember = "MA_MON_HOC";

                // Gán giá trị an toàn
                if (string.IsNullOrEmpty(nv.MaMonHoc))
                {
                    // Check an toàn 1 lần nữa trước khi gán Index
                    if (cboMonHoc.Items.Count > 0)
                    {
                        cboMonHoc.SelectedIndex = 0;
                    }
                }
                else
                {
                    cboMonHoc.SelectedValue = nv.MaMonHoc;
                }
            }
            else
            {
                // Trường hợp xấu nhất (SQL lỗi/mất kết nối): Thêm cứng 1 item để không crash
                cboMonHoc.Items.Add("-- Trống --");
                if (cboMonHoc.Items.Count > 0)
                {
                    cboMonHoc.SelectedIndex = 0;
                }
            }

            // QUAN TRỌNG: Chỉ đăng ký sự kiện SAU KHI đã gán xong dữ liệu ban đầu
            cboMonHoc.SelectedIndexChanged += (s, e) =>
            {
                // Chỉ chạy logic khi NGƯỜI DÙNG tương tác thực sự (có Focus)
                if (cboMonHoc.Focused)
                {
                    object val = (cboMonHoc.SelectedValue == DBNull.Value) ? null : cboMonHoc.SelectedValue;
                    deadlineDAO.UpdateNhiemVuNhanh(nv.MaNhiemVu, "MA_MON_HOC", val);
                    // Thay vì gọi LoadData() gây vẽ lại toàn bộ, bạn có thể chỉ update Model local
                    // hoặc nếu muốn vẽ lại thì phải cực kỳ cẩn thận với loop.
                    LoadData();
                }
            };
            currentX += (int)(totalW * 0.15f);

            // --- 3. CỘT THỜI HẠN (DateTimePicker) ---
            Guna2DateTimePicker dtpHan = new Guna2DateTimePicker
            {
                Size = new Size((int)(totalW * 0.15f) - 10, 32),
                Location = new Point(currentX, 11),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Value = nv.ThoiHan,
                FillColor = Color.White
            };

            // SỬ DỤNG SỰ KIỆN NÀY ĐỂ CHẮC CHẮN LƯU SAU KHI CHỌN XONG
            dtpHan.ValueChanged += (s, e) =>
            {
                // Lưu ngay lập tức khi giá trị thay đổi
                bool success = deadlineDAO.UpdateNhiemVuNhanh(nv.MaNhiemVu, "THOI_HAN", dtpHan.Value);

                if (success)
                {
                    // Debug thử xem nó có chạy vào đây không
                    Console.WriteLine("Lưu ngày thành công!");
                    LoadData(); // LoadData sẽ vẽ lại toàn bộ flpDeadLine
                }
            };
            currentX += (int)(totalW * 0.15f);

            // --- 4. CỘT TRẠNG THÁI ---
            Guna2ComboBox cboStatus = new Guna2ComboBox
            {
                Size = new Size((int)(totalW * 0.15f) - 10, 32),
                Location = new Point(currentX, 11),
                Font = new Font("Segoe UI", 8F),
                BorderRadius = 5
            };
            cboStatus.Items.AddRange(new object[] { "CHƯA_HOAN_THANH", "DA_HOAN_THANH" });
            cboStatus.Text = nv.TrangThai;

            cboStatus.SelectedIndexChanged += (s, e) =>
            {
                // CHỈ CHẠY KHI ĐÃ LOAD XONG VÀ USER CLICK
                if (!isLoading && cboStatus.Focused)
                {
                    deadlineDAO.UpdateNhiemVuNhanh(nv.MaNhiemVu, "TRANG_THAI", cboStatus.Text);
                    LoadData();
                }
            };
            currentX += (int)(totalW * 0.15f);

            // ... (Cột phân loại và nút Xóa giữ nguyên như code của bạn) ...
            // --- 5. CỘT PHÂN LOẠI ---
            string q = PhanLoaiEisenhower(nv.ThoiHan, nv.SoTinChi);
            Label lblQ = new Label
            {
                Text = q,
                Size = new Size((int)(totalW * 0.15f), 55),
                Location = new Point(currentX, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Bold", 9F)
            };
            currentX += lblQ.Width;

            Guna2Button btnDel = new Guna2Button
            {
                Text = "Xóa",
                Size = new Size((int)(totalW * 0.10f), 30),
                Location = new Point(currentX, 12),
                FillColor = Color.Transparent,
                ForeColor = Color.Crimson,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDel.Click += (s, e) =>
            {
                if (MessageBox.Show("Xác nhận xóa?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (deadlineDAO.DeleteNhiemVu(nv.MaNhiemVu)) LoadData();
                }
            };

            row.Controls.Add(txtTieuDe);
            row.Controls.Add(cboMonHoc);
            row.Controls.Add(dtpHan);
            row.Controls.Add(cboStatus);
            row.Controls.Add(lblQ);
            row.Controls.Add(btnDel);

            // QUAN TRỌNG NHẤT: Tắt flag load để bắt đầu nhận sự kiện từ người dùng
            isLoading = false;

            return row;
        }

        private void btn_ThemDeadline_Click(object sender, EventArgs e)
        {
            frm_addDeadline addForm = new frm_addDeadline(this.maNguoiDung);
            // Đăng ký nhận thông báo khi thêm thành công
            addForm.OnSaveSuccess = () =>
            {
                this.LoadData(); // Gọi lại hàm LoadData của frm_deadline
            };

            addForm.ShowDialog(); // Mở dưới dạng Dialog

        }

        private void btnMenuDashboard_Click(object sender, EventArgs e)
        {
         
            this.Close();
        }
    }

}