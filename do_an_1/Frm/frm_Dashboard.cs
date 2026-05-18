using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using do_an_1.DAO;
using do_an_1.DTO;


namespace do_an_1.Frm
{
    public partial class frm_Dashboard : Form
    {
        #region [1] Khai báo biến toàn cục
        private int maNguoiDung;
        private DashboardDAO dao;
        private System.Windows.Forms.Timer timerAlhola;
        private System.Windows.Forms.Timer timerPulse;

        #endregion


        #region [2] Khởi tạo & Load Form
        public frm_Dashboard(int maND)
        {
            InitializeComponent();
            this.maNguoiDung = maND;
            dao = new DashboardDAO();

        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            // Khởi tạo timer
            timerAlhola = new System.Windows.Forms.Timer();
            timerAlhola.Interval = 100; // 100ms cho animation rung
            timerAlhola.Tick += TimerAlhola_Tick;

            timerPulse = new System.Windows.Forms.Timer();
            timerPulse.Interval = 800; // 800ms cho nhấp nháy icon/dot
            timerPulse.Tick += TimerPulse_Tick;


            // Set avatar
            NguoiDungDTO nd = dao.GetThongTinNguoiDung(maNguoiDung);
            if (nd != null && !string.IsNullOrEmpty(nd.HoTen) && picAvatar != null)
            {
                string[] tenParts = nd.HoTen.Trim().Split(' ');
                string chuCaiCuoi = tenParts[tenParts.Length - 1].Substring(0, 1).ToUpper();

                int size = picAvatar.Width;
                Bitmap bmp = new Bitmap(size, size);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.Clear(Color.Transparent);
                    g.FillEllipse(new SolidBrush(Color.FromArgb(74, 107, 191)), 0, 0, size - 1, size - 1);
                    using (Font font = new Font("Segoe UI", size * 0.45f, FontStyle.Bold))
                    {
                        SizeF textSize = g.MeasureString(chuCaiCuoi, font);
                        float x = (size - textSize.Width) / 2;
                        float y = (size - textSize.Height) / 2;
                        g.DrawString(chuCaiCuoi, font, Brushes.White, x, y);
                    }
                }
                picAvatar.Image = bmp;
            }


            // Load toàn bộ dữ liệu
            LoadToanBoDashboard();
            // Gọi Resize lần đầu để set Width cho các control
            frmDashboard_Resize(null, null);



        }

        private void frmDashboard_Resize(object sender, EventArgs e)
        {
            // Đảm bảo flpContent co giãn theo pnlContentWrapper
            if (flpContent != null && pnlContentWrapper != null)
            {
                flpContent.Width = pnlContentWrapper.Width - 20;
            }

            // === EISENHOWER MATRIX ===
            // Mỗi quadrant chiếm 1/4 chiều ngang của pnlEisenhower (nếu có)
            // Tạm thời set width cho từng pnlQ dựa trên cha của chúng
            ResizeQuadrant(tlpQ2, flpQ1);
            ResizeQuadrant(pnlQ2, flpQ2);
            ResizeQuadrant(pnlQ3, flpQ3);
            ResizeQuadrant(pnlQ4, flpQ4);

            // === GHI CHÚ ===
            if (pnlGhiChu != null && flpGhiChu != null)
            {
                flpGhiChu.Width = pnlGhiChu.Width - 10;
                flpGhiChu.Height = pnlGhiChu.Height - 10;
            }

            // === MÔN HỌC ===
            if (pnlMonHoc != null && flpMonHoc != null)
            {
                flpMonHoc.Width = pnlMonHoc.Width - 10;
                flpMonHoc.Height = pnlMonHoc.Height - 10;
            }
        }

        // Hàm helper resize 1 quadrant
        private void ResizeQuadrant(Panel pnlCha, FlowLayoutPanel flpCon)
        {
            if (pnlCha == null || flpCon == null) return;

            // Tìm cha của pnlCha để tính width
            Control chaCuaCha = pnlCha.Parent;
            if (chaCuaCha != null)
            {
                // Giả sử 4 quadrant nằm ngang trong 1 panel cha (vd: tlpEisenhower hoặc pnlEisenhower)
                int quadrantWidth = (chaCuaCha.Width - 30) / 4; // 30 là padding tổng
                if (quadrantWidth < 100) quadrantWidth = 100;

                pnlCha.Width = quadrantWidth;
                pnlCha.Height = chaCuaCha.Height - 10;
            }

            flpCon.Width = pnlCha.Width - 10;
            flpCon.Height = pnlCha.Height - 10;
        }

        #endregion


        #region [3] Phương thức Load dữ liệu (gọi DAO → gán UI)
        private void LoadToanBoDashboard()
        {
            try
            {
                LoadWelcomeBanner();
                LoadAlholaBar();
                LoadHealthBar();
                LoadStatCards();
                LoadEisenhowerMatrix();
                LoadMonHocDuBao();
                LoadHeatmap();
                LoadGhiChuGanDay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ LỖI: " + ex.Message + "\n\n" + ex.StackTrace, "ERROR");
            }
        }

