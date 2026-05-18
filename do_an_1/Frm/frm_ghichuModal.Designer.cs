using LiveChartsCore.Measure;
using static Guna.UI2.WinForms.Suite.Descriptions;
using static System.Net.Mime.MediaTypeNames;

namespace do_an_1.Frm
{
    partial class frm_GhiChuModal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblSub = new Label();
            pnlBody = new Panel();
            lblTieuDe = new Label();
            txtTieuDe = new TextBox();
            lblMonHoc = new Label();
            cboMonHoc = new ComboBox();
            lblTuKhoa = new Label();
            txtTuKhoa = new TextBox();
            lblHintTuKhoa = new Label();
            lblNoiDung = new Label();
            rtbNoiDung = new RichTextBox();
            lblLienKet = new Label();
            rtbLienKet = new RichTextBox();
            lblHintLienKet = new Label();
            pnlFooter = new Panel();
            btnHuy = new Button();
            btnLuu = new Button();
            pnlHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(20, 0, 20, 0);
            pnlHeader.Size = new Size(560, 72);
            pnlHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 42, 74);
            lblTitle.Location = new Point(20, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(267, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "➕  Thêm ghi chú mới";
            // 
            // lblSub
            // 
            lblSub.AutoSize = true;
            lblSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblSub.ForeColor = Color.FromArgb(100, 116, 139);
            lblSub.Location = new Point(22, 46);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(254, 20);
            lblSub.TabIndex = 1;
            lblSub.Text = "Điền thông tin bên dưới rồi nhấn Lưu";
            // 
            // pnlBody
            // 
            pnlBody.AutoScroll = true;
            pnlBody.BackColor = Color.FromArgb(248, 250, 252);
            pnlBody.Controls.Add(lblTieuDe);
            pnlBody.Controls.Add(txtTieuDe);
            pnlBody.Controls.Add(lblMonHoc);
            pnlBody.Controls.Add(cboMonHoc);
            pnlBody.Controls.Add(lblTuKhoa);
            pnlBody.Controls.Add(txtTuKhoa);
            pnlBody.Controls.Add(lblHintTuKhoa);
            pnlBody.Controls.Add(lblNoiDung);
            pnlBody.Controls.Add(rtbNoiDung);
            pnlBody.Controls.Add(lblLienKet);
            pnlBody.Controls.Add(rtbLienKet);
            pnlBody.Controls.Add(lblHintLienKet);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 72);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(20, 16, 20, 0);
            pnlBody.Size = new Size(560, 508);
            pnlBody.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(100, 116, 139);
            lblTieuDe.Location = new Point(20, 16);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(73, 19);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "TIÊU ĐỀ  *";
            // 
            // txtTieuDe
            // 
            txtTieuDe.BackColor = Color.White;
            txtTieuDe.BorderStyle = BorderStyle.FixedSingle;
            txtTieuDe.Font = new System.Drawing.Font("Segoe UI", 11F);
            txtTieuDe.ForeColor = Color.FromArgb(30, 42, 74);
            txtTieuDe.Location = new Point(20, 36);
            txtTieuDe.Name = "txtTieuDe";
            txtTieuDe.PlaceholderText = "VD: Delegate & Event trong C#...";
            txtTieuDe.Size = new Size(500, 32);
            txtTieuDe.TabIndex = 1;
            // 
            // lblMonHoc
            // 
            lblMonHoc.AutoSize = true;
            lblMonHoc.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
            lblMonHoc.ForeColor = Color.FromArgb(100, 116, 139);
            lblMonHoc.Location = new Point(20, 90);
            lblMonHoc.Name = "lblMonHoc";
            lblMonHoc.Size = new Size(142, 19);
            lblMonHoc.TabIndex = 2;
            lblMonHoc.Text = "GẮN VỚI MÔN HỌC";
            // 
            // cboMonHoc
            // 
            cboMonHoc.BackColor = Color.White;
            cboMonHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMonHoc.FlatStyle = FlatStyle.Flat;
            cboMonHoc.Font = new System.Drawing.Font("Segoe UI", 10F);
            cboMonHoc.Location = new Point(20, 110);
            cboMonHoc.Name = "cboMonHoc";
            cboMonHoc.Size = new Size(500, 31);
            cboMonHoc.TabIndex = 3;
            // 
            // lblTuKhoa
            // 
            lblTuKhoa.AutoSize = true;
            lblTuKhoa.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
            lblTuKhoa.ForeColor = Color.FromArgb(100, 116, 139);
            lblTuKhoa.Location = new Point(20, 162);
            lblTuKhoa.Name = "lblTuKhoa";
            lblTuKhoa.Size = new Size(73, 19);
            lblTuKhoa.TabIndex = 4;
            lblTuKhoa.Text = "TỪ KHÓA";
            // 
            // txtTuKhoa
            // 
            txtTuKhoa.BackColor = Color.White;
            txtTuKhoa.BorderStyle = BorderStyle.FixedSingle;
            txtTuKhoa.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtTuKhoa.ForeColor = Color.FromArgb(30, 42, 74);
            txtTuKhoa.Location = new Point(20, 182);
            txtTuKhoa.Name = "txtTuKhoa";
            txtTuKhoa.PlaceholderText = "VD: C#, delegate, event";
            txtTuKhoa.Size = new Size(500, 30);
            txtTuKhoa.TabIndex = 5;
            // 
            // lblHintTuKhoa
            // 
            lblHintTuKhoa.AutoSize = true;
            lblHintTuKhoa.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Italic);
            lblHintTuKhoa.ForeColor = Color.FromArgb(148, 163, 184);
            lblHintTuKhoa.Location = new Point(22, 218);
            lblHintTuKhoa.Name = "lblHintTuKhoa";
            lblHintTuKhoa.Size = new Size(172, 19);
            lblHintTuKhoa.TabIndex = 6;
            lblHintTuKhoa.Text = "Phân cách bằng dấu phẩy";
            // 
            // lblNoiDung
            // 
            lblNoiDung.AutoSize = true;
            lblNoiDung.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
            lblNoiDung.ForeColor = Color.FromArgb(100, 116, 139);
            lblNoiDung.Location = new Point(20, 244);
            lblNoiDung.Name = "lblNoiDung";
            lblNoiDung.Size = new Size(80, 19);
            lblNoiDung.TabIndex = 7;
            lblNoiDung.Text = "NỘI DUNG";
            // 
            // rtbNoiDung
            // 
            rtbNoiDung.BackColor = Color.White;
            rtbNoiDung.BorderStyle = BorderStyle.FixedSingle;
            rtbNoiDung.Font = new System.Drawing.Font("Segoe UI", 10F);
            rtbNoiDung.ForeColor = Color.FromArgb(30, 42, 74);
            rtbNoiDung.Location = new Point(20, 264);
            rtbNoiDung.Name = "rtbNoiDung";
            rtbNoiDung.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbNoiDung.Size = new Size(500, 118);
            rtbNoiDung.TabIndex = 8;
            rtbNoiDung.Text = "";
            // 
            // lblLienKet
            // 
            lblLienKet.AutoSize = true;
            lblLienKet.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
            lblLienKet.ForeColor = Color.FromArgb(100, 116, 139);
            lblLienKet.Location = new Point(20, 385);
            lblLienKet.Name = "lblLienKet";
            lblLienKet.Size = new Size(188, 19);
            lblLienKet.TabIndex = 9;
            lblLienKet.Text = "LINK TÀI LIỆU THAM KHẢO";
            // 
            // rtbLienKet
            // 
            rtbLienKet.BackColor = Color.White;
            rtbLienKet.BorderStyle = BorderStyle.FixedSingle;
            rtbLienKet.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            rtbLienKet.ForeColor = Color.FromArgb(30, 42, 74);
            rtbLienKet.Location = new Point(20, 407);
            rtbLienKet.Name = "rtbLienKet";
            rtbLienKet.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbLienKet.Size = new Size(500, 85);
            rtbLienKet.TabIndex = 10;
            rtbLienKet.Text = "";
            // 
            // lblHintLienKet
            // 
            lblHintLienKet.AutoSize = true;
            lblHintLienKet.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Italic);
            lblHintLienKet.ForeColor = Color.FromArgb(148, 163, 184);
            lblHintLienKet.Location = new Point(20, 495);
            lblHintLienKet.Name = "lblHintLienKet";
            lblHintLienKet.Size = new Size(343, 19);
            lblHintLienKet.TabIndex = 11;
            lblHintLienKet.Text = "Mỗi link 1 dòng  —  VD: https://docs.microsoft.com/...";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.White;
            pnlFooter.Controls.Add(btnHuy);
            pnlFooter.Controls.Add(btnLuu);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 580);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(20, 0, 20, 0);
            pnlFooter.Size = new Size(560, 60);
            pnlFooter.TabIndex = 1;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.White;
            btnHuy.Cursor = Cursors.Hand;
            btnHuy.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            btnHuy.ForeColor = Color.FromArgb(100, 116, 139);
            btnHuy.Location = new Point(340, 12);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(90, 36);
            btnHuy.TabIndex = 0;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(74, 107, 191);
            btnLuu.Cursor = Cursors.Hand;
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(440, 12);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(150, 36);
            btnLuu.TabIndex = 1;
            btnLuu.Text = "💾  Lưu ghi chú";
            btnLuu.UseVisualStyleBackColor = false;
            // 
            // frm_GhiChuModal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(560, 640);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frm_GhiChuModal";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ghi chú — Study Tracker";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlBody.ResumeLayout(false);
            pnlBody.PerformLayout();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        // Controls
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.TextBox txtTieuDe;
        private System.Windows.Forms.Label lblMonHoc;
        private System.Windows.Forms.ComboBox cboMonHoc;
        private System.Windows.Forms.Label lblTuKhoa;
        private System.Windows.Forms.TextBox txtTuKhoa;
        private System.Windows.Forms.Label lblHintTuKhoa;
        private System.Windows.Forms.Label lblNoiDung;
        private System.Windows.Forms.RichTextBox rtbNoiDung;
        private System.Windows.Forms.Label lblLienKet;
        private System.Windows.Forms.RichTextBox rtbLienKet;
        private System.Windows.Forms.Label lblHintLienKet;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
    }
}