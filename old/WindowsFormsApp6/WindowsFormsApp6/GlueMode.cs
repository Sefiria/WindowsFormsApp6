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
    public partial class GlueMode : Mode
    {
        public override long Score { get => m_Score; set { } }
        public bool[][] glue;
        static public Color GlueColor => Color.FromArgb(150, 200, 200);
        int GlueLeft;

        public GlueMode()
        {
            InitializeComponent();
        }
        public override void Initialize()
        {
            base.Initialize();

            MaxHits = 100;

            Render.Image = new Bitmap(320, 320);
            Grid = new Grid(24);
            Grid.Initialize();
            base.Render = this.Render;

            m_Score = MaxHits;
            Block.GenerateParticles = false;

            glue = new bool[Grid.BlockCount][];
            for (int x = 0; x < Grid.BlockCount; x++)
            {
                glue[x] = new bool[Grid.BlockCount];
                for (int y = 0; y < Grid.BlockCount; y++)
                {
                    glue[x][y] = true;
                    GlueLeft++;
                }
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
                for (int x = 0; x < Grid.BlockCount; x++)
                    for (int y = 0; y < Grid.BlockCount; y++)
                        if (glue[x][y])
                            g.FillRectangle(new SolidBrush(GlueColor), x * Grid.BlockSize, y * Grid.BlockSize, Grid.BlockSize, Grid.BlockSize);
                Grid.Draw(g);
            }
            Render.Image = img;

            this.Text = $"Glue - Score : {Score}    Best : {FileData.HitsScore}    - Hit(s) left : {MaxHits - HitsCount}";
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

        private void Form1_KeyDown(object sender, EventArgs e) { }
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
        public override void BlockDestroyed(int V, int X, int Y)
        {
            Color C;

            if (glue[X][Y])
            {
                GlueLeft--;
                glue[X][Y] = false;
                C = GlueColor;
            }
            else
            {
                C = Block.GetColor(Grid, V);
            }

            Grid.ParticlesEngine.Generate(X, Y, 3, C.R, C.G, C.B);
        }

        public override void BlockClickEnded(int X, int Y)
        {
            HitsCount++;
            m_Score--;

            if (HitsCount == MaxHits || GlueLeft == 0)
            {
                GameEndsNotClosed = true;
                Draw(null, null);
                TimerDraw.Stop();
                TimerUpdate.Stop();
                if (FileData.SaveData())
                {
                    if (MessageBox.Show($"{(GlueLeft == 0?"All Glue Destroyed !":"Not more hit")} : The game ends.\nScore : {Score}    Best Score : {FileData.GlueScore}\nWould you want to send your score ?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Net.SendScores(FileData.GetRaw());
                    }
                }
                else
                {
                    MessageBox.Show($"{(GlueLeft == 0 ? "All Glue Destroyed !" : "Not more hit")} : The game ends.\nScore : {Score}\nBest Score : {FileData.GlueScore}", "Game Over");
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
