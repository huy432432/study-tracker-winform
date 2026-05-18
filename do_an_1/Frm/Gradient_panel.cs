using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace do_an_1.Frm
{
    public class Gradient_panel : Panel
    {
        public Color gradientTop { get; set; }
        public Color gradientBottom { get; set; }

        //create constructor for gradient panel
        public Gradient_panel()
        {
            this.Resize += Gradient_panel_Resize;
        }

        private void Gradient_panel_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            
            LinearGradientBrush linear = new LinearGradientBrush(
                this.ClientRectangle,
                this.gradientTop,
                this.gradientBottom,
                90F
             );

            Graphics g = e.Graphics;
            g.FillRectangle(linear, this.ClientRectangle);

            base.OnPaint(e);
        }

    }
}
