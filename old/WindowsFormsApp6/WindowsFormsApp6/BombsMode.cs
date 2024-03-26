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
    public partial class BombsMode : Mode
    {
        List<Bomb> bombs = new List<Bomb>();
        Random rnd = new Random((int)DateTime.Now.Ticks);

        public BombsMode()
        {
            InitializeComponent();
        }
        public override void Initialize()
        {
            base.Initialize();

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
                foreach (Bomb bomb in bombs)
                    bomb.Draw(g);
            }
            Render.Image = img;

            this.Text = $"Bombs - Score :{Score}    Best : {FileData.BombsScore}";
        }

        private void Update(object sender, EventArgs e)
        {
            if (TimerUpdate.Interval > 10)
                TimerUpdate.Interval = 10 + (int)((TimerUpdate.Interval - 10) * 0.9);
            Update_Grid();
            Grid.Update();

            bool BombExplodes = false;
            foreach (Bomb bomb in bombs)
            {
                if (bomb.ShouldExplode)
                {
                    BombExplodes = true;
                    break;
                }
            }
            if (BombExplodes)
            {
                GameEndsNotClosed = true;
                Draw(null, null);
                TimerDraw.Stop();
                TimerUpdate.Stop();
                if (FileData.SaveData())
                {
                    if (MessageBox.Show($"A bomb explodes ! The game ends.\nScore : {Score}    Best Score : {FileData.BombsScore}\nWould you want to send your score ?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Net.SendScores(FileData.GetRaw());
                    }
                }
                else
                {
                    MessageBox.Show($"A bomb explodes ! The game ends.\nScore : {Score}\nBest Score : {FileData.BombsScore}", "Game Over");
                }
                Close();
            }
        }

        public void Update_Grid()
        {
            for (int x = 0; x < Grid.BlockCount; x++)
            {
                if (Grid.grid[x][0].V == 0)
                {
                    Grid.grid[x][0].V = rnd.Next(1, MAIN.Instance.Mode.BlockMaxValue + 1);

                    if (rnd.Next(30) == 5)
                        AddBomb();
                }
            }
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Block block = Grid.GetBlock(e.X / Grid.BlockSize, e.Y / Grid.BlockSize);
            if (block == null)
                return;

            if (!IsBlockABomb(block.X, block.Y))
            {
                if (block.Path.Count >= 3 || IsBlockACombo(e.X / Grid.BlockSize, e.Y / Grid.BlockSize))
                {
                    block.OnClick(sender, e);
                    HitBombs();

                    if (rnd.Next(8) == 6)
                        AddBomb();
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        public override void BlockDestroyed(int VFrom, int X, int Y)
        {
            m_Score++;
            if (VFrom != 0)
            {
                Bomb bomb = GetBomb(X, Y);
                if (bomb != null && VFrom > BlockMaxValue)
                {
                    bombs.Remove(bomb);
                    m_Score += 50;
                }
            }
        }
        public override void BlockCreated()
        {
        }
        public override void BlockFall(int prevX, int prevY, int newX, int newY)
        {
            Bomb bomb = GetBomb(prevX, prevY);
            if (bomb == null)
                return;
            bomb.X = newX;
            bomb.Y = newY;
        }

        public void AddBomb()
        {
            if (bombs.Count < Bomb.MaxBombs)
            {
                int tentatives = 0;
                int X, Y;
                do
                {
                    X = rnd.Next(Grid.BlockCount);
                    Y = 0;
                    tentatives++;
                } while (IsBlockABombOrCombo(X, Y) && tentatives < Grid.BlockCount * Grid.BlockCount);

                bombs.Add(new Bomb(X, Y));
            }
        }
        public bool IsBlockABomb(int X, int Y)
        {
            foreach (Bomb bomb in bombs)
                if (bomb.X == X && bomb.Y == Y)
                    return true;
            return false;
        }
        public bool IsBlockACombo(int X, int Y)
        {
            return Grid.GetBlock(X, Y).V > BlockMaxValue;
        }
        public bool IsBlockABombOrCombo(int X, int Y)
        {
            foreach (Bomb bomb in bombs)
                if (bomb.X == X && bomb.Y == Y || Grid.GetBlock(X, Y).V > BlockMaxValue)
                    return true;
            return false;
        }
        public Bomb GetBomb(int X, int Y)
        {
            foreach (Bomb bomb in bombs)
                if (bomb.X == X && bomb.Y == Y)
                    return bomb;
            return null;
        }
        public void HitBombs()
        {
            foreach (Bomb bomb in bombs)
                bomb.Hit();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (PlayAgain)
                MAIN.Instance.SelectMode(this, null);
        }
    }
}
