using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace do_an_1.Frm
{
    public partial class loginTextbox : UserControl
    {
        public loginTextbox()
        {
            InitializeComponent();
        }

        public string _label = "default value";
        public string Label
        {
            get { return _label; }
            set { _label = value; label1.Text = value; }
        }

        public bool isPassword = false;

        public bool isPassword1
        {
            get { return isPassword; }
            set { isPassword = value; textBox1.UseSystemPasswordChar = value; }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public string TextValue
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        // Property để clear text
        public void ClearText()
        {
            textBox1.Clear();
        }

        // Property để focus
        public void FocusText()
        {
            textBox1.Focus();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void loginTextbox_Paint(object sender, PaintEventArgs e)
        {
            // Cập nhật text của label1 mỗi khi vẽ lại control
            label1.Text = _label;

            // Nếu isPassword là true, hiển thị dấu * thay vì ký tự thực   
            if (isPassword)
            {
                textBox1.UseSystemPasswordChar = true;
            }
            else
            {
                textBox1.UseSystemPasswordChar = false;
            }
        }
    }
}
