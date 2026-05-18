using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using do_an_1.DAO;
using do_an_1.DTO;

namespace do_an_1.Frm
{
    public partial class frm_GhiChu : Form
    {
        // ── Dependencies ────────────────────────────────────────
        private readonly int _maNguoiDung;
        private readonly GhiChuDAO _ghiChuDAO;
        private readonly BaoCaoThongKeDAO _monHocDAO;   // dùng GetMonHocByUser

        // ── State ───────────────────────────────────────────────
        private List<GhiChuDTO> _dsDayDu = new();   // toàn bộ ghi chú từ DB
        private List<MonHocDTO> _dsMonHoc = new();   // danh sách môn học
        private string _searchStr = "";
        private string _filterMon = null;    // null = tất cả

        // ── Màu sắc ─────────────────────────────────────────────
        private readonly Color ColorPrimary = Color.FromArgb(74, 107, 191);
        private readonly Color ColorBorder = Color.FromArgb(226, 232, 240);

        // ════════════════════════════════════════════════════════
        // CONSTRUCTOR
        // ════════════════════════════════════════════════════════
        public frm_GhiChu(int maNguoiDung)
        {
            InitializeComponent();

            _maNguoiDung = maNguoiDung;
            _ghiChuDAO = new GhiChuDAO();
            _monHocDAO = new BaoCaoThongKeDAO();

            SetupEvents();
            SetupPanelBorders();
        }

        // ════════════════════════════════════════════════════════
        // LOAD
        // ════════════════════════════════════════════════════════
        private void frm_GhiChu_Load(object sender, EventArgs e)
        {
            LoadMonHoc();
            LoadGhiChu();
        }

        // ── Môn học (cho ComboBox filter) ───────────────────────
        private void LoadMonHoc()
        {
            _dsMonHoc = _monHocDAO.GetMonHocByUser(_maNguoiDung);

            cboMonFilter.Items.Clear();
            cboMonFilter.Items.Add("📚  Tất cả môn học");

            foreach (var mh in _dsMonHoc)
                cboMonFilter.Items.Add(mh);

            cboMonFilter.DisplayMember = "TenMonHoc";
            cboMonFilter.SelectedIndex = 0;
        }

