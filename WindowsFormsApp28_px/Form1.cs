using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp28_px
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };
        Bitmap RenderImage;
        Graphics g;

        public Form1()
        {
            InitializeComponent();

            MouseStates.Initialize(Render);
            KB.Init();

            Common.Initialize(Render.Width, Render.Height);

            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);

            Cursor.Hide();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            Common.Update();
            ParticlesManager.Bounds = new Rectangle((int)Common.Controllable.X - Render.Width / 2, (int)Common.Controllable.Y - Render.Height / 2, (int)Common.Controllable.X + Render.Width / 2, (int)Common.Controllable.Y + Render.Height / 2);
            KB.Update();
            MouseStates.Update();
            ParticlesManager.Update();
        }

        private void Draw(object sender, EventArgs e)
        {
            g.Clear(Color.Black);

            Common.Draw(g);
            ParticlesManager.Draw(g, Common.Cam);
            Common.DrawUI(g);

            Render.Image = RenderImage;
        }
    }
}
