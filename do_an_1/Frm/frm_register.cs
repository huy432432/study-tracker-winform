using System;
using System.Drawing;
using System.Windows.Forms;
using do_an_1.DAO;
using do_an_1.DTO;
using do_an_1.Utils;

namespace do_an_1.Frm
{
    public partial class frm_register : Form
    {
        #region [1] Khai báo biến toàn cục
        private NguoiDungDAO nguoiDungDAO;
        #endregion

        #region [2] Khởi tạo & Load Form
        public frm_register()
        {
            InitializeComponent();
            nguoiDungDAO = new NguoiDungDAO();
        }
        #endregion

        #region [3] Phương thức Load dữ liệu
        #endregion

        #region [4] Phương thức xử lý nghiệp vụ
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txt_fullName.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ tên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_fullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_emailAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập Email!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_emailAddress.Focus();
                return false;
            }

            if (!txt_emailAddress.Text.Contains("@") || !txt_emailAddress.Text.Contains("."))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_emailAddress.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_useName.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_useName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_password.Text))
            {
                MessageBox.Show("Vui lòng nhập Mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_password.Focus();
                return false;
            }

            if (txt_password.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_password.Focus();
                return false;
            }

            if (txt_password.Text != txt_rePassword.Text)
            {
                MessageBox.Show("Xác nhận mật khẩu không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_rePassword.Focus();
                return false;
            }

            return true;
        }
        #endregion

        #region [5] Hành động (sự kiện click)
        private void btn_signUp_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            if (nguoiDungDAO.KiemTraEmailTonTai(txt_emailAddress.Text.Trim()))
            {
                MessageBox.Show("Email này đã được đăng ký!", "Email tồn tại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NguoiDungDTO nd = new NguoiDungDTO
            {
                HoTen = txt_fullName.Text.Trim(),
                Email = txt_emailAddress.Text.Trim(),
                MatKhauMaHoa = PasswordHelper.MaHoaMatKhau(txt_password.Text),
                GpaMucTieu = 3.60m
            };

            bool ketQua = nguoiDungDAO.DangKy(nd);

            if (ketQua)
            {
                MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new frm_login().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Đăng ký thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbl_signInButton_Click(object sender, EventArgs e)
        {
            new frm_login().Show();
            this.Close();
        }
        #endregion

        #region [6] Tiện ích giao diện
        #endregion
    }
}