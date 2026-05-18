using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using do_an_1.DAO;
using do_an_1.DTO;

namespace do_an_1.Frm
{
    /// <summary>
    /// Modal thêm / sửa ghi chú.
    /// Dùng: var modal = new frm_GhiChuModal(maNguoiDung, dsMonHoc);
    ///       if (modal.ShowDialog() == DialogResult.OK) { ... modal.KetQua ... }
    /// </summary>
    public partial class frm_GhiChuModal : Form
    {
        // ── Đầu vào ─────────────────────────────────────────────
        private readonly int              _maNguoiDung;
        private readonly List<MonHocDTO>  _dsMonHoc;
        private readonly GhiChuDTO        _ghiChuSua;   // null = chế độ thêm mới

        // ── Kết quả trả về ──────────────────────────────────────
        /// <summary>DTO đã điền sau khi người dùng nhấn Lưu.</summary>
        public GhiChuDTO KetQua { get; private set; }

        // ── Màu sắc ─────────────────────────────────────────────
        private readonly Color ColorPrimary = Color.FromArgb(74, 107, 191);
        private readonly Color ColorBorder  = Color.FromArgb(226, 232, 240);
        private readonly Color ColorError   = Color.FromArgb(239, 68, 68);

        // ════════════════════════════════════════════════════════
        // CONSTRUCTOR
        // ════════════════════════════════════════════════════════

        /// <summary>Chế độ THÊM MỚI.</summary>
        public frm_GhiChuModal(int maNguoiDung, List<MonHocDTO> dsMonHoc)
            : this(maNguoiDung, dsMonHoc, null) { }

        /// <summary>Chế độ SỬA — truyền GhiChuDTO hiện có.</summary>
        public frm_GhiChuModal(int maNguoiDung, List<MonHocDTO> dsMonHoc, GhiChuDTO ghiChuSua)
        {
            InitializeComponent();

            _maNguoiDung = maNguoiDung;
            _dsMonHoc    = dsMonHoc ?? new List<MonHocDTO>();
            _ghiChuSua   = ghiChuSua;

            SetupEvents();
            SetupStyle();
            LoadMonHoc();

            if (_ghiChuSua != null)
                FillForm(_ghiChuSua);   // chế độ sửa
        }

        // ════════════════════════════════════════════════════════
        // SETUP
        // ════════════════════════════════════════════════════════
        private void SetupEvents()
        {
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            // Focus visual: viền xanh khi active
            foreach (Control c in pnlBody.Controls)
            {
                if (c is TextBox tb)
                {
                    tb.Enter += (s, e) => ((TextBox)s).BackColor = Color.FromArgb(239, 246, 255);
                    tb.Leave += (s, e) => ((TextBox)s).BackColor = Color.White;
                }
                if (c is RichTextBox rtb)
                {
                    rtb.Enter += (s, e) => ((RichTextBox)s).BackColor = Color.FromArgb(239, 246, 255);
                    rtb.Leave += (s, e) => ((RichTextBox)s).BackColor = Color.White;
                }
            }

            // Vẽ border dưới header
            pnlHeader.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(ColorBorder, 1f),
                    0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            // Vẽ border trên footer
            pnlFooter.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(ColorBorder, 1f),
                    0, 0, pnlFooter.Width, 0);
            };

            // Hover button Lưu
            btnLuu.MouseEnter += (s, e) => btnLuu.BackColor = Color.FromArgb(59, 93, 181);
            btnLuu.MouseLeave += (s, e) => btnLuu.BackColor = ColorPrimary;

            // Hover button Hủy
            btnHuy.MouseEnter += (s, e) => btnHuy.BackColor = Color.FromArgb(241, 245, 249);
            btnHuy.MouseLeave += (s, e) => btnHuy.BackColor = Color.White;

            // Enter = Tab (không submit form)
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape) { DialogResult = DialogResult.Cancel; Close(); }
            };
        }

        private void SetupStyle()
        {
            // Tiêu đề modal
            if (_ghiChuSua == null)
            {
                lblTitle.Text = "➕  Thêm ghi chú mới";
                lblSub.Text   = "Điền thông tin bên dưới rồi nhấn Lưu";
            }
            else
            {
                lblTitle.Text = "✏️  Sửa ghi chú";
                lblSub.Text   = $"Đang chỉnh sửa: {_ghiChuSua.TieuDe}";
            }
        }

        // ════════════════════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ════════════════════════════════════════════════════════
        private void LoadMonHoc()
        {
            cboMonHoc.Items.Clear();

            // Option đầu tiên: không gắn môn
            cboMonHoc.Items.Add(new ComboItem("", "-- Không gắn môn học --"));

            foreach (var mh in _dsMonHoc)
                cboMonHoc.Items.Add(new ComboItem(mh.MaMonHoc, $"{mh.MaMonHoc}  —  {mh.TenMonHoc}"));

            cboMonHoc.DisplayMember = "Display";
            cboMonHoc.ValueMember   = "Value";
            cboMonHoc.SelectedIndex = 0;
        }

        private void FillForm(GhiChuDTO dto)
        {
            txtTieuDe.Text  = dto.TieuDe      ?? "";
            txtTuKhoa.Text  = dto.TuKhoa       ?? "";
            rtbNoiDung.Text = dto.NoiDung      ?? "";
            rtbLienKet.Text = dto.LienKetTaiLieu ?? "";

            // Chọn đúng môn trong combobox
            if (!string.IsNullOrEmpty(dto.MaMonHoc))
            {
                foreach (ComboItem item in cboMonHoc.Items)
                {
                    if (item.Value == dto.MaMonHoc)
                    {
                        cboMonHoc.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        // ════════════════════════════════════════════════════════
        // LƯU
        // ════════════════════════════════════════════════════════
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtTieuDe.Text))
            {
                ShowError(txtTieuDe, "Vui lòng nhập tiêu đề!");
                return;
            }

            string maMonHoc = (cboMonHoc.SelectedItem is ComboItem ci && ci.Value != "")
                ? ci.Value
                : null;

            // Xây dựng DTO kết quả
            KetQua = new GhiChuDTO
            {
                MaGhiChu       = _ghiChuSua?.MaGhiChu ?? 0,
                MaNguoiDung    = _maNguoiDung,
                MaMonHoc       = maMonHoc,
                TieuDe         = txtTieuDe.Text.Trim(),
                NoiDung        = string.IsNullOrWhiteSpace(rtbNoiDung.Text)
                                    ? null : rtbNoiDung.Text.Trim(),
                TuKhoa         = string.IsNullOrWhiteSpace(txtTuKhoa.Text)
                                    ? null : txtTuKhoa.Text.Trim(),
                LienKetTaiLieu = string.IsNullOrWhiteSpace(rtbLienKet.Text)
                                    ? null : rtbLienKet.Text.Trim(),
                NgayTao        = _ghiChuSua?.NgayTao ?? DateTime.Now,
                NgayCapNhat    = DateTime.Now
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        // ════════════════════════════════════════════════════════
        // HELPERS
        // ════════════════════════════════════════════════════════
        private void ShowError(Control control, string msg)
        {
            control.BackColor = Color.FromArgb(255, 240, 240);
            control.Focus();
            MessageBox.Show(msg, "Thiếu thông tin",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.BackColor = Color.White;
        }

        // ── Inner class: item cho ComboBox ────────────────────
        private class ComboItem
        {
            public string Value   { get; }
            public string Display { get; }

            public ComboItem(string value, string display)
            {
                Value   = value;
                Display = display;
            }

            public override string ToString() => Display;
        }
    }
}