        private void LoadWelcomeBanner()
        {
            NguoiDungDTO nd = dao.GetThongTinNguoiDung(maNguoiDung);

            if (nd != null)
            {
                // Tên người dùng
                if (!string.IsNullOrEmpty(nd.HoTen))
                    lblChao.Text = "👋 Chào mừng " + nd.HoTen + "!";
                else
                    lblChao.Text = "👋 Chào mừng!";

                // Quote
                lblQuote.Text = "Study hard, rest well. A healthy mind lives in a healthy body.";

                // GPA hiện tại
                decimal gpaHienTai = dao.GetGPAHienTai(maNguoiDung);
                if (gpaHienTai > 0)
                    lblGPAValue.Text = gpaHienTai.ToString("0.00");
                else
                    lblGPAValue.Text = "--";

                // GPA mục tiêu
                lblGPAMucTieuValue.Text = nd.GpaMucTieu.ToString("0.00");

                // Gap
                if (gpaHienTai > 0)
                {
                    decimal gap = nd.GpaMucTieu - gpaHienTai;
                    if (gap > 0)
                    {
                        // Còn thiếu
                        lblGapText.Text = $"⚠️ Còn thiếu: {gap:0.00}";
                        lblGapText.ForeColor = Color.White;
                        pnlGap.FillColor = Color.FromArgb(255, 152, 0);   // Cam
                    }
                    else
                    {
                        // Đã đạt hoặc vượt
                        lblGapText.Text = "✅ Đã đạt mục tiêu!";
                        lblGapText.ForeColor = Color.White;
                        pnlGap.FillColor = Color.FromArgb(76, 175, 80);   // Xanh lá
                    }
                }
                else
                {
                    // Chưa có điểm
                    lblGapText.Text = "Chưa có dữ liệu điểm";
                    lblGapText.ForeColor = Color.FromArgb(200, 200, 200);
                    pnlGap.FillColor = Color.FromArgb(50, 50, 50);        // Xám mặc định
                }
            }
        }
        private void LoadAlholaBar()
        {
            // Gọi DAO lấy tổng phút học hôm nay
            int tongPhut = dao.GetTongPhutHocHomNay(maNguoiDung);
            double gioThuc = tongPhut / 60.0;

            // Tính BAC
            double bac = TinhBAC(gioThuc);

            // Xác định trạng thái
            string trangThai;
            if (gioThuc < 12)
                trangThai = "SAFE";
            else if (gioThuc < 15)
                trangThai = "CAUTION";
            else
                trangThai = "DANGER";

            // Cập nhật icon
            switch (trangThai)
            {
                case "SAFE":
                    lblAlholaIcon.Text = "✅";
                    lblAlholaIcon.ForeColor = Color.FromArgb(22, 163, 74);
                    lblAlholaTitle.Text = "Tinh thần khỏe mạnh — Sẵn sàng học tập!";
                    lblAlholaDesc.Text = $"Thời gian thức liên tục hôm nay: {gioThuc:F1} giờ. " +
                                       "Nhận thức đang ở mức tối ưu. Hãy duy trì sự cân bằng và nghỉ ngơi đúng lúc.";
                    btnAlholaAction.Text = "😊 Đang ổn";
                    btnAlholaAction.FillColor = Color.FromArgb(22, 163, 74);
                    // Dừng timer nếu đang chạy
                    timerAlhola.Stop();
                    timerPulse.Stop();
                    pnlAlhola.BorderColor = Color.FromArgb(187, 247, 208);
                    pnlAlhola.FillColor = Color.FromArgb(240, 253, 244);
                    CapNhatMauAlholaBar("SAFE");
                    CapNhatTrangThaiNutDeadline();
                    break;

                case "CAUTION":
                    lblAlholaIcon.Text = "⚠️";
                    lblAlholaIcon.ForeColor = Color.FromArgb(232, 168, 64);
                    lblAlholaTitle.Text = "Cảnh báo nhẹ — Bạn nên nghỉ ngơi sớm";
                    lblAlholaDesc.Text = $"Thời gian thức liên tục: {gioThuc:F1} giờ. " +
                                       $"Nhận thức bắt đầu suy giảm, tương đương BAC {bac * 100:F2}%. " +
                                       "Cân nhắc kết thúc phiên học trong 1-2 giờ tới.";
                    btnAlholaAction.Text = "😴 Nghỉ ngơi sớm";
                    btnAlholaAction.FillColor = Color.FromArgb(232, 168, 64);
                    // Chạy timer nhấp nháy
                    timerAlhola.Stop();
                    timerPulse.Start();
                    pnlAlhola.BorderColor = Color.FromArgb(253, 230, 138);
                    pnlAlhola.FillColor = Color.FromArgb(255, 251, 235);
                    CapNhatMauAlholaBar("CAUTION");
                    CapNhatTrangThaiNutDeadline();

                    break;

                case "DANGER":
                    lblAlholaIcon.Text = "🚨";
                    lblAlholaIcon.ForeColor = Color.FromArgb(239, 68, 68);
                    lblAlholaTitle.Text = "NGUY HIỂM: \"Say rượu học thuật\" — Dừng ngay lập tức!";
                    lblAlholaDesc.Text = $"Bạn đã thức {gioThuc:F1} giờ liên tục! " +
                                       $"Nhận thức suy giảm nghiêm trọng, tương đương BAC {bac * 100:F2}% " +
                                       "(vượt ngưỡng lái xe). Hệ thống khóa chức năng thêm Deadline. Hãy ngủ ít nhất 6 giờ.";
                    btnAlholaAction.Text = "😴 NGHỈ NGƠI NGAY!";
                    btnAlholaAction.FillColor = Color.FromArgb(239, 68, 68);
                    // Chạy timer rung + nhấp nháy
                    timerAlhola.Start();
                    timerPulse.Start();
                    pnlAlhola.BorderColor = Color.FromArgb(254, 202, 202);
                    pnlAlhola.FillColor = Color.FromArgb(254, 242, 242);
                    CapNhatMauAlholaBar("DANGER");
                    CapNhatTrangThaiNutDeadline();

                    break;
            }

            // Cập nhật thanh đo
            int phanTram = (int)Math.Min((gioThuc / 24) * 100, 100);
            progAlhola.Value = phanTram;

            // Cập nhật stats
            lblAlholaStats.Text = $"0h  │  Hiện tại: {gioThuc:F1}h  │  ⚠️ Cảnh báo: 12h  │  🚨 Nguy hiểm: 15h";

            // Lưu giờ thức vào Tag để Health bar dùng
            pnlAlhola.Tag = gioThuc;

        }
        private void LoadHealthBar()
        {
            // Lấy giờ thức từ Alhola bar (đã lưu trong Tag)
            double gioThuc = 0;
            if (pnlAlhola.Tag != null)
                gioThuc = Convert.ToDouble(pnlAlhola.Tag);

            // Tính BAC
            double bac = TinhBAC(gioThuc);

            // Lấy số deadline đỏ
            int soDeadlineDo = dao.GetSoDeadlineDo(maNguoiDung);

            // Lấy số phiên Pomodoro hôm nay
            int soPhienPomo = dao.GetSoPhienPomodoroHomNay(maNguoiDung);

            // Tính stress level
            string stressLevel = TinhStressLevel(soDeadlineDo, gioThuc);

            // === 1. Thức liên tục ===
            lblHealthValueThuc.Text = gioThuc >= 15 ? $"{gioThuc:F1} / 15h 🚨" :
                                      gioThuc >= 12 ? $"{gioThuc:F1} / 15h ⚠️" :
                                      $"{gioThuc:F1} / 12h";
            CapNhatMauDot(pnldotThuc, gioThuc < 12 ? "SAFE" : gioThuc < 15 ? "CAUTION" : "DANGER");

            // === 2. BAC ===
            string bacText = bac * 100 < 0.01 ? "~0.01%" : $"~{bac * 100:F2}%";
            string bacTrangThai = gioThuc < 12 ? "Tỉnh táo" : gioThuc < 15 ? "Suy giảm nhẹ" : "Nguy hiểm!";
            lbl_healthValueBAC.Text = $"{bacText} · {bacTrangThai}";
            CapNhatMauDot(pnldotBAC, gioThuc < 12 ? "SAFE" : gioThuc < 15 ? "CAUTION" : "DANGER");

            // === 3. Deadline đỏ ===
            lbl_healthValueDeadline.Text = soDeadlineDo >= 5 ? $"{soDeadlineDo} / 5 🚨" :
                                           soDeadlineDo >= 3 ? $"{soDeadlineDo} / 5 ⚠️" :
                                           $"{soDeadlineDo} / 5";
            CapNhatMauDot(pnl_dotDeadline, soDeadlineDo >= 5 ? "DANGER" : soDeadlineDo >= 3 ? "CAUTION" : "SAFE");

            // === 4. Pomodoro ===
            lbl_HealthValuePomo.Text = $"{soPhienPomo} phiên · {soPhienPomo * 25} phút";
            CapNhatMauDot(pnl_dotPomo, soPhienPomo >= 6 ? "CAUTION" : "SAFE");

            // === 5. Stress ===
            lbl_HealthValueStress.Text = stressLevel;
            CapNhatMauDot(pnl_dotStress, stressLevel == "Cao" ? "DANGER" : stressLevel == "Trung bình" ? "CAUTION" : "SAFE");
        }

