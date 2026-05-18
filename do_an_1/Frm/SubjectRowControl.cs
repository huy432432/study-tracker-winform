using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using do_an_1.DAO;
using do_an_1.DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace do_an_1.Frm
{
    public partial class SubjectRowControl : UserControl
    {
        // ============================================
        // DỮ LIỆU
        // ============================================
        private MonHocDAO monHocDao = new MonHocDAO();
        private int maNguoiDung; // Không gán cứng = 1 nữa
        public MonHocDiemDTO MonHoc { get; set; }
        public List<CotDiemDTO> DsCotDiem { get; set; }
        public List<DiemSoDTO> DsDiemSo { get; set; }
        public decimal DiemMucTieuThang10 { get; set; } // Từ GPA mục tiêu / 4 * 10

        // Trạng thái expand
        private bool isExpanded = false;
        private Panel pnlExpand;
        private int expandHeight = 230;

        // Màu sắc dùng chung
        private Color primaryColor = Color.FromArgb(74, 107, 191);
        private Color textColor = Color.FromArgb(30, 42, 74);
        private Color dangerColor = Color.FromArgb(224, 85, 85);
        private Color successColor = Color.FromArgb(52, 169, 120);
        private Color warningColor = Color.FromArgb(232, 168, 64);
        private Color borderColor = Color.FromArgb(210, 215, 225);

        private Button btnSua;
        private Button btnXoa;
      
        public event EventHandler SuaClick;
        public event EventHandler XoaClick;

        public SubjectRowControl(int maND)
        {
            InitializeComponent();
            this.maNguoiDung = maND; // Gán giá trị được truyền vào cho biến local
            SetupControl();
        }

        private void SetupControl()
        {
            this.Size = new Size(950, 55);
            this.BackColor = Color.White;
            this.Cursor = Cursors.Hand;
            // THÊM DÒNG NÀY - gắn sự kiện click
            this.Click += SubjectRowControl_Click;
            this.Paint += SubjectRowControl_Paint;

            this.MouseMove += SubjectRowControl_MouseMove;
            CreateExpandPanel();
            CreateActionButtons();
        }

        private void SubjectRowControl_MouseMove(object sender, MouseEventArgs e)
        {
            // Đổi cursor khi hover vào vùng header
            if (e.Y <= 55 && e.Y >= 0 && e.X >= 0 && e.X <= this.Width)
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        // ============================================
        // TÍNH TOÁN (ĐỒNG BỘ VỚI DASHBOARD)
        // ============================================

        /// <summary>
        /// Tính điểm cần đạt - GIỐNG HỆT hàm DuBaoDiemMonHoc trong Dashboard
        /// </summary>
        private decimal TinhDiemCanDat()
        {
            if (MonHoc == null || MonHoc.TrongSoConLai <= 0) return 0;

            decimal diemTichLuy = MonHoc.DiemHienTai ?? 0;   // Ví dụ: 2.8
            decimal trongSoConLai = MonHoc.TrongSoConLai.Value; // Ví dụ: 60
            decimal mucTieu = DiemMucTieuThang10;            // Ví dụ: 9.0

            // CÔNG THỨC: (Đích - Hiện tại) / % Còn lại
            // Ví dụ: (9.0 - 2.8) / 0.6 = 10.33
            decimal diemCan = (mucTieu - diemTichLuy) / (trongSoConLai / 100m);

            if (diemCan > 10) return 10.1m; // Đánh dấu không thể đạt được
            if (diemCan < 0) return 0;

            return Math.Round(diemCan, 1);
        }

        /// <summary>
        /// Lấy text dự báo - GIỐNG HỆT Dashboard
        /// </summary>
        private string GetDuBaoText()
        {
            if (MonHoc == null) return "";

            decimal? diemHienTai = MonHoc.DiemHienTai;
            decimal? trongSoConLai = MonHoc.TrongSoConLai;

            if (diemHienTai == null)
                return "Chưa có điểm";
            if (trongSoConLai == null || trongSoConLai == 0)
                return "Đủ điểm thành phần";

            decimal diemCan = TinhDiemCanDat();

            if (diemCan > 10)
                return $"Không khả thi (cần {diemCan:F1}>10)";
            else if (diemCan <= 0)
                return "Đã vượt mục tiêu";
            else if (diemCan <= DiemMucTieuThang10 * 0.7m)
                return $"An toàn (cần ≥{diemCan:F1})";
            else if (diemCan <= DiemMucTieuThang10 * 1.1m)
                return $"Cần ≥{diemCan:F1}";
            else
                return $"Khó (cần ≥{diemCan:F1})";
        }

        /// <summary>
        /// Lấy màu chữ dự báo
        /// </summary>
        private Color GetDuBaoColor()
        {
            string text = GetDuBaoText();
            if (text.Contains("❌")) return dangerColor;
            if (text.Contains("✅")) return successColor;
            return warningColor;
        }

        /// <summary>
        /// Tính % tiến độ = 100 - trọng số còn lại
        /// </summary>
        private int GetTienDo()
        {
            if (MonHoc?.TrongSoConLai == null) return 0;
            return (int)(100 - MonHoc.TrongSoConLai.Value);
        }

        /// <summary>
        /// Lấy màu progress bar
        /// </summary>
        private Color GetProgressColor(int tienDo)
        {
            if (tienDo >= 80) return successColor;
            if (tienDo >= 50) return warningColor;
            return dangerColor;
        }

        /// <summary>
        /// Lấy điểm hiển thị (nếu null thì 0)
        /// </summary>
        private decimal GetDiemHienTai()
        {
            return MonHoc?.DiemHienTai ?? 0;
        }

        // ============================================
        // TẠO BUTTON THAO TÁC THẬT
        // ============================================
        private void CreateActionButtons()
        {
            btnSua = new Button()
            {
                Text = "✏️",
                Font = new Font("Segoe UI", 9f),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(32, 28),
                BackColor = Color.FromArgb(240, 244, 255),
                ForeColor = primaryColor,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Paint += BtnSua_Paint;
            btnSua.Click += BtnSua_Click;
            this.Controls.Add(btnSua);

            btnXoa = new Button()
            {
                Text = "🗑️",
                Font = new Font("Segoe UI", 9f),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(32, 28),
                BackColor = Color.FromArgb(254, 242, 242),
                ForeColor = dangerColor,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Paint += BtnXoa_Paint;
            btnXoa.Click += BtnXoa_Click;
            this.Controls.Add(btnXoa);
           
        }

        // Handler riêng cho từng nút
        private void BtnSua_Click(object sender, EventArgs e)
        {
            OnSuaClick(e);
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            // 1. Hỏi xác nhận để tránh bấm nhầm
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa môn học này và toàn bộ điểm số liên quan không?",
                                        "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // 2. Kích hoạt Event (Đã khai báo ở các bước trước)
                // Lưu ý: Sử dụng đúng tên Event bạn đã đặt (ví dụ: XoaClick)
                XoaClick?.Invoke(this, e);
            }
        }


        // ============================================
        // PAINT BO GÓC CHO BUTTON
        // ============================================
        private void BtnSua_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            int radius = 6;
            Color borderColor = Color.FromArgb(180, 200, 230);
            Color bgColor = btn.BackColor;

            // Vẽ nền bo góc
            using (GraphicsPath path = RoundedRect(0, 0, btn.Width, btn.Height, radius))
            using (SolidBrush bgBrush = new SolidBrush(bgColor))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillPath(bgBrush, path);
            }

            // Vẽ viền
            using (GraphicsPath borderPath = RoundedRect(0, 0, btn.Width - 1, btn.Height - 1, radius))
            using (Pen borderPen = new Pen(borderColor, 1f))
            {
                e.Graphics.DrawPath(borderPen, borderPath);
            }

            // Vẽ chữ
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font,
                                  new Rectangle(0, 0, btn.Width, btn.Height),
                                  btn.ForeColor,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void BtnXoa_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            int radius = 6;
            Color borderColor = Color.FromArgb(240, 200, 200);
            Color bgColor = btn.BackColor;

            using (GraphicsPath path = RoundedRect(0, 0, btn.Width, btn.Height, radius))
            using (SolidBrush bgBrush = new SolidBrush(bgColor))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillPath(bgBrush, path);
            }

            using (GraphicsPath borderPath = RoundedRect(0, 0, btn.Width - 1, btn.Height - 1, radius))
            using (Pen borderPen = new Pen(borderColor, 1f))
            {
                e.Graphics.DrawPath(borderPen, borderPath);
            }

            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font,
                                  new Rectangle(0, 0, btn.Width, btn.Height),
                                  btn.ForeColor,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // ============================================
        // VẼ HÀNG MÔN HỌC
        // ============================================
        private void SubjectRowControl_Paint(object sender, PaintEventArgs e)
        {
            if (MonHoc == null) return;

            Graphics g = e.Graphics;
            int w = this.Width;
            int headerH = 55; // LUÔN CỐ ĐỊNH 55 cho header

            // Nền - dùng màu hiện tại của control
            g.Clear(this.BackColor);

            // Border bottom (chỉ ở vị trí 55)
            using (Pen borderPen = new Pen(borderColor, 1f))
            {
                g.DrawLine(borderPen, 0, headerH - 1, w, headerH - 1);
            }

            // Border trái khi expand
            if (isExpanded)
            {
                using (Pen leftPen = new Pen(primaryColor, 3f))
                {
                    g.DrawLine(leftPen, 1, 0, 1, headerH);
                }
            }

            // Font
            Font boldFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            Font regularFont = new Font("Segoe UI", 9f, FontStyle.Regular);
            Font smallFont = new Font("Segoe UI", 8f, FontStyle.Regular);

            SolidBrush primaryBrush = new SolidBrush(primaryColor);
            SolidBrush textBrush = new SolidBrush(textColor);

            // Tỉ lệ cột
            float[] colRatios = { 9f, 22f, 6f, 13f, 12f, 11f, 10f, 9f, 8f };

            StringFormat sfCenter = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            float x = 0;
            float y = 0;

            // TẤT CẢ CÁC CỘT DÙNG headerH (55) THAY VÌ h
            // === CỘT 0: MÃ MÔN ===
            float colW0 = (w * colRatios[0]) / 100f;
            g.DrawString(MonHoc.MaMonHoc ?? "", boldFont, primaryBrush,
                        new RectangleF(x, y, colW0, headerH), sfCenter);
            x += colW0;

            // === CỘT 1: TÊN MÔN HỌC ===
            float colW1 = (w * colRatios[1]) / 100f;
            string tenMon = CatChuVuaO(g, MonHoc.TenMonHoc ?? "",
                                       new Font("Segoe UI", 9f, FontStyle.Bold), colW1 - 10);
            g.DrawString(tenMon, boldFont, textBrush,
                        new RectangleF(x + 5, y, colW1 - 10, headerH), sfCenter);
            x += colW1;

            // === CỘT 2: TÍN CHỈ ===
            float colW2 = (w * colRatios[2]) / 100f;
            g.DrawString(MonHoc.SoTinChi.ToString(), regularFont, textBrush,
                        new RectangleF(x, y, colW2, headerH), sfCenter);
            x += colW2;

            // === CỘT 3: GIẢNG VIÊN ===
            float colW3 = (w * colRatios[3]) / 100f;
            string gv = CatChuVuaO(g, MonHoc.TenGiangVien ?? "",
                                  new Font("Segoe UI", 8f), colW3 - 6);
            g.DrawString(gv, smallFont, textBrush,
                        new RectangleF(x + 3, y, colW3 - 6, headerH), sfCenter);
            x += colW3;

            // === CỘT 4: HÌNH THỨC THI (BADGE) ===
            float colW4 = (w * colRatios[4]) / 100f;
            VeBadge(g, MonHoc.HinhThucThi ?? "", x, y, colW4, headerH, sfCenter);
            x += colW4;

            // === CỘT 5: ĐIỂM HIỆN TẠI ===
            float colW5 = (w * colRatios[5]) / 100f;
            decimal diemHT = GetDiemHienTai();
            string diemText = MonHoc.DiemHienTai != null ? $"{diemHT:F1}/10" : "--/10";
            Color diemColor = diemHT < 5.0m ? dangerColor :
                             diemHT < 7.0m ? warningColor : successColor;
            using (SolidBrush diemBrush = new SolidBrush(diemColor))
            {
                g.DrawString(diemText, boldFont, diemBrush,
                            new RectangleF(x, y, colW5, headerH), sfCenter);
            }
            x += colW5;

            // === CỘT 6: DỰ BÁO ===
            float colW6 = (w * colRatios[6]) / 100f;
            string duBaoText = GetDuBaoText();
            Color duBaoColor = GetDuBaoColor();
            using (SolidBrush duBaoBrush = new SolidBrush(duBaoColor))
            {
                g.DrawString(duBaoText, smallFont, duBaoBrush,
                            new RectangleF(x + 2, y, colW6 - 4, headerH), sfCenter);
            }
            x += colW6;

            // === CỘT 7: TIẾN ĐỘ (PROGRESS BAR) ===
            float colW7 = (w * colRatios[7]) / 100f;
            int tienDo = GetTienDo();
            Color barColor = GetProgressColor(tienDo);
            VeProgressBar(g, tienDo, barColor, x, y, colW7, headerH, smallFont, textBrush, sfCenter);
            x += colW7;

            // === CỘT 8: CẬP NHẬT VỊ TRÍ BUTTON ===
            float colW8 = (w * colRatios[8]) / 100f;
            CapNhatViTriButton(x, colW8, headerH);


            // Giải phóng
            boldFont.Dispose();
            regularFont.Dispose();
            smallFont.Dispose();
            primaryBrush.Dispose();
            textBrush.Dispose();
            sfCenter.Dispose();
        }

        // ============================================
        // CẬP NHẬT VỊ TRÍ BUTTON THAO TÁC
        // ============================================
        private void CapNhatViTriButton(float cellX, float cellW, float cellH)
        {
            if (btnSua == null || btnXoa == null) return;

            btnSua.Visible = true;
            btnXoa.Visible = true;

            float btnW = 32;
            float btnH = 28;
            float btnY = (int)((cellH - btnH) / 2);

            // Nút Sửa - căn giữa 2 nút trong ô
            float tongBtnW = btnW * 2 + 6; // 2 nút + khoảng cách
            float startX = cellX + (cellW - tongBtnW) / 2;

            btnSua.Location = new Point((int)startX, (int)btnY);
            btnSua.Size = new Size((int)btnW, (int)btnH);

            btnXoa.Location = new Point((int)(startX + btnW + 6), (int)btnY);
            btnXoa.Size = new Size((int)btnW, (int)btnH);

            // Bring to front để không bị che
            btnSua.BringToFront();
            btnXoa.BringToFront();
        }


        // ============================================
        // HELPER: Cắt chữ vừa ô
        // ============================================
        private string CatChuVuaO(Graphics g, string text, Font font, float maxWidth)
        {
            if (string.IsNullOrEmpty(text)) return "";
            SizeF size = g.MeasureString(text, font);
            if (size.Width <= maxWidth) return text;

            string result = text;
            while (result.Length > 0 && g.MeasureString(result + "...", font).Width > maxWidth)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result + "...";
        }

        // ============================================
        // HELPER: Vẽ Badge hình thức thi (có bo góc)
        // ============================================
        private void VeBadge(Graphics g, string hinhThuc, float cellX, float cellY,
                             float cellW, float cellH, StringFormat sf)
        {
            Color bg, fg;
            switch (hinhThuc.ToLower())
            {
                case "trắc_nghiệm":
                    bg = Color.FromArgb(232, 239, 255);
                    fg = Color.FromArgb(74, 107, 191);
                    break;
                case "tự_luận":
                    bg = Color.FromArgb(254, 243, 199);
                    fg = Color.FromArgb(180, 83, 9);
                    break;
                case "bài_tập_lớn":
                    bg = Color.FromArgb(237, 233, 254);
                    fg = Color.FromArgb(109, 40, 217);
                    break;
                default:
                    bg = Color.FromArgb(243, 244, 246);
                    fg = Color.FromArgb(107, 114, 128);
                    break;
            }

            float badgeW = 100;
            float badgeH = 30;
            float badgeX = cellX + (cellW - badgeW) / 2;
            float badgeY = cellY + (cellH - badgeH) / 2;
            float radius = 6; // Bán kính bo góc

            // Vẽ nền bo góc
            using (GraphicsPath path = RoundedRect(badgeX, badgeY, badgeW, badgeH, radius))
            using (SolidBrush bgBrush = new SolidBrush(bg))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillPath(bgBrush, path);
            }

            // Vẽ chữ
            using (SolidBrush fgBrush = new SolidBrush(fg))
            using (Font badgeFont = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            {
                g.DrawString(hinhThuc, badgeFont, fgBrush,
                            new RectangleF(badgeX, badgeY, badgeW, badgeH), sf);
            }
        }

        // ============================================
        // HELPER: Vẽ Progress Bar (có bo góc)
        // ============================================
        private void VeProgressBar(Graphics g, int tienDo, Color barColor,
                                   float cellX, float cellY, float cellW, float cellH,
                                   Font font, Brush textBrush, StringFormat sf)
        {
            float barW = cellW - 20;
            float barH = 14;
            float barX = cellX + 10;
            float barY = cellY + (cellH - barH) / 2;
            float radius = 4; // Bán kính bo góc

            // Vẽ nền bar bo góc
            using (GraphicsPath bgPath = RoundedRect(barX, barY, barW, barH, radius))
            using (SolidBrush barBg = new SolidBrush(Color.FromArgb(226, 232, 240)))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillPath(barBg, bgPath);
            }

            // Vẽ fill bar bo góc (nếu có tiến độ > 0)
            if (tienDo > 0)
            {
                float fillWidth = barW * tienDo / 100f;
                using (GraphicsPath fillPath = RoundedRect(barX, barY, fillWidth, barH, radius))
                using (SolidBrush barFill = new SolidBrush(barColor))
                {
                    g.FillPath(barFill, fillPath);
                }
            }

            // Vẽ text % (phía trên hoặc bên cạnh bar)
            g.DrawString($"{tienDo}%", font, textBrush,
                        new RectangleF(cellX, cellY, cellW, cellH), sf);
        }


        // ============================================
        // HELPER: Tạo GraphicsPath bo góc
        // ============================================
        private GraphicsPath RoundedRect(float x, float y, float width, float height, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2;

            // Đảm bảo radius không quá lớn
            if (radius > width / 2) radius = width / 2;
            if (radius > height / 2) radius = height / 2;

            RectangleF arcRect = new RectangleF(x, y, diameter, diameter);

            // Top-left
            path.AddArc(arcRect, 180, 90);
            // Top-right
            arcRect.X = x + width - diameter;
            path.AddArc(arcRect, 270, 90);
            // Bottom-right
            arcRect.Y = y + height - diameter;
            path.AddArc(arcRect, 0, 90);
            // Bottom-left
            arcRect.X = x;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();
            return path;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Cập nhật vị trí button khi control thay đổi kích thước
            if (MonHoc != null)
            {
                this.Invalidate(); // Sẽ gọi Paint, trong Paint gọi CapNhatViTriButton
            }
        }
        // ============================================
        // PANEL EXPAND
        // ============================================
        private void CreateExpandPanel()
        {
            pnlExpand = new Panel()
            {
                Size = new Size(this.Width, expandHeight),
                Location = new Point(0, 55),
                BackColor = Color.FromArgb(248, 250, 255),
                Visible = false,
                BorderStyle = BorderStyle.None
            };
            this.Controls.Add(pnlExpand);
        }

        private void SubjectRowControl_Click(object sender, EventArgs e)
        {
            // Chỉ toggle nếu click vào vùng header (trên cùng 55px)
            Point mousePos = this.PointToClient(Cursor.Position);

            if (mousePos.Y >= 0 && mousePos.Y <= 55)
            {
                ToggleExpand();
            }
        }

        public void ToggleExpand()
        {
            isExpanded = !isExpanded;

            if (isExpanded)
            {
                this.Height = 55 + expandHeight;
                pnlExpand.Visible = true;
                pnlExpand.Size = new Size(this.Width, expandHeight);
                LoadExpandContent();
            }
            else
            {
                this.Height = 55;
                pnlExpand.Visible = false;
            }

            this.Invalidate(new Rectangle(0, 0, this.Width, 55));

            if (this.Parent is FlowLayoutPanel flp)
            {
                flp.PerformLayout();
            }
        }

        private void LoadExpandContent()
        {
            pnlExpand.Controls.Clear();
            pnlExpand.Padding = new Padding(16, 12, 16, 12);

            int panelWidth = pnlExpand.Width - pnlExpand.Padding.Left - pnlExpand.Padding.Right;
            int halfW = (panelWidth / 2) - 10;

            // ========================================
            // PANEL TRÁI: ĐIỂM THÀNH PHẦN
            // ========================================
            Panel pnlDiem = new Panel()
            {
                Location = new Point(pnlExpand.Padding.Left, pnlExpand.Padding.Top),
                Size = new Size(halfW, expandHeight - 24),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            // Bo góc và border cho panel trái
            pnlDiem.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = RoundedRect(0, 0, pnlDiem.Width - 1, pnlDiem.Height - 1, 10))
                using (Pen borderPen = new Pen(Color.FromArgb(210, 215, 225), 1f))
                using (SolidBrush bgBrush = new SolidBrush(Color.White))
                {
                    g.FillPath(bgBrush, path);
                    g.DrawPath(borderPen, path);
                }
            };

            // --- HEADER: "Điểm thành phần" + "➕ Thêm cột điểm" ---
            Panel pnlHeaderDiem = new Panel()
            {
                Location = new Point(12, 12),
                Size = new Size(pnlDiem.Width - 24, 36),
                BackColor = Color.Transparent
            };

            Label lblTitle = new Label()
            {
                Text = "📊 Điểm thành phần",
                Location = new Point(0, 6),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = textColor,
                AutoSize = true
            };
            pnlHeaderDiem.Controls.Add(lblTitle);

            // Nút "Thêm cột điểm" bên phải
            Button btnThemCot = new Button()
            {
                Text = "➕ Thêm cột điểm",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(240, 244, 255),
                ForeColor = primaryColor,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnThemCot.FlatAppearance.BorderSize = 0;
            btnThemCot.Location = new Point(pnlHeaderDiem.Width - btnThemCot.Width, 3);
            btnThemCot.Paint += (s, e) =>
            {
                Button btn = s as Button;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = RoundedRect(0, 0, btn.Width - 1, btn.Height - 1, 8))
                using (SolidBrush bgBrush = new SolidBrush(btn.BackColor))
                using (Pen borderPen = new Pen(Color.FromArgb(180, 200, 230), 1f))
                {
                    e.Graphics.FillPath(bgBrush, path);
                    e.Graphics.DrawPath(borderPen, path);
                }
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font,
                    new Rectangle(0, 0, btn.Width, btn.Height),
                    btn.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            btnThemCot.Click += (s, e) =>
            {
                // TODO: Mở form thêm cột điểm
                MessageBox.Show("Mở form thêm cột điểm");
            };
            pnlHeaderDiem.Controls.Add(btnThemCot);

            pnlDiem.Controls.Add(pnlHeaderDiem);

            // --- MID: Danh sách các row điểm ---
            Panel pnlListDiem = new Panel()
            {
                Location = new Point(12, 54),
                Size = new Size(pnlDiem.Width - 24, expandHeight - 120),
                AutoScroll = true,
                BackColor = Color.Transparent
            };

            int rowY = 0;
            int rowHeight = 48;

            if (DsCotDiem != null)
            {
                foreach (var cot in DsCotDiem)
                {
                    // Tìm điểm tương ứng
                    decimal? giaTri = null;
                    if (DsDiemSo != null)
                    {
                        var diem = DsDiemSo.Find(d => d.MaCotDiem == cot.MaCotDiem);
                        if (diem != null) giaTri = diem.GiaTri;
                    }

                    // Panel cho 1 row điểm
                    Panel pnlRowDiem = new Panel()
                    {
                        Location = new Point(0, rowY),
                        Size = new Size(pnlListDiem.Width - 30, rowHeight),
                        BackColor = Color.White,
                        Tag = cot.MaCotDiem
                    };

                    // Vẽ border bottom cho row
                    pnlRowDiem.Paint += (s, e) =>
                    {
                        Graphics g = e.Graphics;
                        using (Pen bottomPen = new Pen(Color.FromArgb(230, 234, 240), 1f))
                        {
                            g.DrawLine(bottomPen, 0, pnlRowDiem.Height - 1, pnlRowDiem.Width, pnlRowDiem.Height - 1);
                        }
                    };

                    // Tên loại điểm
                    Label lblTenCot = new Label()
                    {
                        Text = $"{cot.TenCotDiem}",
                        Location = new Point(10, 13),
                        Size = new Size(120, 22),
                        Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                        ForeColor = textColor,
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    pnlRowDiem.Controls.Add(lblTenCot);

                    // Panel bo góc hiển thị điểm (màu xanh lá nhẹ)
                    Panel pnlGiaTri = new Panel()
                    {
                        Location = new Point(135, 8),
                        Size = new Size(50, 32),
                        BackColor = Color.FromArgb(232, 245, 233),
                        Cursor = Cursors.IBeam
                    };

                  

                    TextBox txtDiem = new TextBox()
                    {
                        Text = giaTri.HasValue ? $"{giaTri.Value:F1}" : "",
                        Size = new Size(40, 20),
                        Location = new Point(5, 6),
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.FromArgb(232, 245, 233),
                        Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                        TextAlign = HorizontalAlignment.Center
                    };

                    pnlGiaTri.Paint += (s, e) =>
                    {
                        Graphics g = e.Graphics;
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        // Kiểm tra giá trị hiện tại của txtDiem để quyết định màu viền
                        bool isError = false;
                        if (decimal.TryParse(txtDiem.Text, out decimal valCheck))
                        {
                            if (valCheck < 0 || valCheck > 10) isError = true;
                        }

                        Color currentBg = isError ? Color.FromArgb(255, 235, 238) : pnlGiaTri.BackColor;
                        Color currentBorder = isError ? Color.Red : Color.FromArgb(165, 214, 167);

                        using (GraphicsPath path = RoundedRect(0, 0, pnlGiaTri.Width - 1, pnlGiaTri.Height - 1, 6))
                        using (SolidBrush bgBrush = new SolidBrush(currentBg))
                        using (Pen borderPen = new Pen(currentBorder, isError ? 1.5f : 1f)) // Viền dày hơn nếu lỗi
                        {
                            g.FillPath(bgBrush, path);
                            g.DrawPath(borderPen, path);
                        }
                    };

                    // Sự kiện kiểm tra điểm hợp lệ và đổi màu
                    txtDiem.TextChanged += (s, e) =>
                    {
                        if (decimal.TryParse(txtDiem.Text, out decimal val))
                        {
                            if (val < 0 || val > 10)
                            {
                                pnlGiaTri.BackColor = Color.FromArgb(255, 235, 238); // Nền đỏ nhạt
                                txtDiem.BackColor = Color.FromArgb(255, 235, 238);
                            }
                            else
                            {
                                pnlGiaTri.BackColor = Color.FromArgb(232, 245, 233); // Nền xanh nhạt
                                txtDiem.BackColor = Color.FromArgb(232, 245, 233);
                            }
                        }
                        else if (string.IsNullOrEmpty(txtDiem.Text))
                        {
                            pnlGiaTri.BackColor = Color.FromArgb(232, 245, 233);
                            txtDiem.BackColor = Color.FromArgb(232, 245, 233);
                        }
                        pnlGiaTri.Invalidate(); // Ép panel gọi lại sự kiện Paint để vẽ viền đỏ
                    };

                    // Sự kiện lưu vào CSDL khi rời khỏi ô nhập
                    txtDiem.LostFocus += (s, e) =>
                    {
                        // 1. Kiểm tra rỗng
                        if (string.IsNullOrWhiteSpace(txtDiem.Text)) return;

                        // 2. Ép kiểu dữ liệu (decimal khớp với CSDL)
                        if (decimal.TryParse(txtDiem.Text, out decimal val))
                        {
                            // Kiểm tra miền giá trị (Ràng buộc CHK_DIEM_SO_GIA_TRI trong SQL)
                            if (val >= 0 && val <= 10)
                            {
                                // 3. Gọi DAO để thực thi sp_UpsertDiemSo
                                // Ở đây bạn dùng monHocDao đã khai báo ở đầu Class
                                bool success = monHocDao.UpsertDiemSo(cot.MaCotDiem, val);

                                if (success)
                                {
                                    // Cập nhật lại list local để UI Header vẽ lại chính xác
                                    var currentDiem = DsDiemSo.Find(d => d.MaCotDiem == cot.MaCotDiem);
                                    if (currentDiem != null)
                                    {
                                        currentDiem.GiaTri = val;
                                    }
                                    else
                                    {
                                        DsDiemSo.Add(new DiemSoDTO { MaCotDiem = cot.MaCotDiem, GiaTri = val });
                                    }

                                    // 4. Cập nhật giao diện Header của chính dòng môn học này (Điểm hiện tại)
                                    this.Invalidate(new Rectangle(0, 0, this.Width, 55));

                                    // 5. Cập nhật GPA Summary tổng quát trên Form chính (sp_GetGpaSummary)
                                    if (this.FindForm() is frm_MonHoc parentForm)
                                    {
                                        // Hàm này trong code của bạn đã gọi sp_GetGpaSummary và DashboardDAO
                                        parentForm.CapNhatGPASummary();
                                    }

                                    // Hiệu ứng đổi màu báo lưu thành công
                                    txtDiem.ForeColor = Color.DarkGreen;

                                    //load lại nội dung expand để cập nhật các cột điểm
                                    LoadExpandContent();

                                    //load lại 
                                    // 2. Tự tính lại điểm trung bình cho MonHoc (Object đang hiển thị)
                                    decimal tong = 0;
                                    foreach (var c in DsCotDiem)
                                    {
                                        var d = DsDiemSo.Find(x => x.MaCotDiem == c.MaCotDiem);
                                        if (d != null) tong += (d.GiaTri ?? 0) * (decimal)c.TrongSo / 100m;
                                    }

                                    // Gán lại vào object để hàm Paint lấy dữ liệu mới này để vẽ
                                    this.MonHoc.DiemHienTai = tong;

                                

                                    // THÊM: Tính lại trọng số còn lại
                                    decimal tongTrongSoDaCoDiem = 0;
                                    foreach (var c in DsCotDiem)
                                    {
                                        // Nếu cột điểm này đã có trong danh sách DsDiemSo
                                        if (DsDiemSo.Exists(x => x.MaCotDiem == c.MaCotDiem))
                                        {
                                            tongTrongSoDaCoDiem += (decimal)c.TrongSo;
                                        }
                                    }

                                    // Giả sử tổng môn là 100%, thì còn lại = 100 - đã có
                                    this.MonHoc.TrongSoConLai = 100 - tongTrongSoDaCoDiem;

                                    // Sau đó mới Invalidate để vẽ lại
                                    this.Invalidate(new Rectangle(0, 0, this.Width, 55));
                                }
                            }
                            else
                            {
                                MessageBox.Show("Điểm phải từ 0 đến 10 theo quy định!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtDiem.Focus();
                                txtDiem.SelectAll();
                            }
                        }
                    };

                    pnlGiaTri.Controls.Add(txtDiem);
                    pnlRowDiem.Controls.Add(pnlGiaTri);

                    //trong số
                    Panel pnlTrongSo = new Panel()
                    {
                        Location = new Point(190, 8),
                        Size = new Size(55, 32),
                        BackColor = Color.FromArgb(255, 248, 225)
                    };

                    TextBox txtTrongSo = new TextBox()
                    {
                        Text = cot.TrongSo.ToString(),
                        Size = new Size(45, 20),
                        Location = new Point(5, 7),
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.FromArgb(255, 248, 225),
                        Font = new Font("Segoe UI", 8f, FontStyle.Italic),
                        TextAlign = HorizontalAlignment.Center
                    };

                    txtTrongSo.LostFocus += (s, e) =>
                    {
                        if (int.TryParse(txtTrongSo.Text, out int tsVal))
                        {
                            // GỌI DAO CẬP NHẬT TRỌNG SỐ
                            monHocDao.UpdateTrongSo(cot.MaCotDiem, tsVal);
                        }
                    };

                    pnlTrongSo.Controls.Add(txtTrongSo);
                    pnlRowDiem.Controls.Add(pnlTrongSo);

                    // Button Sửa
                    Button btnSuaDiem = new Button()
                    {
                        Text = "✏️",
                        Font = new Font("Segoe UI", 8f),
                        FlatStyle = FlatStyle.Flat,
                        Size = new Size(30, 28),
                        Location = new Point(255, 10),
                        BackColor = Color.FromArgb(240, 244, 255),
                        ForeColor = primaryColor,
                        Cursor = Cursors.Hand,
                        Tag = cot.MaCotDiem
                    };
                    btnSuaDiem.FlatAppearance.BorderSize = 0;
                    btnSuaDiem.Paint += (s, e) =>
                    {
                        Button btn = s as Button;
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        using (GraphicsPath path = RoundedRect(0, 0, btn.Width - 1, btn.Height - 1, 6))
                        using (SolidBrush bgBrush = new SolidBrush(btn.BackColor))
                        using (Pen borderPen = new Pen(Color.FromArgb(180, 200, 230), 1f))
                        {
                            e.Graphics.FillPath(bgBrush, path);
                            e.Graphics.DrawPath(borderPen, path);
                        }
                        TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font,
                            new Rectangle(0, 0, btn.Width, btn.Height),
                            btn.ForeColor,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    };
                    btnSuaDiem.Click += (s, e) =>
                    {
                        Button btn = s as Button;
                        int maCot = (int)btn.Tag;
                        // TODO: Mở form sửa điểm
                        MessageBox.Show($"Sửa điểm cho cột {maCot}");
                    };
                    pnlRowDiem.Controls.Add(btnSuaDiem);

                    // Button Xóa
                    Button btnXoaDiem = new Button()
                    {
                        Text = "🗑️",
                        Font = new Font("Segoe UI", 8f),
                        FlatStyle = FlatStyle.Flat,
                        Size = new Size(30, 28),
                        Location = new Point(290, 10),
                        BackColor = Color.FromArgb(254, 242, 242),
                        ForeColor = dangerColor,
                        Cursor = Cursors.Hand,
                        Tag = cot.MaCotDiem
                    };
                    btnXoaDiem.FlatAppearance.BorderSize = 0;
                    btnXoaDiem.Paint += (s, e) =>
                    {
                        Button btn = s as Button;
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        using (GraphicsPath path = RoundedRect(0, 0, btn.Width - 1, btn.Height - 1, 6))
                        using (SolidBrush bgBrush = new SolidBrush(btn.BackColor))
                        using (Pen borderPen = new Pen(Color.FromArgb(240, 200, 200), 1f))
                        {
                            e.Graphics.FillPath(bgBrush, path);
                            e.Graphics.DrawPath(borderPen, path);
                        }
                        TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font,
                            new Rectangle(0, 0, btn.Width, btn.Height),
                            btn.ForeColor,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    };
                    btnXoaDiem.Click += (s, e) =>
                    {
                        Button btn = s as Button;
                        int maCot = (int)btn.Tag;
                        if (MessageBox.Show("Xóa cột điểm này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            // TODO: Xóa cột điểm
                            LoadExpandContent();
                        }
                    };
                    pnlRowDiem.Controls.Add(btnXoaDiem);

                    pnlListDiem.Controls.Add(pnlRowDiem);
                    rowY += rowHeight + 2;
                }
            }

            pnlDiem.Controls.Add(pnlListDiem);

            // --- BOTTOM: Tổng trọng số ---
            decimal tongTS = 0;
            if (DsCotDiem != null)
                foreach (var c in DsCotDiem) tongTS += c.TrongSo;

            bool duTrongSo = tongTS == 100;

            Panel pnlTongTS = new Panel()
            {
                Location = new Point(12, pnlDiem.Height - 48),
                Size = new Size(pnlDiem.Width - 24, 36),
                BackColor = Color.Transparent
            };
            pnlTongTS.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Color bgColor = duTrongSo ? Color.FromArgb(232, 245, 233) : Color.FromArgb(245, 245, 245);
                Color textColor2 = duTrongSo ? Color.FromArgb(46, 125, 50) : Color.FromArgb(158, 158, 158);
                Color borderColor2 = duTrongSo ? Color.FromArgb(165, 214, 167) : Color.FromArgb(224, 224, 224);

                string text = duTrongSo ? $"Tổng trọng số: {tongTS}% ✓" : $"Tổng trọng số: {tongTS}% (cần 100%)";

                using (GraphicsPath path = RoundedRect(0, 0, pnlTongTS.Width - 1, pnlTongTS.Height - 1, 8))
                using (SolidBrush bgBrush = new SolidBrush(bgColor))
                using (Pen borderPen = new Pen(borderColor2, 1f))
                {
                    g.FillPath(bgBrush, path);
                    g.DrawPath(borderPen, path);
                }

                TextRenderer.DrawText(e.Graphics, text, new Font("Segoe UI", 8f, FontStyle.Bold),
                    new Rectangle(0, 0, pnlTongTS.Width, pnlTongTS.Height),
                    textColor2,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            pnlDiem.Controls.Add(pnlTongTS);

            pnlExpand.Controls.Add(pnlDiem);

            // ========================================
            // PANEL PHẢI: DỰ BÁO ĐIỂM
            // ========================================
            Panel pnlDuBao = new Panel()
            {
                Location = new Point(pnlExpand.Padding.Left + halfW + 20, pnlExpand.Padding.Top),
                Size = new Size(halfW, expandHeight - 24),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            pnlDuBao.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = RoundedRect(0, 0, pnlDuBao.Width - 1, pnlDuBao.Height - 1, 10))
                using (Pen borderPen = new Pen(Color.FromArgb(210, 215, 225), 1f))
                using (SolidBrush bgBrush = new SolidBrush(Color.White))
                {
                    g.FillPath(bgBrush, path);
                    g.DrawPath(borderPen, path);
                }
            };

            Label lblDBTitle = new Label()
            {
                Text = "🎯 Dự báo điểm",
                Location = new Point(16, 16),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = textColor,
                AutoSize = true
            };
            pnlDuBao.Controls.Add(lblDBTitle);

            decimal diemHT = GetDiemHienTai();
            Label lblDiemHT = new Label()
            {
                Text = $"Điểm hiện tại: {diemHT:F2}/10",
                Location = new Point(16, 52),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = textColor,
                AutoSize = true
            };
            pnlDuBao.Controls.Add(lblDiemHT);

            Label lblMucTieu = new Label()
            {
                Text = $"Mục tiêu: {DiemMucTieuThang10:F1}/10",
                Location = new Point(16, 82),
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true
            };
            pnlDuBao.Controls.Add(lblMucTieu);

            decimal diemCan = TinhDiemCanDat();
            string duBaoText = GetDuBaoText();
            Color duBaoColor = GetDuBaoColor();

            // Panel bo góc cho kết quả dự báo
            Panel pnlKetQuaDB = new Panel()
            {
                Location = new Point(16, 115),
                Size = new Size(pnlDuBao.Width - 32, 44),
                BackColor = Color.FromArgb(255, 253, 235)
            };
            pnlKetQuaDB.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Color bgColor;
                Color borderColor2;
                Color textColor2;

                if (duBaoText.Contains("Không khả thi"))
                {
                    bgColor = Color.FromArgb(255, 235, 238);
                    borderColor2 = Color.FromArgb(239, 154, 154);
                    textColor2 = dangerColor;
                }
                else if (duBaoText.Contains("An toàn") || duBaoText.Contains("vượt mục tiêu"))
                {
                    bgColor = Color.FromArgb(232, 245, 233);
                    borderColor2 = Color.FromArgb(165, 214, 167);
                    textColor2 = successColor;
                }
                else
                {
                    bgColor = Color.FromArgb(255, 253, 235);
                    borderColor2 = Color.FromArgb(255, 224, 130);
                    textColor2 = Color.FromArgb(245, 127, 23);
                }

                using (GraphicsPath path = RoundedRect(0, 0, pnlKetQuaDB.Width - 1, pnlKetQuaDB.Height - 1, 8))
                using (SolidBrush bgBrush = new SolidBrush(bgColor))
                using (Pen borderPen = new Pen(borderColor2, 1f))
                {
                    g.FillPath(bgBrush, path);
                    g.DrawPath(borderPen, path);
                }

                TextRenderer.DrawText(e.Graphics, duBaoText, new Font("Segoe UI", 10f, FontStyle.Bold),
                    new Rectangle(0, 0, pnlKetQuaDB.Width, pnlKetQuaDB.Height),
                    textColor2,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            pnlDuBao.Controls.Add(pnlKetQuaDB);

            // Các cột còn thiếu điểm
            string cacCotConThieu = "";
            if (DsCotDiem != null && DsDiemSo != null)
            {
                var cotThieu = new List<string>();
                foreach (var cot in DsCotDiem)
                {
                    var diem = DsDiemSo.Find(d => d.MaCotDiem == cot.MaCotDiem);
                    if (diem == null || diem.GiaTri == null)
                        cotThieu.Add($"{cot.TenCotDiem} ({cot.TrongSo}%)");
                }
                if (cotThieu.Count > 0)
                    cacCotConThieu = "Còn thiếu điểm cho: " + string.Join(", ", cotThieu);
                else
                    cacCotConThieu = "✅ Đã có đủ tất cả điểm thành phần";
            }

            Label lblCotThieu = new Label()
            {
                Text = cacCotConThieu,
                Location = new Point(16, 172),
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(139, 156, 192),
                AutoSize = true,
                MaximumSize = new Size(pnlDuBao.Width - 32, 0)
            };
            pnlDuBao.Controls.Add(lblCotThieu);

            pnlExpand.Controls.Add(pnlDuBao);
        }

        // ============================================
        // SỰ KIỆN THAY ĐỔI ĐIỂM
        // ============================================
        private void NudDiem_ValueChanged(object sender, EventArgs e)
        {
            // TODO: Cập nhật điểm vào DB
            // Sau khi cập nhật DB, gọi LoadExpandContent() để refresh
            NumericUpDown nud = sender as NumericUpDown;
            int maCotDiem = (int)nud.Tag;
            // Gọi DAO cập nhật điểm...
        }

        private void BtnXoaCot_Click(object sender, EventArgs e)
        {
            // TODO: Xóa cột điểm
            Button btn = sender as Button;
            int maCotDiem = (int)btn.Tag;
            if (MessageBox.Show("Xóa cột điểm này?", "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Gọi DAO xóa...
                LoadExpandContent();
            }
        }

        // ============================================
        // EVENT CLICK SỬA / XÓA
 

        public virtual void OnSuaClick(EventArgs e)
        {
            SuaClick?.Invoke(this, e);
        }

        protected virtual void OnXoaClick(EventArgs e)
        {
            XoaClick?.Invoke(this, e);
        }
    }
}