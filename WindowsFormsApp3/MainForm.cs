using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp3.Entities;
using WindowsFormsApp3.Properties;

namespace WindowsFormsApp3
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

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            InputPart.ManageMouse(e);
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            InputPart.ManageMouseMove(e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            InputPart.ManageMouseUp();
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            InputPart.ManageMouseLeave();
        }

        private void FormUpdate(object sender, EventArgs e)
        {
            SharedCore.MouseLocation = new Point(MousePosition.X - Location.X - 11, MousePosition.Y - Location.Y - 32);

            UpdatePart.Update(this);
        }

        private void FormDraw(object sender, EventArgs e)
        {
            DrawPart.Draw(this);
        }

        public void LoadGame()
        {
            UpdatePart.Init();

            SharedData.Player = new Player(Render.Width / 2F, Render.Height / 2F);
        }
    }
}