        private void LoadStatCards()
        {
            // 1. Số môn học
            int soMonHoc = dao.GetSoMonHoc(maNguoiDung);
            lblStatValueMon.Text = soMonHoc.ToString();
            lblStatLabelMon.Text = "Môn đang học";

            // 2. Số deadline mở
            int soDeadline = dao.GetSoDeadline(maNguoiDung);
            lblStatValueDeadline.Text = soDeadline.ToString();
            lblStatLabelDeadline.Text = "Deadline mở";

            // 3. Chỉ số kỷ luật (tạm tính theo công thức đơn giản)
            int soPhienPomo = dao.GetSoPhienPomodoroHomNay(maNguoiDung);
            int kyLuat = soPhienPomo > 0 ? Math.Min(100, soPhienPomo * 10 + 50) : 0;
            lblStatValueKyLuat.Text = kyLuat + "%";
            lblStatLabelKyLuat.Text = "Chỉ số Kỷ luật";

            // 4. Số ghi chú
            int soGhiChu = dao.GetSoGhiChu(maNguoiDung);
            lblStatValueGhiChu.Text = soGhiChu.ToString();
            lblStatLabelGhiChu.Text = "Ghi chú";
        }

        private void LoadEisenhowerMatrix()
        {
            // Clear cũ
            flpQ1.Controls.Clear();
            flpQ2.Controls.Clear();
            flpQ3.Controls.Clear();
            flpQ4.Controls.Clear();

            List<NhiemVuEisenhowerDTO> dsNhiemVu = dao.GetNhiemVuChoEisenhower(maNguoiDung);



            if (dsNhiemVu.Count == 0)
            {
                lblEisenhowerCount.Text = "0 tasks";
                HienThiPlaceholder(flpQ1, "Không có task nào");
                return;
            }

            int countQ1 = 0, countQ2 = 0, countQ3 = 0, countQ4 = 0;

            foreach (NhiemVuEisenhowerDTO nv in dsNhiemVu)
            {
                string quadrant = PhanLoaiEisenhower(nv.ThoiHan, nv.SoTinChi);
                Panel item = TaoTaskItem(nv);

                // Set border trái theo quadrant
                Color borderColor;
                switch (quadrant)
                {
                    case "Q1": borderColor = Color.FromArgb(239, 68, 68); break;   // Đỏ
                    case "Q2": borderColor = Color.FromArgb(245, 158, 11); break;  // Vàng
                    case "Q3": borderColor = Color.FromArgb(59, 130, 246); break;  // Xanh dương
                    default: borderColor = Color.FromArgb(156, 163, 175); break;   // Xám
                }

                //Vẽ border trái 4px
                item.Paint += (s, e) =>
                {
                    using (Pen pen = new Pen(borderColor, 4))
                    {
                        e.Graphics.DrawLine(pen, 0, 0, 0, item.Height);
                    }
                };

                // Click event
                item.Click += (s, e) =>
                {
                    // TODO: Mở form chi tiết task
                    MessageBox.Show($"Task: {nv.TieuDe}\nDeadline: {nv.ThoiHan:dd/MM/yyyy HH:mm}", "Chi tiết task");
                };

                // Add vào flp tương ứng
                switch (quadrant)
                {

                    case "Q1": flpQ1.Controls.Add(item); countQ1++; break;
                    case "Q2": flpQ2.Controls.Add(item); countQ2++; break;
                    case "Q3": flpQ3.Controls.Add(item); countQ3++; break;
                    case "Q4": flpQ4.Controls.Add(item); countQ4++; break;
                }
            }

            // Placeholder cho quadrant trống
            if (countQ1 == 0) HienThiPlaceholder(flpQ1, "Không có việc khẩn");
            if (countQ2 == 0) HienThiPlaceholder(flpQ2, "Không có việc quan trọng");
            if (countQ3 == 0) HienThiPlaceholder(flpQ3, "Không có việc gấp");
            if (countQ4 == 0) HienThiPlaceholder(flpQ4, "Không có việc");



        }

        private void LoadMonHocDuBao()
        {
            flpMonHoc.Controls.Clear();

            List<MonHocDiemDTO> dsMon = dao.GetMonHocVaDiem(maNguoiDung);

            if (dsMon.Count == 0)
            {
                lblMonHocCanhBao.Text = "";
                HienThiPlaceholder(flpMonHoc, "Chưa có môn học nào");
                return;
            }

            // Lấy GPA mục tiêu thang 4 → quy ra thang 10 tương đương
            NguoiDungDTO nd = dao.GetThongTinNguoiDung(maNguoiDung);
            decimal diemMucTieuThang10 = nd.GpaMucTieu / 4.0m * 10.0m; // 3.6 → 9.0

            int monCanhBao = 0;
            int monKhongKhaThi = 0;

            foreach (MonHocDiemDTO mon in dsMon)
            {
                
                Panel item = TaoSubjectItem(mon, diemMucTieuThang10);
                item.Click += (s, e) =>
                {
                    // TODO: Mở form chi tiết môn học
                    MessageBox.Show($"Môn: {mon.TenMonHoc}\nĐiểm: {mon.DiemHienTai:F1}/10", "Chi tiết môn học");
                };
                flpMonHoc.Controls.Add(item);

                // Đếm môn cảnh báo
                if (mon.DiemHienTai < 5.0m)
                    monKhongKhaThi++;
                else if (mon.DiemHienTai < 7.0m)
                    monCanhBao++;
            }

            // Cập nhật label cảnh báo
            if (monKhongKhaThi > 0)
                lblMonHocCanhBao.Text = $"⚠️ {monKhongKhaThi} môn dưới 5.0";
            else
                lblMonHocCanhBao.Text = "✅ Tất cả ổn";

        }

        private void LoadHeatmap()
        {
            int thangHienTai = DateTime.Now.Month;
            int namHienTai = DateTime.Now.Year;

            Dictionary<int, int> duLieu = dao.GetDuLieuHeatmap(maNguoiDung, thangHienTai, namHienTai);

            lblHeatmapThang.Text = $"Tháng {thangHienTai}/{namHienTai}";
            VeCalendarHeader();
            VeCalendarBody(thangHienTai, namHienTai, duLieu);
            TaoHeatmapLegend(duLieu);
        }

