using SharedClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class ZenMode : Mode
    {

        public ZenMode()
        {
            InitializeComponent();
        }
        public override void Initialize()
        {
            base.Initialize();

            BlockMaxValue = 7;
            Render.Image = new Bitmap(320, 320);
            Grid = new Grid();
            Grid.Initialize();
            base.Render = this.Render;

            Grid LoadedGrid = FileData.LoadZenState();
            if (LoadedGrid == null)
            {
                m_Score = 0;
            }
            else
            {
                Grid = LoadedGrid;
                m_Score = FileData.ZenScore;
            }

            TimerDraw.Tick += Draw;
            TimerUpdate.Tick += Update;
            TimerDraw.Start();
            TimerUpdate.Start();
        }

        private void Draw(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(Render.Image);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.FillRectangle(new SolidBrush(Color.Black), Render.Bounds);
                Grid.Draw(g);
            }
            Render.Image = img;

            this.Text = $"Zen - Score : {Score}    Best : {FileData.ZenScore}";
        }

        private void Update(object sender, EventArgs e)
        {
            if (TimerUpdate.Interval > 10)
                TimerUpdate.Interval = 10 + (int)((TimerUpdate.Interval - 10) * 0.9);
            Update_Grid();
            Grid.Update();

        }

        public void Update_Grid()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int x = 0; x < Grid.BlockCount; x++)
            {
                if (Grid.grid[x][0].V == 0)
                {
                    Grid.grid[x][0].V = rnd.Next(1, MAIN.Instance.Mode.BlockMaxValue + 1);
                }
            }
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Block block = Grid.GetBlock(e.X / Grid.BlockSize, e.Y / Grid.BlockSize);
            if (block == null)
                return;

            block.OnClick(sender, e);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        public override void BlockDestroyed(int VFrom, int X, int Y)
        {
            m_Score++;
        }
        public override void BlockCreated()
        {
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            GameEndsNotClosed = true;
            FileData.SaveData();
            FileData.SaveZenState(this);
            Net.SendScores(FileData.GetRaw());
            MessageBox.Show("ZenMode Score Saved. Scores sent to server (only if server alive)");
            base.OnClosing(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (PlayAgain)
                MAIN.Instance.SelectMode(this, null);
        }
    }
}
