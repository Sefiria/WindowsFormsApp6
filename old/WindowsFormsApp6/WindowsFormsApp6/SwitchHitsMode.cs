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
    public partial class SwitchHitsMode : Mode
    {
        Block blockAtDown = null;
        public int ContinuousCombo = 0;

        public SwitchHitsMode()
        {
            InitializeComponent();
        }
        public override void Initialize()
        {
            base.Initialize();

            BlockMaxValue = 9;
            MaxHits = 30;

            Render.Image = new Bitmap(320, 320);
            Grid = new Grid(10, true);
            Grid.Initialize();
            Grid.ShowPath = false;

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
                if (blockAtDown != null)
                {
                    Pen pen = new Pen(Color.White, 2F);
                    g.DrawEllipse(pen, blockAtDown.Bounds);
                    Point M = Render.PointToClient(MousePosition);
                    Block block = Grid.GetBlock(M.X / Grid.BlockSize, M.Y / Grid.BlockSize);
                    if (block != null)
                        g.DrawEllipse(pen, block.Bounds);
                }
            }
            Render.Image = img;

            this.Text = $"SwitchHits - Score : {Score}    Best : {FileData.SwitchHitsScore}    - Hit(s) left : {MaxHits - HitsCount}";
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
                    if (MessageBox.Show($"Not more hit : The game ends.\nScore : {Score}    Best Score : {FileData.SwitchHitsScore}\nWould you want to send your score ?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Net.SendScores(FileData.GetRaw());
                    }
                }
                else
                {
                    MessageBox.Show($"Not more hit : The game ends.\nScore : {Score}\nBest Score : {FileData.SwitchHitsScore}", "Game Over");
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
                    Block FindBlockPlaceAfterFall(int X, int Y)
                    {
                        int fallY = Y + 1;
                        while(fallY < Grid.BlockCount)
                        {
                            if (Grid.grid[x][fallY].V > 0)
                                return Grid.GetBlock(X, fallY - 1);
                            else
                                fallY++;
                        }
                        return null;
                    }
                    bool TryCreateNon3PathBlock(int V)
                    {
                        while (V < BlockMaxValue && Grid.grid[x][0].V == 0)
                        {
                            Block B = FindBlockPlaceAfterFall(x, 0);
                            int BPathCount = B.GetPath(V).Count;
                            if (B != null && BPathCount > 2)
                            {
                                V++;
                            }
                            else
                            {
                                Grid.grid[x][0].V = V;
                                return true;
                            }
                        }
                        return false;
                    }
                    if(!TryCreateNon3PathBlock(rnd.Next(1, MAIN.Instance.Mode.BlockMaxValue + 1)))
                        TryCreateNon3PathBlock(1);
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

                blockAtDown = block;
            }
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            if(blockAtDown == null)
            {
                blockAtDown = null;
            }

            if (!MAIN.Instance.Mode.DestroyingAllBonuses)
            {
                Block blockAtUp = Grid.GetBlock(e.X / Grid.BlockSize, e.Y / Grid.BlockSize);
                if (blockAtUp == null)
                    return;

                ContinuousCombo = 0;

                int X = blockAtUp.X - blockAtDown.X;
                int Y = blockAtUp.Y - blockAtDown.Y;
                if (X != 0 ^ Y != 0)
                {
                    if(X == -1 || X == 1 || Y == -1 || Y == 1)
                    {
                        int V = blockAtUp.V;
                        blockAtUp.V = blockAtDown.V;
                        blockAtDown.V = V;

                        HitsCount++;
                        if (HitsCount == MaxHits)
                            Grid.DestroyAllBonuses();
                    }
                }

                blockAtDown = null;
            }
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            blockAtDown = null;
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
        public override void BlockClickEnded(int X, int Y)
        {
        }
        public override void ComboMade(int X, int Y, int V, int PathCount)
        {
            base.ComboMade(X, Y, V, PathCount);

            Score += PathCount * ContinuousCombo;
            ContinuousCombo++;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (PlayAgain)
                MAIN.Instance.SelectMode(this, null);
        }
    }
}
