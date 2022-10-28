using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp2.Entities;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2
{
    public partial class MainForm : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        public Bitmap Img;

        public MainForm()
        {
            InitializeComponent();

            Img = new Bitmap(Render.Width, Render.Height);
            SharedCore.Load(this);

            TimerUpdate.Tick += FormUpdate;
            TimerDraw.Tick += FormDraw;
            Disposed += MainForm_Disposed;

            LoadGame();
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            SharedCore.Dispose();
            Img.Dispose();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            InputPart.ManageMouse(e);
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            InputPart.ManageMouseMove(e);
        }

        private void FormUpdate(object sender, EventArgs e)
        {
            UpdatePart.Update(this);
        }

        private void FormDraw(object sender, EventArgs e)
        {
            DrawPart.Draw(this);
        }

        public void LoadGame()
        {
            MapResManager.Load("pal_basic");

            SharedData.World = new World(32, 32);
            Point PPos = SharedData.World.Current.GetAvailableSpot();
            SharedData.Player = new Player(PPos.X, PPos.Y);

            //SharedData.Map.Tiles[0, 0] = 4;
            //SharedData.Map.Tiles[1, 0] = 4;
            //SharedData.Map.Tiles[0, 1] = 4;
            //SharedData.Map.Tiles[1, 1] = 4;

            //SharedData.Map.SetFromStrings(new List<string>()
            //{
            //    "            1111",
            //    " 11111111   1221",
            //    " 1222222111 1001",
            //    " 1000000221 1111",
            //    " 110000000112222",
            //    " 21000000021    ",
            //    "  1110000011    ",
            //    "  2210000012    ",
            //    "    1111111     ",
            //    "    2213212     ",
            //    "      10011     ",
            //    "      10021     ",
            //    "      10001     ",
            //    "      11111     ",
            //    "      22222     ",
            //    "                ",
            //});
        }
    }
}
