using BGAnim;
using Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Horizon
{
    public partial class Form1 : Form
    {
        BGAnimPBA BG;
        Timer timerRender = new Timer() { Interval = 10, Enabled = true };
        Timer timerUpdate = new Timer() { Interval = 10, Enabled = true };

        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BG = new BGAnimPBA($"{Directory.GetCurrentDirectory()}\\test.pba");

            timerUpdate.Tick += Update;
            timerRender.Tick += RenderDraw;
        }

        private void RenderDraw(object sender, EventArgs e)
        {
            Bitmap result = new Bitmap(Render.Width, Render.Height);

            using (Graphics g = Graphics.FromImage(result))
            {
                //g.DrawImage(BG.Animate(), Point.Empty);
                g.FillRectangle(Brushes.Black, 0, 0, Render.Width, Render.Height);
                MapStateMachine.Instance.Render(g);
            }

            Render.Image = result;
        }

        private void Update(object sender, EventArgs e)
        {
            MapStateMachine.Instance.Update();
        }
    }
}
