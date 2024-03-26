using SharedClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class TimerMode : Mode
    {
        Stopwatch timer = new Stopwatch();
        int BonusTime = 0;
        bool FirstClick = true;

        public TimerMode()
        {
            InitializeComponent();
        }
        public override void Initialize()
        {
            base.Initialize();

            BlockMaxValue = 4;
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
                int time = 30 - timer.Elapsed.Seconds + BonusTime;
                if (time <= 10)
                {
                    Font font = new Font(DefaultFont.FontFamily, 30 + (11 - time) * 20, FontStyle.Bold);
                    Size TextSize = TextRenderer.MeasureText(time.ToString(), font);
                    g.DrawString(time.ToString(), font, new SolidBrush(Color.FromArgb(55 + (10 - time) * 10, Color.White)), MAIN.Instance.Mode.Render.Width / 2 - TextSize.Width / 2, MAIN.Instance.Mode.Render.Height / 2 - TextSize.Height / 2);
                }
            }
            Render.Image = img;

            this.Text = $"Timer - Score : {Score}    Best : {FileData.ZenScore}    - Time Left : {30 - timer.Elapsed.Seconds + BonusTime}";
        }

        private void Update(object sender, EventArgs e)
        {
            if (TimerUpdate.Interval > 10)
                TimerUpdate.Interval = 10 + (int)((TimerUpdate.Interval - 10) * 0.9);
            Update_Grid();
            Grid.Update();

            if(30 - timer.Elapsed.Seconds + BonusTime <= 0)
            {
                GameEndsNotClosed = true;
                Draw(null, null);
                TimerDraw.Stop();
                TimerUpdate.Stop();
                if (FileData.SaveData())
                {
                    if (MessageBox.Show($"The Timer Ends ! The game ends.\nScore : {Score}    Best Score : {FileData.TimerScore}\nWould you want to send your score ?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Net.SendScores(FileData.GetRaw());
                    }
                }
                else
                {
                    MessageBox.Show($"The Timer Ends ! The game ends.\nScore : {Score}\nBest Score : {FileData.TimerScore}", "Game Over");
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
            Block block = Grid.GetBlock(e.X / Grid.BlockSize, e.Y / Grid.BlockSize);
            if (block == null)
                return;

            block.OnClick(sender, e);

            if(FirstClick)
            {
                FirstClick = false;
                timer.Start();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        public override void BlockDestroyed(int VFrom, int X, int Y)
        {
            m_Score++;
        }
        public override void BlockComboDestroyed(int ComboVal, int X, int Y, List<Block> BlocksExploded)
        {
            if (ComboVal > Block.MaxValue)
            {
                BonusTime += Block.GetBonusTime(ComboVal);
            }
        }
        public override void BlockCreated()
        {
            m_Score--;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (PlayAgain)
                MAIN.Instance.SelectMode(this, null);
        }
    }
}
