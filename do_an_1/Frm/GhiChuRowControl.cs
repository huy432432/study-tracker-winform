using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using do_an_1.DTO;

namespace do_an_1.Frm
{
    public partial class GhiChuRowControl : UserControl
    {
        // ── Dữ liệu ────────────────────────────────────────────────
        public GhiChuDTO GhiChu { get; set; }
        public string TenMonHoc { get; set; }   // Truyền vào từ ngoài để hiển thị

        // ── Events ra ngoài ────────────────────────────────────────
        public event EventHandler SuaClick;
        public event EventHandler XoaClick;

        // ── Màu sắc ────────────────────────────────────────────────
        private readonly Color ColorPrimary = Color.FromArgb(74, 107, 191);
        private readonly Color ColorDanger = Color.FromArgb(224, 85, 85);
        private readonly Color ColorSuccess = Color.FromArgb(52, 169, 120);
        private readonly Color ColorText = Color.FromArgb(30, 42, 74);
        private readonly Color ColorMuted = Color.FromArgb(100, 116, 139);
        private readonly Color ColorBorder = Color.FromArgb(226, 232, 240);
        private readonly Color ColorTagBg = Color.FromArgb(232, 239, 255);
        private readonly Color ColorHover = Color.FromArgb(248, 250, 252);

        // ── Trạng thái ─────────────────────────────────────────────
        private bool _isExpanded = false;
        private bool _isHovered = false;
        private const int ROW_H = 52;
        private const int EXPAND_H = 200;

        // ── Controls ───────────────────────────────────────────────
        private Panel _pnlExpand;
        private Button _btnSua;
        private Button _btnXoa;

        // ── Tọa độ các cột (tính từ trái) ─────────────────────────
        //  Tiêu đề  | Môn học  | Từ khóa  | Ngày      | Thao tác
        //  0-34%    | 34-52%   | 52-72%   | 72-85%    | 85-100%
        private int ColTitle => 16;
        private int ColTitleW => (int)(Width * 0.33);
        private int ColMon => ColTitle + ColTitleW + 8;
        private int ColMonW => (int)(Width * 0.17);
        private int ColTag => ColMon + ColMonW + 8;
        private int ColTagW => (int)(Width * 0.20);
        private int ColDate => ColTag + ColTagW + 8;
        private int ColDateW => (int)(Width * 0.12);
        private int ColAction => ColDate + ColDateW + 8;

        // ────────────────────────────────────────────────────────────
        public GhiChuRowControl()
        {
            InitializeComponent();
            SetupControl();
        }

        // ────────────────────────────────────────────────────────────
        private void SetupControl()
        {
            this.Height = ROW_H;
            this.Cursor = Cursors.Hand;

            this.Paint += OnPaint;
            this.Click += OnRowClick;
            this.MouseEnter += (s, e) => { _isHovered = true; Invalidate(); };
            this.MouseLeave += (s, e) => { _isHovered = false; Invalidate(); };
            this.Resize += (s, e) => { LayoutButtons(); Invalidate(); };

            CreateExpandPanel();
            CreateButtons();
        }

        // ══════════════════════════════════════════════════════════
        // VẼ ROW HEADER
        // ══════════════════════════════════════════════════════════
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Nền hover
            if (_isHovered)
            {
                using var hBrush = new SolidBrush(ColorHover);
                g.FillRectangle(hBrush, 0, 0, Width, ROW_H);
            }

            // Đường kẻ dưới row
            using var borderPen = new Pen(ColorBorder, 1f);
            g.DrawLine(borderPen, 0, ROW_H - 1, Width, ROW_H - 1);

            if (GhiChu == null) return;

            int cy = ROW_H / 2;   // tâm dọc

            // ── 1. Tiêu đề ──────────────────────────────────────
            string title = GhiChu.TieuDe ?? "";
            DrawEllipsisText(g, title,
                new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ColorText,
                new Rectangle(ColTitle, cy - 10, ColTitleW, 20));

