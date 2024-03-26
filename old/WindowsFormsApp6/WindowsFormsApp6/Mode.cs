using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public class Mode : Form
    {
        public Grid Grid;
        public PictureBox Render;
        public Timer TimerDraw = new Timer() { Enabled = false, Interval = 10 };
        public Timer TimerUpdate = new Timer() { Enabled = false, Interval = 10 };
        public int BlockMaxValue = 3;
        public int MaxHits = 0, HitsCount = 0;
        public bool DestroyingAllBonuses = false;
        public bool AllBonusesDestroyed = false;
        public bool GameEndsNotClosed = false;
        public bool PlayAgain = false;
        KeyboardHook hook = new KeyboardHook(true);

        internal long m_Score = 0;
        public virtual long Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                if (m_Score + value < 0)
                {
                    m_Score = 0;
                    return;
                }

                if (m_Score >= long.MaxValue - value)
                {
                    m_Score = long.MaxValue - 1;
                    return;
                }

                m_Score = value;
            }
        }

        public void SlowMo()
        {
            TimerUpdate.Interval = 50;
        }

        public virtual void BlockDestroyed(int V, int X, int Y)
        {
        }
        public virtual void BlockComboDestroyed(int ComboVal, int X, int Y, List<Block> blockExploded)
        {

        }

        public virtual void BlockCreated()
        {
        }
        public virtual void BlockClickEnded(int X, int Y)
        {

        }
        public virtual void BlockFall(int prevX, int prevY, int newX, int newY)
        {

        }
        public virtual void ComboMade(int X, int Y, int V, int PathCount)
        {
        }

        public virtual void GetUsers(string[] users) { }
        public virtual void UserDisconnected(string user) { }

        public virtual void Initialize()
        {
            Block.GenerateParticles = true;
            hook.KeyDown += delegate (Keys key, bool Shift, bool Ctrl, bool Alt) { if(key == Keys.Escape) WindowState = FormWindowState.Minimized; };
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Mode
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Mode";
            this.ResumeLayout(false);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!(this is ZenMode) && !(this is AttackWaitOpponent) && !(this is Attack) && !GameEndsNotClosed && MessageBox.Show("Are you sure to exit ? You will lose your score.", "Exit", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                MAIN.Instance.Show();
                return;
            }
            TimerDraw.Stop();
            TimerUpdate.Stop();
            if (!(this is ZenMode) && !(this is AttackWaitOpponent) && !(this is Attack) && GameEndsNotClosed && MessageBox.Show("Would you want to play again ?", "Again", MessageBoxButtons.YesNo) == DialogResult.Yes)
                PlayAgain = true;
            else
                MAIN.Instance.Show();

            if(Grid != null)
                Grid.ParticlesEngine.Flush();
            base.OnClosing(e);
        }
    }
}
