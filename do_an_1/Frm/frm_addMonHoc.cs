using System;
using System.Drawing;
using System.Windows.Forms;
using do_an_1.DAO;
using do_an_1.DTO;

namespace do_an_1.Frm
{
    public partial class frm_addMonHoc : Form
    {
        // ======== PROPERTIES & FIELDS ========
        private int _maNguoiDung; // Dùng để lưu mã người dùng truyền từ form chính

        public bool IsEditMode { get; set; } = false;
        public string OriginalMaMonHoc { get; set; } = string.Empty;
        public MonHocResult Result { get; private set; } = null;

        // ======== CONSTRUCTORS ========

        // 1. Constructor mặc định (bắt buộc phải có InitializeComponent)
        // lấy dữ liệu từ form chính để truyền vào constructor này, nếu không sẽ bị lỗi thiếu tham số

        public frm_addMonHoc() //truyền tham số từ form chính vào constructor này, nếu không sẽ bị lỗi thiếu tham số
        {
            InitializeComponent();
            SetupForm();
        }

        // 2. Constructor cho chế độ THÊM MỚI (Khắc phục lỗi int -> bool)
        public frm_addMonHoc(int maND) : this()
        {
            this._maNguoiDung = maND;
            this.IsEditMode = false;
            SetupMode();
        }

        // 3. Constructor cho chế độ SỬA (Khắc phục lỗi không tìm thấy constructor 6 tham số)
        public frm_addMonHoc(int maND, string maMonHoc, string tenMonHoc, int soTinChi,
                             string tenGiangVien, string hinhThucThi) : this()
        {
            this._maNguoiDung = maND;
            this.IsEditMode = true;
            this.OriginalMaMonHoc = maMonHoc;

            // Điền dữ liệu cũ vào form
            txtMaMonHoc.Text = maMonHoc;
            txtTenMonHoc.Text = tenMonHoc;
            numSoTinChi.Text = soTinChi.ToString();
            txtGiangVien.Text = tenGiangVien;

            // Khóa mã môn học khi sửa (thường không cho đổi mã khóa chính)
            txtMaMonHoc.Enabled = false;

            // Set selected item cho ComboBox
            int index = cboHinhThucThi.FindStringExact(hinhThucThi);
            if (index >= 0) cboHinhThucThi.SelectedIndex = index;

            SetupMode();
        }

        // ======== SETUP GIAO DIỆN ========
        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void SetupMode()
        {
            if (IsEditMode)
            {
                this.Text = "✏️ Sửa thông tin môn học";
                lblTitle.Text = "Sửa thông tin môn học";
                lblSubtitle.Text = "Cập nhật thông tin chi tiết cho môn học";
                btnLuu.Text = "💾 Cập nhật môn học";
                txtMaMonHoc.Enabled = false;
            }
            else
            {
                this.Text = "➕ Thêm môn học mới";
                lblTitle.Text = "Thêm môn học mới";
                lblSubtitle.Text = "Nhập thông tin chi tiết cho môn học";
                btnLuu.Text = "💾 Lưu môn học";
                txtMaMonHoc.Enabled = true;
            }
        }

        // ======== EVENTS ========
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // --- 1. Validate dữ liệu đầu vào ---
            if (string.IsNullOrWhiteSpace(txtMaMonHoc.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã môn học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaMonHoc.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenMonHoc.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên môn học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenMonHoc.Focus();
                return;
            }

            if (!int.TryParse(numSoTinChi.Text, out int soTinChi) || soTinChi < 1 || soTinChi > 10)
            {
                MessageBox.Show("Số tín chỉ phải từ 1 đến 10!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numSoTinChi.Focus();
                return;
            }

            if (cboHinhThucThi.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Hình thức thi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string selectedHT = cboHinhThucThi.Text;
            string hinhThucSQL = "KHÁC"; // Giá trị mặc định

            // Chuyển đổi sang định dạng SQL chấp nhận (CHECK constraint)
            if (selectedHT.Contains("Tự luận")) hinhThucSQL = "TỰ_LUẬN";
            else if (selectedHT.Contains("Trắc nghiệm")) hinhThucSQL = "TRẮC_NGHIỆM";
            else if (selectedHT.Contains("Bài tập lớn")) hinhThucSQL = "BÀI_TẬP_LỚN";
            // --- 2. Đóng gói dữ liệu vào DTO ---
            MonHocDTO mon = new MonHocDTO
            {

                MaNguoiDung = this._maNguoiDung,
                MaMonHoc = txtMaMonHoc.Text.Trim(),
                TenMonHoc = txtTenMonHoc.Text.Trim(),
                SoTinChi = soTinChi,
                TenGiangVien = txtGiangVien.Text.Trim(),
                HinhThucThi = hinhThucSQL,
                //lấy học kì dưới dạng yyyy.1 hoặc yyyy.2 dựa vào tháng hiện tại nếu tháng 1-6 thì là học kì 2 của năm trước, nếu tháng 7-12 thì là học kì 1 của năm hiện tại
                HocKy = DateTime.Now.Month >= 7 ? $"{DateTime.Now.Year}.1" : $"{DateTime.Now.Year - 1}.2"
            };

            // --- 3. Gọi DAO để lưu vào CSDL ---
            MonHocDAO dao = new MonHocDAO();
            bool isSuccess = false;

            try
            {
                if (IsEditMode)
                    isSuccess = dao.UpdateMonHoc(mon);
                else
                    isSuccess = dao.InsertMonHoc(mon);

                if (isSuccess)
                {
                    MessageBox.Show(IsEditMode ? "Cập nhật thành công!" : "Thêm mới thành công!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK; // Đánh dấu thành công để Form chính load lại
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lưu thất bại. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void numSoTinChi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}