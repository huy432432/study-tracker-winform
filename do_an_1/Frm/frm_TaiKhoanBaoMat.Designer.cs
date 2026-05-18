namespace do_an_1.Frm
{
    partial class frm_TaiKhoanBaoMat
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
            pnlTopbar = new Panel();
            lblLogoIcon = new Label();
            lblLogoTitle = new Label();
            lblLogoSubtitle = new Label();
            btnBackToDashboard = new Button();
            pnlContentWrapper = new Panel();
            pnlContent = new Panel();
            lblPageHeader = new Label();
            grpProfile = new GroupBox();
            pbAvatar = new PictureBox();
            lblAvatarInitial = new Label();
            btnUploadAvatar = new Button();
            lblAvatarHint = new Label();
            lblHoTen = new Label();
            txtHoTen = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblGpaMucTieu = new Label();
            nudGpaMucTieu = new NumericUpDown();
            lblNgayTao = new Label();
            txtNgayTao = new TextBox();
            btnSaveProfile = new Button();
            grpPassword = new GroupBox();
            lblCurrentPassword = new Label();
            txtCurrentPassword = new TextBox();
            lblNewPassword = new Label();
            txtNewPassword = new TextBox();
            pgbPasswordStrength = new ProgressBar();
            lblPasswordHint = new Label();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            btnChangePassword = new Button();
            pnlDangerZone = new Panel();
            lblDangerTitle = new Label();
            lblDangerDesc = new Label();
            btnDeleteAccount = new Button();
            pnlDeleteOverlay = new Panel();
            pnlDeleteConfirm = new Panel();
            lblDeleteIcon = new Label();
            lblDeleteTitle = new Label();
            lblDeleteSubtitle = new Label();
            txtDeleteConfirm = new TextBox();
            btnCancelDelete = new Button();
            btnConfirmDelete = new Button();
            pnlTopbar.SuspendLayout();
            pnlContentWrapper.SuspendLayout();
            pnlContent.SuspendLayout();
            grpProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbAvatar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGpaMucTieu).BeginInit();
            grpPassword.SuspendLayout();
            pnlDangerZone.SuspendLayout();
            pnlDeleteOverlay.SuspendLayout();
            pnlDeleteConfirm.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTopbar
            // 
            pnlTopbar.BackColor = Color.FromArgb(245, 247, 252);
            pnlTopbar.Controls.Add(lblLogoIcon);
            pnlTopbar.Controls.Add(lblLogoTitle);
            pnlTopbar.Controls.Add(lblLogoSubtitle);
            pnlTopbar.Controls.Add(btnBackToDashboard);
            pnlTopbar.Dock = DockStyle.Top;
            pnlTopbar.Location = new Point(0, 0);
            pnlTopbar.Name = "pnlTopbar";
            pnlTopbar.Padding = new Padding(20, 0, 20, 0);
            pnlTopbar.Size = new Size(718, 60);
            pnlTopbar.TabIndex = 2;
            // 
            // lblLogoIcon
            // 
            lblLogoIcon.AutoSize = true;
            lblLogoIcon.Font = new Font("Segoe UI", 18F);
            lblLogoIcon.Location = new Point(3, 10);
            lblLogoIcon.Name = "lblLogoIcon";
            lblLogoIcon.Size = new Size(59, 41);
            lblLogoIcon.TabIndex = 0;
            lblLogoIcon.Text = "📚";
            // 
            // lblLogoTitle
            // 
            lblLogoTitle.AutoSize = true;
            lblLogoTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblLogoTitle.ForeColor = Color.FromArgb(30, 42, 74);
            lblLogoTitle.Location = new Point(64, 8);
            lblLogoTitle.Name = "lblLogoTitle";
            lblLogoTitle.Size = new Size(168, 32);
            lblLogoTitle.TabIndex = 1;
            lblLogoTitle.Text = "Study Tracker";
            // 
            // lblLogoSubtitle
            // 
            lblLogoSubtitle.AutoSize = true;
            lblLogoSubtitle.Font = new Font("Segoe UI", 8F);
            lblLogoSubtitle.ForeColor = Color.FromArgb(139, 156, 192);
            lblLogoSubtitle.Location = new Point(66, 32);
            lblLogoSubtitle.Name = "lblLogoSubtitle";
            lblLogoSubtitle.Size = new Size(138, 19);
            lblLogoSubtitle.TabIndex = 2;
            lblLogoSubtitle.Text = "BALANCE IS THE KEY";
            // 
            // btnBackToDashboard
            // 
            btnBackToDashboard.BackColor = Color.White;
            btnBackToDashboard.Cursor = Cursors.Hand;
            btnBackToDashboard.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 245);
            btnBackToDashboard.FlatStyle = FlatStyle.Flat;
            btnBackToDashboard.Font = new Font("Segoe UI", 10F);
            btnBackToDashboard.ForeColor = Color.FromArgb(90, 107, 140);
            btnBackToDashboard.Location = new Point(576, 8);
            btnBackToDashboard.Name = "btnBackToDashboard";
            btnBackToDashboard.Size = new Size(130, 36);
            btnBackToDashboard.TabIndex = 3;
            btnBackToDashboard.Text = "← Dashboard";
            btnBackToDashboard.UseVisualStyleBackColor = false;
            // 
            // pnlContentWrapper
            // 
            pnlContentWrapper.AutoScroll = true;
            pnlContentWrapper.Controls.Add(pnlContent);
            pnlContentWrapper.Dock = DockStyle.Fill;
            pnlContentWrapper.Location = new Point(0, 60);
            pnlContentWrapper.Name = "pnlContentWrapper";
            pnlContentWrapper.Size = new Size(718, 640);
            pnlContentWrapper.TabIndex = 0;
            // 
            // pnlContent
            // 
            pnlContent.AutoScroll = true;
            pnlContent.BackColor = Color.Transparent;
            pnlContent.Controls.Add(lblPageHeader);
            pnlContent.Controls.Add(grpProfile);
            pnlContent.Controls.Add(grpPassword);
            pnlContent.Controls.Add(pnlDangerZone);
            pnlContent.Location = new Point(0, 0);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(747, 620);
            pnlContent.TabIndex = 0;
            // 
            // lblPageHeader
            // 
            lblPageHeader.AutoSize = true;
            lblPageHeader.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblPageHeader.ForeColor = Color.FromArgb(30, 42, 74);
            lblPageHeader.Location = new Point(0, 0);
            lblPageHeader.Name = "lblPageHeader";
            lblPageHeader.Size = new Size(451, 41);
            lblPageHeader.TabIndex = 0;
            lblPageHeader.Text = "👤 Quản lý Tài khoản & Bảo mật";
            // 
            // grpProfile
            // 
            grpProfile.Controls.Add(pbAvatar);
            grpProfile.Controls.Add(lblAvatarInitial);
            grpProfile.Controls.Add(btnUploadAvatar);
            grpProfile.Controls.Add(lblAvatarHint);
            grpProfile.Controls.Add(lblHoTen);
            grpProfile.Controls.Add(txtHoTen);
            grpProfile.Controls.Add(lblEmail);
            grpProfile.Controls.Add(txtEmail);
            grpProfile.Controls.Add(lblGpaMucTieu);
            grpProfile.Controls.Add(nudGpaMucTieu);
            grpProfile.Controls.Add(lblNgayTao);
            grpProfile.Controls.Add(txtNgayTao);
            grpProfile.Controls.Add(btnSaveProfile);
            grpProfile.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            grpProfile.ForeColor = Color.FromArgb(30, 42, 74);
            grpProfile.Location = new Point(6, 57);
            grpProfile.Name = "grpProfile";
            grpProfile.Padding = new Padding(20);
            grpProfile.Size = new Size(700, 317);
            grpProfile.TabIndex = 4;
            grpProfile.TabStop = false;
            grpProfile.Text = "📋 Thông tin cá nhân";
            // 
            // pbAvatar
            // 
            pbAvatar.BackColor = Color.FromArgb(123, 147, 212);
            pbAvatar.Location = new Point(24, 36);
            pbAvatar.Name = "pbAvatar";
            pbAvatar.Size = new Size(80, 80);
            pbAvatar.SizeMode = PictureBoxSizeMode.CenterImage;
            pbAvatar.TabIndex = 0;
            pbAvatar.TabStop = false;
            // 
            // lblAvatarInitial
            // 
            lblAvatarInitial.BackColor = Color.Transparent;
            lblAvatarInitial.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            lblAvatarInitial.ForeColor = Color.White;
            lblAvatarInitial.Location = new Point(24, 36);
            lblAvatarInitial.Name = "lblAvatarInitial";
            lblAvatarInitial.Size = new Size(80, 80);
            lblAvatarInitial.TabIndex = 1;
            lblAvatarInitial.Text = "A";
            lblAvatarInitial.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnUploadAvatar
            // 
            btnUploadAvatar.BackColor = Color.White;
            btnUploadAvatar.Cursor = Cursors.Hand;
            btnUploadAvatar.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 245);
            btnUploadAvatar.FlatStyle = FlatStyle.Flat;
            btnUploadAvatar.Font = new Font("Segoe UI", 10F);
            btnUploadAvatar.ForeColor = Color.FromArgb(90, 107, 140);
            btnUploadAvatar.Location = new Point(120, 40);
            btnUploadAvatar.Name = "btnUploadAvatar";
            btnUploadAvatar.Size = new Size(180, 36);
            btnUploadAvatar.TabIndex = 2;
            btnUploadAvatar.Text = "📷 Chọn ảnh đại diện";
            btnUploadAvatar.UseVisualStyleBackColor = false;
            // 
            // lblAvatarHint
            // 
            lblAvatarHint.AutoSize = true;
            lblAvatarHint.Font = new Font("Segoe UI", 9F);
            lblAvatarHint.ForeColor = Color.FromArgb(139, 156, 192);
            lblAvatarHint.Location = new Point(120, 82);
            lblAvatarHint.Name = "lblAvatarHint";
            lblAvatarHint.Size = new Size(251, 20);
            lblAvatarHint.TabIndex = 3;
            lblAvatarHint.Text = "Chọn file ảnh (jpg, png). Tối đa 2MB.";
            // 
            // lblHoTen
            // 
            lblHoTen.AutoSize = true;
            lblHoTen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblHoTen.ForeColor = Color.FromArgb(139, 156, 192);
            lblHoTen.Location = new Point(24, 136);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(75, 20);
            lblHoTen.TabIndex = 4;
            lblHoTen.Text = "HỌ TÊN *";
            // 
            // txtHoTen
            // 
            txtHoTen.BorderStyle = BorderStyle.FixedSingle;
            txtHoTen.Font = new Font("Segoe UI", 11F);
            txtHoTen.Location = new Point(24, 158);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(310, 32);
            txtHoTen.TabIndex = 5;
            txtHoTen.Text = "Nguyễn Văn An";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(139, 156, 192);
            lblEmail.Location = new Point(358, 136);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(55, 20);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "EMAIL";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.FromArgb(248, 250, 255);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Enabled = false;
            txtEmail.Font = new Font("Segoe UI", 11F);
            txtEmail.Location = new Point(358, 158);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(310, 32);
            txtEmail.TabIndex = 7;
            txtEmail.Text = "an.nguyen@example.com";
            // 
            // lblGpaMucTieu
            // 
            lblGpaMucTieu.AutoSize = true;
            lblGpaMucTieu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblGpaMucTieu.ForeColor = Color.FromArgb(139, 156, 192);
            lblGpaMucTieu.Location = new Point(24, 200);
            lblGpaMucTieu.Name = "lblGpaMucTieu";
            lblGpaMucTieu.Size = new Size(114, 20);
            lblGpaMucTieu.TabIndex = 8;
            lblGpaMucTieu.Text = "GPA MỤC TIÊU";
            // 
            // nudGpaMucTieu
            // 
            nudGpaMucTieu.BorderStyle = BorderStyle.FixedSingle;
            nudGpaMucTieu.DecimalPlaces = 2;
            nudGpaMucTieu.Font = new Font("Segoe UI", 11F);
            nudGpaMucTieu.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            nudGpaMucTieu.Location = new Point(24, 222);
            nudGpaMucTieu.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
            nudGpaMucTieu.Name = "nudGpaMucTieu";
            nudGpaMucTieu.Size = new Size(310, 32);
            nudGpaMucTieu.TabIndex = 9;
            nudGpaMucTieu.Value = new decimal(new int[] { 360, 0, 0, 131072 });
            // 
            // lblNgayTao
            // 
            lblNgayTao.AutoSize = true;
            lblNgayTao.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNgayTao.ForeColor = Color.FromArgb(139, 156, 192);
            lblNgayTao.Location = new Point(358, 200);
            lblNgayTao.Name = "lblNgayTao";
            lblNgayTao.Size = new Size(173, 20);
            lblNgayTao.TabIndex = 10;
            lblNgayTao.Text = "NGÀY TẠO TÀI KHOẢN";
            // 
            // txtNgayTao
            // 
            txtNgayTao.BackColor = Color.FromArgb(248, 250, 255);
            txtNgayTao.BorderStyle = BorderStyle.FixedSingle;
            txtNgayTao.Enabled = false;
            txtNgayTao.Font = new Font("Segoe UI", 11F);
            txtNgayTao.Location = new Point(358, 222);
            txtNgayTao.Name = "txtNgayTao";
            txtNgayTao.Size = new Size(310, 32);
            txtNgayTao.TabIndex = 11;
            txtNgayTao.Text = "15/03/2026";
            // 
            // btnSaveProfile
            // 
            btnSaveProfile.BackColor = Color.FromArgb(74, 107, 191);
            btnSaveProfile.Cursor = Cursors.Hand;
            btnSaveProfile.FlatAppearance.BorderSize = 0;
            btnSaveProfile.FlatStyle = FlatStyle.Flat;
            btnSaveProfile.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveProfile.ForeColor = Color.White;
            btnSaveProfile.Location = new Point(520, 260);
            btnSaveProfile.Name = "btnSaveProfile";
            btnSaveProfile.Size = new Size(150, 38);
            btnSaveProfile.TabIndex = 12;
            btnSaveProfile.Text = "💾 Lưu thay đổi";
            btnSaveProfile.UseVisualStyleBackColor = false;
            // 
            // grpPassword
            // 
            grpPassword.Controls.Add(lblCurrentPassword);
            grpPassword.Controls.Add(txtCurrentPassword);
            grpPassword.Controls.Add(lblNewPassword);
            grpPassword.Controls.Add(txtNewPassword);
            grpPassword.Controls.Add(pgbPasswordStrength);
            grpPassword.Controls.Add(lblPasswordHint);
            grpPassword.Controls.Add(lblConfirmPassword);
            grpPassword.Controls.Add(txtConfirmPassword);
            grpPassword.Controls.Add(btnChangePassword);
            grpPassword.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            grpPassword.ForeColor = Color.FromArgb(30, 42, 74);
            grpPassword.Location = new Point(6, 380);
            grpPassword.Name = "grpPassword";
            grpPassword.Padding = new Padding(20);
            grpPassword.Size = new Size(700, 300);
            grpPassword.TabIndex = 5;
            grpPassword.TabStop = false;
            grpPassword.Text = "🔒 Đổi mật khẩu";
            // 
            // lblCurrentPassword
            // 
            lblCurrentPassword.AutoSize = true;
            lblCurrentPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCurrentPassword.ForeColor = Color.FromArgb(139, 156, 192);
            lblCurrentPassword.Location = new Point(24, 36);
            lblCurrentPassword.Name = "lblCurrentPassword";
            lblCurrentPassword.Size = new Size(170, 20);
            lblCurrentPassword.TabIndex = 0;
            lblCurrentPassword.Text = "MẬT KHẨU HIỆN TẠI *";
            // 
            // txtCurrentPassword
            // 
            txtCurrentPassword.BorderStyle = BorderStyle.FixedSingle;
            txtCurrentPassword.Font = new Font("Segoe UI", 11F);
            txtCurrentPassword.Location = new Point(24, 58);
            txtCurrentPassword.Name = "txtCurrentPassword";
            txtCurrentPassword.PlaceholderText = "Nhập mật khẩu hiện tại";
            txtCurrentPassword.Size = new Size(640, 32);
            txtCurrentPassword.TabIndex = 1;
            txtCurrentPassword.UseSystemPasswordChar = true;
            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNewPassword.ForeColor = Color.FromArgb(139, 156, 192);
            lblNewPassword.Location = new Point(24, 98);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(136, 20);
            lblNewPassword.TabIndex = 2;
            lblNewPassword.Text = "MẬT KHẨU MỚI *";
            // 
            // txtNewPassword
            // 
            txtNewPassword.BorderStyle = BorderStyle.FixedSingle;
            txtNewPassword.Font = new Font("Segoe UI", 11F);
            txtNewPassword.Location = new Point(24, 120);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PlaceholderText = "Ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường và số";
            txtNewPassword.Size = new Size(640, 32);
            txtNewPassword.TabIndex = 3;
            txtNewPassword.UseSystemPasswordChar = true;
            // 
            // pgbPasswordStrength
            // 
            pgbPasswordStrength.Location = new Point(24, 156);
            pgbPasswordStrength.Name = "pgbPasswordStrength";
            pgbPasswordStrength.Size = new Size(640, 6);
            pgbPasswordStrength.TabIndex = 4;
            // 
            // lblPasswordHint
            // 
            lblPasswordHint.AutoSize = true;
            lblPasswordHint.Font = new Font("Segoe UI", 8F);
            lblPasswordHint.ForeColor = Color.FromArgb(139, 156, 192);
            lblPasswordHint.Location = new Point(24, 166);
            lblPasswordHint.Name = "lblPasswordHint";
            lblPasswordHint.Size = new Size(331, 19);
            lblPasswordHint.TabIndex = 5;
            lblPasswordHint.Text = "Mật khẩu sẽ được mã hóa bằng BCrypt trước khi lưu";
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblConfirmPassword.ForeColor = Color.FromArgb(139, 156, 192);
            lblConfirmPassword.Location = new Point(24, 184);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(220, 20);
            lblConfirmPassword.TabIndex = 6;
            lblConfirmPassword.Text = "XÁC NHẬN MẬT KHẨU MỚI *";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmPassword.Font = new Font("Segoe UI", 11F);
            txtConfirmPassword.Location = new Point(24, 206);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PlaceholderText = "Nhập lại mật khẩu mới";
            txtConfirmPassword.Size = new Size(640, 32);
            txtConfirmPassword.TabIndex = 7;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // btnChangePassword
            // 
            btnChangePassword.BackColor = Color.FromArgb(74, 107, 191);
            btnChangePassword.Cursor = Cursors.Hand;
            btnChangePassword.FlatAppearance.BorderSize = 0;
            btnChangePassword.FlatStyle = FlatStyle.Flat;
            btnChangePassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnChangePassword.ForeColor = Color.White;
            btnChangePassword.Location = new Point(504, 250);
            btnChangePassword.Name = "btnChangePassword";
            btnChangePassword.Size = new Size(160, 38);
            btnChangePassword.TabIndex = 8;
            btnChangePassword.Text = "🔐 Đổi mật khẩu";
            btnChangePassword.UseVisualStyleBackColor = false;
            // 
            // pnlDangerZone
            // 
            pnlDangerZone.BackColor = Color.FromArgb(254, 242, 242);
            pnlDangerZone.BorderStyle = BorderStyle.FixedSingle;
            pnlDangerZone.Controls.Add(lblDangerTitle);
            pnlDangerZone.Controls.Add(lblDangerDesc);
            pnlDangerZone.Controls.Add(btnDeleteAccount);
            pnlDangerZone.Location = new Point(6, 686);
            pnlDangerZone.Name = "pnlDangerZone";
            pnlDangerZone.Padding = new Padding(16);
            pnlDangerZone.Size = new Size(700, 120);
            pnlDangerZone.TabIndex = 6;
            // 
            // lblDangerTitle
            // 
            lblDangerTitle.AutoSize = true;
            lblDangerTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblDangerTitle.ForeColor = Color.FromArgb(224, 85, 85);
            lblDangerTitle.Location = new Point(16, 14);
            lblDangerTitle.Name = "lblDangerTitle";
            lblDangerTitle.Size = new Size(219, 30);
            lblDangerTitle.TabIndex = 0;
            lblDangerTitle.Text = "⚠️ Vùng nguy hiểm";
            // 
            // lblDangerDesc
            // 
            lblDangerDesc.Font = new Font("Segoe UI", 9F);
            lblDangerDesc.ForeColor = Color.FromArgb(90, 107, 140);
            lblDangerDesc.Location = new Point(16, 42);
            lblDangerDesc.Name = "lblDangerDesc";
            lblDangerDesc.Size = new Size(660, 36);
            lblDangerDesc.TabIndex = 1;
            lblDangerDesc.Text = "Hành động xóa tài khoản sẽ xóa VĨNH VIỄN tất cả dữ liệu: môn học, điểm số, deadline, ghi chú, nhật ký học tập. Hành động này KHÔNG THỂ HOÀN TÁC.";
            // 
            // btnDeleteAccount
            // 
            btnDeleteAccount.BackColor = Color.FromArgb(224, 85, 85);
            btnDeleteAccount.Cursor = Cursors.Hand;
            btnDeleteAccount.FlatAppearance.BorderSize = 0;
            btnDeleteAccount.FlatStyle = FlatStyle.Flat;
            btnDeleteAccount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDeleteAccount.ForeColor = Color.White;
            btnDeleteAccount.Location = new Point(460, 81);
            btnDeleteAccount.Name = "btnDeleteAccount";
            btnDeleteAccount.Size = new Size(220, 29);
            btnDeleteAccount.TabIndex = 2;
            btnDeleteAccount.Text = "🗑️ Xóa tài khoản vĩnh viễn";
            btnDeleteAccount.UseVisualStyleBackColor = false;
            // 
            // pnlDeleteOverlay
            // 
            pnlDeleteOverlay.BackColor = Color.FromArgb(128, 0, 0, 0);
            pnlDeleteOverlay.Controls.Add(pnlDeleteConfirm);
            pnlDeleteOverlay.Dock = DockStyle.Fill;
            pnlDeleteOverlay.Location = new Point(0, 0);
            pnlDeleteOverlay.Name = "pnlDeleteOverlay";
            pnlDeleteOverlay.Size = new Size(718, 700);
            pnlDeleteOverlay.TabIndex = 3;
            pnlDeleteOverlay.Visible = false;
            // 
            // pnlDeleteConfirm
            // 
            pnlDeleteConfirm.BackColor = Color.White;
            pnlDeleteConfirm.BorderStyle = BorderStyle.FixedSingle;
            pnlDeleteConfirm.Controls.Add(lblDeleteIcon);
            pnlDeleteConfirm.Controls.Add(lblDeleteTitle);
            pnlDeleteConfirm.Controls.Add(lblDeleteSubtitle);
            pnlDeleteConfirm.Controls.Add(txtDeleteConfirm);
            pnlDeleteConfirm.Controls.Add(btnCancelDelete);
            pnlDeleteConfirm.Controls.Add(btnConfirmDelete);
            pnlDeleteConfirm.Location = new Point(325, 220);
            pnlDeleteConfirm.Name = "pnlDeleteConfirm";
            pnlDeleteConfirm.Padding = new Padding(24);
            pnlDeleteConfirm.Size = new Size(450, 260);
            pnlDeleteConfirm.TabIndex = 0;
            // 
            // lblDeleteIcon
            // 
            lblDeleteIcon.Font = new Font("Segoe UI", 36F);
            lblDeleteIcon.Location = new Point(24, 16);
            lblDeleteIcon.Name = "lblDeleteIcon";
            lblDeleteIcon.Size = new Size(400, 50);
            lblDeleteIcon.TabIndex = 0;
            lblDeleteIcon.Text = "⚠️";
            lblDeleteIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDeleteTitle
            // 
            lblDeleteTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblDeleteTitle.ForeColor = Color.FromArgb(224, 85, 85);
            lblDeleteTitle.Location = new Point(24, 64);
            lblDeleteTitle.Name = "lblDeleteTitle";
            lblDeleteTitle.Size = new Size(400, 30);
            lblDeleteTitle.TabIndex = 1;
            lblDeleteTitle.Text = "Xác nhận xóa tài khoản";
            lblDeleteTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDeleteSubtitle
            // 
            lblDeleteSubtitle.Font = new Font("Segoe UI", 10F);
            lblDeleteSubtitle.ForeColor = Color.FromArgb(90, 107, 140);
            lblDeleteSubtitle.Location = new Point(24, 98);
            lblDeleteSubtitle.Name = "lblDeleteSubtitle";
            lblDeleteSubtitle.Size = new Size(400, 40);
            lblDeleteSubtitle.TabIndex = 2;
            lblDeleteSubtitle.Text = "Tất cả dữ liệu của bạn sẽ bị xóa vĩnh viễn.\nVui lòng nhập XÓA để xác nhận.";
            lblDeleteSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtDeleteConfirm
            // 
            txtDeleteConfirm.BorderStyle = BorderStyle.FixedSingle;
            txtDeleteConfirm.Font = new Font("Segoe UI", 12F);
            txtDeleteConfirm.Location = new Point(24, 142);
            txtDeleteConfirm.Name = "txtDeleteConfirm";
            txtDeleteConfirm.PlaceholderText = "Nhập XÓA để xác nhận";
            txtDeleteConfirm.Size = new Size(400, 34);
            txtDeleteConfirm.TabIndex = 3;
            txtDeleteConfirm.TextAlign = HorizontalAlignment.Center;
            // 
            // btnCancelDelete
            // 
            btnCancelDelete.BackColor = Color.White;
            btnCancelDelete.Cursor = Cursors.Hand;
            btnCancelDelete.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 245);
            btnCancelDelete.FlatStyle = FlatStyle.Flat;
            btnCancelDelete.Font = new Font("Segoe UI", 10F);
            btnCancelDelete.ForeColor = Color.FromArgb(90, 107, 140);
            btnCancelDelete.Location = new Point(110, 190);
            btnCancelDelete.Name = "btnCancelDelete";
            btnCancelDelete.Size = new Size(100, 38);
            btnCancelDelete.TabIndex = 4;
            btnCancelDelete.Text = "Hủy";
            btnCancelDelete.UseVisualStyleBackColor = false;
            // 
            // btnConfirmDelete
            // 
            btnConfirmDelete.BackColor = Color.FromArgb(224, 85, 85);
            btnConfirmDelete.Cursor = Cursors.Hand;
            btnConfirmDelete.Enabled = false;
            btnConfirmDelete.FlatAppearance.BorderSize = 0;
            btnConfirmDelete.FlatStyle = FlatStyle.Flat;
            btnConfirmDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConfirmDelete.ForeColor = Color.White;
            btnConfirmDelete.Location = new Point(220, 190);
            btnConfirmDelete.Name = "btnConfirmDelete";
            btnConfirmDelete.Size = new Size(160, 38);
            btnConfirmDelete.TabIndex = 5;
            btnConfirmDelete.Text = "🗑️ Xóa vĩnh viễn";
            btnConfirmDelete.UseVisualStyleBackColor = false;
            // 
            // frm_TaiKhoanBaoMat
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 244, 252);
            ClientSize = new Size(718, 700);
            Controls.Add(pnlContentWrapper);
            Controls.Add(pnlTopbar);
            Controls.Add(pnlDeleteOverlay);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frm_TaiKhoanBaoMat";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Study Tracker - Tài khoản & Bảo mật";
            pnlTopbar.ResumeLayout(false);
            pnlTopbar.PerformLayout();
            pnlContentWrapper.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            grpProfile.ResumeLayout(false);
            grpProfile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbAvatar).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGpaMucTieu).EndInit();
            grpPassword.ResumeLayout(false);
            grpPassword.PerformLayout();
            pnlDangerZone.ResumeLayout(false);
            pnlDangerZone.PerformLayout();
            pnlDeleteOverlay.ResumeLayout(false);
            pnlDeleteConfirm.ResumeLayout(false);
            pnlDeleteConfirm.PerformLayout();
            ResumeLayout(false);
        }

        // Helper method to create sidebar buttons
        private void CreateSidebarButton(System.Windows.Forms.Button btn, string text, int x, int y, bool isActive = false)
        {
            btn.Text = text;
            btn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new System.Drawing.Size(172, 32);
            btn.Location = new System.Drawing.Point(x, y);
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.Cursor = System.Windows.Forms.Cursors.Hand;

            if (isActive)
            {
                btn.BackColor = System.Drawing.Color.FromArgb(232, 239, 255);
                btn.ForeColor = System.Drawing.Color.FromArgb(74, 107, 191);
                btn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            }
            else
            {
                btn.BackColor = System.Drawing.Color.Transparent;
                btn.ForeColor = System.Drawing.Color.FromArgb(90, 107, 140);
            }

            btn.UseVisualStyleBackColor = false;
        }

        #endregion

        // Control declarations
        private System.Windows.Forms.Panel pnlTopbar;
        private System.Windows.Forms.Label lblLogoIcon;
        private System.Windows.Forms.Label lblLogoTitle;
        private System.Windows.Forms.Label lblLogoSubtitle;
        private System.Windows.Forms.Button btnBackToDashboard;

        private System.Windows.Forms.Panel pnlContentWrapper;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblPageHeader;

        // Profile
        private System.Windows.Forms.GroupBox grpProfile;
        private System.Windows.Forms.PictureBox pbAvatar;
        private System.Windows.Forms.Label lblAvatarInitial;
        private System.Windows.Forms.Button btnUploadAvatar;
        private System.Windows.Forms.Label lblAvatarHint;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblGpaMucTieu;
        private System.Windows.Forms.NumericUpDown nudGpaMucTieu;
        private System.Windows.Forms.Label lblNgayTao;
        private System.Windows.Forms.TextBox txtNgayTao;
        private System.Windows.Forms.Button btnSaveProfile;

        // Password
        private System.Windows.Forms.GroupBox grpPassword;
        private System.Windows.Forms.Label lblCurrentPassword;
        private System.Windows.Forms.TextBox txtCurrentPassword;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.ProgressBar pgbPasswordStrength;
        private System.Windows.Forms.Label lblPasswordHint;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnChangePassword;

        // Danger Zone
        private System.Windows.Forms.Panel pnlDangerZone;
        private System.Windows.Forms.Label lblDangerTitle;
        private System.Windows.Forms.Label lblDangerDesc;
        private System.Windows.Forms.Button btnDeleteAccount;

        // Delete Confirmation
        private System.Windows.Forms.Panel pnlDeleteOverlay;
        private System.Windows.Forms.Panel pnlDeleteConfirm;
        private System.Windows.Forms.Label lblDeleteIcon;
        private System.Windows.Forms.Label lblDeleteTitle;
        private System.Windows.Forms.Label lblDeleteSubtitle;
        private System.Windows.Forms.TextBox txtDeleteConfirm;
        private System.Windows.Forms.Button btnCancelDelete;
        private System.Windows.Forms.Button btnConfirmDelete;

        private System.Windows.Forms.OpenFileDialog openFileDialogAvatar;
    }
}