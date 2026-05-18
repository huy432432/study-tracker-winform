using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.Frm
{
    // Tạo Label có hiệu ứng mờ
    public class FadeLabel : Label
    {
        public FadeLabel()
        {
            this.ForeColor = Color.FromArgb(100, 100, 100);
            this.Font = new Font("Segoe UI", 8, FontStyle.Italic);
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Vẽ bóng mờ nhẹ
            using (var shadow = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
            {
                e.Graphics.DrawString(this.Text, this.Font, shadow, 1, 1);
            }
            base.OnPaint(e);
        }
    }
}
