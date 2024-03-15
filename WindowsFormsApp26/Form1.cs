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

namespace WindowsFormsApp26
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };

        public Form1()
        {
            InitializeComponent();
            KB.Init();
            MouseStates.Initialize();
            Core.Instance.InitializeCore(ref Render);
            Core.Instance.InitializeScenes();
            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            Core.Instance.Update();
            KB.Update();
            MouseStates.Update();
        }

        private void Draw(object sender, EventArgs e)
        {
            Core.Instance.Draw();
        }


        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = false;
        }
        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = true;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            MouseStates.OldPosition = MouseStates.Position;
            MouseStates.Position = e.Location;
        }
    }
}