            // ── 2. Tag môn học ──────────────────────────────────
            string monLabel = string.IsNullOrEmpty(GhiChu.MaMonHoc)
                ? "Chung"
                : (TenMonHoc?.Length > 16
                    ? TenMonHoc.Substring(0, 14) + "…"
                    : TenMonHoc ?? GhiChu.MaMonHoc);

            bool isChung = string.IsNullOrEmpty(GhiChu.MaMonHoc);
            Color tagBg = isChung ? Color.FromArgb(241, 245, 249) : ColorTagBg;
            Color tagFg = isChung ? ColorMuted : ColorPrimary;

            DrawTag(g, monLabel, ColMon, cy - 11, tagBg, tagFg);

            // ── 3. Từ khóa (tối đa 3 tag) ───────────────────────
            if (!string.IsNullOrEmpty(GhiChu.TuKhoa))
            {
                var tags = GhiChu.TuKhoa.Split(',');
                int tx = ColTag;
                foreach (var t in tags)
                {
                    string tag = "#" + t.Trim();
                    int tw = MeasureTagWidth(g, tag);
                    if (tx + tw > ColTag + ColTagW) break;
                    DrawTag(g, tag, tx, cy - 11, Color.FromArgb(243, 244, 246),
                                                  Color.FromArgb(107, 114, 128));
                    tx += tw + 4;
                }
            }
            else
            {
                using var mutedBrush = new SolidBrush(ColorMuted);
                g.DrawString("--", new Font("Segoe UI", 8.5f), mutedBrush,
                    ColTag, cy - 8);
            }

            // ── 4. Ngày cập nhật ────────────────────────────────
            string dateStr = GhiChu.NgayCapNhat.ToString("dd/MM/yyyy");
            using var dateBrush = new SolidBrush(ColorMuted);
            g.DrawString(dateStr, new Font("Segoe UI", 8.5f), dateBrush,
                ColDate, cy - 8);

            // ── 5. Mũi tên expand ───────────────────────────────
            int ax = ColAction - 28;
            int ay = cy;
            string arrow = _isExpanded ? "▲" : "▼";
            using var arrowBrush = new SolidBrush(ColorMuted);
            g.DrawString(arrow, new Font("Segoe UI", 7f), arrowBrush, ax, ay - 7);
        }

        // ══════════════════════════════════════════════════════════
        // EXPAND PANEL — Nội dung + Link
        // ══════════════════════════════════════════════════════════
        private void CreateExpandPanel()
        {
            _pnlExpand = new Panel
            {
                Location = new Point(0, ROW_H),
                Size = new Size(Width, EXPAND_H),
                BackColor = Color.FromArgb(250, 252, 255),
                Visible = false
            };

            // Vẽ border và nội dung bên trong
            _pnlExpand.Paint += PaintExpandPanel;
            _pnlExpand.Resize += (s, e) => _pnlExpand.Invalidate();

            this.Controls.Add(_pnlExpand);
        }

