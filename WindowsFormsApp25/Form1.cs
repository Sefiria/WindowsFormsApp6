using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp25
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerTick = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        DB DB;

        public Form1()
        {
            InitializeComponent();
            KB.Init();
            MouseStates.Initialize();
            Core.Instance.InitializeCore(ref Render);
            DB = new DB();
            TimerUpdate.Tick += Update;
            TimerTick.Tick += Tick;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            Core.Instance.Update();

            KB.Update();
            MouseStates.Update();
        }

        private void Tick(object sender, EventArgs e)
        {
            Core.Instance.Ticks++;
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
    }
}
