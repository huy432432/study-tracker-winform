namespace do_an_1.Frm
{
    partial class frmpomodoro
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
            components = new System.ComponentModel.Container();
            lblHeader = new Label();
            lblLabelMonHoc = new Label();
            cboMonHoc = new ComboBox();
            lblLabelNhiemVu = new Label();
            cboNhiemVu = new ComboBox();
            lblLabelThoiGian = new Label();
            cboThoiGian = new ComboBox();
            lblTimerDisplay = new Label();
            lblStatusLabel = new Label();
            btnStart = new Button();
            btnReset = new Button();
            pnlWarning = new Panel();
            lblWarningDesc = new Label();
            lblWarningTitle = new Label();
            pnlLog = new Panel();
            lblLogMessage = new Label();
            pomodoroTimer = new System.Windows.Forms.Timer(components);
            pnlWarning.SuspendLayout();
            pnlLog.SuspendLayout();
            SuspendLayout();
            // 
            // lblHeader
            // 
            lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeader.ForeColor = Color.FromArgb(59, 130, 246);
            lblHeader.Location = new Point(19, 9);
            lblHeader.Margin = new Padding(4, 0, 4, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(741, 54);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "⏱ CẤU HÌNH PHIÊN HỌC TẬP TRUNG";
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblLabelMonHoc
            // 
            lblLabelMonHoc.AutoSize = true;
            lblLabelMonHoc.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLabelMonHoc.ForeColor = Color.FromArgb(148, 163, 184);
            lblLabelMonHoc.Location = new Point(47, 108);
            lblLabelMonHoc.Margin = new Padding(4, 0, 4, 0);
            lblLabelMonHoc.Name = "lblLabelMonHoc";
            lblLabelMonHoc.Size = new Size(268, 21);
            lblLabelMonHoc.TabIndex = 1;
            lblLabelMonHoc.Text = "📖 1. Chọn Môn Học (MON_HOC):";
            // 
            // cboMonHoc
            // 
            cboMonHoc.BackColor = Color.FromArgb(15, 23, 42);
            cboMonHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMonHoc.FlatStyle = FlatStyle.Flat;
            cboMonHoc.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboMonHoc.ForeColor = Color.FromArgb(248, 250, 252);
            cboMonHoc.FormattingEnabled = true;
            cboMonHoc.Items.AddRange(new object[] { "Lập trình C# WinForms & SQL Server", "Khoa Học Dữ Liệu Với Python" });
            cboMonHoc.Location = new Point(51, 143);
            cboMonHoc.Margin = new Padding(4, 5, 4, 5);
            cboMonHoc.Name = "cboMonHoc";
            cboMonHoc.Size = new Size(665, 31);
            cboMonHoc.TabIndex = 2;
            // 
            // lblLabelNhiemVu
            // 
            lblLabelNhiemVu.AutoSize = true;
            lblLabelNhiemVu.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLabelNhiemVu.ForeColor = Color.FromArgb(148, 163, 184);
            lblLabelNhiemVu.Location = new Point(47, 208);
            lblLabelNhiemVu.Margin = new Padding(4, 0, 4, 0);
            lblLabelNhiemVu.Name = "lblLabelNhiemVu";
            lblLabelNhiemVu.Size = new Size(381, 21);
            lblLabelNhiemVu.TabIndex = 3;
            lblLabelNhiemVu.Text = "📋 2. Liên kết Deadline / Nhiệm vụ (NHIEM_VU):";
            // 
            // cboNhiemVu
            // 
            cboNhiemVu.BackColor = Color.FromArgb(15, 23, 42);
            cboNhiemVu.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhiemVu.FlatStyle = FlatStyle.Flat;
            cboNhiemVu.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboNhiemVu.ForeColor = Color.FromArgb(248, 250, 252);
            cboNhiemVu.FormattingEnabled = true;
            cboNhiemVu.Items.AddRange(new object[] { "-- Học tự do / Nghiên cứu (không gán task) --", "Làm bài tập lớn WinForms 3-Tier", "Viết báo cáo thống kê định kỳ" });
            cboNhiemVu.Location = new Point(51, 243);
            cboNhiemVu.Margin = new Padding(4, 5, 4, 5);
            cboNhiemVu.Name = "cboNhiemVu";
            cboNhiemVu.Size = new Size(665, 31);
            cboNhiemVu.TabIndex = 4;
            // 
            // lblLabelThoiGian
            // 
            lblLabelThoiGian.AutoSize = true;
            lblLabelThoiGian.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLabelThoiGian.ForeColor = Color.FromArgb(148, 163, 184);
            lblLabelThoiGian.Location = new Point(47, 308);
            lblLabelThoiGian.Margin = new Padding(4, 0, 4, 0);
            lblLabelThoiGian.Name = "lblLabelThoiGian";
            lblLabelThoiGian.Size = new Size(258, 21);
            lblLabelThoiGian.TabIndex = 5;
            lblLabelThoiGian.Text = "⏱ 3. Thiết lập thời lượng phiên:";
            // 
            // cboThoiGian
            // 
            cboThoiGian.BackColor = Color.FromArgb(15, 23, 42);
            cboThoiGian.DropDownStyle = ComboBoxStyle.DropDownList;
            cboThoiGian.FlatStyle = FlatStyle.Flat;
            cboThoiGian.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboThoiGian.ForeColor = Color.FromArgb(248, 250, 252);
            cboThoiGian.FormattingEnabled = true;
            cboThoiGian.Items.AddRange(new object[] { "Tiêu chuẩn Pomodoro (25 phút tập trung · 5 phút nghỉ)", "Phiên chuyên sâu (50 phút tập trung · 10 phút nghỉ)", "Test(30s , 5s)" });
            cboThoiGian.Location = new Point(51, 343);
            cboThoiGian.Margin = new Padding(4, 5, 4, 5);
            cboThoiGian.Name = "cboThoiGian";
            cboThoiGian.Size = new Size(665, 31);
            cboThoiGian.TabIndex = 6;
            // 
            // lblTimerDisplay
            // 
            lblTimerDisplay.Font = new Font("Consolas", 56F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTimerDisplay.ForeColor = Color.FromArgb(248, 250, 252);
            lblTimerDisplay.Location = new Point(16, 408);
            lblTimerDisplay.Margin = new Padding(4, 0, 4, 0);
            lblTimerDisplay.Name = "lblTimerDisplay";
            lblTimerDisplay.Size = new Size(741, 138);
            lblTimerDisplay.TabIndex = 7;
            lblTimerDisplay.Text = "25:00";
            lblTimerDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStatusLabel
            // 
            lblStatusLabel.BackColor = Color.FromArgb(30, 41, 59);
            lblStatusLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStatusLabel.ForeColor = Color.FromArgb(168, 85, 247);
            lblStatusLabel.Location = new Point(153, 562);
            lblStatusLabel.Margin = new Padding(4, 0, 4, 0);
            lblStatusLabel.Name = "lblStatusLabel";
            lblStatusLabel.Size = new Size(467, 46);
            lblStatusLabel.TabIndex = 8;
            lblStatusLabel.Text = "✓ Sẵn sàng ghi nhận PHIEN_HOC";
            lblStatusLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(16, 185, 129);
            btnStart.Cursor = Cursors.Hand;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(167, 638);
            btnStart.Margin = new Padding(4, 5, 4, 5);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(200, 62);
            btnStart.TabIndex = 9;
            btnStart.Text = "▶ Bắt Đầu Học";
            btnStart.UseVisualStyleBackColor = false;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.FromArgb(71, 85, 105);
            btnReset.Cursor = Cursors.Hand;
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(407, 638);
            btnReset.Margin = new Padding(4, 5, 4, 5);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(200, 62);
            btnReset.TabIndex = 10;
            btnReset.Text = "🚫 Hủy Phiên";
            btnReset.UseVisualStyleBackColor = false;
            // 
            // pnlWarning
            // 
            pnlWarning.BackColor = Color.FromArgb(30, 27, 38);
            pnlWarning.Controls.Add(lblWarningDesc);
            pnlWarning.Controls.Add(lblWarningTitle);
            pnlWarning.Location = new Point(51, 731);
            pnlWarning.Margin = new Padding(4, 5, 4, 5);
            pnlWarning.Name = "pnlWarning";
            pnlWarning.Size = new Size(667, 115);
            pnlWarning.TabIndex = 11;
            // 
            // lblWarningDesc
            // 
            lblWarningDesc.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblWarningDesc.ForeColor = Color.FromArgb(203, 213, 225);
            lblWarningDesc.Location = new Point(16, 43);
            lblWarningDesc.Margin = new Padding(4, 0, 4, 0);
            lblWarningDesc.Name = "lblWarningDesc";
            lblWarningDesc.Size = new Size(633, 62);
            lblWarningDesc.TabIndex = 1;
            lblWarningDesc.Text = "Hệ thống tự động theo dõi khoảng cách giữa các phiên. Cảnh báo nếu sinh viên thức liên tục > 17 tiếng.";
            // 
            // lblWarningTitle
            // 
            lblWarningTitle.AutoSize = true;
            lblWarningTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblWarningTitle.ForeColor = Color.FromArgb(239, 68, 68);
            lblWarningTitle.Location = new Point(16, 12);
            lblWarningTitle.Margin = new Padding(4, 0, 4, 0);
            lblWarningTitle.Name = "lblWarningTitle";
            lblWarningTitle.Size = new Size(361, 21);
            lblWarningTitle.TabIndex = 0;
            lblWarningTitle.Text = "🛡 Quy tắc kiểm soát nhận thức (Alhola 2007)";
            // 
            // pnlLog
            // 
            pnlLog.BackColor = Color.FromArgb(15, 23, 42);
            pnlLog.BorderStyle = BorderStyle.FixedSingle;
            pnlLog.Controls.Add(lblLogMessage);
            pnlLog.Location = new Point(51, 869);
            pnlLog.Margin = new Padding(4, 5, 4, 5);
            pnlLog.Name = "pnlLog";
            pnlLog.Size = new Size(666, 68);
            pnlLog.TabIndex = 12;
            // 
            // lblLogMessage
            // 
            lblLogMessage.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblLogMessage.ForeColor = Color.FromArgb(148, 163, 184);
            lblLogMessage.Location = new Point(13, 8);
            lblLogMessage.Margin = new Padding(4, 0, 4, 0);
            lblLogMessage.Name = "lblLogMessage";
            lblLogMessage.Size = new Size(637, 51);
            lblLogMessage.TabIndex = 0;
            lblLogMessage.Text = "⏳ Sẵn sàng. Chọn môn, nhiệm vụ (nếu có) và nhấn Bắt Đầu.";
            lblLogMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pomodoroTimer
            // 
            pomodoroTimer.Interval = 1000;
            // 
            // frmpomodoro
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 41, 59);
            ClientSize = new Size(773, 977);
            Controls.Add(pnlLog);
            Controls.Add(pnlWarning);
            Controls.Add(btnReset);
            Controls.Add(btnStart);
            Controls.Add(lblStatusLabel);
            Controls.Add(lblTimerDisplay);
            Controls.Add(cboThoiGian);
            Controls.Add(lblLabelThoiGian);
            Controls.Add(cboNhiemVu);
            Controls.Add(lblLabelNhiemVu);
            Controls.Add(cboMonHoc);
            Controls.Add(lblLabelMonHoc);
            Controls.Add(lblHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "frmpomodoro";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Phiên Học Pomodoro Chuẩn Logic · Study Tracker";
            pnlWarning.ResumeLayout(false);
            pnlWarning.PerformLayout();
            pnlLog.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblLabelMonHoc;
        private System.Windows.Forms.ComboBox cboMonHoc;
        private System.Windows.Forms.Label lblLabelNhiemVu;
        private System.Windows.Forms.ComboBox cboNhiemVu;
        private System.Windows.Forms.Label lblLabelThoiGian;
        private System.Windows.Forms.ComboBox cboThoiGian;
        private System.Windows.Forms.Label lblTimerDisplay;
        private System.Windows.Forms.Label lblStatusLabel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Panel pnlWarning;
        private System.Windows.Forms.Label lblWarningTitle;
        private System.Windows.Forms.Label lblWarningDesc;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.Label lblLogMessage;
        private System.Windows.Forms.Timer pomodoroTimer;
    }
}