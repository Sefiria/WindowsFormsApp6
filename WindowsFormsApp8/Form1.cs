using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image, ImageUI;

        public Form1()
        {
            InitializeComponent();

            Core.W = Render.Width;
            Core.H = Render.Height;
            Image = new Bitmap(Core.W, Core.H);
            Core.g = Graphics.FromImage(Image);
            ImageUI = new Bitmap(Core.W, Core.H);
            ImageUI.MakeTransparent(Color.White);

            RenderClass.Initialize();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Core.IsMouseDown = true;
            RenderClass.MouseDown();
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            Core.IsMouseDown = false;
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            Core.IsMouseDown = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            RenderClass.MouseMove();
            Core.MousePosition = e.Location;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Update(object sender, EventArgs e)
        {
            RenderClass.Update();
        }
        private void Draw(object sender, EventArgs e)
        {
            Bitmap renderImage = new Bitmap(Core.W, Core.H);
            using (Graphics g = Graphics.FromImage(renderImage))
            {
                using (Core.gui = Graphics.FromImage(ImageUI))
                {
                    RenderClass.Draw();

                    g.DrawImage(Image, 0, 0);
                    g.DrawImage(ImageUI, 0, 0);

                    Core.gui.Clear(Color.White);
                }
                ImageUI.MakeTransparent(Color.White);
            }

            Render.Image = renderImage;
        }
    }
}
