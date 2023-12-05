using Cast.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cast
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };
        public Form1()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            Core.RW = Render.Width;
            Core.RH = Render.Height;
            Core.Image = new Bitmap(Core.iRW, Core.iRH);
            Core.g = Graphics.FromImage(Core.Image);
        }

        private void Update(object _, EventArgs e)
        {
            Core.MS = PointToClient(MousePosition);

            Core.Cam.Update();
            Core.Map.Update();

            KB.Update();

            Core.Ticks++;
        }
        private void Draw(object _, EventArgs e)
        {
            Core.g.Clear(Color.Black);
            Core.Map.prepare_minimap();
            Core.Map.Draw(Core.g);
            Core.Map.draw_minimap(Core.g);
            DrawUI(Core.g);

            Render.Image = Core.Image;
        }
        private void DrawUI(Graphics g)
        {
            Core.Map.DrawUI(Core.g);
            Core.Cam.DrawHUD();
        }
    }
}
