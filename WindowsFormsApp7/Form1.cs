using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        Bitmap ImagePal;
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 50 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        bool RenderMouseDown = false, RenderPalMouseDown = false;

        public Form1()
        {
            InitializeComponent();

            Core.MainForm = this;
            Core.TileSz = 1;
            Core.Zoom = 24;
            Core.RW = 16;
            Core.RH = 16;

            Core.Image = new Bitmap(Render.Width, Render.Height);
            Core.g = Graphics.FromImage(Core.Image);

            Core.ImageUI = new Bitmap(Render.Width, Render.Height).Transparent();
            Core.gui = Graphics.FromImage(Core.ImageUI);

            ImagePal = new Bitmap(RenderPal.Width, RenderPal.Height);
            Core.gp = Graphics.FromImage(ImagePal);
            Core.RPW = ImagePal.Width;
            Core.RPH = ImagePal.Height;

            Core.ExactRenderW = Render.Width;
            Core.ExactRenderH = Render.Height;

            RenderClass.Initialize();
            RenderPalClass.Initialize();

            TimerUpdate.Tick += Update; ;
            TimerDraw.Tick += Draw;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                RenderMouseDown = true;
            else if (e.Button == MouseButtons.Right)
                RenderClass.MouseRightDown = true;
            Core.MousePosition = e.Location;
            RenderClass.MouseDown(e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            RenderMouseDown = false;
            RenderClass.MouseRightDown = false;
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            RenderMouseDown = false;
            RenderClass.MouseRightDown = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            if (RenderMouseDown || RenderClass.MouseRightDown)
                RenderClass.MouseMove(e);
            Core.MousePosition = e.Location;
        }
        private void RenderPal_MouseDown(object sender, MouseEventArgs e)
        {
            Core.MousePosition = e.Location;
            RenderPalClass.MouseDown(e);
            RenderPalMouseDown = true;
        }
        private void RenderPal_MouseUp(object sender, MouseEventArgs e)
        {
            RenderPalMouseDown = false;
        }
        private void RenderPal_MouseLeave(object sender, EventArgs e)
        {
            RenderPalMouseDown = false;
        }
        private void RenderPal_MouseMove(object sender, MouseEventArgs e)
        {
            RenderPalClass.MouseMove(e);
            Core.MousePosition = e.Location;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.O)
            {
                var dial = new OpenFileDialog();
                if (dial.ShowDialog() == DialogResult.OK)
                {
                    LoadImported(dial.FileName);
                }
                return;
            }

            //if (e.KeyCode == Keys.A)
            //{
            //    Color c;
            //    byte r, g, b;
            //    int howmuch = 5;
            //    foreach(var px in RenderPalClass.Pixels)
            //    {
            //        for (int i = 0; i < howmuch; i++)
            //        {
            //            c = px.Gradient.Last();
            //            bool ok = c.R + c.G + c.B > 765 * 0.75;
            //            if (!ok) continue;
            //            r = (byte)Math.Max(c.R, (byte)(c.R + 5));
            //            g = (byte)Math.Max(c.G, (byte)(c.G + 5));
            //            b = (byte)Math.Max(c.B, (byte)(c.B + 5));
            //            c = Color.FromArgb(r, g, b);
            //            px.Gradient.Add(c);
            //        }
            //    }
            //    return;
            //}

            RenderClass.KeyDown(e);
            RenderPalClass.KeyDown(e);
            if (e.KeyCode == Keys.C)
                RenderClass.Initialize();
        }

        private void LoadImported(string fileName)
        {
            var img = ((Bitmap) Image.FromFile(fileName));
            ImageIndexing.Process(img);
            RenderPalClass.Initialize();
        }

        private void Update(object sender, EventArgs e)
        {
            RenderClass.Update();
        }

        private void Draw(object sender, EventArgs e)
        {
            RenderClass.Draw();
            RenderPalClass.Draw();

            Bitmap img = new Bitmap(Core.ImageUI.Width, Core.ImageUI.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(Core.Image, 0, 0);
                g.DrawImage(Core.ImageUI, 0, 0);
            }

            Render.Image = img;
            RenderPal.Image = ImagePal;
        }
    }
}