        // ── Ghi chú từ DB → render ──────────────────────────────
        private void LoadGhiChu()
        {
            try
            {
                _dsDayDu = _ghiChuDAO.GetDanhSach(_maNguoiDung);
                RenderRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải ghi chú: " + ex.Message,
                    "Study Tracker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ════════════════════════════════════════════════════════
        // RENDER
        // ════════════════════════════════════════════════════════
        private void RenderRows()
        {
            flpGhiChu.SuspendLayout();
            flpGhiChu.Controls.Clear();

            // ── Lọc ─────────────────────────────────────────────
            var filtered = _dsDayDu.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(_searchStr))
            {
                string s = _searchStr.ToLower();
                filtered = filtered.Where(g =>
                    (g.TieuDe ?? "").ToLower().Contains(s) ||
                    (g.NoiDung ?? "").ToLower().Contains(s) ||
                    (g.TuKhoa ?? "").ToLower().Contains(s));
            }

            if (!string.IsNullOrEmpty(_filterMon))
                filtered = filtered.Where(g => g.MaMonHoc == _filterMon);

            var list = filtered.ToList();

            // ── Cập nhật số lượng ────────────────────────────────
            lblCount.Text = $"{list.Count} ghi chú";

            // ── Empty state ──────────────────────────────────────
            bool isEmpty = list.Count == 0;
            flpGhiChu.Visible = !isEmpty;
            pnlEmpty.Visible = isEmpty;

            if (isEmpty)
            {
                lblEmptyText.Text = _dsDayDu.Count == 0
                    ? "Chưa có ghi chú nào\nNhấn ➕ Thêm ghi chú để bắt đầu!"
                    : "Không tìm thấy ghi chú phù hợp";
                flpGhiChu.ResumeLayout();
                return;
            }

            // ── Tạo row cho mỗi ghi chú ─────────────────────────
            foreach (var dto in list)
            {
                string tenMon = _dsMonHoc
                    .FirstOrDefault(m => m.MaMonHoc == dto.MaMonHoc)
                    ?.TenMonHoc;

                var row = new GhiChuRowControl
                {
                    GhiChu = dto,
                    TenMonHoc = tenMon,
                    Width = flpGhiChu.ClientSize.Width - 2,
                    Margin = new Padding(0, 0, 0, 1)
                };

                // Sự kiện Sửa
                row.SuaClick += (s, e) => MoModalSua(dto);

                // Sự kiện Xóa
                row.XoaClick += (s, e) => XoaGhiChu(dto);

                flpGhiChu.Controls.Add(row);
            }

            // Resize row theo chiều rộng flp khi form resize
            flpGhiChu.Resize += (s, e) =>
            {
                foreach (GhiChuRowControl r in flpGhiChu.Controls.OfType<GhiChuRowControl>())
                    r.Width = flpGhiChu.ClientSize.Width - 2;
            };

            flpGhiChu.ResumeLayout();
        }

        // ════════════════════════════════════════════════════════
        // THÊM MỚI
        // ════════════════════════════════════════════════════════
        private void BtnThem_Click(object sender, EventArgs e)
        {
            using var modal = new frm_GhiChuModal(_maNguoiDung, _dsMonHoc);

            if (modal.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                int newId = _ghiChuDAO.Insert(modal.KetQua);

                if (newId > 0)
                {
                    LoadGhiChu();
                    ShowToast("✅  Đã thêm ghi chú mới!");
                }
                else
                {
                    MessageBox.Show("Không thể thêm ghi chú. Vui lòng thử lại.",
                        "Study Tracker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Study Tracker",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════
        // SỬA
        // ════════════════════════════════════════════════════════
        private void MoModalSua(GhiChuDTO dto)
        {
            using var modal = new frm_GhiChuModal(_maNguoiDung, _dsMonHoc, dto);

            if (modal.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                bool ok = _ghiChuDAO.Update(modal.KetQua);

                if (ok)
                {
                    LoadGhiChu();
                    ShowToast("✅  Đã cập nhật ghi chú!");
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật. Vui lòng thử lại.",
                        "Study Tracker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Study Tracker",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════
        // XÓA
        // ════════════════════════════════════════════════════════
        private void XoaGhiChu(GhiChuDTO dto)
        {
            var confirm = MessageBox.Show(
                $"Xóa ghi chú \"{dto.TieuDe}\"?\nHành động này không thể hoàn tác.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool ok = _ghiChuDAO.Delete(dto.MaGhiChu, _maNguoiDung);

                if (ok)
                {
                    LoadGhiChu();
                    ShowToast("🗑️  Đã xóa ghi chú!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message, "Study Tracker",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════
        // FILTER & SEARCH (real-time)
        // ════════════════════════════════════════════════════════
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            _searchStr = txtSearch.Text.Trim();
            RenderRows();
        }

        private void CboMonFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMonFilter.SelectedItem is MonHocDTO mh)
                _filterMon = mh.MaMonHoc;
            else
                _filterMon = null;   // "Tất cả môn học"

            RenderRows();
        }

        // ════════════════════════════════════════════════════════
        // SETUP EVENTS & STYLE
        // ════════════════════════════════════════════════════════
        private void SetupEvents()
        {
            Load += frm_GhiChu_Load;
            btnThem.Click += BtnThem_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            cboMonFilter.SelectedIndexChanged += CboMonFilter_SelectedIndexChanged;

            // Hover button Thêm
            btnThem.MouseEnter += (s, e) =>
                btnThem.BackColor = Color.FromArgb(59, 93, 181);
            btnThem.MouseLeave += (s, e) =>
                btnThem.BackColor = ColorPrimary;

            // Focus style txtSearch
            txtSearch.Enter += (s, e) =>
            {
                txtSearch.BackColor = Color.White;
                txtSearch.ForeColor = Color.FromArgb(30, 42, 74);
            };
            txtSearch.Leave += (s, e) =>
                txtSearch.BackColor = Color.FromArgb(248, 250, 252);
        }

        private void SetupPanelBorders()
        {
            // Đường kẻ dưới header
            pnlHeader.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(ColorBorder, 1f),
                    0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);

            // Đường kẻ dưới filter
            pnlFilter.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(ColorBorder, 1f),
                    0, pnlFilter.Height - 1, pnlFilter.Width, pnlFilter.Height - 1);

            // Đường kẻ dưới table header
            pnlTableHeader.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(210, 218, 235), 1.5f),
                    0, pnlTableHeader.Height - 1, pnlTableHeader.Width, pnlTableHeader.Height - 1);
        }

        // ════════════════════════════════════════════════════════
        // TOAST NOTIFICATION (nhỏ gọn, tự ẩn)
        // ════════════════════════════════════════════════════════
        private void ShowToast(string msg)
        {
            var toast = new Label
            {
                Text = msg,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(34, 197, 94),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(280, 42),
                Padding = new Padding(10, 0, 10, 0)
            };

            // Bo góc bằng Region
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, 16, 16, 180, 90);
            path.AddArc(toast.Width - 16, 0, 16, 16, 270, 90);
            path.AddArc(toast.Width - 16, toast.Height - 16, 16, 16, 0, 90);
            path.AddArc(0, toast.Height - 16, 16, 16, 90, 90);
            path.CloseFigure();
            toast.Region = new Region(path);

            // Vị trí: góc dưới phải form
            toast.Location = new Point(
                ClientSize.Width - toast.Width - 20,
                ClientSize.Height - toast.Height - 20);

            Controls.Add(toast);
            toast.BringToFront();

            // Tự ẩn sau 2.5 giây
            var timer = new System.Windows.Forms.Timer { Interval = 2500 };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                Controls.Remove(toast);
                toast.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }

        private void lblIcon_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}