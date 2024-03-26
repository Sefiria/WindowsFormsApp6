using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolActivityMaker
{
    public class TimeControl : Control
    {
        NumericUpDown Hours, Minutes;

        public TimeControl()
        {
            Initialize();
            Hours.Value = 0;
            Minutes.Value = 0;
        }
        public TimeControl(TimeSpan time)
        {
            Initialize();
            Hours.Value = time.Hours;
            Minutes.Value = time.Minutes;
        }
        public void Initialize()
        {
            Hours = new NumericUpDown();
            Minutes = new NumericUpDown();
            Hours.Controls[0].Enabled = false;
            Minutes.Controls[0].Enabled = false;
            Hours.Location = Point.Empty;
            Hours.Size = new Size(32, 22);
            Hours.Location = new Point(Hours.Size.Width + 5 + 8 + 5, 0);
            Hours.Size = new Size(32, 22);
            Minutes.Location = Point.Empty;
            Hours.CreateGraphics();
            Minutes.CreateGraphics();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawString(":", DefaultFont, Brushes.Black, Hours.Size.Width + 5, 0);
        }
    }
}
