using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script
{
    public partial class RichTextBoxExtended : RichTextBox
    {
        public class DrawFieldEventArgs : EventArgs
        {
            public Graphics graphics;

            public DrawFieldEventArgs(Graphics _graphics)
            {
                graphics = _graphics;
            }
        }
        public delegate void DrawFieldEventHandler(object sender, DrawFieldEventArgs e);
        [Browsable(true)]
        public event DrawFieldEventHandler OnDraw;

        private const int WM_PAINT = 15;

        public RichTextBoxExtended()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT)
            {
                this.Invalidate();
                base.WndProc(ref m);
                using (Graphics g = Graphics.FromHwnd(this.Handle))
                    OnDraw?.Invoke(this, new DrawFieldEventArgs(g));

            }
            base.WndProc(ref m);
        }

    }
}