        private void LoadGhiChuGanDay()
        {
            flpGhiChu.Controls.Clear();

            List<GhiChuDTO> dsGhiChu = dao.GetGhiChuGanDay(maNguoiDung);

            if (dsGhiChu.Count == 0)
            {
                HienThiPlaceholder(flpGhiChu, "Chưa có ghi chú nào");
                return;
            }

            foreach (GhiChuDTO gc in dsGhiChu)
            {
                Panel item = TaoGhiChuItem(gc);
                item.Click += (s, e) =>
                {
                    // TODO: Mở form chi tiết ghi chú
                    MessageBox.Show($"Ghi chú: {gc.TieuDe}\n{gc.NoiDung}", "Chi tiết ghi chú");
                };
                flpGhiChu.Controls.Add(item);
            }
        }


        #endregion

        #region [4] Phương thức xử lý nghiệp vụ (tính toán thuần, không SQL, không UI)


        private double TinhGioThucHomNay(int tongPhut)
        {
            // Đổi phút sang giờ (làm tròn 1 số lẻ)
            return Math.Round(tongPhut / 60.0, 1);
        }

        private double TinhBAC(double gioThuc)
        {
            // BAC ước lượng dựa trên thời gian thức liên tục
            // < 12h:  0.01% + (giờ/12) * 0.03%
            // 12-15h: 0.04% + ((giờ-12)/3) * 0.01%
            // >= 15h: 0.05% + ((giờ-15)/9) * 0.05%, tối đa 0.10%

            double bac;

            if (gioThuc < 12)
            {
                bac = 0.0001 + (gioThuc / 12.0) * 0.0003;
            }
            else if (gioThuc < 15)
            {
                bac = 0.0004 + ((gioThuc - 12.0) / 3.0) * 0.0001;
            }
            else
            {
                bac = 0.0005 + ((gioThuc - 15.0) / 9.0) * 0.0005;
                bac = Math.Min(bac, 0.001); // Tối đa 0.10%
            }

            return Math.Round(bac, 5);
        }

        private string PhanLoaiEisenhower(DateTime thoiHan, int soTinChi)
        {
            // KHẨN CẤP = còn <= 24 giờ
            bool khanCap = (thoiHan - DateTime.Now).TotalHours <= 72;

            // QUAN TRỌNG = tín chỉ >= 3
            bool quanTrong = soTinChi >= 3;

            if (khanCap && quanTrong)
                return "Q1"; // Đỏ: Làm ngay
            else if (!khanCap && quanTrong)
                return "Q2"; // Vàng: Lên lịch
            else if (khanCap && !quanTrong)
                return "Q3"; // Xanh dương: Ủy quyền
            else
                return "Q4"; // Xám: Bỏ qua
        }

        private string TinhThoiGianConLai(DateTime thoiHan)
        {
            TimeSpan khoangCach = thoiHan - DateTime.Now;

            if (khoangCach.TotalSeconds < 0)
            {
                // Quá hạn
                double tongPhut = Math.Abs(khoangCach.TotalMinutes);
                double tongGio = Math.Abs(khoangCach.TotalHours);
                double tongNgay = Math.Abs(khoangCach.TotalDays);

                if (tongNgay >= 1)
                    return $"Quá hạn {(int)tongNgay} ngày";
                else if (tongGio >= 1)
                    return $"Quá hạn {(int)tongGio} giờ";
                else
                    return "Quá hạn vài phút";
            }
            else
            {
                // Còn hạn
                if (khoangCach.TotalDays >= 1)
                    return $"Còn {(int)khoangCach.TotalDays} ngày";
                else if (khoangCach.TotalHours >= 1)
                    return $"Còn {(int)khoangCach.TotalHours} giờ";
                else
                    return $"Còn {(int)khoangCach.TotalMinutes} phút";
            }
        }

        private string TinhThoiGianTuLucTao(DateTime ngayTao)
        {
            TimeSpan khoangCach = DateTime.Now - ngayTao;

            if (khoangCach.TotalMinutes < 1)
                return "🕐 Vừa xong";
            else if (khoangCach.TotalMinutes < 60)
                return $"🕐 {(int)khoangCach.TotalMinutes} phút trước";
            else if (khoangCach.TotalHours < 24)
                return $"🕐 {(int)khoangCach.TotalHours} giờ trước";
            else if (khoangCach.TotalDays < 2)
                return "🕐 Hôm qua";
            else if (khoangCach.TotalDays < 7)
                return $"🕐 {(int)khoangCach.TotalDays} ngày trước";
            else
                return $"🕐 {ngayTao:dd/MM/yyyy}";
        }

        private string DuBaoDiemMonHoc(decimal? diemHienTai, decimal? trongSoConLai, decimal diemMucTieu)
        {
            // 1. Kiểm tra đầu vào
            if (diemHienTai == null || trongSoConLai == null || trongSoConLai == 0)
            {
                return diemHienTai == null ? "📋 Chưa có điểm" : "✅ Đủ điểm thành phần";
            }

            // 2. CÔNG THỨC CHUẨN:
            // diemHienTai lúc này là điểm tích lũy (ví dụ: 2.8)
            // diemMucTieu là đích đến hệ 10 (ví dụ: 9.0)
            decimal diemCan = (diemMucTieu - diemHienTai.Value) / (trongSoConLai.Value / 100m);

            // 3. Phân loại hiển thị
            if (diemCan > 10)
            {
                // Nếu điểm cần chỉ nhỉnh hơn 10 một chút (do làm tròn), có thể coi là cần 10 tuyệt đối
                if (diemCan < 10.5m)
                    return $"⚠️ Cần tối đa (cần 10.0)";
                return $"❌ Không khả thi (cần {diemCan:F1})";
            }
            else if (diemCan <= 0)
            {
                return "✅ Đã chắc chắn đạt mục tiêu";
            }
            else if (diemCan <= 5.0m) // Nếu chỉ cần 5 điểm thì rất an toàn
            {
                return $"✅ An toàn (cần ≥{diemCan:F1})";
            }
            else if (diemCan <= 8.5m)
            {
                return $"🎯 Mục tiêu (cần ≥{diemCan:F1})";
            }
            else
            {
                return $"🔥 Thử thách (cần ≥{diemCan:F1})";
            }
        }
        private string TinhMucDoUuTien(decimal diemHienTai, int soTinChi)
        {
            // Điểm < 5.0: priority-high (đỏ) - bất kể tín chỉ
            // Điểm 5.0-7.0 + Tín chỉ >= 3: priority-high (đỏ)
            // Điểm 5.0-7.0 + Tín chỉ < 3: priority-medium (vàng)
            // Điểm >= 7.0: priority-low (xanh)

            if (diemHienTai < 5.0m)
                return "priority-high";
            else if (diemHienTai < 7.0m && soTinChi >= 3)
                return "priority-high";
            else if (diemHienTai < 7.0m && soTinChi < 3)
                return "priority-medium";
            else
                return "priority-low";
        }

