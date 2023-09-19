using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PointAnima
{
    public partial class AnimaForm : Form
    {
        Bitmap img;
        Graphics g;
        int W, H;
        Timer TUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TDraw = new Timer() { Enabled = true, Interval = 10 };
        List<Anima> Frames = new List<Anima>();
        int FrameId => (int)PlayingFrame;
        APoint HandlingAPoint = null;
        bool Playing = false, IsLerp = false;
        float PlayingFrame = 0F;
        float PlayingSpeed = 0.1F;
        Anima Cpy = null;
        int FrameTime;
        long StartTick = DateTime.Now.Ticks;
        long Ticks => DateTime.Now.Ticks;

        const float DATA_SCALE = 50F;

        public AnimaForm()
        {
            InitializeComponent();

            W = Render.Width;
            H = Render.Height;
            img = new Bitmap(W, H);
            g = Graphics.FromImage(img);
            TUpdate.Tick += Update;
            TDraw.Tick += Draw;
            FrameTime = (int)numFrameTime.Value;

            Frames.AddRange(new List<Anima>(){
                new Anima() { Name = "Main", Position = new Point(W / 2, H / 2) },
                new Anima() { Name = "Main", Position = new Point(W / 2, H / 2) },
                new Anima() { Name = "Main", Position = new Point(W / 2, H / 2) },
                new Anima() { Name = "Main", Position = new Point(W / 2, H / 2) }
            });
            Frames.ForEach(a => a.Create());
        }

        private void Update(object sender, EventArgs e)
        {
            if (Playing)
            {
                PlayingFrame += PlayingSpeed;
                if (PlayingFrame >= trackBar1.Maximum + 1)
                    PlayingFrame = 0;
                else
                    trackBar1.Value = FrameId;
            }
            lbFramePerFrameCount.Text = $"{FrameId} / {trackBar1.Maximum}";
        }

        private void Draw(object sender, EventArgs e)
        {
            g.Clear(Color.Black);

            if (IsLerp && Playing)
                DrawLerp();
            else
                Frames[FrameId].Draw(g, DATA_SCALE);
            Frames[FrameId].Get(mouse_position, DATA_SCALE)?.DrawNode(g, new Pen(Color.Cyan, 5F), Frames[FrameId].Position, DATA_SCALE);

            Render.Image = img;
        }
        private void DrawLerp()
        {
            int A = FrameId, B = FrameId + 1 >= Frames.Count ? 0 : FrameId + 1;
            float t = (((Ticks - StartTick) / 10000000F) % (FrameTime * 1000F)) / 100F;
            Frames[A].DrawLerp(g, DATA_SCALE, Frames[B], t);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            LoadFrame(trackBar1.Value);
        }
        private void LoadFrame(int value)
        {
            PlayingFrame = value;
        }

        bool mouse_down = false;
        PointF mouse_position = PointF.Empty;
        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if(mouse_down == false)
            {
                HandlingAPoint = Frames[FrameId].Get(mouse_position, DATA_SCALE);
            }
            mouse_down = true;
            Render_MouseMove(sender, e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
            HandlingAPoint = null;
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            mouse_down = false;
            HandlingAPoint = null;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                btPlay_Click(null, null);
            }

            if (e.Control && e.KeyCode == Keys.C)
                Cpy = new Anima(Frames[FrameId]);
            if (e.Control && e.KeyCode == Keys.V && Cpy != null)
                Frames[FrameId] = Cpy;
        }

        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            mouse_position = e.Location;
            if (mouse_down)
            {
                if(HandlingAPoint != null)
                    HandlingAPoint.Position = mouse_position.MinusF(Frames[FrameId].Position).Div(DATA_SCALE);
            }
        }

        private void btLessFrames_Click(object sender, EventArgs e)
        {
            if (trackBar1.Maximum > 1)
            {
                trackBar1.Maximum--;
                Frames.RemoveAt(Frames.Count - 1);
            }
            while (trackBar1.Value >= trackBar1.Maximum) trackBar1.Value--;
        }
        private void btMoreFrames_Click(object sender, EventArgs e)
        {
            if (trackBar1.Maximum < 10)
            {
                trackBar1.Maximum++;
                Frames.Add(new Anima(Frames.Last()));
            }
        }

        private void btLerp_Click(object sender, EventArgs e)
        {
            IsLerp = !IsLerp;
            if (IsLerp)
                btLerp.BackColor = Color.FromArgb(200, 255, 200);
            else
                btLerp.BackColor = Color.FromArgb(255, 200, 200);
        }
        private void btPlay_Click(object sender, EventArgs e)
        {
            if (Playing)
                PlayingFrame = 0;
            Playing = !Playing;
            if (Playing)
                btPlay.BackColor = Color.FromArgb(200, 255, 200);
            else
                btPlay.BackColor = Color.FromArgb(255, 200, 200);
        }
        private void numFrameTime_ValueChanged(object sender, EventArgs e)
        {
            FrameTime = (int)numFrameTime.Value;
        }
    }
}
