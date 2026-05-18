// Frm/frmpomodoro.cs
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using do_an_1.DAO;
using do_an_1.DTO;

namespace do_an_1.Frm
{
    public partial class frmpomodoro : Form
    {
        // Timer variables
        private int secondsRemaining;
        private bool isRunning = false;
        private bool isWorkSession = true;
        private int workMinutes = 25;
        private int breakMinutes = 5;

        // Session tracking
        private DateTime sessionStartTime;
        private int currentUserId;
        private string currentSubjectId;
        private int? currentTaskId;

        // DAO
        private PomodoroDAO pomodoroDAO;

        public frmpomodoro(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
            pomodoroDAO = new PomodoroDAO();
            InitializeForm();
            LoadUserData();
            LoadStatistics();
        }

        private void InitializeForm()
        {
            cboThoiGian.SelectedIndex = 0;

            secondsRemaining = workMinutes * 60; // THÊM DÒNG NÀY
            UpdateTimerDisplay(secondsRemaining);

            // Wire up events
            btnStart.Click += btnStart_Click;
            btnReset.Click += btnReset_Click;
            pomodoroTimer.Tick += PomodoroTimer_Tick;
            cboThoiGian.SelectedIndexChanged += cboThoiGian_SelectedIndexChanged;
            cboMonHoc.SelectedIndexChanged += cboMonHoc_SelectedIndexChanged;
            cboNhiemVu.SelectedIndexChanged += cboNhiemVu_SelectedIndexChanged;
        }

        private async void LoadUserData()
        {
            try
            {
                // Load subjects
                var subjects = await System.Threading.Tasks.Task.Run(() =>
                    pomodoroDAO.GetSubjectsByUser(currentUserId));

                cboMonHoc.Items.Clear();
                cboMonHoc.Items.Add("-- Chọn môn học --");

                foreach (DataRow row in subjects.Rows)
                {
                    cboMonHoc.Items.Add(row["TEN_MON_HOC"].ToString());
                }
                cboMonHoc.SelectedIndex = 0;

                // Load tasks
                LoadTasks();

                LogMessage("Đã tải dữ liệu thành công");
            }
            catch (Exception ex)
            {
                LogMessage($"Lỗi tải dữ liệu: {ex.Message}", true);
                MessageBox.Show($"Không thể tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTasks(string subjectId = null)
        {
            try
            {
                var tasks = pomodoroDAO.GetTasksBySubject(currentUserId, subjectId, false);

                cboNhiemVu.Items.Clear();
                cboNhiemVu.Items.Add("-- Học tự do / Nghiên cứu (không gán task) --");

                foreach (var task in tasks)
                {
                    string taskDisplay = task.IsOverdue ?
                        $"⚠️ {task.TieuDe} (QUÁ HẠN)" :
                        $"{task.TieuDe} (Còn {task.DaysLeft} ngày)";
                    cboNhiemVu.Items.Add(taskDisplay);

                    // Store task ID in Tag (có thể dùng Dictionary)
                }
                cboNhiemVu.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogMessage($"Lỗi tải nhiệm vụ: {ex.Message}", true);
            }
        }

        private void LoadStatistics()
        {
            try
            {
                var stats = pomodoroDAO.GetStudyStatistics(currentUserId);

                if (stats.TotalSessions == 0)
                {
                    LogMessage("Chào mừng! Hãy bắt đầu phiên học đầu tiên của bạn");
                }
                else
                {
                    LogMessage($"📊 Thống kê: {stats.TotalSessions} phiên, {stats.TotalMinutes} phút, chuỗi {stats.CurrentStreakDays} ngày");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Lỗi tải thống kê: {ex.Message}", true);
            }
        }

        private void cboMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isRunning && cboMonHoc.SelectedIndex > 0)
            {
                var subjects = pomodoroDAO.GetSubjectsByUser(currentUserId);
                var selectedSubject = cboMonHoc.SelectedItem.ToString();
                var subject = subjects.AsEnumerable()
                    .FirstOrDefault(r => r["TEN_MON_HOC"].ToString() == selectedSubject);

                if (subject != null)
                {
                    currentSubjectId = subject["MA_MON_HOC"].ToString();
                    LoadTasks(currentSubjectId);
                }
            }
            else if (cboMonHoc.SelectedIndex == 0)
            {
                currentSubjectId = null;
                LoadTasks();
            }
        }

        private void cboNhiemVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isRunning && cboNhiemVu.SelectedIndex > 0)
            {
                var selectedTaskText = cboNhiemVu.SelectedItem.ToString();
                // Parse task ID from display text (simplified)
                var tasks = pomodoroDAO.GetTasksBySubject(currentUserId, currentSubjectId, false);
                var matchedTask = tasks.FirstOrDefault(t => selectedTaskText.Contains(t.TieuDe));
                currentTaskId = matchedTask?.MaNhiemVu;
            }
            else
            {
                currentTaskId = null;
            }
        }

        private void cboThoiGian_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                switch (cboThoiGian.SelectedIndex)
                {
                    case 0: // 25/5
                        workMinutes = 25;
                        breakMinutes = 5;
                        secondsRemaining = workMinutes * 60;
                        break;

                    case 1: // 50/10
                        workMinutes = 50;
                        breakMinutes = 10;
                        secondsRemaining = workMinutes * 60;
                        break;

                    case 2: // TEST NHANH 5 GIÂY
                        workMinutes = 1;
                        breakMinutes = 1;
                        secondsRemaining = 5;
                        break;
                }

