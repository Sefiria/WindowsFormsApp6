using ComputeSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        Timer TimeUpdate = new Timer() { Enabled = true, Interval = 50 };
        Data Data;
        Graphics g;
        Bitmap PreRender, UndoRender;
        new bool MouseDown = false;
        Point PrevMouseLocation = Point.Empty;
        int W, H;
        MouseButtons MouseButton = MouseButtons.None;
        static int pw = 10;

        public Form1()
        {
            InitializeComponent();

            Initialize();

            TimeUpdate.Tick += GlobalUpdate;
        }
        private void GlobalUpdate(object sender, EventArgs e)
        {
            _Update();
            _Draw();
        }

        private void Initialize()
        {
            Data = new Data();
            PreRender = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(PreRender);
            g.Clear(Color.Black);
            W = Render.Width;
            H = Render.Height;
        }

        private void _Draw()
        {
            var img = PreRender.Resized(Render.Width, Render.Height);
            var stream = img.ToMemoryStream();
            using (var texture = GraphicsDevice.GetDefault().LoadReadWriteTexture2D<Bgra32, Float4>(stream))
            {
                GraphicsDevice.GetDefault().For(texture.Width, texture.Height, 5);
                texture.Save(stream, ImageFormat.Png);
                Render.Image = Image.FromStream(stream);
            }
        }

        private void _Update()
        {
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            UndoRender = (Bitmap)Render.Image;

            MouseDown = true;
            MouseButton = e.Button;

            if (MouseButton == MouseButtons.Right)
            {
                int x = e.Location.X;
                int y = e.Location.Y;
                Tools.FloodFill(PreRender, new Point(x, y), Color.Black, Color.White);
                x = W - e.Location.X;
                y = e.Location.Y;
                Tools.FloodFill(PreRender, new Point(x, y), Color.Black, Color.White);
                x = e.Location.X;
                y = H - e.Location.Y;
                Tools.FloodFill(PreRender, new Point(x, y), Color.Black, Color.White);
                x = W - e.Location.X;
                y = H - e.Location.Y;
                Tools.FloodFill(PreRender, new Point(x, y), Color.Black, Color.White);
            }
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = false;
        }
        private void Render_MouseLeave(object sender, EventArgs e) 
        {
            MouseDown = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                g.Clear(Color.Black);
            }
            if (e.KeyCode == Keys.Z)
            {
                g.DrawImage(UndoRender, 0, 0);
            }
            //if (e.KeyCode == Keys.Down && pw > 2)
            //{
            //    pw--;
            //}
            //if (e.KeyCode == Keys.Up && pw < 50)
            //{
            //    pw++;
            //}
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseDown)
            {
                if (MouseButton == MouseButtons.Left)
                {
                    int xs = PrevMouseLocation.X;
                    int ys = PrevMouseLocation.Y;
                    int xe = e.Location.X;
                    int ye = e.Location.Y;

                    pw = (Math.Max(xe, W / 2) - Math.Min(xe, W / 2) + (Math.Max(ye, H / 2) - Math.Min(ye, H / 2))) / 10;
                    int v = (int)(200 - (pw * 10F / (W / 2F + H / 2F)) * 255);
                    if (v > 255) v = 255;
                    if (v < 0) v = 0;
                    Pen pen = new Pen(Color.FromArgb(50, v, v, v), pw);

                    void d()
                    {
                        g.DrawLine(pen, xs, ys, xe, ye);
                        g.DrawLine(pen, xs - (pw / 2), ys - (pw / 2), xe - (pw / 2), ye - (pw / 2));
                        g.DrawLine(pen, xs + (pw / 2), ys - (pw / 2), xe + (pw / 2), ye - (pw / 2));
                        g.DrawLine(pen, xs + (pw / 2), ys - (pw / 2), xe + (pw / 2), ye - (pw / 2));
                        g.DrawLine(pen, xs + (pw / 2), ys + (pw / 2), xe + (pw / 2), ye + (pw / 2));
                    }
                    d();
                    xs = W - PrevMouseLocation.X;
                    ys = PrevMouseLocation.Y;
                    xe = W - e.Location.X;
                    ye = e.Location.Y;
                    d();
                    xs = PrevMouseLocation.X;
                    ys = H - PrevMouseLocation.Y;
                    xe = e.Location.X;
                    ye = H - e.Location.Y;
                    d();
                    xs = W - PrevMouseLocation.X;
                    ys = H - PrevMouseLocation.Y;
                    xe = W - e.Location.X;
                    ye = H - e.Location.Y;
                    d();
                }
            }
            PrevMouseLocation = e.Location;
        }
    }
}
