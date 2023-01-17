using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze
{
    public partial class Form1 : Form
    {
        Bitmap Image;
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };
        Grid Grid = new Grid(16, 16);

        public Form1()
        {
            InitializeComponent();

            Core.W = Render.Width;
            Core.H = Render.Height;
            Core.X = Core.W / 2;
            Core.Y = Core.H / 2;
            Image = new Bitmap(Render.Width, Render.Height);
            Core.g = Graphics.FromImage(Image);
            Core.GlobalTime = new Stopwatch();
            Core.MatchTime = new Stopwatch();
            Core.GlobalTime.Start();

            Grid.Generate();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            Core.FormPosition = new Point(Location.X + Render.Left + 9, Location.Y + Render.Top + 31);
            Core.FormFocused = Render.ClientRectangle.Contains(Render.PointToClient(MousePosition));
        }

        private void Draw(object sender, EventArgs e)
        {
            Image = new Bitmap(Render.Width, Render.Height);
            Core.g = Graphics.FromImage(Image);

            Grid.Draw(Core.g, 16);

            Render.Image = Image;
        }





        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Render_MouseMove(null, null);
        }

        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Core.FormFocused)
                return;

            //Point pt = MousePosition.ToTile();

            //if (e.Button == MouseButtons.Left)
            //{

            //}
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode == Keys.Z)
        }
    }
}
