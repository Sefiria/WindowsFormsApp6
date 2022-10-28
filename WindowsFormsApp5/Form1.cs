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
            Render.Image = PreRender.Resized(Render.Width, Render.Height);
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
                    g.DrawLine(Pens.White, xs, ys, xe, ye);
                    xs = W - PrevMouseLocation.X;
                    ys = PrevMouseLocation.Y;
                    xe = W - e.Location.X;
                    ye = e.Location.Y;
                    g.DrawLine(Pens.White, xs, ys, xe, ye);
                    xs = PrevMouseLocation.X;
                    ys = H - PrevMouseLocation.Y;
                    xe = e.Location.X;
                    ye = H - e.Location.Y;
                    g.DrawLine(Pens.White, xs, ys, xe, ye);
                    xs = W - PrevMouseLocation.X;
                    ys = H - PrevMouseLocation.Y;
                    xe = W - e.Location.X;
                    ye = H - e.Location.Y;
                    g.DrawLine(Pens.White, xs, ys, xe, ye);
                }
            }
            PrevMouseLocation = e.Location;
        }
    }
}