        private string TinhStressLevel(int soDeadlineDo, double gioThuc)
        {
            // Low: deadline đỏ < 2 && giờ thức < 8
            // Medium: deadline đỏ 2-4 || giờ thức 8-12
            // High: deadline đỏ > 4 || giờ thức > 12

            if (soDeadlineDo > 4 || gioThuc > 12)
                return "Cao";
            else if (soDeadlineDo >= 2 || gioThuc >= 8)
                return "Trung bình";
            else
                return "Thấp";
        }


        #endregion

        #region [5] Hành động (sự kiện click, mở form khác)
        private void btnPomodoro_Click(object sender, EventArgs e)
        {
            // Mở form Pomodoro (sẽ làm sau)
            MessageBox.Show("Chức năng Pomodoro sẽ được phát triển ở bước sau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            // Mở form Thêm môn học (sẽ làm sau)
            frm_MonHoc formMonHoc = new frm_MonHoc(maNguoiDung);
            formMonHoc.ShowDialog();
        }

        private void btnThemDeadline_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu đang DANGER thì khóa
            double gioThuc = 0;
            if (pnlAlhola.Tag != null)
                gioThuc = Convert.ToDouble(pnlAlhola.Tag);

            if (gioThuc >= 15)
            {
                MessageBox.Show("⚠️ Bạn đang trong trạng thái nguy hiểm (thức >15h).\nHệ thống tạm khóa chức năng thêm Deadline để bảo vệ sức khỏe.\nHãy nghỉ ngơi ít nhất 6 giờ trước khi tạo deadline mới.",
                                "Cảnh báo sức khỏe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở form Thêm nhiệm vụ (sẽ làm sau)
            MessageBox.Show("Chức năng Thêm Deadline sẽ được phát triển ở bước sau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAlholaAction_Click(object sender, EventArgs e)
        {
            double gioThuc = 0;
            if (pnlAlhola.Tag != null)
                gioThuc = Convert.ToDouble(pnlAlhola.Tag);

            if (gioThuc >= 15)
            {
                // DANGER: Khuyên nghỉ ngơi ngay
                DialogResult result = MessageBox.Show(
                    "Bạn đã thức quá lâu! Hệ thống khuyên bạn nên:\n\n" +
                    "1. Dừng mọi phiên học ngay lập tức\n" +
                    "2. Ngủ ít nhất 6-8 giờ\n" +
                    "3. Quay lại sau khi nghỉ ngơi đầy đủ\n\n" +
                    "Bạn có muốn thoát ứng dụng không?",
                    "🚨 Cảnh báo nghiêm trọng",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else if (gioThuc >= 12)
            {
                // CAUTION: Khuyên nghỉ sớm
                MessageBox.Show(
                    "Bạn đã học được " + gioThuc.ToString("F1") + " giờ.\n" +
                    "Nhận thức đang bắt đầu suy giảm. Hãy cân nhắc:\n\n" +
                    "• Nghỉ ngơi 15-30 phút\n" +
                    "• Uống nước và vận động nhẹ\n" +
                    "• Không tạo thêm deadline mới",
                    "⚠️ Nhắc nhở nghỉ ngơi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                // SAFE: Khuyến khích tiếp tục
                MessageBox.Show(
                    "Bạn đang ở trạng thái tốt! Thời gian thức: " + gioThuc.ToString("F1") + " giờ.\n\n" +
                    "Mẹo duy trì hiệu suất:\n" +
                    "• Uống đủ nước\n" +
                    "• Áp dụng Pomodoro 25/5\n" +
                    "• Nghỉ giải lao sau mỗi 2 giờ",
                    "😊 Trạng thái tốt",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }


        //Các nút menu Sidebar
        private void btnMenuDashboard_Click(object sender, EventArgs e)
        {
            // Đang ở Dashboard, load lại
            LoadToanBoDashboard();
        }

        private void btnMenuMonHoc_Click(object sender, EventArgs e)
        {
           
            frm_MonHoc formMonHoc = new frm_MonHoc(maNguoiDung);
            formMonHoc.Show();
            this.Hide(); // Đóng form Dashboard hiện tại
        }

        private void btnMenuDeadline_Click(object sender, EventArgs e)
        {
            frm_deadline formDeadline = new frm_deadline(maNguoiDung);
          // Đóng form Dashboard hiện tại
            formDeadline.Show();
        }

        private void btnMenuPomodoro_Click(object sender, EventArgs e)
        {
          
            frmpomodoro formPomo = new frmpomodoro(maNguoiDung);

            formPomo.Show();

            btnPomodoro_Click(sender, e);
        }

        private void btnMenuKhoTriThuc_Click(object sender, EventArgs e)
        {
            frm_GhiChu formGhiChu = new frm_GhiChu(maNguoiDung);
            formGhiChu.Show();

        }

        private void btnMenuBaoCao_Click(object sender, EventArgs e)
        {
            frm_baoCaoThongKe formBaoCao = new frm_baoCaoThongKe(maNguoiDung);
            formBaoCao.ShowDialog();
        
        }
            

        private void btnMenuTaiKhoan_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form Dashboard hiện tại
            MessageBox.Show("Chức năng Tài khoản sẽ được phát triển ở bước sau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnMenuDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Dừng timer
                timerAlhola.Stop();
                timerPulse.Stop();

                // Ẩn form hiện tại, mở form Login
                this.Hide();
                frm_login login = new frm_login(); // Sẽ mở sau khi có form Login
                // login.Show();
                this.Close();
            }
        }

        private void btnXemTatCaMonHoc_Click(object sender, EventArgs e)
        {
            btnMenuMonHoc_Click(sender, e);
        }

        private void btnMoKhoTriThuc_Click(object sender, EventArgs e)
        {
            btnMenuKhoTriThuc_Click(sender, e);
        }

        private void lblThemGhiChu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Thêm ghi chú sẽ được phát triển ở bước sau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region [6] Tiện ích giao diện (tạo control động, placeholder, animation)
        private Panel TaoTaskItem(NhiemVuEisenhowerDTO nv)
        {
            Panel pnl = new Panel
            {
                Height = 55,
                Width = 250,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0, 0, 0, 4),
                Padding = new Padding(8, 6, 8, 6),
                Cursor = Cursors.Hand
                // BỎ DÒNG Anchor
            };

            pnl.Tag = nv;

            // Label tiêu đề
            Label lblTitle = new Label
            {
                Text = nv.TieuDe,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 42, 74),
                Location = new Point(4, 4),
                AutoSize = true
            };
            pnl.Controls.Add(lblTitle);

            // Label môn học
            string monHocInfo = !string.IsNullOrEmpty(nv.MaMonHoc) ? $"{nv.MaMonHoc} · {nv.SoTinChi} tín" : "Không gắn môn";
            Label lblMeta = new Label
            {
                Text = monHocInfo,
                Font = new Font("Segoe UI", 7),
                ForeColor = Color.FromArgb(139, 156, 192),
                Location = new Point(4, 22),
                AutoSize = true
            };
            pnl.Controls.Add(lblMeta);

            // Badge thời gian còn lại
            string thoiGianText = TinhThoiGianConLai(nv.ThoiHan);
            Label lblBadge = new Label
            {
                Text = thoiGianText,
                Font = new Font("Segoe UI", 7, FontStyle.Bold),
                ForeColor = thoiGianText.Contains("Quá hạn") ? Color.FromArgb(185, 28, 28) :
                            thoiGianText.Contains("giờ") && !thoiGianText.Contains("Quá") ? Color.FromArgb(146, 64, 14) :
                            Color.FromArgb(6, 95, 70),
                BackColor = thoiGianText.Contains("Quá hạn") ? Color.FromArgb(254, 232, 232) :
                            thoiGianText.Contains("giờ") && !thoiGianText.Contains("Quá") ? Color.FromArgb(254, 243, 199) :
                            Color.FromArgb(209, 250, 229),
                Location = new Point(4, 38),
                AutoSize = true,
                Padding = new Padding(6, 1, 6, 1)
            };
            pnl.Controls.Add(lblBadge);

            // Hover effect nhẹ nhàng
            pnl.MouseEnter += (s, e) => { pnl.BackColor = Color.FromArgb(245, 247, 255); };
            pnl.MouseLeave += (s, e) => { pnl.BackColor = Color.White; };

            return pnl;
        }

        private Panel TaoSubjectItem(MonHocDiemDTO mon, decimal diemMucTieuThang10)
        {
            string priority = TinhMucDoUuTien(mon.DiemHienTai ?? 0, mon.SoTinChi);

            // Màu sắc theo priority
            Color borderColor, backColor;
            if (priority == "priority-high")
            {
                borderColor = Color.FromArgb(239, 68, 68);
                backColor = Color.FromArgb(254, 242, 242);
            }
            else if (priority == "priority-medium")
            {
                borderColor = Color.FromArgb(232, 168, 64);
                backColor = Color.FromArgb(255, 251, 235);
            }
            else
            {
                borderColor = Color.FromArgb(34, 169, 120);
                backColor = Color.FromArgb(240, 253, 244);
            }

            Panel pnl = new Panel
            {
                Height = 70,
                Width = flpMonHoc.Width - 20,  // Lấy Width từ flpMonHoc
                //Dock = DockStyle.Fill,
                BackColor = backColor,
                Margin = new Padding(0, 0, 0, 5),
                Padding = new Padding(10, 8, 10, 8),
                Cursor = Cursors.Hand

            };

            // Vẽ border trái bằng Paint
            pnl.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(borderColor, 4))
                {
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnl.Height);
                }
            };

            // Ô tín chỉ (bên trái)
            Panel pnlCredit = new Panel
            {
                Size = new Size(36, 36),
                Location = new Point(8, 12),
                BackColor = Color.FromArgb(232, 239, 255),
                Cursor = Cursors.Default
            };
            // Bo góc cho pnlCredit (nếu Guna2Panel thì dùng BorderRadius, tạm dùng Panel thường)
            Label lblCredit = new Label
            {
                Text = mon.SoTinChi.ToString(),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 107, 191),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            pnlCredit.Controls.Add(lblCredit);
            pnl.Controls.Add(pnlCredit);

            // Tên môn học
            Label lblTenMon = new Label
            {
                Text = mon.TenMonHoc,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 42, 74),
                Location = new Point(54, 8),
                AutoSize = true
            };
            pnl.Controls.Add(lblTenMon);

            // Meta: Giảng viên + Hình thức thi
            Label lblMeta = new Label
            {
                Text = $"{mon.TenGiangVien} · {mon.HinhThucThi}",
                Font = new Font("Segoe UI", 7),
                ForeColor = Color.FromArgb(139, 156, 192),
                Location = new Point(54, 28),
                AutoSize = true
            };
            pnl.Controls.Add(lblMeta);

            // Dự báo điểm
            string duBaoText = DuBaoDiemMonHoc(mon.DiemHienTai, mon.TrongSoConLai, diemMucTieuThang10);
            Color duBaoColor = duBaoText.Contains("❌") ? Color.FromArgb(185, 28, 28) :
                               duBaoText.Contains("✅") ? Color.FromArgb(6, 95, 70) :
                               Color.FromArgb(146, 64, 14);
            Label lblDuBao = new Label
            {
                Text = duBaoText,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = duBaoColor,
                Location = new Point(54, 44),
                AutoSize = true
            };
            pnl.Controls.Add(lblDuBao);

            // Điểm hiện tại (bên phải)
            string diemText = mon.DiemHienTai != null ? $"{mon.DiemHienTai:F1}/10" : "--/10";
            Color diemColor = (mon.DiemHienTai ?? 0) < 5.0m ? Color.FromArgb(224, 85, 85) :
                              (mon.DiemHienTai ?? 0) < 7.0m ? Color.FromArgb(232, 168, 64) :
                              Color.FromArgb(34, 169, 120);
            Label lblDiem = new Label
            {
                Text = diemText,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = diemColor,
                Location = new Point(pnl.Width - 70, 12),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            pnl.Controls.Add(lblDiem);

            // Hover effect
            pnl.MouseEnter += (s, e) => { pnl.BackColor = ControlPaint.Light(backColor); };
            pnl.MouseLeave += (s, e) => { pnl.BackColor = backColor; };

            return pnl;
        }

        private Panel TaoGhiChuItem(GhiChuDTO ghiChu)
        {
            Panel pnl = new Panel
            {
                Height = 55,
                Width = flpGhiChu.Width - 20,  // Lấy Width từ flpGhiChu
                //Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 252, 255),
                Margin = new Padding(0, 0, 0, 4),
                Padding = new Padding(10, 6, 10, 6),
                Cursor = Cursors.Hand
            };

            // Tag môn học
            string tagText = !string.IsNullOrEmpty(ghiChu.MaMonHoc) ? ghiChu.MaMonHoc : "GHI CHÚ";
            Label lblTag = new Label
            {
                Text = tagText,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 107, 191),
                BackColor = Color.FromArgb(232, 239, 255),
                Location = new Point(8, 8),
                AutoSize = true,
                Padding = new Padding(6, 2, 6, 2)
            };
            pnl.Controls.Add(lblTag);

            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = ghiChu.TieuDe,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 42, 74),
                Location = new Point(lblTag.Right + 8, 8),
                AutoSize = true
            };
            pnl.Controls.Add(lblTitle);

            // Nội dung (nếu có, rút gọn 1 dòng)
            if (!string.IsNullOrEmpty(ghiChu.NoiDung))
            {
                string noiDungRutGon = ghiChu.NoiDung.Length > 50
                    ? ghiChu.NoiDung.Substring(0, 50) + "..."
                    : ghiChu.NoiDung;

                Label lblNoiDung = new Label
                {
                    Text = noiDungRutGon,
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.FromArgb(139, 156, 192),
                    Location = new Point(8, 30),
                    AutoSize = true
                };
                pnl.Controls.Add(lblNoiDung);
            }

            // Thời gian
            string thoiGianText = TinhThoiGianTuLucTao(ghiChu.NgayTao);
            Label lblTime = new Label
            {
                Text = thoiGianText,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(139, 156, 192),
                Location = new Point(pnl.Width - 120, 8),
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                TextAlign = ContentAlignment.TopRight
            };
            pnl.Controls.Add(lblTime);

            // Hover effect
            pnl.MouseEnter += (s, e) =>
            {
                pnl.BackColor = Color.White;
                pnl.BackColor = Color.FromArgb(255, 255, 255);
            };
            pnl.MouseLeave += (s, e) =>
            {
                pnl.BackColor = Color.FromArgb(250, 252, 255);
            };

            return pnl;
        }

