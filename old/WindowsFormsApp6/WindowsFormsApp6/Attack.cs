using SharedClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Attack : Mode
    {
        public Stopwatch timer = new Stopwatch();
        Point Border => new Point(50, 50);
        Point OffsetScreen => new Point(0, Border.Y);
        Point OffsetInfos => new Point(OffsetScreen.X + 320, OffsetScreen.Y);
        Point OffsetScreenOpponent => new Point(OffsetInfos.X + Border.X, OffsetInfos.Y);
        int StartTime = 60;
        public Grid GridOpponent;
        long ScoreOpponent = 0;
        Point OpponentCursorLocation = new Point(160, 160);
        Queue<int>[] QueueNewBlocksUp = null;

        public string OpponentName;
        public IPEndPoint Opponent;

        public Attack(string OpponentName, IPEndPoint Opponent)
        {
            this.OpponentName = OpponentName;
            this.Opponent = Opponent;
            InitializeComponent();
        }
        public override void Initialize()
        {
            base.Initialize();

            BlockMaxValue = 3;
            Render.Image = new Bitmap(Render.Width, Render.Height);
            Grid = new Grid(32);
            GridOpponent = new Grid();
            Grid.Initialize();
            GridOpponent.Initialize();
            GridOpponent.ShowPath = false;
            GridOpponent.CallModeEvent_BlockLevel = false;
            base.Render = this.Render;

            QueueNewBlocksUp = new Queue<int>[Grid.BlockCount];
            for (int i = 0; i < Grid.BlockCount; i++)
                QueueNewBlocksUp[i] = new Queue<int>();

            string MsgGrid = "";
            for (int x = 0; x < Grid.BlockCount; x++)
                for (int y = 0; y < Grid.BlockCount; y++)
                    MsgGrid += (char)Grid.grid[x][y].V;
            Net.SendMsg($"AttackP2P_GridInitialize|{MsgGrid}", Opponent);

            m_Score = 0;

            TimerDraw.Tick += Draw;
            TimerUpdate.Tick += Update;
            TimerDraw.Start();
            TimerUpdate.Start();
            
            Net.SendMsg($"AttackP2P_Ready|{Opponent.Address}");
        }

        private void Draw(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(Render.Image);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.FillRectangle(new SolidBrush(Color.DimGray), Render.Bounds);
                g.FillRectangle(new SolidBrush(Color.Black), OffsetScreen.X, OffsetScreen.Y, 320, 320);
                g.FillRectangle(new SolidBrush(Color.Black), OffsetScreenOpponent.X, OffsetScreenOpponent.Y, 320, 320);
                Grid.Draw(g, OffsetScreen.X, OffsetScreen.Y);
                GridOpponent.Draw(g, OffsetScreenOpponent.X, OffsetScreenOpponent.Y);
                int time = StartTime - (int)timer.Elapsed.TotalSeconds;


                Size UserNameSize = TextRenderer.MeasureText(lbTimeLeft.Text, lbTimeLeft.Font);
                lbTimeLeft.Location = new Point(OffsetInfos.X + Border.X / 2 - UserNameSize.Width / 2, OffsetInfos.Y + Border.Y / 2 - UserNameSize.Height / 2);
                UserNameSize = TextRenderer.MeasureText(lbScoreUser1.Text, lbScoreUser1.Font);
                lbScoreUser1.Location = new Point(OffsetInfos.X + Border.X / 2 - UserNameSize.Width / 2, OffsetInfos.Y + Border.Y / 2 - UserNameSize.Height / 2 + 25);
                UserNameSize = TextRenderer.MeasureText(lbScoreUser2.Text, lbScoreUser2.Font);
                lbScoreUser2.Location = new Point(OffsetInfos.X + Border.X / 2 - UserNameSize.Width / 2, OffsetInfos.Y + Border.Y / 2 - UserNameSize.Height / 2 + 50);
                lbTimeLeft.Text = time.ToString();
                lbScoreUser1.Text = Score.ToString();
                lbScoreUser2.Text = ScoreOpponent.ToString();

                int A = int.Parse(lbScoreUser1.Text);
                int B = int.Parse(lbScoreUser2.Text);
                lbTimeLeft.ForeColor = Color.FromArgb(255, 255 - (byte)(timer.Elapsed.Seconds / (float)StartTime * 255F), 0);
                lbScoreUser1.ForeColor = Color.White;
                lbScoreUser2.ForeColor = Color.White;
                if (A > B)
                    lbScoreUser1.ForeColor = Color.Yellow;
                else if (A < B)
                    lbScoreUser2.ForeColor = Color.Yellow;

                if (time <= 10)
                {
                    Font font = new Font(DefaultFont.FontFamily, StartTime + (11 - time) * 20, FontStyle.Bold);
                    Size TextSize = TextRenderer.MeasureText(time.ToString(), font);
                    g.DrawString(time.ToString(), font, new SolidBrush(Color.FromArgb(55 + (10 - time) * 10, Color.White)), 320 / 2 - TextSize.Width / 2, 320 / 2 - TextSize.Height / 2);
                }
                g.DrawIcon(Properties.Resources.OpponentCursor, OffsetScreenOpponent.X + OpponentCursorLocation.X - 6, OffsetScreenOpponent.Y + OpponentCursorLocation.Y - 6);
            }
            Render.Image = img;

            this.Text = $"Attack! - Time Left : {StartTime - (int)timer.Elapsed.TotalSeconds}";
        }

        private void Update(object sender, EventArgs e)
        {
            if (TimerUpdate.Interval > 10)
                TimerUpdate.Interval = 10 + (int)((TimerUpdate.Interval - 10) * 0.9);
            Update_Grid();
            Grid.Update(OffsetScreen.X, OffsetScreen.Y);
            GridOpponent.Update();

            if (StartTime - (int)timer.Elapsed.TotalSeconds <= 0)
            {
                GameEndsNotClosed = true;
                Draw(null, null);
                TimerDraw.Stop();
                TimerUpdate.Stop();
                Net.SendMsg($"AttackP2P_Gameover|{Score},{Opponent.Address}");
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
                    Net.SendMsg($"AttackP2P_NewBlock|{x},{0},{Grid.grid[x][0].V}", Opponent);
                }
                /*if (GridOpponent.grid[x][0].V == 0)
                    Net.SendMsg($"AttackP2P_AskBlock|{x},{0}", Opponent);*/
                if (GridOpponent.grid[x][0].V == 0 && QueueNewBlocksUp[x].Count > 0)
                    GridOpponent.grid[x][0].V = QueueNewBlocksUp[x].Dequeue();
            }
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Block block = Grid.GetBlock((e.X - OffsetScreen.X) / Grid.BlockSize, (e.Y - OffsetScreen.Y) / Grid.BlockSize);
            if (block == null)
                return;

            block.OnClick(sender, e);
            MAIN.Instance.Mode.ComboMade(block.X, block.Y, block.V, block.Path.Count);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        public override void BlockClickEnded(int X, int Y)
        {
            Net.SendMsg($"AttackP2P_Score|{Score}", Opponent);
            Net.SendMsg($"AttackP2P_Click|{X},{Y}", Opponent);
        }
        public override void BlockDestroyed(int V, int X, int Y)
        {
            Score++;
        }
        public override void ComboMade(int X, int Y, int V, int PathCount)
        {
            Net.SendMsg($"AttackP2P_ComboMade|{X},{Y},{V},{PathCount}", Opponent);
        }
        public override void BlockCreated()
        {
        }
        public override void BlockComboDestroyed(int ComboVal, int X, int Y, List<Block> blockExploded)
        {
            if (blockExploded.Count == 0)
                return;

            if (blockExploded[0].RelatedGrid == GridOpponent)
            {
                foreach (Block block in blockExploded)
                {
                    //Grid.grid[block.X][block.Y].Sealed = true;
                    GridOpponent.ParticlesEngine.AddIncreasingOffsetToParticles(block.X, block.Y, - OffsetScreenOpponent.X);
                }
            }
            else
            {
                foreach (Block block in blockExploded)
                {
                    //GridOpponent.grid[block.X][block.Y].Sealed = true;
                    Grid.ParticlesEngine.AddIncreasingOffsetToParticles(block.X, block.Y, OffsetScreenOpponent.X);
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (PlayAgain)
                MAIN.Instance.SelectMode(this, null);
        }

        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            if (Opponent != null && new Rectangle(OffsetScreen.X, OffsetScreen.Y, 320, 320).Contains(e.Location))
                Net.SendMsg($"AttackP2P_Cursor|{e.X},{e.Y}", Opponent);
        }

        public void AttackP2P_GridInitialize(string msg)
        {
            for (int x = 0; x < Grid.BlockCount; x++)
                for (int y = 0; y < Grid.BlockCount; y++)
                    GridOpponent.grid[x][y].V = msg[x * Grid.BlockCount + y];
        }
        public void AttackP2P_Cursor(string msg)
        {
            OpponentCursorLocation = new Point(int.Parse(msg.Split(',')[0]), int.Parse(msg.Split(',')[1]));
        }
        public void AttackP2P_Score(string msg)
        {
            if(!this.IsDisposed)
                Invoke(new Action(() => ScoreOpponent = int.Parse(msg)));
        }
        public void AttackP2P_Click(string msg)
        {
            string[] data = msg.Split(',');
            GridOpponent.grid[int.Parse(data[0])][int.Parse(data[1])].OnClick(null, null);
        }
        public void AttackP2P_NewBlock(string msg)
        {
            string[] data = msg.Split(',');
            QueueNewBlocksUp[int.Parse(data[0])].Enqueue(int.Parse(data[2]));
        }
        public void AttackP2P_ComboMade(string msg)
        {
            string[] data = msg.Split(',');
            GridOpponent.grid[int.Parse(data[0])][int.Parse(data[1])].V = int.Parse(data[2]);
        }
        public void AttackP2P_AskBlock(string msg)
        {
            string[] data = msg.Split(',');
            Net.SendMsg($"AttackP2P_NewBlock|{data[0]},{data[1]},{Grid.grid[int.Parse(data[0])][int.Parse(data[1])].V}", Opponent);
        }
        public void AttackP2P_Gameover(string msg)
        {
            string[] data = msg.Split(',');
            if (data[0] == "W")
                MessageBox.Show("You Win !");
            else if (data[0] == "L")
                MessageBox.Show($"{OpponentName} Wins !");
            else if (data[0] == "D")
                MessageBox.Show("Draw!");
            Invoke(new Action(() => Close()));
        }
    }
}
 