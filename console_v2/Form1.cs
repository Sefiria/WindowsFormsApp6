using System;
using System.Drawing;
using System.Windows.Forms;
using Tooling;
using static System.Net.Mime.MediaTypeNames;

namespace console_v2
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerTickSecond = new Timer() { Enabled = true, Interval = 1000 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        public Form1()
        {
            InitializeComponent();
            KB.Init();
            MouseStates.Initialize();
            Core.Instance.InitializeCore(ref Render);
            Core.Instance.InitializeScenes();
            TimerUpdate.Tick += Update;
            TimerTickSecond.Tick += TickSecond;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            Core.Instance.Update();

            KB.Update();
            MouseStates.Update();
        }

        private void TickSecond(object sender, EventArgs e)
        {
            Core.Instance.TickSecond();
        }

        private void Draw(object sender, EventArgs e)
        {
            Core.Instance.Draw();
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = true;
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            MouseStates.Position = e.Location;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

    }
}
