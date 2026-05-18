namespace do_an_1.Frm
{
    partial class frm_GhiChu
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
            lblIcon = new Label();
            lblTitle = new Label();
            lblSub = new Label();
            btnThem = new Button();
            pnlFilter = new Panel();
            txtSearch = new TextBox();
            cboMonFilter = new ComboBox();
            lblCount = new Label();
            pnlTableHeader = new Panel();
            lblColTitle = new Label();
            lblColMon = new Label();
            lblColTag = new Label();
            lblColDate = new Label();
            lblColAction = new Label();
            flpGhiChu = new FlowLayoutPanel();
            pnlEmpty = new Panel();
            lblEmptyIcon = new Label();
            lblEmptyText = new Label();
            pnlHeader.SuspendLayout();
            pnlFilter.SuspendLayout();
            pnlTableHeader.SuspendLayout();
            pnlEmpty.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(lblIcon);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(btnThem);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(20, 0, 20, 0);
            pnlHeader.Size = new Size(1000, 70);
            pnlHeader.TabIndex = 4;
            pnlHeader.Paint += pnlHeader_Paint;
            // 
            // lblIcon
            // 
            lblIcon.AutoSize = true;
            lblIcon.Font = new Font("Segoe UI", 20F);
            lblIcon.Location = new Point(20, 14);
            lblIcon.Name = "lblIcon";
            lblIcon.Size = new Size(67, 46);
            lblIcon.TabIndex = 0;
            lblIcon.Text = "📝";
            lblIcon.Click += lblIcon_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 42, 74);
            lblTitle.Location = new Point(93, 7);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(259, 35);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Kho tri thức & Ghi chú";
            // 
            // lblSub
            // 
            lblSub.AutoSize = true;
            lblSub.Font = new Font("Segoe UI", 9F);
            lblSub.ForeColor = Color.FromArgb(100, 116, 139);
            lblSub.Location = new Point(93, 47);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(364, 20);
            lblSub.TabIndex = 2;
            lblSub.Text = "Lưu trữ kiến thức, công thức và tài liệu theo từng môn";
            // 
            // btnThem
            // 
            btnThem.Anchor = AnchorStyles.Right;
            btnThem.BackColor = Color.FromArgb(74, 107, 191);
            btnThem.Cursor = Cursors.Hand;
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(810, 14);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(152, 38);
            btnThem.TabIndex = 3;
            btnThem.Text = "➕  Thêm ghi chú";
            btnThem.UseVisualStyleBackColor = false;
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.White;
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(cboMonFilter);
            pnlFilter.Controls.Add(lblCount);
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Location = new Point(0, 70);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Padding = new Padding(20, 0, 20, 0);
            pnlFilter.Size = new Size(1000, 58);
            pnlFilter.TabIndex = 3;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(248, 250, 252);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.ForeColor = Color.FromArgb(30, 42, 74);
            txtSearch.Location = new Point(20, 13);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Tìm tiêu đề, nội dung, từ khóa...";
            txtSearch.Size = new Size(260, 30);
            txtSearch.TabIndex = 0;
            // 
            // cboMonFilter
            // 
            cboMonFilter.BackColor = Color.FromArgb(248, 250, 252);
            cboMonFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMonFilter.FlatStyle = FlatStyle.Flat;
            cboMonFilter.Font = new Font("Segoe UI", 10F);
            cboMonFilter.Location = new Point(294, 13);
            cboMonFilter.Name = "cboMonFilter";
            cboMonFilter.Size = new Size(220, 31);
            cboMonFilter.TabIndex = 1;
            // 
            // lblCount
            // 
            lblCount.AutoSize = true;
            lblCount.Font = new Font("Segoe UI", 9F);
            lblCount.ForeColor = Color.FromArgb(100, 116, 139);
            lblCount.Location = new Point(530, 20);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(69, 20);
            lblCount.TabIndex = 2;
            lblCount.Text = "0 ghi chú";
            // 
            // pnlTableHeader
            // 
            pnlTableHeader.BackColor = Color.FromArgb(248, 250, 252);
            pnlTableHeader.Controls.Add(lblColTitle);
            pnlTableHeader.Controls.Add(lblColMon);
            pnlTableHeader.Controls.Add(lblColTag);
            pnlTableHeader.Controls.Add(lblColDate);
            pnlTableHeader.Controls.Add(lblColAction);
            pnlTableHeader.Dock = DockStyle.Top;
            pnlTableHeader.Location = new Point(0, 128);
            pnlTableHeader.Name = "pnlTableHeader";
            pnlTableHeader.Padding = new Padding(16, 0, 16, 0);
            pnlTableHeader.Size = new Size(1000, 38);
            pnlTableHeader.TabIndex = 2;
            // 
            // lblColTitle
            // 
            lblColTitle.AutoSize = true;
            lblColTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblColTitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblColTitle.Location = new Point(16, 11);
            lblColTitle.Name = "lblColTitle";
            lblColTitle.Size = new Size(59, 19);
            lblColTitle.TabIndex = 0;
            lblColTitle.Text = "TIÊU ĐỀ";
            // 
            // lblColMon
            // 
            lblColMon.AutoSize = true;
            lblColMon.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblColMon.ForeColor = Color.FromArgb(100, 116, 139);
            lblColMon.Location = new Point(346, 11);
            lblColMon.Name = "lblColMon";
            lblColMon.Size = new Size(79, 19);
            lblColMon.TabIndex = 1;
            lblColMon.Text = "MÔN HỌC";
            // 
            // lblColTag
            // 
            lblColTag.AutoSize = true;
            lblColTag.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblColTag.ForeColor = Color.FromArgb(100, 116, 139);
            lblColTag.Location = new Point(510, 11);
            lblColTag.Name = "lblColTag";
            lblColTag.Size = new Size(73, 19);
            lblColTag.TabIndex = 2;
            lblColTag.Text = "TỪ KHÓA";
            // 
            // lblColDate
            // 
            lblColDate.AutoSize = true;
            lblColDate.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblColDate.ForeColor = Color.FromArgb(100, 116, 139);
            lblColDate.Location = new Point(710, 11);
            lblColDate.Name = "lblColDate";
            lblColDate.Size = new Size(81, 19);
            lblColDate.TabIndex = 3;
            lblColDate.Text = "CẬP NHẬT";
            // 
            // lblColAction
            // 
            lblColAction.AutoSize = true;
            lblColAction.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblColAction.ForeColor = Color.FromArgb(100, 116, 139);
            lblColAction.Location = new Point(836, 11);
            lblColAction.Name = "lblColAction";
            lblColAction.Size = new Size(79, 19);
            lblColAction.TabIndex = 4;
            lblColAction.Text = "THAO TÁC";
            // 
            // flpGhiChu
            // 
            flpGhiChu.AutoScroll = true;
            flpGhiChu.BackColor = Color.FromArgb(240, 244, 252);
            flpGhiChu.Dock = DockStyle.Fill;
            flpGhiChu.FlowDirection = FlowDirection.TopDown;
            flpGhiChu.Location = new Point(0, 166);
            flpGhiChu.Name = "flpGhiChu";
            flpGhiChu.Padding = new Padding(0, 4, 0, 12);
            flpGhiChu.Size = new Size(1000, 514);
            flpGhiChu.TabIndex = 0;
            flpGhiChu.WrapContents = false;
            // 
            // pnlEmpty
            // 
            pnlEmpty.BackColor = Color.FromArgb(240, 244, 252);
            pnlEmpty.Controls.Add(lblEmptyIcon);
            pnlEmpty.Controls.Add(lblEmptyText);
            pnlEmpty.Dock = DockStyle.Fill;
            pnlEmpty.Location = new Point(0, 166);
            pnlEmpty.Name = "pnlEmpty";
            pnlEmpty.Size = new Size(1000, 514);
            pnlEmpty.TabIndex = 1;
            pnlEmpty.Visible = false;
            // 
            // lblEmptyIcon
            // 
            lblEmptyIcon.Font = new Font("Segoe UI", 48F);
            lblEmptyIcon.Location = new Point(330, 160);
            lblEmptyIcon.Name = "lblEmptyIcon";
            lblEmptyIcon.Size = new Size(200, 80);
            lblEmptyIcon.TabIndex = 0;
            lblEmptyIcon.Text = "📝";
            lblEmptyIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEmptyText
            // 
            lblEmptyText.Font = new Font("Segoe UI", 12F);
            lblEmptyText.ForeColor = Color.FromArgb(148, 163, 184);
            lblEmptyText.Location = new Point(230, 250);
            lblEmptyText.Name = "lblEmptyText";
            lblEmptyText.Size = new Size(400, 60);
            lblEmptyText.TabIndex = 1;
            lblEmptyText.Text = "Chưa có ghi chú nào\nNhấn ➕ Thêm ghi chú để bắt đầu!";
            lblEmptyText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frm_GhiChu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 244, 252);
            ClientSize = new Size(1000, 680);
            Controls.Add(flpGhiChu);
            Controls.Add(pnlEmpty);
            Controls.Add(pnlTableHeader);
            Controls.Add(pnlFilter);
            Controls.Add(pnlHeader);
            Name = "frm_GhiChu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kho tri thức & Ghi chú — Study Tracker";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            pnlTableHeader.ResumeLayout(false);
            pnlTableHeader.PerformLayout();
            pnlEmpty.ResumeLayout(false);
            ResumeLayout(false);
        }

        // Controls
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cboMonFilter;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Panel pnlTableHeader;
        private System.Windows.Forms.Label lblColTitle;
        private System.Windows.Forms.Label lblColMon;
        private System.Windows.Forms.Label lblColTag;
        private System.Windows.Forms.Label lblColDate;
        private System.Windows.Forms.Label lblColAction;
        private System.Windows.Forms.FlowLayoutPanel flpGhiChu;
        private System.Windows.Forms.Panel pnlEmpty;
        private System.Windows.Forms.Label lblEmptyIcon;
        private System.Windows.Forms.Label lblEmptyText;
    }
}