using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using do_an_1.DAO;
using do_an_1.DTO;
using do_an_1.Frm;
using do_an_1.Utils;

namespace do_an_1
{

    public partial class frm_login : Form
    {

        #region [1] Khai báo biến
        private DAO.NguoiDungDAO nguoiDungDAO;
        #endregion

        public frm_login()
        {
            InitializeComponent();
            nguoiDungDAO = new NguoiDungDAO();

        }

        private void btn_signIn_Click(object sender, EventArgs e)
        {
            string email = txt_loginEmail.Text.Trim();
            string matKhau = txt_loginPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau))
            {
                HienThiLoi("Vui lòng nhập đầy đủ email và mật khẩu.");
                return;
            }

            string matKhauMaHoa = PasswordHelper.MaHoaMatKhau(matKhau);
            NguoiDungDTO nd = nguoiDungDAO.DangNhap(email, matKhauMaHoa);

            if (nd != null)
            {


                MessageBox.Show($"Đăng nhập thành công! Chào mừng {nd.HoTen}.",
                                "Thành công",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                frm_Dashboard mainForm = new frm_Dashboard(nd.MaNguoiDung);
                mainForm.ShowDialog();
                this.Hide();
            }
            else
            {
                HienThiLoi("Email hoặc mật khẩu không đúng.");
                // Xóa mật khẩu
                txt_loginPassword.Clear();
                txt_loginPassword.Focus(); // Đặt con trỏ vào ô mật khẩu
            }
        }
        private void HienThiLoi(string message)
        {
            MessageBox.Show(message, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void lbl_signUp_Click(object sender, EventArgs e)
        {
            frm_register register = new frm_register();

            register.ShowDialog();

        }

        private void lbl_forgotPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ quản trị viên để đặt lại mật khẩu.",
                  "Quên mật khẩu",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information);
        }

        private void txt_loginPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void frm_login_Load(object sender, EventArgs e)
        {

        }
    }
}
