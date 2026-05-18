using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using do_an_1.DAO;
using do_an_1.DTO;
using DrawingColor = System.Drawing.Color;
using Sizeee = System.Drawing.Size;

using QuestPDF.Infrastructure;

namespace do_an_1.Frm
{
    public partial class frm_baoCaoThongKe : Form
    {
        private int _maNguoiDung;
        private BaoCaoThongKeDAO _baoCaoDAO;

        // ── Bảng màu thống nhất ──────────────────────────────────────────
        private readonly SKColor ColorTask = SKColor.Parse("#3B82F6");  // xanh dương
        private readonly SKColor ColorStress = SKColor.Parse("#EF4444");  // đỏ
        private readonly SKColor ColorStandard = SKColor.Parse("#CBD5E1");  // xám nhạt
        private readonly SKColor ColorActual = SKColor.Parse("#10B981");  // xanh lá

        private readonly DrawingColor BgPage = DrawingColor.FromArgb(241, 245, 249);  // #F1F5F9
        private readonly DrawingColor BgCard = DrawingColor.White;
        private readonly DrawingColor TextDark = DrawingColor.FromArgb(15, 23, 42);     // #0F1726
        private readonly DrawingColor TextMuted = DrawingColor.FromArgb(100, 116, 139);  // #64748B
        private readonly DrawingColor BorderClr = DrawingColor.FromArgb(226, 232, 240);  // #E2E8F0

        // ── Constructor ──────────────────────────────────────────────────
        public frm_baoCaoThongKe(int maNguoiDung)
        {
            InitializeComponent();

            _maNguoiDung = maNguoiDung;
            _baoCaoDAO = new BaoCaoThongKeDAO();

            // FIX 1: Bỏ Bottom anchor tránh pnlTableCard biến mất
            pnlTableCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // FIX 2: Buộc scrollbar xuất hiện đúng
            this.AutoScrollMinSize = new Sizeee(0, pnlTableCard.Bottom + 30);

            // Chart margins
            cartesianChart1.DrawMargin = new LiveChartsCore.Measure.Margin(60, 20, 50, 75);
            cartesianChart2.DrawMargin = new LiveChartsCore.Measure.Margin(60, 20, 20, 75);

            // Responsive anchors
            pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFilters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tlpCharts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            ApplyModernUI();
        }

        // ── Load ─────────────────────────────────────────────────────────
        private void frm_baoCaoThongKe_Load(object sender, EventArgs e)
        {
            LoadSemesters();

            // FIX 3: Đổi học kỳ → reload môn → reload data (đúng thứ tự)
            cboSemesters.SelectedIndexChanged += (s, ev) =>
            {
                LoadSubjects(cboSemesters.SelectedItem?.ToString());
            };

            // FIX 4: Đổi môn cũng reload data
            cboSubjects.SelectedIndexChanged += SubjectChanged;

            LoadAllReportData();
        }

        private void SubjectChanged(object sender, EventArgs e) => LoadAllReportData();

        // ── Styling ───────────────────────────────────────────────────────
        private void ApplyModernUI()
        {
            this.BackColor = BgPage;

            // ── Header ──
            pnlHeader.BackColor = BgCard;
            pnlHeader.Padding = new Padding(20, 0, 20, 0);
            StyleCard(pnlHeader);
            lblHeaderTitle.ForeColor = TextDark;       // FIX 5: text vô hình → hiện rõ
            lblHeaderTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblSubHeader.ForeColor = TextMuted;

            // ── Filter bar ──
            pnlFilters.BackColor = BgCard;
            StyleCard(pnlFilters);
            StyleComboBox(cboSemesters);
            StyleComboBox(cboSubjects);
            StyleButton(btnIn);

            // ── Chart panels ──
            foreach (Panel p in new[] { pnlChart1, pnlChart2, pnlChart3 })
            {
                p.BackColor = BgCard;
                StyleCard(p);
            }

            // ── Table card ──
            pnlTableCard.BackColor = BgCard;
            StyleCard(pnlTableCard);

            // ── DataGrid ──
            StyleDataGrid();
        }

