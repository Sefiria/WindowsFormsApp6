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

namespace WindowsFormsApp27_comb
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };
        Bitmap RenderImage;
        Graphics g;

        public Form1()
        {
            InitializeComponent();

            DB.Initialize();
            MouseStates.Initialize();
            Common.SceneGame.Initialize();
            Common.SceneCurrent = Common.SceneMap;

            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);

            Cursor.Hide();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            Common.SceneCurrent.Update();
            MouseStates.Update();
        }

        private void Draw(object sender, EventArgs e)
        {
            g.Clear(Color.Black);

            Common.SceneCurrent.Draw(g);
            Common.SceneCurrent.DrawUI(g);

            Render.Image = RenderImage;
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
            MouseStates.OldPosition = MouseStates.Position;
            MouseStates.Position = e.Location;
        }
    }
}