        private void PaintExpandPanel(object sender, PaintEventArgs e)
        {
            if (GhiChu == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int pw = _pnlExpand.Width;
            int ph = _pnlExpand.Height;
            int half = pw / 2 - 24;
            int pad = 16;

            // ── Panel trái: Nội dung ───────────────────────────
            DrawCard(g, pad, pad, half, ph - pad * 2);

            using var titleFont = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var bodyFont = new Font("Segoe UI", 9f);
            using var titleBrush = new SolidBrush(ColorMuted);
            using var bodyBrush = new SolidBrush(ColorText);

            g.DrawString("📄  NỘI DUNG", titleFont, titleBrush,
                pad + 12, pad + 12);
            g.DrawLine(new Pen(ColorBorder, 1f),
                pad + 12, pad + 30, pad + half - 12, pad + 30);

            string content = string.IsNullOrEmpty(GhiChu.NoiDung)
                ? "(Chưa có nội dung)"
                : GhiChu.NoiDung;

            var contentRect = new RectangleF(pad + 12, pad + 38,
                                              half - 24, ph - pad * 2 - 50);
            g.DrawString(content, bodyFont, bodyBrush, contentRect);

            // ── Panel phải: Link tài liệu ──────────────────────
            int rx = pad + half + pad;
            DrawCard(g, rx, pad, half, ph - pad * 2);

            g.DrawString("🔗  TÀI LIỆU THAM KHẢO", titleFont, titleBrush,
                rx + 12, pad + 12);
            g.DrawLine(new Pen(ColorBorder, 1f),
                rx + 12, pad + 30, rx + half - 12, pad + 30);

            // Vẽ từng link
            int ly = pad + 42;
            if (!string.IsNullOrEmpty(GhiChu.LienKetTaiLieu))
            {
                var links = GhiChu.LienKetTaiLieu.Split('\n');
                foreach (var link in links)
                {
                    string lnk = link.Trim();
                    if (string.IsNullOrEmpty(lnk)) continue;

                    // Truncate link hiển thị
                    string display = lnk.Length > 48 ? lnk.Substring(0, 46) + "…" : lnk;

                    // Vẽ underline giả (link style)
                    using var linkBrush = new SolidBrush(Color.FromArgb(37, 99, 235));
                    using var linkFont = new Font("Segoe UI", 8.5f, FontStyle.Underline);
                    g.DrawString("🔗 " + display, linkFont, linkBrush, rx + 12, ly);

                    // Gắn click vùng link (lưu vào tag)
                    ly += 22;
                    if (ly > ph - pad * 2 - 20) break;
                }
            }
            else
            {
                g.DrawString("(Không có link tham khảo)", bodyFont, titleBrush,
                    rx + 12, ly);
            }

            // Meta: ngày tạo / cập nhật
            string meta = $"Tạo: {GhiChu.NgayTao:dd/MM/yyyy}   |   " +
                          $"Cập nhật: {GhiChu.NgayCapNhat:dd/MM/yyyy}";
            using var metaBrush = new SolidBrush(ColorMuted);
            using var metaFont = new Font("Segoe UI", 7.5f);
            g.DrawString(meta, metaFont, metaBrush,
                rx + 12, ph - pad * 2 - 18);
        }

        // ══════════════════════════════════════════════════════════
        // CLICK MỞ LINK (click vào expand panel)
        // ══════════════════════════════════════════════════════════
        private void CreateLinkClickHandler()
        {
            _pnlExpand.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(GhiChu?.LienKetTaiLieu)) return;

                var pt = _pnlExpand.PointToClient(Cursor.Position);
                int pw = _pnlExpand.Width;
                int half = pw / 2 - 24;
                int pad = 16;
                int rx = pad + half + pad;

                // Vùng link: x > rx, y bắt đầu từ pad+42
                if (pt.X < rx) return;

                var links = GhiChu.LienKetTaiLieu.Split('\n');
                int ly = pad + 42;
                foreach (var link in links)
                {
                    string lnk = link.Trim();
                    if (string.IsNullOrEmpty(lnk)) continue;

                    if (pt.Y >= ly && pt.Y <= ly + 22)
                    {
                        try { Process.Start(new ProcessStartInfo(lnk) { UseShellExecute = true }); }
                        catch { }
                        return;
                    }
                    ly += 22;
                }
            };
        }

        // ══════════════════════════════════════════════════════════
        // BUTTONS SỬA / XÓA
        // ══════════════════════════════════════════════════════════
            private void CreateButtons()
            {
                _btnSua = new Button
                {
                    Text = "✏️",
                    Size = new Size(32, 28),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(240, 244, 255),
                    ForeColor = ColorPrimary,
                    Cursor = Cursors.Hand,
                    Visible = false
                };
                _btnSua.FlatAppearance.BorderSize = 0;
                _btnSua.Click += (s, e) => { e = EventArgs.Empty; SuaClick?.Invoke(this, e); };

                _btnXoa = new Button
                {
                    Text = "🗑️",
                    Size = new Size(32, 28),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(254, 242, 242),
                    ForeColor = ColorDanger,
                    Cursor = Cursors.Hand,
                    Visible = false
                };
                _btnXoa.FlatAppearance.BorderSize = 0;
                _btnXoa.Click += (s, e) => { e = EventArgs.Empty; XoaClick?.Invoke(this, e); };

                this.Controls.Add(_btnSua);
                this.Controls.Add(_btnXoa);

                // Hiện button khi hover row
                this.MouseEnter += (s, e) => { _btnSua.Visible = true; _btnXoa.Visible = true; };
                this.MouseLeave += (s, e) =>
                {
                    // Ẩn khi chuột ra khỏi TOÀN BỘ control (kể cả button)
                    var pos = this.PointToClient(Cursor.Position);
                    if (!this.ClientRectangle.Contains(pos))
                    {
                        _btnSua.Visible = false;
                        _btnXoa.Visible = false;
                    }
                };

                _btnSua.MouseLeave += (s, e) =>
                {
                    var pos = this.PointToClient(Cursor.Position);
                    if (!this.ClientRectangle.Contains(pos))
                    {
                        _btnSua.Visible = false;
                        _btnXoa.Visible = false;
                    }
                };
            _btnXoa.MouseLeave += OnButtonMouseLeave;
            LayoutButtons();
            }
        private void OnButtonMouseLeave(object sender, EventArgs e)
        {
            var pos = this.PointToClient(Cursor.Position);
            if (!this.ClientRectangle.Contains(pos))
            {
                _btnSua.Visible = false;
                _btnXoa.Visible = false;
            }
        }

        // Trong CreateButtons():
        

        private void LayoutButtons()
        {
            if (_btnSua == null || _btnXoa == null) return;
            int cy = (ROW_H - 28) / 2;
            _btnXoa.Location = new Point(Width - 44, cy);
            _btnSua.Location = new Point(Width - 82, cy);
        }

        // ══════════════════════════════════════════════════════════
        // TOGGLE EXPAND
        // ══════════════════════════════════════════════════════════
        private void OnRowClick(object sender, EventArgs e)
        {
            // Không toggle khi click vào button
            var pos = this.PointToClient(Cursor.Position);
            if (_btnSua.Bounds.Contains(pos) || _btnXoa.Bounds.Contains(pos)) return;

            _isExpanded = !_isExpanded;
            this.Height = _isExpanded ? ROW_H + EXPAND_H : ROW_H;
            _pnlExpand.Width = this.Width;
            _pnlExpand.Visible = _isExpanded;

            if (_isExpanded)
            {
                _pnlExpand.Invalidate();
                CreateLinkClickHandler();
            }

            Invalidate();

            // Thông báo cho parent FlowLayoutPanel resize
            Parent?.PerformLayout();
        }

        // ══════════════════════════════════════════════════════════
        // HELPERS VẼ
        // ══════════════════════════════════════════════════════════
        private void DrawCard(Graphics g, int x, int y, int w, int h)
        {
            using var path = RoundedRect(x, y, w, h, 10);
            using var bg = new SolidBrush(Color.White);
            using var pen = new Pen(ColorBorder, 1f);
            g.FillPath(bg, path);
            g.DrawPath(pen, path);
        }

        private void DrawTag(Graphics g, string text, int x, int y,
                              Color bgColor, Color fgColor)
        {
            var font = new Font("Segoe UI", 8f, FontStyle.Bold);
            SizeF sz = g.MeasureString(text, font);
            int tw = (int)sz.Width + 12;
            int th = 22;

            using var path = RoundedRect(x, y, tw, th, 5);
            using var bg = new SolidBrush(bgColor);
            using var fg = new SolidBrush(fgColor);
            g.FillPath(bg, path);
            g.DrawString(text, font, fg, x + 6, y + 4);
            font.Dispose();
        }

        private int MeasureTagWidth(Graphics g, string text)
        {
            using var font = new Font("Segoe UI", 8f, FontStyle.Bold);
            return (int)g.MeasureString(text, font).Width + 16;
        }

        private void DrawEllipsisText(Graphics g, string text, Font font,
                                       Color color, Rectangle rect)
        {
            using var brush = new SolidBrush(color);
            var format = new StringFormat
            {
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(text, font, brush, rect, format);
        }

        private GraphicsPath RoundedRect(int x, int y, int w, int h, int r)
        {
            var path = new GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 180, 90);
            path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
            path.AddArc(x + w - r * 2, y + h - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(x, y + h - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}