        private void StyleCard(Panel p)
        {
            p.Paint += (s, e) =>
            {
                var g = e.Graphics;
                var rect = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
                using var pen = new Pen(BorderClr, 1);
                g.DrawRectangle(pen, rect);
            };
        }

        private void StyleComboBox(ComboBox cbo)
        {
            cbo.FlatStyle = FlatStyle.Flat;
            cbo.BackColor = DrawingColor.White;
            cbo.ForeColor = TextDark;
            cbo.Font = new Font("Segoe UI", 10F);
        }

        private void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = DrawingColor.FromArgb(29, 78, 216);
            btn.BackColor = DrawingColor.FromArgb(37, 99, 235);
            btn.ForeColor = DrawingColor.White;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void StyleDataGrid()
        {
            var dgv = dgvReport;

            dgv.BackgroundColor = DrawingColor.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = DrawingColor.FromArgb(241, 245, 249);
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 46;
            dgv.ColumnHeadersHeight = 52;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Header style
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = DrawingColor.FromArgb(248, 250, 252),
                ForeColor = TextMuted,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = DrawingColor.FromArgb(248, 250, 252),
                SelectionForeColor = TextMuted,
                WrapMode = DataGridViewTriState.False
            };

            // Cell style
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = DrawingColor.White,
                ForeColor = TextDark,          // FIX 5 (DataGrid)
                Font = new Font("Segoe UI", 9.5F),
                SelectionBackColor = DrawingColor.FromArgb(239, 246, 255),
                SelectionForeColor = TextDark,
                WrapMode = DataGridViewTriState.False
            };

