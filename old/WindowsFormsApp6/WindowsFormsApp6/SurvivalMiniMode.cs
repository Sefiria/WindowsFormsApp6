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
    public partial class SurvivalMiniMode : Mode
    {
        public override long Score { get => m_Score; set { } }

        public SurvivalMiniMode()
        {
            InitializeComponent();
        }
        public override void Initialize()
        {
            base.Initialize();

            Render.Image = new Bitmap(320, 320);
            BlockMaxValue = 5;
            Grid = new Grid(32);
            Grid.Initialize();
            base.Render = this.Render;

            m_Score = 0;

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

            this.Text = $"Survival MINI - Score : {Score}    Best : {FileData.SurvivalMiniScore}";
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
            m_Score--;
        }
        public override void BlockClickEnded(int X, int Y)
        {
            bool NotDoneYet = false;
            for(int x = 0; x < Grid.BlockCount && !NotDoneYet; x++)
            {
                for (int y = 0; y < Grid.BlockCount && !NotDoneYet; y++)
                {
                    Block block = Grid.grid[x][y];
                    if (block.Falling || (block.V > 0 && block.Path.Count > 2))
                    {
                        block = Grid.GetBlock(x, y + 1);
                        if (block != null && block.V > 0)
                            NotDoneYet = true;
                    }
                }
            }

            if(!NotDoneYet)
            {
                GameEndsNotClosed = true;
                Draw(null, null);
                TimerDraw.Stop();
                TimerUpdate.Stop();
                if (FileData.SaveData())
                {
                    if (MessageBox.Show($"Not more combo : The game ends.\nScore : {Score}    Best Score : {FileData.SurvivalMiniScore}\nWould you want to send your score ?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Net.SendScores(FileData.GetRaw());
                    }
                }
                else
                {
                    MessageBox.Show($"Not more combo : The game ends.\nScore : {Score}\nBest Score : {FileData.SurvivalMiniScore}", "Game Over");
                }
                Close();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (PlayAgain)
                MAIN.Instance.SelectMode(this, null);
        }
    }
}