                UpdateTimerDisplay(secondsRemaining);
                isWorkSession = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cboMonHoc.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn môn học!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isRunning)
            {
                var warning = pomodoroDAO.CheckHealthWarning(currentUserId);

                if (warning.ShouldWarn)
                {
                    var result = MessageBox.Show($"{warning.WarningMessage}\n\nTiếp tục?",
                        "Cảnh báo sức khỏe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                        return;
                }

                StartSession();
            }
            else
            {
                PauseSession();
            }
        }

        private void StartSession()
        {
            isRunning = true;
            sessionStartTime = DateTime.Now;
            btnStart.Text = "⏸ Tạm Dừng";
            btnStart.BackColor = Color.FromArgb(245, 158, 11);
            pomodoroTimer.Start();

            LogMessage($"Bắt đầu phiên {(isWorkSession ? "học tập" : "nghỉ giải lao")} - Môn: {cboMonHoc.SelectedItem}");
        }

        private void PauseSession()
        {
            isRunning = false;
            btnStart.Text = "▶ Tiếp Tục";
            btnStart.BackColor = Color.FromArgb(16, 185, 129);
            pomodoroTimer.Stop();
            LogMessage("Tạm dừng phiên học");
        }

        private void PomodoroTimer_Tick(object sender, EventArgs e)
        {
            if (secondsRemaining > 0)
            {
                secondsRemaining--;
                UpdateTimerDisplay(secondsRemaining);
            }

            if (secondsRemaining == 0)
            {
                pomodoroTimer.Stop();
                CompleteSession();
            }
        }

        private void CompleteSession()
        {
            isRunning = false;

            if (isWorkSession)
            {
                var endTime = DateTime.Now;
                int actualMinutes = (int)(endTime - sessionStartTime).TotalMinutes;

                var session = new PomodoroSessionDTO
                {
                    MaNguoiDung = currentUserId,
                    MaMonHoc = currentSubjectId,
                    ThoiGianBatDau = sessionStartTime,
                    ThoiGianKetThuc = endTime,
                    ThoiLuongPhut = actualMinutes,
                    MaNhiemVu = currentTaskId
                };

                try
                {
                    pomodoroDAO.SaveStudySession(session);
                    LogMessage($"✅ Đã lưu phiên học: {actualMinutes} phút");

                    if (currentTaskId.HasValue)
                    {
                        LogMessage($"🎉 Hoàn thành nhiệm vụ!");
                        LoadTasks(currentSubjectId);
                        currentTaskId = null;
                    }

                    LoadStatistics();
                }
                catch (Exception ex)
                {
                    LogMessage($"❌ Lỗi lưu phiên học: {ex.Message}", true);
                }
            }

            isWorkSession = !isWorkSession;

            MessageBox.Show(isWorkSession ?
                "🎉 Hoàn thành nghỉ giải lao! Bắt đầu phiên học mới!" :
                "🎉 Hoàn thành phiên học! Nghỉ giải lao nào!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (cboThoiGian.SelectedIndex == 2)
            {
                secondsRemaining = 5;
            }
            else
            {
                int newMinutes = isWorkSession ? workMinutes : breakMinutes;
                secondsRemaining = newMinutes * 60;
            }
            UpdateTimerDisplay(secondsRemaining);

            btnStart.Text = "▶ Bắt Đầu Học";
            btnStart.BackColor = Color.FromArgb(16, 185, 129);

            System.Media.SystemSounds.Beep.Play();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                var result = MessageBox.Show("Hủy phiên học hiện tại?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
            }

            pomodoroTimer.Stop();
            isRunning = false;
            isWorkSession = true;

            switch (cboThoiGian.SelectedIndex)
            {
                case 0:
                    workMinutes = 25;
                    breakMinutes = 5;
                    break;

                case 1:
                    workMinutes = 50;
                    breakMinutes = 10;
                    break;

                case 2:
                    workMinutes = 1;
                    breakMinutes = 1;
                    break;
            }
            if (cboThoiGian.SelectedIndex == 2)
            {
                secondsRemaining = 5;
            }
            else
            {
                secondsRemaining = workMinutes * 60;
            }
            UpdateTimerDisplay(secondsRemaining);

            btnStart.Text = "▶ Bắt Đầu Học";
            btnStart.BackColor = Color.FromArgb(16, 185, 129);

            LogMessage("Đã đặt lại phiên học");
        }

        private void UpdateTimerDisplay(int seconds)
        {
            int minutes = seconds / 60;
            int secs = seconds % 60;
            lblTimerDisplay.Text = $"{minutes:D2}:{secs:D2}";

            lblTimerDisplay.ForeColor = seconds <= 60 ? Color.FromArgb(239, 68, 68) :
                                      seconds <= 300 ? Color.FromArgb(245, 158, 11) :
                                      Color.FromArgb(248, 250, 252);
        }

        private void LogMessage(string message, bool isError = false)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            lblLogMessage.ForeColor = isError ? Color.FromArgb(239, 68, 68) : Color.FromArgb(148, 163, 184);
            lblLogMessage.Text = $"{(isError ? "⚠" : "⏳")} [{timestamp}] {message}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isRunning)
            {
                var result = MessageBox.Show("Phiên học đang diễn ra. Thoát?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            base.OnFormClosing(e);
        }
    }
}