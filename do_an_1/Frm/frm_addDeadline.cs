using do_an_1.DAO;
using System;
using System.Data;
using System.Windows.Forms;

namespace do_an_1.Frm
{
    public partial class frm_addDeadline : Form
    {
        private int maNguoiDung;
        private DeadlineDAO deadlineDAO = new DeadlineDAO();

        // Delegate để báo hiệu cho Form cha (frm_deadline) load lại dữ liệu
        public Action OnSaveSuccess { get; set; }

        public frm_addDeadline(int maND)
        {
            InitializeComponent();
            this.maNguoiDung = maND;

            // Đăng ký sự kiện
            this.Load += frm_addDeadline_Load;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += (s, e) => this.Close();
        }

        private void frm_addDeadline_Load(object sender, EventArgs e)
        {
            dtp_thoiHanDeadline.BackColor = Color.White;
            dtp_thoiHanDeadline.FillColor = Color.White;
            LoadComboBoxMonHoc();
        }

        private void LoadComboBoxMonHoc()
        {
            DataTable dt = deadlineDAO.GetDanhSachMonHoc(maNguoiDung);
            if (dt != null && dt.Rows.Count > 0)
            {
                cbo_MonHoc.DataSource = dt;
                cbo_MonHoc.DisplayMember = "DISPLAY_NAME";
                cbo_MonHoc.ValueMember = "MA_MON_HOC";
            }
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu đầu vào cơ bản
            if (string.IsNullOrWhiteSpace(txtTieuDe.Text))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Kết hợp ngày từ DTP và giờ/phút từ TextBox
            DateTime ngayChon = dtp_thoiHanDeadline.Value;
            int gio = 0, phut = 0;

            // Kiểm tra tính hợp lệ của giờ (0-23) và phút (0-59) để tránh lỗi khởi tạo DateTime
            if (!int.TryParse(txt_gioDeadline.Text, out gio) || gio < 0 || gio > 23 ||
                !int.TryParse(txt_phutDeadline.Text, out phut) || phut < 0 || phut > 59)
            {
                MessageBox.Show("Giờ hoặc phút không hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime thoiHanCuoi = new DateTime(ngayChon.Year, ngayChon.Month, ngayChon.Day, gio, phut, 0);

            // --- BỔ SUNG: KIỂM TRA THỜI GIAN HIỆN TẠI ---
            if (thoiHanCuoi <= DateTime.Now)
            {
                MessageBox.Show("Thời hạn deadline phải lớn hơn thời điểm hiện tại!", "Lỗi thời gian", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return; // Dừng lại không gọi DAO nữa
            }
            // ------------------------------------------

            // 3. Lấy mã môn học (có thể null nếu chọn "-- Không có môn --")
            object maMonHoc = cbo_MonHoc.SelectedValue;
            if (maMonHoc != null && (maMonHoc.ToString() == "" || maMonHoc == DBNull.Value))
                maMonHoc = null;

            // 4. Gọi DAO để thêm mới
            try
            {
                bool success = deadlineDAO.AddDeadline(
                    txtTieuDe.Text,
                    maMonHoc,
                    thoiHanCuoi,
                    maNguoiDung
                );

                if (success)
                {
                    MessageBox.Show("Thêm deadline thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Báo cho Form cha biết để load lại dữ liệu
                    OnSaveSuccess?.Invoke();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại, vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Bắt các lỗi từ SQL hoặc lỗi throw từ DAO (nếu có)
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}