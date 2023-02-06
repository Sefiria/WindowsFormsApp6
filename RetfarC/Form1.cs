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

namespace RetfarC
{
    public partial class Form1 : Form
    {
        Bitmap Image;
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };

        public Form1()
        {
            InitializeComponent();

            Core.W = Render.Width;
            Core.H = Render.Height;
            Core.Wh = Core.W / 2;
            Core.Hh = Core.H / 2;
            Core.TW = WorldConfig.TileWidth;
            Core.TH = WorldConfig.TileHeight;
            Core.PointToClient = Render.PointToClient;
            Image = new Bitmap(Render.Width, Render.Height);
            Core.g = Graphics.FromImage(Image);
            Core.GlobalTime = new Stopwatch();
            Core.GlobalTime.Start();

            WorldMgr.InitializeWorld();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            Core.FormPosition = new Point(Location.X + Render.Left + 9, Location.Y + Render.Top + 31);
            Core.FormFocused = Render.ClientRectangle.Contains(Render.PointToClient(MousePosition));

            WorldMgr.Update();
        }

        private void Draw(object sender, EventArgs e)
        {
            Image = new Bitmap(Render.Width, Render.Height);
            Core.g = Graphics.FromImage(Image);
            Core.g.Clear(Color.Black);

            WorldMgr.Draw();
            Data.DrawUI();
            GlobalUI.Draw();

            Render.Image = Image;
        }





        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            EntityMgr.DiffuseClick(e);

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
            if (e.KeyCode == Keys.ControlKey)
                Core.ShowInventory = !Core.ShowInventory;

            if (e.KeyCode == Keys.Menu)
                GlobalUI.DisplayBuild = !GlobalUI.DisplayBuild;

            UIBuild.KeyDown(e);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            Core.W = Render.Width;
            Core.H = Render.Height;
            Core.Wh = Core.W / 2;
            Core.Hh = Core.H / 2;
        }
    }
}
