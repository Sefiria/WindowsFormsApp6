using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Entities;
using WindowsFormsApp1.Plants;
using WindowsFormsApp1.StandFactories;

namespace WindowsFormsApp1
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

            LoadGame();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            InputPart.ManageMouse(e);
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
            SharedData.Player = new Player(6, 8);
            new Trash(6, 1);
            Stand_Factory.Create("ble", 2, 1);
            Stand_Factory.Create("tomate", 10, 6);
            new Plant(Marchandise_Factory.Create("ble"), 2, 2, 3);
            new Plant(Marchandise_Factory.Create("ble"), 2, 3, 3);
            new Plant(Marchandise_Factory.Create("tomate"), 2, 10, 8);
            new Plant(Marchandise_Factory.Create("tomate"), 2, 11, 8);
        }
    }
}
