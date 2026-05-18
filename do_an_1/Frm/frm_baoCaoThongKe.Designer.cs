namespace do_an_1.Frm
{
    partial class frm_baoCaoThongKe
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultLegend skDefaultLegend1 = new LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultLegend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_baoCaoThongKe));
            LiveChartsCore.Drawing.Padding padding1 = new LiveChartsCore.Drawing.Padding();
            LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultTooltip skDefaultTooltip1 = new LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultTooltip();
            LiveChartsCore.Drawing.Padding padding2 = new LiveChartsCore.Drawing.Padding();
            LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultLegend skDefaultLegend2 = new LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultLegend();
            LiveChartsCore.Drawing.Padding padding3 = new LiveChartsCore.Drawing.Padding();
            LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultTooltip skDefaultTooltip2 = new LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultTooltip();
            LiveChartsCore.Drawing.Padding padding4 = new LiveChartsCore.Drawing.Padding();
            LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultLegend skDefaultLegend3 = new LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultLegend();
            LiveChartsCore.Drawing.Padding padding5 = new LiveChartsCore.Drawing.Padding();
            LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultTooltip skDefaultTooltip3 = new LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultTooltip();
            LiveChartsCore.Drawing.Padding padding6 = new LiveChartsCore.Drawing.Padding();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblSubHeader = new Label();
            lblHeaderTitle = new Label();
            pnlFilters = new Panel();
            btnIn = new Button();
            cboSubjects = new ComboBox();
            lblSubject = new Label();
            cboSemesters = new ComboBox();
            lblSemester = new Label();
            tlpCharts = new TableLayoutPanel();
            pnlChart1 = new Panel();
            cartesianChart1 = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            lblNoteChart1 = new Label();
            lblTitleChart1 = new Label();
            pnlChart2 = new Panel();
            cartesianChart2 = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            lblNoteChart2 = new Label();
            lblTitleChart2 = new Label();
            pnlChart3 = new Panel();
            pieChart1 = new LiveChartsCore.SkiaSharpView.WinForms.PieChart();
            lblTitleChart3 = new Label();
            lblTableTitle = new Label();
            dgvReport = new DataGridView();
            colMonHoc = new DataGridViewTextBoxColumn();
            colTinChi = new DataGridViewTextBoxColumn();
            colGioPomo = new DataGridViewTextBoxColumn();
            colGhiChu = new DataGridViewTextBoxColumn();
            colHoanThanh = new DataGridViewTextBoxColumn();
            colDanhGia = new DataGridViewTextBoxColumn();
            pnlTableCard = new Panel();
            pnlHeader.SuspendLayout();
            pnlFilters.SuspendLayout();
            tlpCharts.SuspendLayout();
            pnlChart1.SuspendLayout();
            pnlChart2.SuspendLayout();
            pnlChart3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
            pnlTableCard.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(lblSubHeader);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Location = new Point(20, 20);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1360, 80);
            pnlHeader.TabIndex = 0;
            // 
            // lblSubHeader
            // 
            lblSubHeader.AutoSize = true;
            lblSubHeader.Font = new Font("Segoe UI", 9.5F);
            lblSubHeader.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubHeader.Location = new Point(20, 48);
            lblSubHeader.Name = "lblSubHeader";
            lblSubHeader.Size = new Size(663, 21);
            lblSubHeader.TabIndex = 1;
            lblSubHeader.Text = "📊 Xử lý Aggregate Data từ CSDL 7 bảng - Kiểm định chất lượng học tập theo Quy chế đào tạo";
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.Location = new Point(18, 13);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(522, 37);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "PHÂN HỆ BÁO CÁO THỐNG KÊ CHI TIẾT";
            // 
            // pnlFilters
            // 
            pnlFilters.BackColor = Color.White;
            pnlFilters.Controls.Add(btnIn);
            pnlFilters.Controls.Add(cboSubjects);
            pnlFilters.Controls.Add(lblSubject);
            pnlFilters.Controls.Add(cboSemesters);
            pnlFilters.Controls.Add(lblSemester);
            pnlFilters.Location = new Point(20, 115);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Size = new Size(1339, 60);
            pnlFilters.TabIndex = 1;
            // 
            // btnIn
            // 
            btnIn.BackColor = Color.FromArgb(37, 99, 235);
            btnIn.Cursor = Cursors.Hand;
            btnIn.FlatAppearance.BorderSize = 0;
            btnIn.FlatStyle = FlatStyle.Flat;
            btnIn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnIn.ForeColor = Color.White;
            btnIn.Location = new Point(1205, 15);
            btnIn.Name = "btnIn";
            btnIn.Size = new Size(120, 32);
            btnIn.TabIndex = 4;
            btnIn.Text = "🔍 Áp Dụng";
            btnIn.UseVisualStyleBackColor = false;
            btnIn.Click += btnIn_Click;
            // 
            // cboSubjects
            // 
            cboSubjects.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSubjects.Font = new Font("Segoe UI", 10F);
            cboSubjects.FormattingEnabled = true;
            cboSubjects.Location = new Point(406, 15);
            cboSubjects.Name = "cboSubjects";
            cboSubjects.Size = new Size(230, 31);
            cboSubjects.TabIndex = 3;
            // 
            // lblSubject
            // 
            lblSubject.AutoSize = true;
            lblSubject.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSubject.Location = new Point(317, 19);
            lblSubject.Name = "lblSubject";
            lblSubject.Size = new Size(83, 21);
            lblSubject.TabIndex = 2;
            lblSubject.Text = "Môn Học:";
            // 
            // cboSemesters
            // 
            cboSemesters.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSemesters.Font = new Font("Segoe UI", 10F);
            cboSemesters.FormattingEnabled = true;
            cboSemesters.Location = new Point(116, 17);
            cboSemesters.Name = "cboSemesters";
            cboSemesters.Size = new Size(180, 31);
            cboSemesters.TabIndex = 1;
            // 
            // lblSemester
            // 
            lblSemester.AutoSize = true;
            lblSemester.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSemester.Location = new Point(31, 21);
            lblSemester.Name = "lblSemester";
            lblSemester.Size = new Size(66, 21);
            lblSemester.TabIndex = 0;
            lblSemester.Text = "Học Kỳ:";
            // 
            // tlpCharts
            // 
            tlpCharts.ColumnCount = 2;
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCharts.Controls.Add(pnlChart1, 0, 0);
            tlpCharts.Controls.Add(pnlChart2, 1, 0);
            tlpCharts.Controls.Add(pnlChart3, 0, 1);
            tlpCharts.Location = new Point(20, 190);
            tlpCharts.Name = "tlpCharts";
            tlpCharts.RowCount = 2;
            tlpCharts.RowStyles.Add(new RowStyle(SizeType.Absolute, 420F));
            tlpCharts.RowStyles.Add(new RowStyle(SizeType.Absolute, 360F));
            tlpCharts.Size = new Size(1342, 791);
            tlpCharts.TabIndex = 2;
            // 
            // pnlChart1
            // 
            pnlChart1.BackColor = Color.White;
            pnlChart1.Controls.Add(cartesianChart1);
            pnlChart1.Controls.Add(lblNoteChart1);
            pnlChart1.Controls.Add(lblTitleChart1);
            pnlChart1.Dock = DockStyle.Fill;
            pnlChart1.Location = new Point(3, 3);
            pnlChart1.Name = "pnlChart1";
            pnlChart1.Size = new Size(665, 414);
            pnlChart1.TabIndex = 0;
            // 
            // cartesianChart1
            // 
            cartesianChart1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cartesianChart1.AutoUpdateEnabled = true;
            cartesianChart1.ChartTheme = null;
            skDefaultLegend1.AnimationsSpeed = TimeSpan.Parse("00:00:00.1500000");
            skDefaultLegend1.Content = null;
            skDefaultLegend1.IsValid = false;
            skDefaultLegend1.Opacity = 1F;
            padding1.Bottom = 0F;
            padding1.Left = 0F;
            padding1.Right = 0F;
            padding1.Top = 0F;
            skDefaultLegend1.Padding = padding1;
            skDefaultLegend1.RemoveOnCompleted = false;
            skDefaultLegend1.RotateTransform = 0F;
            skDefaultLegend1.X = 0F;
            skDefaultLegend1.Y = 0F;
            cartesianChart1.Legend = skDefaultLegend1;
            cartesianChart1.Location = new Point(18, 52);
            cartesianChart1.MatchAxesScreenDataRatio = false;
            cartesianChart1.Name = "cartesianChart1";
            cartesianChart1.Size = new Size(551, 257);
            cartesianChart1.TabIndex = 3;
            skDefaultTooltip1.AnimationsSpeed = TimeSpan.Parse("00:00:00.1500000");
            skDefaultTooltip1.Content = null;
            skDefaultTooltip1.IsValid = false;
            skDefaultTooltip1.Opacity = 1F;
            padding2.Bottom = 0F;
            padding2.Left = 0F;
            padding2.Right = 0F;
            padding2.Top = 0F;
            skDefaultTooltip1.Padding = padding2;
            skDefaultTooltip1.RemoveOnCompleted = false;
            skDefaultTooltip1.RotateTransform = 0F;
            skDefaultTooltip1.Wedge = 10;
            skDefaultTooltip1.X = 0F;
            skDefaultTooltip1.Y = 0F;
            cartesianChart1.Tooltip = skDefaultTooltip1;
            cartesianChart1.TooltipFindingStrategy = LiveChartsCore.Measure.TooltipFindingStrategy.Automatic;
            cartesianChart1.UpdaterThrottler = TimeSpan.Parse("00:00:00.0500000");
            // 
            // lblNoteChart1
            // 
            lblNoteChart1.BackColor = Color.FromArgb(248, 250, 252);
            lblNoteChart1.Font = new Font("Segoe UI", 8.5F);
            lblNoteChart1.ForeColor = Color.FromArgb(100, 116, 139);
            lblNoteChart1.Location = new Point(21, 352);
            lblNoteChart1.Name = "lblNoteChart1";
            lblNoteChart1.Padding = new Padding(8, 4, 8, 4);
            lblNoteChart1.Size = new Size(644, 52);
            lblNoteChart1.TabIndex = 2;
            lblNoteChart1.Text = "\U0001f9e0 Hiệu ứng Zeigarnik: Stress = (Task quá hạn + Task sát hạn) / Tổng task chưa hoàn thành × 100. Dữ liệu từ bảng NHIEM_VU và DEADLINE.";
            // 
            // lblTitleChart1
            // 
            lblTitleChart1.AutoSize = true;
            lblTitleChart1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitleChart1.ForeColor = Color.FromArgb(30, 41, 59);
            lblTitleChart1.Location = new Point(15, 15);
            lblTitleChart1.Name = "lblTitleChart1";
            lblTitleChart1.Size = new Size(240, 25);
            lblTitleChart1.TabIndex = 0;
            lblTitleChart1.Text = "📈 Deadline & Chỉ Số Stress";
            // 
            // pnlChart2
            // 
            pnlChart2.BackColor = Color.White;
            pnlChart2.Controls.Add(cartesianChart2);
            pnlChart2.Controls.Add(lblNoteChart2);
            pnlChart2.Controls.Add(lblTitleChart2);
            pnlChart2.Dock = DockStyle.Fill;
            pnlChart2.Location = new Point(674, 3);
            pnlChart2.Name = "pnlChart2";
            pnlChart2.Size = new Size(665, 414);
            pnlChart2.TabIndex = 1;
            // 
            // cartesianChart2
            // 
            cartesianChart2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cartesianChart2.AutoUpdateEnabled = true;
            cartesianChart2.ChartTheme = null;
            skDefaultLegend2.AnimationsSpeed = TimeSpan.Parse("00:00:00.1500000");
            skDefaultLegend2.Content = null;
            skDefaultLegend2.IsValid = false;
            skDefaultLegend2.Opacity = 1F;
            padding3.Bottom = 0F;
            padding3.Left = 0F;
            padding3.Right = 0F;
            padding3.Top = 0F;
            skDefaultLegend2.Padding = padding3;
            skDefaultLegend2.RemoveOnCompleted = false;
            skDefaultLegend2.RotateTransform = 0F;
            skDefaultLegend2.X = 0F;
            skDefaultLegend2.Y = 0F;
            cartesianChart2.Legend = skDefaultLegend2;
            cartesianChart2.Location = new Point(18, 52);
            cartesianChart2.MatchAxesScreenDataRatio = false;
            cartesianChart2.Name = "cartesianChart2";
            cartesianChart2.Size = new Size(551, 257);
            cartesianChart2.TabIndex = 4;
            skDefaultTooltip2.AnimationsSpeed = TimeSpan.Parse("00:00:00.1500000");
            skDefaultTooltip2.Content = null;
            skDefaultTooltip2.IsValid = false;
            skDefaultTooltip2.Opacity = 1F;
            padding4.Bottom = 0F;
            padding4.Left = 0F;
            padding4.Right = 0F;
            padding4.Top = 0F;
            skDefaultTooltip2.Padding = padding4;
            skDefaultTooltip2.RemoveOnCompleted = false;
            skDefaultTooltip2.RotateTransform = 0F;
            skDefaultTooltip2.Wedge = 10;
            skDefaultTooltip2.X = 0F;
            skDefaultTooltip2.Y = 0F;
            cartesianChart2.Tooltip = skDefaultTooltip2;
            cartesianChart2.TooltipFindingStrategy = LiveChartsCore.Measure.TooltipFindingStrategy.Automatic;
            cartesianChart2.UpdaterThrottler = TimeSpan.Parse("00:00:00.0500000");
            // 
            // lblNoteChart2
            // 
            lblNoteChart2.BackColor = Color.FromArgb(248, 250, 252);
            lblNoteChart2.Font = new Font("Segoe UI", 8.5F);
            lblNoteChart2.ForeColor = Color.FromArgb(100, 116, 139);
            lblNoteChart2.Location = new Point(10, 359);
            lblNoteChart2.Name = "lblNoteChart2";
            lblNoteChart2.Padding = new Padding(8, 4, 8, 4);
            lblNoteChart2.Size = new Size(644, 45);
            lblNoteChart2.TabIndex = 3;
            lblNoteChart2.Text = "🎓 Quy chế Bộ GD&ĐT: Chuẩn = Số tín chỉ × 30 giờ. Thực tế = SUM(ThoiLuong) FROM PHIEN_HOC.";
            // 
            // lblTitleChart2
            // 
            lblTitleChart2.AutoSize = true;
            lblTitleChart2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitleChart2.ForeColor = Color.FromArgb(30, 41, 59);
            lblTitleChart2.Location = new Point(15, 15);
            lblTitleChart2.Name = "lblTitleChart2";
            lblTitleChart2.Size = new Size(319, 25);
            lblTitleChart2.TabIndex = 1;
            lblTitleChart2.Text = "📊 Giờ Học: Tiêu Chuẩn vs Thực Tế";
            // 
            // pnlChart3
            // 
            pnlChart3.BackColor = Color.White;
            tlpCharts.SetColumnSpan(pnlChart3, 2);
            pnlChart3.Controls.Add(pieChart1);
            pnlChart3.Controls.Add(lblTitleChart3);
            pnlChart3.Dock = DockStyle.Fill;
            pnlChart3.Location = new Point(3, 423);
            pnlChart3.Name = "pnlChart3";
            pnlChart3.Size = new Size(1336, 365);
            pnlChart3.TabIndex = 2;
            // 
            // pieChart1
            // 
            pieChart1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pieChart1.AutoUpdateEnabled = true;
            pieChart1.BackColor = Color.White;
            pieChart1.ChartTheme = null;
            skDefaultLegend3.AnimationsSpeed = TimeSpan.Parse("00:00:00.1500000");
            skDefaultLegend3.Content = null;
            skDefaultLegend3.IsValid = false;
            skDefaultLegend3.Opacity = 1F;
            padding5.Bottom = 0F;
            padding5.Left = 0F;
            padding5.Right = 0F;
            padding5.Top = 0F;
            skDefaultLegend3.Padding = padding5;
            skDefaultLegend3.RemoveOnCompleted = false;
            skDefaultLegend3.RotateTransform = 0F;
            skDefaultLegend3.X = 0F;
            skDefaultLegend3.Y = 0F;
            pieChart1.Legend = skDefaultLegend3;
            pieChart1.Location = new Point(188, 54);
            pieChart1.Name = "pieChart1";
            pieChart1.Size = new Size(871, 278);
            pieChart1.TabIndex = 4;
            skDefaultTooltip3.AnimationsSpeed = TimeSpan.Parse("00:00:00.1500000");
            skDefaultTooltip3.Content = null;
            skDefaultTooltip3.IsValid = false;
            skDefaultTooltip3.Opacity = 1F;
            padding6.Bottom = 0F;
            padding6.Left = 0F;
            padding6.Right = 0F;
            padding6.Top = 0F;
            skDefaultTooltip3.Padding = padding6;
            skDefaultTooltip3.RemoveOnCompleted = false;
            skDefaultTooltip3.RotateTransform = 0F;
            skDefaultTooltip3.Wedge = 10;
            skDefaultTooltip3.X = 0F;
            skDefaultTooltip3.Y = 0F;
            pieChart1.Tooltip = skDefaultTooltip3;
            pieChart1.UpdaterThrottler = TimeSpan.Parse("00:00:00.0500000");
            pieChart1.Load += pieChart1_Load;
            // 
            // lblTitleChart3
            // 
            lblTitleChart3.AutoSize = true;
            lblTitleChart3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitleChart3.ForeColor = Color.FromArgb(30, 41, 59);
            lblTitleChart3.Location = new Point(15, 15);
            lblTitleChart3.Name = "lblTitleChart3";
            lblTitleChart3.Size = new Size(356, 25);
            lblTitleChart3.TabIndex = 3;
            lblTitleChart3.Text = "🍕 Phân Phối Thời Gian Học Theo Môn";
            // 
            // lblTableTitle
            // 
            lblTableTitle.AutoSize = true;
            lblTableTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTableTitle.ForeColor = Color.FromArgb(30, 41, 59);
            lblTableTitle.Location = new Point(15, 15);
            lblTableTitle.Name = "lblTableTitle";
            lblTableTitle.Size = new Size(427, 25);
            lblTableTitle.TabIndex = 5;
            lblTableTitle.Text = "📋 Bảng Tổng Hợp Hiệu Suất (SQL GROUP BY)";
            // 
            // dgvReport
            // 
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToOrderColumns = true;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.BackgroundColor = Color.White;
            dgvReport.BorderStyle = BorderStyle.None;
            dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(71, 85, 105);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(248, 250, 252);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvReport.ColumnHeadersHeight = 50;
            dgvReport.Columns.AddRange(new DataGridViewColumn[] { colMonHoc, colTinChi, colGioPomo, colGhiChu, colHoanThanh, colDanhGia });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvReport.DefaultCellStyle = dataGridViewCellStyle2;
            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.Location = new Point(3, 53);
            dgvReport.Name = "dgvReport";
            dgvReport.ReadOnly = true;
            dgvReport.RowHeadersVisible = false;
            dgvReport.RowHeadersWidth = 51;
            dgvReport.RowTemplate.Height = 40;
            dgvReport.ScrollBars = ScrollBars.Horizontal;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.Size = new Size(1322, 285);
            dgvReport.TabIndex = 4;
            dgvReport.CellFormatting += dgvReport_CellFormatting;
            // 
            // colMonHoc
            // 
            colMonHoc.HeaderText = "MÔN HỌC";
            colMonHoc.MinimumWidth = 6;
            colMonHoc.Name = "colMonHoc";
            colMonHoc.ReadOnly = true;
            // 
            // colTinChi
            // 
            colTinChi.HeaderText = "TÍN CHỈ";
            colTinChi.MinimumWidth = 6;
            colTinChi.Name = "colTinChi";
            colTinChi.ReadOnly = true;
            // 
            // colGioPomo
            // 
            colGioPomo.HeaderText = "GIỜ POMODORO";
            colGioPomo.MinimumWidth = 6;
            colGioPomo.Name = "colGioPomo";
            colGioPomo.ReadOnly = true;
            // 
            // colGhiChu
            // 
            colGhiChu.HeaderText = "GIỜ CHUẨN";
            colGhiChu.MinimumWidth = 6;
            colGhiChu.Name = "colGhiChu";
            colGhiChu.ReadOnly = true;
            // 
            // colHoanThanh
            // 
            colHoanThanh.HeaderText = "HOÀN THÀNH";
            colHoanThanh.MinimumWidth = 6;
            colHoanThanh.Name = "colHoanThanh";
            colHoanThanh.ReadOnly = true;
            // 
            // colDanhGia
            // 
            colDanhGia.HeaderText = "ĐÁNH GIÁ";
            colDanhGia.MinimumWidth = 6;
            colDanhGia.Name = "colDanhGia";
            colDanhGia.ReadOnly = true;
            // 
            // pnlTableCard
            // 
            pnlTableCard.BackColor = Color.White;
            pnlTableCard.Controls.Add(dgvReport);
            pnlTableCard.Controls.Add(lblTableTitle);
            pnlTableCard.Location = new Point(23, 984);
            pnlTableCard.Name = "pnlTableCard";
            pnlTableCard.Size = new Size(1339, 417);
            pnlTableCard.TabIndex = 3;
            // 
            // frm_baoCaoThongKe
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1394, 1055);
            Controls.Add(pnlTableCard);
            Controls.Add(tlpCharts);
            Controls.Add(pnlFilters);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.5F);
            Name = "frm_baoCaoThongKe";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Study Tracker - Báo cáo Thống kê";
            Load += frm_baoCaoThongKe_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            tlpCharts.ResumeLayout(false);
            pnlChart1.ResumeLayout(false);
            pnlChart1.PerformLayout();
            pnlChart2.ResumeLayout(false);
            pnlChart2.PerformLayout();
            pnlChart3.ResumeLayout(false);
            pnlChart3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
            pnlTableCard.ResumeLayout(false);
            pnlTableCard.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private Panel pnlHeader;
        private Label lblHeaderTitle;
        private Label lblSubHeader;
        private Panel pnlFilters;
        private Label lblSemester;
        private ComboBox cboSemesters;
        private Label lblSubject;
        private ComboBox cboSubjects;
        private Button btnIn;
        private TableLayoutPanel tlpCharts;
        private Panel pnlChart1;
        private Panel pnlChart2;
        private Panel pnlChart3;
        private Label lblTitleChart1;
        private Label lblTitleChart2;
        private Label lblTitleChart3;
        private Label lblNoteChart1;
        private Label lblNoteChart2;
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart cartesianChart1;
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart cartesianChart2;
        private LiveChartsCore.SkiaSharpView.WinForms.PieChart pieChart1;
        private Label lblTableTitle;
        private DataGridView dgvReport;
        private DataGridViewTextBoxColumn colMonHoc;
        private DataGridViewTextBoxColumn colTinChi;
        private DataGridViewTextBoxColumn colGioPomo;
        private DataGridViewTextBoxColumn colGhiChu;
        private DataGridViewTextBoxColumn colHoanThanh;
        private DataGridViewTextBoxColumn colDanhGia;
        private Panel pnlTableCard;
    }
}