        private void HienThiPlaceholder(FlowLayoutPanel panel, string message)
        {
            // Chỉ hiện placeholder nếu flp TRỐNG (chưa có control nào)
            if (panel.Controls.Count > 0) return;

            Label lblPlaceholder = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(139, 156, 192),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblPlaceholder",
                Margin = new Padding(5, 10, 5, 0)
            };

            panel.Controls.Add(lblPlaceholder);
        }


        private void CapNhatMauDot(Panel dot, string trangThai)
        {
            switch (trangThai)
            {
                case "SAFE":
                    dot.BackColor = Color.FromArgb(34, 169, 120);
                    break;
                case "CAUTION":
                    dot.BackColor = Color.FromArgb(232, 168, 64);
                    break;
                case "DANGER":
                    dot.BackColor = Color.FromArgb(239, 68, 68);
                    break;
            }
        }

        private void CapNhatMauAlholaBar(string trangThai)
        {
            switch (trangThai)
            {
                case "SAFE":
                    pnlAlhola.BorderColor = Color.FromArgb(187, 247, 208);
                    pnlAlhola.FillColor = Color.FromArgb(240, 253, 244);
                    lblAlholaIcon.ForeColor = Color.FromArgb(22, 163, 74);
                    progAlhola.ProgressColor = Color.FromArgb(22, 163, 74);
                    progAlhola.ProgressColor2 = Color.FromArgb(34, 197, 94);
                    break;
                case "CAUTION":
                    pnlAlhola.BorderColor = Color.FromArgb(253, 230, 138);
                    pnlAlhola.FillColor = Color.FromArgb(255, 251, 235);
                    lblAlholaIcon.ForeColor = Color.FromArgb(232, 168, 64);
                    progAlhola.ProgressColor = Color.FromArgb(232, 168, 64);
                    progAlhola.ProgressColor2 = Color.FromArgb(250, 204, 21);
                    break;
                case "DANGER":
                    pnlAlhola.BorderColor = Color.FromArgb(254, 202, 202);
                    pnlAlhola.FillColor = Color.FromArgb(254, 242, 242);
                    lblAlholaIcon.ForeColor = Color.FromArgb(239, 68, 68);
                    progAlhola.ProgressColor = Color.FromArgb(239, 68, 68);
                    progAlhola.ProgressColor2 = Color.FromArgb(248, 113, 113);
                    break;
            }
            pnlAlhola.Invalidate();
            pnlAlhola.Refresh();
        }


        private void VeCalendarHeader()
        {
            // Xóa header cũ nếu có
            foreach (Control ctrl in pnlCalendar.Controls)
            {
                if (ctrl.Name == "pnlCalendarHeader")
                {
                    pnlCalendar.Controls.Remove(ctrl);
                    ctrl.Dispose();
                    break;
                }
            }

            // Tạo header mới - KHÔNG dùng Dock, dùng Location cố định
            Panel pnlHeader = new Panel
            {
                Name = "pnlCalendarHeader",
                Height = 24,
                Width = pnlCalendar.Width,
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };

            string[] thuLabels = { "T2", "T3", "T4", "T5", "T6", "T7", "CN" };
            int cellWidth = pnlHeader.Width / 7;

            for (int i = 0; i < 7; i++)
            {
                Label lbl = new Label
                {
                    Text = thuLabels[i],
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    ForeColor = Color.FromArgb(139, 156, 192),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(cellWidth, 24),
                    Location = new Point(i * cellWidth, 0)
                };
                pnlHeader.Controls.Add(lbl);
            }

            pnlCalendar.Controls.Add(pnlHeader);
        }

        private void VeCalendarBody(int thang, int nam, Dictionary<int, int> duLieu)
        {
            // Xóa body cũ nếu có
            foreach (Control ctrl in pnlCalendar.Controls)
            {
                if (ctrl.Name == "pnlCalendarBody")
                {
                    pnlCalendar.Controls.Remove(ctrl);
                    ctrl.Dispose();
                    break;
                }
            }

            // Ngày đầu tháng và số ngày
            DateTime ngayDau = new DateTime(nam, thang, 1);
            int soNgay = DateTime.DaysInMonth(nam, thang);

            // Thứ của ngày đầu tháng (0=CN, 1=T2... 6=T7)
            int thuDauTien = (int)ngayDau.DayOfWeek;
            // Quy đổi: T2=0, T3=1... CN=6
            int thuDauTienAdjusted = thuDauTien == 0 ? 6 : thuDauTien - 1;

            // Số tuần cần hiển thị
            int tongO = thuDauTienAdjusted + soNgay;
            int soTuan = (int)Math.Ceiling(tongO / 7.0);

            // Tính toán kích thước
            int headerHeight = 24;
            int bodyHeight = pnlCalendar.Height - headerHeight;
            int cellWidth = pnlCalendar.Width / 7;
            int cellHeight = Math.Min(bodyHeight / soTuan, 30);

            // Tạo body mới - KHÔNG dùng Dock, dùng Location và Size cố định
            Panel pnlBody = new Panel
            {
                Name = "pnlCalendarBody",
                Width = pnlCalendar.Width,
                Height = bodyHeight,
                Location = new Point(0, headerHeight), // Bắt đầu NGAY DƯỚI header
                BackColor = Color.Transparent
            };

            int ngayHienTai = 1;

            for (int tuan = 0; tuan < soTuan; tuan++)
            {
                for (int thu = 0; thu < 7; thu++)
                {
                    int x = thu * cellWidth;
                    int y = tuan * cellHeight;

                    // Ô trống trước ngày 1 hoặc sau ngày cuối
                    if ((tuan == 0 && thu < thuDauTienAdjusted) || ngayHienTai > soNgay)
                    {
                        Panel oTrong = new Panel
                        {
                            Size = new Size(cellWidth - 4, cellHeight - 4),
                            Location = new Point(x + 2, y + 2),
                            BackColor = Color.Transparent
                        };
                        pnlBody.Controls.Add(oTrong);
                    }
                    else
                    {
                        // Lấy giờ học từ dữ liệu
                        int phut = duLieu.ContainsKey(ngayHienTai) ? duLieu[ngayHienTai] : 0;
                        int gio = phut / 60;

                        // Xác định màu
                        Color mauO;
                        string intensity;
                        if (gio >= 4) { mauO = Color.FromArgb(74, 107, 191); intensity = "high"; }
                        else if (gio >= 2) { mauO = Color.FromArgb(139, 163, 217); intensity = "medium"; }
                        else if (gio >= 1) { mauO = Color.FromArgb(197, 211, 240); intensity = "low"; }
                        else { mauO = Color.FromArgb(237, 242, 251); intensity = "none"; }

                        Panel oNgay = new Panel
                        {
                            Size = new Size(cellWidth - 4, cellHeight - 4),
                            Location = new Point(x + 2, y + 2),
                            BackColor = mauO,
                            Tag = $"{ngayHienTai}/{thang}: {gio} giờ học",
                            Cursor = Cursors.Hand
                        };

                        // Label số ngày
                        Label lblNgay = new Label
                        {
                            Text = ngayHienTai.ToString(),
                            Font = new Font("Segoe UI", 7, FontStyle.Bold),
                            ForeColor = (intensity == "high" || intensity == "medium") ? Color.White : Color.FromArgb(139, 156, 192),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Fill
                        };
                        oNgay.Controls.Add(lblNgay);

                        pnlBody.Controls.Add(oNgay);
                        ngayHienTai++;
                    }
                }
            }

            pnlCalendar.Controls.Add(pnlBody);
        }

        private void TaoHeatmapLegend(Dictionary<int, int> duLieu)
        {
            // Xóa legend cũ nếu có
            foreach (Control ctrl in pnlHeatmapLegend.Controls)
            {
                if (ctrl.Name == "lblTongGio" || ctrl.Name == "pnlLegend")
                {
                    // Sẽ xóa hết rồi tạo lại
                }
            }
            pnlHeatmapLegend.Controls.Clear();

            // Tính tổng giờ từ dữ liệu
            int tongPhut = duLieu.Values.Sum();
            double tongGio = tongPhut / 60.0;

            // Panel chứa legend
            Panel pnlLegend = new Panel
            {
                Name = "pnlLegend",
                Height = 20,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent
            };

            // Các mức màu
            string[] labels = { "0-1h", "1-2h", "2-4h", "4h+" };
            Color[] colors = {
        Color.FromArgb(237, 242, 251),
        Color.FromArgb(197, 211, 240),
        Color.FromArgb(139, 163, 217),
        Color.FromArgb(74, 107, 191)
    };

            int startX = 10;
            for (int i = 0; i < 4; i++)
            {
                // Ô màu
                Panel dot = new Panel
                {
                    Size = new Size(14, 14),
                    Location = new Point(startX, 3),
                    BackColor = colors[i]
                };
                pnlLegend.Controls.Add(dot);

                // Label
                Label lbl = new Label
                {
                    Text = labels[i],
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.FromArgb(139, 156, 192),
                    Location = new Point(startX + 18, 2),
                    AutoSize = true
                };
                pnlLegend.Controls.Add(lbl);

                startX += 60;
            }

            pnlHeatmapLegend.Controls.Add(pnlLegend);

            // Label tổng giờ
            Label lblTongGio = new Label
            {
                Name = "lblTongGio",
                Text = $"Tổng: {tongGio:F1}h",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 42, 74),
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleRight,
                Height = 20
            };
            pnlHeatmapLegend.Controls.Add(lblTongGio);
        }

        private void CapNhatTrangThaiNutDeadline()
        {
            double gioThuc = 0;
            if (pnlAlhola.Tag != null)
                gioThuc = Convert.ToDouble(pnlAlhola.Tag);

            if (gioThuc >= 15)
            {
                btnThemDeadline.Enabled = false;
                btnThemDeadline.Text = "🔒 Deadline";
                btnThemDeadline.ForeColor = Color.FromArgb(139, 156, 192);
            }
            else
            {
                btnThemDeadline.Enabled = true;
                btnThemDeadline.Text = "➕ Deadline";
                btnThemDeadline.ForeColor = Color.FromArgb(30, 42, 74);
            }
        }

        private int rungCount = 0;
        private int rungDirection = 1;

        private void TimerAlhola_Tick(object sender, EventArgs e)
        {
            // Rung panel Alhola khi DANGER
            if (pnlAlhola != null)
            {
                int rungOffset = 3;
                pnlAlhola.Left += rungDirection * rungOffset;
                rungCount++;

                if (rungCount % 2 == 0)
                    rungDirection *= -1;

                if (rungCount >= 6)
                {
                    timerAlhola.Stop();
                    pnlAlhola.Left = pnlAlhola.Parent != null
                        ? (pnlAlhola.Parent.Width - pnlAlhola.Width) / 2
                        : 0;
                    rungCount = 0;
                    rungDirection = 1;
                }
            }
        }

        private bool pulseOn = true;

        private void TimerPulse_Tick(object sender, EventArgs e)
        {
            // Nhấp nháy icon Alhola
            if (lblAlholaIcon != null)
            {
                if (pulseOn)
                {
                    lblAlholaIcon.Font = new Font(lblAlholaIcon.Font.FontFamily, lblAlholaIcon.Font.Size + 3, FontStyle.Bold);
                }
                else
                {
                    lblAlholaIcon.Font = new Font(lblAlholaIcon.Font.FontFamily, lblAlholaIcon.Font.Size - 3, FontStyle.Bold);
                }
            }

            pulseOn = !pulseOn;
        }

        private void FlpStats_Resize(object sender, EventArgs e)
        {
            if (flpStats == null || flpStats.Controls.Count != 4) return;

            // Xóa hết margin và padding, tính width đều nhau
            int gap = 8;
            int totalWidth = flpStats.ClientSize.Width;
            int cardWidth = (totalWidth - gap * 3) / 4;
            if (cardWidth < 150) cardWidth = 150;

            // Set width + margin cho từng panel
            pnlStatMon.Width = cardWidth;
            pnlStatMon.Margin = new Padding(0, 0, gap, 0);

            pnlStatDeadline.Width = cardWidth;
            pnlStatDeadline.Margin = new Padding(0, 0, gap, 0);

            pnlStatKyLuat.Width = cardWidth;
            pnlStatKyLuat.Margin = new Padding(0, 0, gap, 0);

            pnlStatGhiChu.Width = cardWidth;
            pnlStatGhiChu.Margin = new Padding(0, 0, 0, 0); // Cái cuối không cần gap phải
        }
        // Trong Region 6, THAY bằng:
        private void PnlContentWrapper_Resize(object sender, EventArgs e)
        {
            if (tlpMain == null) return;

            // Lấy width thực tế từ cột 1 của tlpMain (cột chứa content)
            int newWidth = tlpMain.GetColumnWidths()[1] - 24;
            if (newWidth < 600) newWidth = 600;

            pnlWelcome.Width = newWidth;
            pnlAlhola.Width = newWidth;
            pnlHealth.Width = newWidth;
            flpStats.Width = newWidth;
            tlpMainGrid.Width = newWidth;
            tlpBottomRow.Width = newWidth;
        }

        private void flpStats_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frm_Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }

    #endregion
}

