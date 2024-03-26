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
    public partial class HitsMode : Mode
    {
        public HitsMode()
        {
            InitializeComponent();
        }
        public override void Initialize()
        {
            base.Initialize();

            MaxHits = 25;

            Render.Image = new Bitmap(320, 320);
            Grid = new Grid();
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

            this.Text = $"Hits - Score : {Score}    Best : {FileData.HitsScore}    - Hit(s) left : {MaxHits - HitsCount}";
        }

        private void Update(object sender, EventArgs e)
        {
            if (TimerUpdate.Interval > 10)
                TimerUpdate.Interval = 10 + (int)((TimerUpdate.Interval - 10) * 0.9);
            Update_Grid();
            Grid.Update();

            if (AllBonusesDestroyed)
            {
                GameEndsNotClosed = true;
                Draw(null, null);
                TimerDraw.Stop();
                TimerUpdate.Stop();
                if (FileData.SaveData())
                {
                    if (MessageBox.Show($"Not more hit : The game ends.\nScore : {Score}    Best Score : {FileData.HitsScore}\nWould you want to send your score ?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Net.SendScores(FileData.GetRaw());
                    }
                }
                else
                {
                    MessageBox.Show($"Not more hit : The game ends.\nScore : {Score}\nBest Score : {FileData.HitsScore}", "Game Over");
                }
                Close();
            }
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
            if (!MAIN.Instance.Mode.DestroyingAllBonuses)
            {
                Block block = Grid.GetBlock(e.X / Grid.BlockSize, e.Y / Grid.BlockSize);
                if (block == null)
                    return;

                block.OnClick(sender, e);
            }
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
            HitsCount++;

            if (HitsCount == MaxHits)
                Grid.DestroyAllBonuses();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (PlayAgain)
                MAIN.Instance.SelectMode(this, null);
        }
    }
}