            // Alternating row DrawingColor
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = DrawingColor.FromArgb(250, 252, 255),
                ForeColor = TextDark,
                SelectionBackColor = DrawingColor.FromArgb(239, 246, 255),
                SelectionForeColor = TextDark,
            };

            // Column widths
            if (dgv.Columns["colMonHoc"] != null) dgv.Columns["colMonHoc"].Width = 340;
            if (dgv.Columns["colTinChi"] != null) dgv.Columns["colTinChi"].Width = 100;
            if (dgv.Columns["colGioPomo"] != null) dgv.Columns["colGioPomo"].Width = 180;
            if (dgv.Columns["colGhiChu"] != null) dgv.Columns["colGhiChu"].Width = 160;
            if (dgv.Columns["colHoanThanh"] != null) dgv.Columns["colHoanThanh"].Width = 160;
            if (dgv.Columns["colDanhGia"] != null) dgv.Columns["colDanhGia"].Width = 160;

            // Center-align number columns
            foreach (string col in new[] { "colTinChi", "colGioPomo", "colGhiChu", "colHoanThanh" })
            {
                if (dgv.Columns[col] != null)
                    dgv.Columns[col].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
            }
        }

        // ── Load dữ liệu bộ lọc ─────────────────────────────────────────
        private void LoadSemesters()
        {
            var list = _baoCaoDAO.GetHocKyByUser(_maNguoiDung);
            cboSemesters.Items.Clear();
            cboSemesters.Items.Add("Tất cả");
            list.ForEach(h => cboSemesters.Items.Add(h));
            cboSemesters.SelectedIndex = 0;
        }

        private void LoadSubjects(string hocKy)
        {
            // Tạm unsubscribe tránh LoadAllReportData gọi 2 lần
            cboSubjects.SelectedIndexChanged -= SubjectChanged;

            var list = _baoCaoDAO.GetMonHocByUser(_maNguoiDung, hocKy);

            // FIX: Tạo list DTO đồng nhất, không trộn lẫn string và object nữa
            var displayList = new List<MonHocDTO>();

            // Thêm item "Tất cả" dưới dạng DTO
            displayList.Add(new MonHocDTO { MaMonHoc = "Tất cả", TenMonHoc = "Tất cả" });
            displayList.AddRange(list);

            cboSubjects.DataSource = null;
            cboSubjects.DataSource = displayList;

            // Ép nó hiển thị đúng thuộc tính TenMonHoc
            cboSubjects.DisplayMember = "TenMonHoc";
            cboSubjects.ValueMember = "MaMonHoc";
            cboSubjects.SelectedIndex = 0;

            cboSubjects.SelectedIndexChanged += SubjectChanged;

            // Sau khi đổi học kỳ, reload luôn
            LoadAllReportData();
        }

        // ── Load toàn bộ báo cáo ─────────────────────────────────────────
        private void LoadAllReportData()
        {
            try
            {
                string hocKy = cboSemesters.SelectedItem?.ToString();
                string maMon = (cboSubjects.SelectedItem is MonHocDTO m) ? m.MaMonHoc : null;

                var dataTuan = _baoCaoDAO.GetThongKeTheoTuan(_maNguoiDung, hocKy, maMon);
                var dataGio = _baoCaoDAO.GetThongKeGioHoc(_maNguoiDung, hocKy, maMon);
                var dataPie = _baoCaoDAO.GetPhanBoThoiGian(_maNguoiDung, hocKy);

                VeBieuDoStress(dataTuan);
                VeBieuDoGioHoc(dataGio);
                VeBieuDoPhanBo(dataPie);
                LoadBangTongHop(dataGio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải dữ liệu: " + ex.Message,
                    "Study Tracker",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // ── Bảng tổng hợp ────────────────────────────────────────────────
        private void LoadBangTongHop(List<ThongKeGioHocDTO> dataGio)
        {
            dgvReport.Rows.Clear();

            if (dataGio.Count == 0)
            {
                // Hiển thị dòng thông báo khi không có dữ liệu
                dgvReport.Rows.Add("(Chưa có dữ liệu)", "-", "-", "-", "-", "-");
                return;
            }

            foreach (var item in dataGio)
            {
                // FIX 2: Cột mapping đúng — colGhiChu → Giờ Chuẩn
                dgvReport.Rows.Add(
                    item.TenMonHoc,
                    item.SoTinChi,
                    $"{item.GioThucTe:F1} giờ",    // colGioPomo  — giờ thực tế
                    $"{item.GioChuan:F0} giờ",       // colGhiChu   — giờ chuẩn (đã đổi header)
                    $"{item.TyLeHoanThanh:F1}%",     // colHoanThanh
                    item.DanhGia                     // colDanhGia
                );
            }

            // Tổng cộng
            double tongThucTe = dataGio.Sum(x => x.GioThucTe);
            double tongChuan = dataGio.Sum(x => x.GioChuan);
            double tyLeTB = tongChuan > 0 ? Math.Round((tongThucTe / tongChuan) * 100, 1) : 0;

            int iTotal = dgvReport.Rows.Add(
                "TỔNG CỘNG",
                dataGio.Sum(x => x.SoTinChi),
                $"{tongThucTe:F1} giờ",
                $"{tongChuan:F0} giờ",
                $"{tyLeTB:F1}%",
                tyLeTB >= 100 ? "Xuất sắc" : tyLeTB >= 50 ? "Tốt" : "Cần cố gắng"
            );

            // Style dòng tổng
            var rowTotal = dgvReport.Rows[iTotal];
            foreach (DataGridViewCell cell in rowTotal.Cells)
            {
                cell.Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                cell.Style.BackColor = DrawingColor.FromArgb(239, 246, 255);
                cell.Style.ForeColor = DrawingColor.FromArgb(37, 99, 235);
            }
        }

        // ── Biểu đồ 1: Stress & Nhiệm vụ theo tuần ──────────────────────
        private void VeBieuDoStress(List<ThongKeTuanDTO> data)
        {
            if (data.Count == 0)
            {
                cartesianChart1.Series = Array.Empty<ISeries>();
                return;
            }

            cartesianChart1.Series = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Name        = "Nhiệm vụ",
                    Values      = data.Select(x => x.TongNhiemVu).ToArray(),
                    Fill        = new SolidColorPaint(ColorTask),
                    MaxBarWidth = 36,
                    ScalesYAt   = 0   // FIX 4: trục Y trái
                },
                new LineSeries<double>
                {
                    Name           = "Stress (%)",
                    Values         = data.Select(x => x.StressIndex).ToArray(),
                    Stroke         = new SolidColorPaint(ColorStress) { StrokeThickness = 3 },
                    Fill           = null,
                    GeometrySize   = 10,
                    LineSmoothness = 0.5,
                    ScalesYAt      = 1  // FIX 4: trục Y phải
                }
            };

            cartesianChart1.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels          = data.Select(x => x.TenTuan).ToArray(),
                    LabelsRotation  = -20,
                    TextSize        = 11,
                    SeparatorsPaint = null
                }
            };

            // FIX 4: Dual Y-axis rõ ràng
            cartesianChart1.YAxes = new Axis[]
            {
                new Axis  // Trái — số nhiệm vụ
                {
                    Name            = "Số nhiệm vụ",
                    NameTextSize    = 10,
                    TextSize        = 11,
                    MinLimit        = 0,
                    SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#E5E7EB"))
                },
                new Axis  // Phải — stress %
                {
                    Name            = "Stress (%)",
                    NameTextSize    = 10,
                    TextSize        = 11,
                    MinLimit        = 0,
                    MaxLimit        = 100,
                    Position        = LiveChartsCore.Measure.AxisPosition.End,
                    SeparatorsPaint = null
                }
            };

            cartesianChart1.LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom;
        }

        // ── Biểu đồ 2: Giờ chuẩn vs thực tế ─────────────────────────────
        // 2. SỬA LỖI BUILD 'PrimaryValue' VÀ LOGIC TÍNH TOÁN TOOLTIP
        private void VeBieuDoGioHoc(List<ThongKeGioHocDTO> data)
        {
            if (data.Count == 0) { cartesianChart2.Series = Array.Empty<ISeries>(); return; }

            var dataList = data;
            var labels = data.Select(x => x.TenMonHoc).ToArray();

            cartesianChart2.Series = new ISeries[]
            {
        new ColumnSeries<double>
        {
            Name = "Giờ chuẩn (BộGD&ĐT)",
            Values = data.Select(x => x.GioChuan).ToArray(),
            Fill = new SolidColorPaint(ColorStandard),
            YToolTipLabelFormatter = point => {
                int index = point.Index;
                if (index < 0 || index >= dataList.Count) return "";
                return $"{dataList[index].TenMonHoc}: {point.Coordinate.PrimaryValue:N0} giờ (Quy định)";
            }
        },
        new ColumnSeries<double>
        {
            Name = "Giờ thực tế (Pomodoro)",
            Values = data.Select(x => x.GioThucTe > 0 ? x.GioThucTe : 0.2).ToArray(),
            Fill = new SolidColorPaint(ColorActual),
            YToolTipLabelFormatter = point => {
                int index = point.Index;
                if (index < 0 || index >= dataList.Count) return "";
                double val = point.Coordinate.PrimaryValue;
                double realVal = val <= 0.2 ? 0 : val;
                return $"{dataList[index].TenMonHoc}: {realVal:N1} giờ (Đã học)";
            }
        }
            };

            cartesianChart2.XAxes = new[]
      {
        new Axis
        {
            Labels = labels,           // Mã môn: SQL101, NET101, ...
            LabelsRotation = 45,
            Name = "Môn học"
        }
    };
        }

        // ── Biểu đồ 3: Pie chart phân bổ thời gian ───────────────────────
        private void VeBieuDoPhanBo(List<PhanBoThoiGianDTO> data)
        {
            // ✅ Xóa label "no data" cũ nếu có
            var oldLabel = pnlChart3.Controls.Find("lblNoDataPie", false);
            foreach (Control c in oldLabel) pnlChart3.Controls.Remove(c);
            pieChart1.Visible = true;

            if (data.Count == 0)
            {
                // ✅ Hiện label thay vì để chart trống
                pieChart1.Visible = false;
                var lbl = new Label
                {
                    Name = "lblNoDataPie",
                    Text = "📊 Chưa có dữ liệu Pomodoro\nHãy bắt đầu một phiên học để xem phân bổ thời gian",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = DrawingColor.FromArgb(148, 163, 184),
                    Dock = DockStyle.Fill
                };
                pnlChart3.Controls.Add(lbl);
                lbl.BringToFront();
                return;
            }

            var colors = new[]
            {
        "#3B82F6", "#10B981", "#F59E0B", "#EF4444",
        "#8B5CF6", "#06B6D4", "#EC4899", "#84CC16"
    };

            pieChart1.Series = data.Select((x, i) =>
                new PieSeries<double>
                {
                    Name = $"{x.TenMonHoc} ({x.TyLePhanTram:F1}%)",
                    Values = new double[] { x.TongGio },
                    Fill = new SolidColorPaint(SKColor.Parse(colors[i % colors.Length])),

                    DataLabelsSize = 13,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = _ => $"{x.TyLePhanTram:F1}%",

                    InnerRadius = 55,
                    Stroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 }
                }).ToArray();

            pieChart1.LegendPosition = LiveChartsCore.Measure.LegendPosition.Right;
        }

        // ── Cell formatting — cột Đánh giá ───────────────────────────────
        private void dgvReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvReport.Columns[e.ColumnIndex].Name == "colDanhGia" && e.Value != null)
            {
                string val = e.Value.ToString();
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                switch (val)
                {
                    case "Xuất sắc":
                        e.CellStyle.ForeColor = DrawingColor.FromArgb(5, 150, 105);  // xanh đậm
                        e.CellStyle.BackColor = DrawingColor.FromArgb(209, 250, 229);
                        break;
                    case "Tốt":
                        e.CellStyle.ForeColor = DrawingColor.FromArgb(37, 99, 235);  // xanh dương
                        e.CellStyle.BackColor = DrawingColor.FromArgb(219, 234, 254);
                        break;
                    default: // Cần cố gắng
                        e.CellStyle.ForeColor = DrawingColor.FromArgb(185, 28, 28);  // đỏ
                        e.CellStyle.BackColor = DrawingColor.FromArgb(254, 226, 226);
                        break;
                }
            }

            // Cột % hoàn thành — màu theo mức
            if (dgvReport.Columns[e.ColumnIndex].Name == "colHoanThanh" && e.Value != null)
            {
                string raw = e.Value.ToString().Replace("%", "").Trim();
                if (double.TryParse(raw, out double pct))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.CellStyle.ForeColor = pct >= 100 ? DrawingColor.FromArgb(5, 150, 105)
                                          : pct >= 50 ? DrawingColor.FromArgb(37, 99, 235)
                                          : DrawingColor.FromArgb(185, 28, 28);
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e) => LoadAllReportData();

        private void pieChart1_Load(object sender, EventArgs e)
        {

        }


        // ════════════════════════════════════════════════════════════════
        // XUẤT PDF
        // ════════════════════════════════════════════════════════════════
        private void btnIn_Click(object sender, EventArgs e)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            string hocKy = cboSemesters.SelectedItem?.ToString();
            string maMon = (cboSubjects.SelectedItem is MonHocDTO mhSel) ? mhSel.MaMonHoc : null;

            // FIX 6: hiển thị tên đúng
            string tenHocKy = string.IsNullOrEmpty(hocKy) || hocKy == "Tất cả"
                              ? "Tất cả học kỳ" : hocKy;
            string tenMon = maMon == null
                              ? "Tất cả môn học"
                              : (cboSubjects.SelectedItem as MonHocDTO)?.TenMonHoc ?? "Tất cả môn học";

            List<ThongKeGioHocDTO> dataGio;
            List<DiemMonHocDTO> dataDiem;

            try
            {
                dataGio = _baoCaoDAO.GetThongKeGioHoc(_maNguoiDung, hocKy, maMon);
                dataDiem = _baoCaoDAO.GetDiemMonHoc(_maNguoiDung, hocKy, maMon);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message,
                    "Study Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] imgChart1 = CaptureControl(cartesianChart1);
            byte[] imgChart2 = CaptureControl(cartesianChart2);
            byte[] imgChart3 = CaptureControl(pieChart1);

            using var dlg = new SaveFileDialog
            {
                Title = "Lưu báo cáo PDF",
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = $"BaoCao_StudyTracker_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
                DefaultExt = "pdf"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            string savePath = dlg.FileName;

            try
            {
                btnIn.Enabled = false;
                btnIn.Text = "⏳ Đang xuất...";

                // Tất cả màu QuestPDF dùng hex string → không bị ambiguous
                const string clrHeader = "#1E2A4A";
                const string clrMuted = "#64748B";
                const string clrPrimary = "#4A6BBF";
                const string clrSuccess = "#059669";
                const string clrDanger = "#B91C1C";
                const string clrInfo = "#2563EB";
                const string clrWarning = "#EAB308";
                const string clrBorder = "#E2E8F0";
                const string clrRowAlt = "#F8FAFC";
                const string clrTotalBg = "#EFF6FF";
                const string clrGpaBg = "#F0FDF4";
                const string clrHeaderBlue = "#4A6BBF";
                const string clrHeaderGreen = "#34A978";

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());
                        page.Margin(1.5f, Unit.Centimetre);
                        page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(9));

                        // ── Header ──
                        page.Header().Column(col =>
                        {
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("BÁO CÁO THỐNG KÊ HỌC TẬP")
                                     .Bold().FontSize(16).FontColor(clrHeader);
                                    c.Item().Text(txt =>
                                    {
                                        txt.Span("Học kỳ: ").SemiBold(); txt.Span(tenHocKy);
                                        txt.Span("   |   ");
                                        txt.Span("Môn: ").SemiBold(); txt.Span(tenMon);
                                    });
                                });
                                row.ConstantItem(200).AlignRight().Column(c =>
                                {
                                    c.Item().Text("Study Tracker")
                                     .SemiBold().FontSize(11).FontColor(clrPrimary);
                                    c.Item().Text($"Xuất ngày {DateTime.Now:dd/MM/yyyy HH:mm}")
                                     .FontSize(8).FontColor(clrMuted);
                                });
                            });
                            col.Item().PaddingTop(4).LineHorizontal(1f).LineColor(clrBorder);
                        });

                        // ── Footer ──
                        page.Footer().AlignCenter().Text(txt =>
                        {
                            txt.Span("Trang ").FontSize(8).FontColor(clrMuted);
                            txt.CurrentPageNumber().FontSize(8).FontColor(clrMuted);
                            txt.Span(" / ").FontSize(8).FontColor(clrMuted);
                            txt.TotalPages().FontSize(8).FontColor(clrMuted);
                        });

                        // ── Content ──
                        page.Content().PaddingTop(8).Column(col =>
                        {
                            // Section 1: Biểu đồ
                            col.Item().Text("1. BIỂU ĐỒ TỔNG QUAN")
                               .Bold().FontSize(11).FontColor(clrHeader);

                            col.Item().PaddingTop(6).Row(row =>
                            {
                                // FIX 7: dùng Action<> thay vì local function để tránh lỗi compiler
                                Action<string, byte[]> addChart = (title, img) =>
                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Text(title).FontSize(8).SemiBold().FontColor(clrMuted);
                                        c.Item().PaddingTop(3).Border(0.5f).BorderColor(clrBorder)
                                         .Image(img).FitWidth();
                                    });

                                addChart("Stress & Nhiệm vụ theo tuần", imgChart1);
                                row.ConstantItem(8);
                                addChart("Giờ chuẩn vs Thực tế (Pomodoro)", imgChart2);
                                row.ConstantItem(8);
                                addChart("Phân bổ thời gian học", imgChart3);
                            });

                            col.Item().PaddingTop(14);

                            // Section 2: Bảng giờ học
                            col.Item().Text("2. BẢNG THỐNG KÊ GIỜ HỌC")
                               .Bold().FontSize(11).FontColor(clrHeader);

                            col.Item().PaddingTop(6).Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    cols.RelativeColumn(3.5f); cols.RelativeColumn(0.8f);
                                    cols.RelativeColumn(1.2f); cols.RelativeColumn(1.2f);
                                    cols.RelativeColumn(1.2f); cols.RelativeColumn(1.2f);
                                });

                                table.Header(h =>
                                {
                                    foreach (var t in new[] {
                                        "MÔN HỌC","TÍN CHỈ","GIỜ THỰC TẾ",
                                        "GIỜ CHUẨN","HOÀN THÀNH","ĐÁNH GIÁ" })
                                        h.Cell().Background(clrHeaderBlue).Padding(6)
                                         .Text(t).FontSize(8).Bold().FontColor(Colors.White);
                                });

                                bool alt = false;
                                foreach (var item in dataGio)
                                {
                                    string bg = alt ? clrRowAlt : Colors.White;
                                    alt = !alt;
                                    string dc = item.DanhGia == "Xuất sắc" ? clrSuccess
                                              : item.DanhGia == "Tốt" ? clrInfo : clrDanger;

                                    table.Cell().Background(bg).Padding(5).Text(item.TenMonHoc).FontSize(8);
                                    table.Cell().Background(bg).Padding(5).AlignCenter().Text(item.SoTinChi.ToString()).FontSize(8);
                                    table.Cell().Background(bg).Padding(5).AlignCenter().Text($"{item.GioThucTe:F1}h").FontSize(8);
                                    table.Cell().Background(bg).Padding(5).AlignCenter().Text($"{item.GioChuan:F0}h").FontSize(8);
                                    table.Cell().Background(bg).Padding(5).AlignCenter().Text($"{item.TyLeHoanThanh:F1}%").FontSize(8);
                                    table.Cell().Background(bg).Padding(5).Text(item.DanhGia).FontSize(8).SemiBold().FontColor(dc);
                                }

                                if (dataGio.Count > 0)
                                {
                                    double tt = dataGio.Sum(x => x.GioThucTe);
                                    double tc = dataGio.Sum(x => x.GioChuan);
                                    double tl = tc > 0 ? Math.Round(tt / tc * 100, 1) : 0;
                                    foreach (var t in new[] {
                                        "TỔNG CỘNG", dataGio.Sum(x => x.SoTinChi).ToString(),
                                        $"{tt:F1}h", $"{tc:F0}h", $"{tl:F1}%", "" })
                                        table.Cell().Background(clrTotalBg).Padding(5)
                                             .Text(t).FontSize(8).Bold().FontColor(clrInfo);
                                }
                            });

                            col.Item().PaddingTop(14);

                            // Section 3: Bảng điểm
                            col.Item().Text("3. BẢNG ĐIỂM TỪNG MÔN")
                               .Bold().FontSize(11).FontColor(clrHeader);

                            col.Item().PaddingTop(6).Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    cols.RelativeColumn(3.5f); cols.RelativeColumn(0.7f);
                                    cols.RelativeColumn(1f); cols.RelativeColumn(1.2f);
                                    cols.RelativeColumn(1f); cols.RelativeColumn(1f);
                                    cols.RelativeColumn(1.5f);
                                });

                                table.Header(h =>
                                {
                                    foreach (var t in new[] {
                                        "MÔN HỌC","TC","HỌC KỲ",
                                        "ĐIỂM TB","CHỮ","HỆ 4","XẾP LOẠI" })
                                        h.Cell().Background(clrHeaderGreen).Padding(6)
                                         .Text(t).FontSize(8).Bold().FontColor(Colors.White);
                                });

                                bool alt = false;
                                foreach (var item in dataDiem)
                                {
                                    string bg = alt ? clrRowAlt : Colors.White;
                                    alt = !alt;
                                    bool chuaXong = item.SoCotDaNhap < item.TongSoCot;
                                    string xlColor = item.XepLoai switch
                                    {
                                        "Xuất sắc" => clrSuccess,
                                        "Giỏi" => clrInfo,
                                        "Khá" => clrPrimary,
                                        "Trung bình" => clrWarning,
                                        "Yếu" => clrDanger,
                                        _ => clrMuted
                                    };

                                    table.Cell().Background(bg).Padding(5).Text(item.TenMonHoc).FontSize(8);
                                    table.Cell().Background(bg).Padding(5).AlignCenter().Text(item.SoTinChi.ToString()).FontSize(8);
                                    table.Cell().Background(bg).Padding(5).AlignCenter().Text(item.HocKy ?? "--").FontSize(8);
                                    table.Cell().Background(bg).Padding(5).AlignCenter()
                                         .Text(chuaXong ? $"{item.DiemTrungBinh:F2} (*)" : $"{item.DiemTrungBinh:F2}").FontSize(8);
                                    table.Cell().Background(bg).Padding(5).AlignCenter().Text(item.DiemChu).FontSize(8).Bold();
                                    table.Cell().Background(bg).Padding(5).AlignCenter()
                                         .Text(chuaXong ? "--" : $"{item.DiemHe4:F1}").FontSize(8).Bold();
                                    table.Cell().Background(bg).Padding(5)
                                         .Text(item.XepLoai).FontSize(8).SemiBold().FontColor(xlColor);
                                }

                                var monXong = dataDiem
                                    .Where(x => x.SoCotDaNhap == x.TongSoCot && x.TongSoCot > 0)
                                    .ToList();

                                if (monXong.Count > 0)
                                {
                                    double tongTC = monXong.Sum(x => x.SoTinChi);
                                    double gpa = tongTC > 0
                                        ? Math.Round(monXong.Sum(x => x.DiemHe4 * x.SoTinChi) / tongTC, 2) : 0;
                                    string gpaXL = gpa >= 3.6 ? "Xuất sắc"
                                                  : gpa >= 3.2 ? "Giỏi"
                                                  : gpa >= 2.5 ? "Khá"
                                                  : gpa >= 2.0 ? "Trung bình" : "Yếu";

                                    foreach (var t in new[] {
                                        "GPA TÍCH LŨY", $"{(int)tongTC} TC",
                                        "", "", "", $"{gpa:F2}", gpaXL })
                                        table.Cell().Background(clrGpaBg).Padding(5)
                                             .Text(t).FontSize(8).Bold().FontColor(clrSuccess);
                                }
                            });

                            col.Item().PaddingTop(8)
                               .Text("(*) Môn chưa đủ điểm — điểm hiển thị là tạm tính")
                               .FontSize(7.5f).Italic().FontColor(clrMuted);
                        });
                    });
                }).GeneratePdf(savePath);

                var res = MessageBox.Show(
                    $"Xuất PDF thành công!\n{savePath}\n\nMở file ngay?",
                    "Study Tracker", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo(savePath)
                        { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất PDF: " + ex.Message,
                    "Study Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnIn.Enabled = true;
                btnIn.Text = "🖨️ In báo cáo";
            }
        }

        private byte[] CaptureControl(Control ctrl)
        {
            try
            {
                ctrl.Update();
                Application.DoEvents();

                using var bmp = new System.Drawing.Bitmap(ctrl.Width, ctrl.Height);
                ctrl.DrawToBitmap(bmp,
                    new System.Drawing.Rectangle(0, 0, ctrl.Width, ctrl.Height));

                using var ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
            catch
            {
                using var bmp = new System.Drawing.Bitmap(300, 120);
                using var g = System.Drawing.Graphics.FromImage(bmp);
                g.Clear(DrawingColor.White);
                g.DrawString("(Không có dữ liệu)",
                    System.Drawing.SystemFonts.DefaultFont,
                    System.Drawing.Brushes.Gray, 80, 50);

                using var ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}