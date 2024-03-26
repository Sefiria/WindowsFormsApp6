using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGAnim
{
    public partial class Form1 : Form
    {
        enum Tool { Pen, Fill, Eraser }

        BGAnim Animation = new BGAnim();
        short maxUndo = 5;
        List<Bitmap> BGUndo = new List<Bitmap>();
        List<Bitmap> MGUndo = new List<Bitmap>();
        List<Bitmap> FGUndo = new List<Bitmap>();
        Size RenderSize = new Size(320, 320);
        Rectangle RenderBounds = new Rectangle(0, 0, 320, 320);
        Tool m_tool = Tool.Pen;
        Tool tool { get => m_tool; set { m_tool = value; HighlightTool(); } }
        bool MouseHoldBG = false, MouseHoldMid = false, MouseHoldFG = false;
        Point PrevMouseBG, PrevMouseMid, PrevMouseFG;
        Point MouseClient => PointToClient(MousePosition);
        Point MouseBG => RenderBG.PointToClient(MousePosition);
        Point MouseMid => RenderMG.PointToClient(MousePosition);
        Point MouseFG => RenderFG.PointToClient(MousePosition);
        Timer timerResultAnim = new Timer() { Interval = 50, Enabled = true };


        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            btClearBG_Click(null, null);
            btClearMid_Click(null, null);
            btClearFG_Click(null, null);

            cbbDirectionBG.SelectedIndex = 0;
            cbbDirectionFG.SelectedIndex = 1;

            tool = Tool.Pen;

            timerResultAnim.Tick += RenderResultTick;
        }

        private void btBGColor_Click(object sender, EventArgs e)
        {
            Color PrevColor = Animation.TransparentColor;

            colorDialog1.Color = PrevColor;
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;

            Animation.TransparentColor = colorDialog1.Color;
            btBGColor.BackColor = Animation.TransparentColor;

            ReplaceColor(Animation.BG, PrevColor, Animation.TransparentColor);
            ReplaceColor(Animation.MG, PrevColor, Animation.TransparentColor);
            ReplaceColor(Animation.FG, PrevColor, Animation.TransparentColor);

            RenderBG.Image = Animation.BG;
            RenderMG.Image = Animation.MG;
            RenderFG.Image = Animation.FG;
        }
        private void ReplaceColor(Bitmap img, Color prev, Color next)
        {
            using (Graphics g = Graphics.FromImage(img))
            {
                // Set the image attribute's color mappings
                ColorMap[] colorMap = new ColorMap[1];
                colorMap[0] = new ColorMap();
                colorMap[0].OldColor = prev;
                colorMap[0].NewColor = next;
                ImageAttributes attr = new ImageAttributes();
                attr.SetRemapTable(colorMap);
                // Draw using the color map
                g.DrawImage(img, RenderBounds, RenderBounds.X, RenderBounds.Y, RenderBounds.Width, RenderBounds.Height, GraphicsUnit.Pixel, attr);
            }
        }
        private void btPenColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = btPenColor.BackColor;
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            btPenColor.BackColor = colorDialog1.Color;
        }
        private void btFillColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = btFillColor.BackColor;
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            btFillColor.BackColor = colorDialog1.Color;
        }

        private void btClearBG_Click(object sender, EventArgs e)
        {
            Animation.BG = new Bitmap(RenderSize.Width, RenderSize.Height);
            using (Graphics g = Graphics.FromImage(Animation.BG))
                g.FillRectangle(new SolidBrush(btBGColor.BackColor), RenderBounds);
            RenderBG.Image = Animation.BG;
        }
        private void btClearMid_Click(object sender, EventArgs e)
        {
            Animation.MG = new Bitmap(RenderSize.Width, RenderSize.Height);
            using (Graphics g = Graphics.FromImage(Animation.MG))
                g.FillRectangle(new SolidBrush(btBGColor.BackColor), RenderBounds);
            RenderMG.Image = Animation.MG;
        }
        private void btClearFG_Click(object sender, EventArgs e)
        {
            Animation.FG = new Bitmap(RenderSize.Width, RenderSize.Height);
            using (Graphics g = Graphics.FromImage(Animation.FG))
                g.FillRectangle(new SolidBrush(btBGColor.BackColor), RenderBounds);
            RenderFG.Image = Animation.FG;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                if (BGUndo.Count > 0 && RenderBG.Bounds.Contains(MouseClient))
                {
                    Animation.BG = BGUndo.Last();
                    BGUndo.Remove(BGUndo.Last());
                    RenderBG.Image = Animation.BG;
                }
                if (MGUndo.Count > 0 && RenderMG.Bounds.Contains(MouseClient))
                {
                    Animation.MG = MGUndo.Last();
                    MGUndo.Remove(MGUndo.Last());
                    RenderMG.Image = Animation.MG;
                }
                if (FGUndo.Count > 0 && RenderFG.Bounds.Contains(MouseClient))
                {
                    Animation.FG = FGUndo.Last();
                    FGUndo.Remove(FGUndo.Last());
                    RenderFG.Image = Animation.FG;
                }
            }
        }

        private void btPen_Click(object sender, EventArgs e)
        {
            tool = Tool.Pen;
        }
        private void btFill_Click(object sender, EventArgs e)
        {
            tool = Tool.Fill;
        }
        private void btEraser_Click(object sender, EventArgs e)
        {
            tool = Tool.Eraser;
        }
        private void HighlightTool()
        {
            btPen.BackColor = btFill.BackColor = btEraser.BackColor = SystemColors.Control;
            switch(tool)
            {
                case Tool.Pen: btPen.BackColor = Color.Turquoise; break;
                case Tool.Fill: btFill.BackColor = Color.Turquoise; break;
                case Tool.Eraser: btEraser.BackColor = Color.Turquoise; break;
            }
        }
        private void SpeedCUR_Scroll(object sender, EventArgs e)
        {
            Animation.Speed = SpeedCUR.Value / 10F;
            SpeedNUM.Value = SpeedCUR.Value;
        }
        private void SpeedNUM_ValueChanged(object sender, EventArgs e)
        {
            Animation.Speed = (int)SpeedNUM.Value / 10F;
            SpeedCUR.Value = (int)SpeedNUM.Value;
        }

        private static bool ColorMatch(Color a, Color b)
        {
            return (a.ToArgb() & 0xffffff) == (b.ToArgb() & 0xffffff);
        }

        static void FloodFill(Bitmap bmp, Point pt, Color targetColor, Color replacementColor)
        {
            Queue<Point> q = new Queue<Point>();
            q.Enqueue(pt);
            while (q.Count > 0)
            {
                Point n = q.Dequeue();
                if (!ColorMatch(bmp.GetPixel(n.X, n.Y), targetColor))
                    continue;
                Point w = n, e = new Point(n.X + 1, n.Y);
                while ((w.X >= 0) && ColorMatch(bmp.GetPixel(w.X, w.Y), targetColor))
                {
                    bmp.SetPixel(w.X, w.Y, replacementColor);
                    if ((w.Y > 0) && ColorMatch(bmp.GetPixel(w.X, w.Y - 1), targetColor))
                        q.Enqueue(new Point(w.X, w.Y - 1));
                    if ((w.Y < bmp.Height - 1) && ColorMatch(bmp.GetPixel(w.X, w.Y + 1), targetColor))
                        q.Enqueue(new Point(w.X, w.Y + 1));
                    w.X--;
                }
                while ((e.X <= bmp.Width - 1) && ColorMatch(bmp.GetPixel(e.X, e.Y), targetColor))
                {
                    bmp.SetPixel(e.X, e.Y, replacementColor);
                    if ((e.Y > 0) && ColorMatch(bmp.GetPixel(e.X, e.Y - 1), targetColor))
                        q.Enqueue(new Point(e.X, e.Y - 1));
                    if ((e.Y < bmp.Height - 1) && ColorMatch(bmp.GetPixel(e.X, e.Y + 1), targetColor))
                        q.Enqueue(new Point(e.X, e.Y + 1));
                    e.X++;
                }
            }
        }
        float Lerp(float v0, float v1, float t)
        {
            return (1F - t) * v0 + t * v1;
        }
        Point Lerp2(Point v0, Point v1, float t)
        {
            return new Point((int)Lerp(v0.X, v1.X, t), (int)Lerp(v0.Y, v1.Y, t));
        }

        #region Event BG
        private void RenderBG_MouseDown(object sender, MouseEventArgs e)
        {
            if (tool == Tool.Fill)
            {
                FloodFill(Animation.BG, MouseBG, Animation.BG.GetPixel(MouseBG.X, MouseBG.Y), btFillColor.BackColor);
                RenderBG.Image = Animation.BG;
                return;
            }

            MouseHoldBG = true;
            PrevMouseBG = MouseBG;

            if (BGUndo.Count >= maxUndo)
                BGUndo.RemoveAt(0);
            BGUndo.Add(new Bitmap(Animation.BG));
        }
        private void RenderBG_MouseUp(object sender, MouseEventArgs e)
        {
            MouseHoldBG = false;
        }
        private void RenderBG_MouseLeave(object sender, EventArgs e)
        {
            MouseHoldBG = false;
        }
        private void RenderBG_MouseMove(object sender, MouseEventArgs e)
        {
            if (tool == Tool.Pen || tool == Tool.Eraser)
            {
                if (!MouseHoldBG)
                    return;

                Point CurMouse = MouseBG;
                Color color = tool == Tool.Eraser ? btBGColor.BackColor : btPenColor.BackColor;

                float hs = PenSize.Value / 2;
                float length = (float)Math.Sqrt((CurMouse.X - PrevMouseBG.X) * (CurMouse.X - PrevMouseBG.X) + (CurMouse.Y - PrevMouseBG.Y) * (CurMouse.Y - PrevMouseBG.Y));
                using (Graphics g = Graphics.FromImage(Animation.BG))
                {
                    for(float t = 0F; t <= 1F; t += 1F / length)
                    {
                        Point pos = Lerp2(PrevMouseBG, CurMouse, t);
                        g.FillEllipse(new SolidBrush(color), pos.X - hs, pos.Y - hs, hs * 2, hs * 2);
                    }
                }
                RenderBG.Image = Animation.BG;

                PrevMouseBG = CurMouse;
            }
        }
        #endregion
        #region Event Mid
        private void RenderMG_MouseDown(object sender, MouseEventArgs e)
        {
            if (tool == Tool.Fill)
            {
                FloodFill(Animation.MG, MouseMid, Animation.MG.GetPixel(MouseMid.X, MouseMid.Y), btFillColor.BackColor);
                RenderMG.Image = Animation.MG;
                return;
            }

            MouseHoldMid = true;
            PrevMouseMid = MouseMid;

            if (MGUndo.Count >= maxUndo)
                MGUndo.RemoveAt(0);
            MGUndo.Add(new Bitmap(Animation.MG));
        }
        private void RenderMG_MouseUp(object sender, MouseEventArgs e)
        {
            MouseHoldMid = false;
        }
        
        private void RenderMG_MouseLeave(object sender, EventArgs e)
        {
            MouseHoldMid = false;
        }
        
        private void RenderMG_MouseMove(object sender, MouseEventArgs e)
        {
            if (tool == Tool.Pen || tool == Tool.Eraser)
            {
                if (!MouseHoldMid)
                    return;

                Point CurMouse = MouseMid;
                Color color = tool == Tool.Eraser ? btBGColor.BackColor : btPenColor.BackColor;

                float hs = PenSize.Value / 2;
                float length = (float)Math.Sqrt((CurMouse.X - PrevMouseMid.X) * (CurMouse.X - PrevMouseMid.X) + (CurMouse.Y - PrevMouseMid.Y) * (CurMouse.Y - PrevMouseMid.Y));
                using (Graphics g = Graphics.FromImage(Animation.MG))
                {
                    for (float t = 0F; t <= 1F; t += 1F / length)
                    {
                        Point pos = Lerp2(PrevMouseMid, CurMouse, t);
                        g.FillEllipse(new SolidBrush(color), pos.X - hs, pos.Y - hs, hs * 2, hs * 2);
                    }
                }
                RenderMG.Image = Animation.MG;

                PrevMouseMid = CurMouse;
            }
        }
        #endregion
        #region Event FG
        private void RenderFG_MouseDown(object sender, MouseEventArgs e)
        {
            if (tool == Tool.Fill)
            {
                FloodFill(Animation.FG, MouseFG, Animation.FG.GetPixel(MouseFG.X, MouseFG.Y), btFillColor.BackColor);
                RenderFG.Image = Animation.FG;
                return;
            }

            MouseHoldFG = true;
            PrevMouseFG = MouseFG;

            if (FGUndo.Count >= maxUndo)
                FGUndo.RemoveAt(0);
            FGUndo.Add(new Bitmap(Animation.FG));
        }
        private void RenderFG_MouseUp(object sender, MouseEventArgs e)
        {
            MouseHoldFG = false;
        }
        private void RenderFG_MouseLeave(object sender, EventArgs e)
        {
            MouseHoldFG = false;
        }

        private void cbbDirectionBG_SelectedIndexChanged(object sender, EventArgs e)
        {
            Animation.DirectionBG = (BGAnim.AnimationDirection) Enum.Parse(typeof(BGAnim.AnimationDirection), (string)cbbDirectionBG.SelectedItem);
        }
        private void cbbDirectionFG_SelectedIndexChanged(object sender, EventArgs e)
        {
            Animation.DirectionFG = (BGAnim.AnimationDirection)Enum.Parse(typeof(BGAnim.AnimationDirection), (string)cbbDirectionFG.SelectedItem);
        }
        
        private void RenderFG_MouseMove(object sender, MouseEventArgs e)
        {
            if (tool == Tool.Pen || tool == Tool.Eraser)
            {
                if (!MouseHoldFG)
                    return;

                Point CurMouse = MouseFG;
                Color color = tool == Tool.Eraser ? btBGColor.BackColor : btPenColor.BackColor;

                float hs = PenSize.Value / 2;
                float length = (float)Math.Sqrt((CurMouse.X - PrevMouseFG.X) * (CurMouse.X - PrevMouseFG.X) + (CurMouse.Y - PrevMouseFG.Y) * (CurMouse.Y - PrevMouseFG.Y));
                using (Graphics g = Graphics.FromImage(Animation.FG))
                {
                    for (float t = 0F; t <= 1F; t += 1F / length)
                    {
                        Point pos = Lerp2(PrevMouseFG, CurMouse, t);
                        g.FillEllipse(new SolidBrush(color), pos.X - hs, pos.Y - hs, hs * 2, hs * 2);
                    }
                }
                RenderFG.Image = Animation.FG;

                PrevMouseFG = CurMouse;
            }
        }
        #endregion


        private void RenderResultTick(object sender, EventArgs e)
        {
            RenderResult.Image = Animation.Animate();
        }

        private void btLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog();
            dial.Filter = "BAN files (*.ban)|*.ban";
            dial.InitialDirectory = Directory.GetCurrentDirectory();

            if (dial.ShowDialog() != DialogResult.OK)
                return;

            Animation = new BGAnim(dial.FileName);

            RenderBG.Image = Animation.BG;
            RenderMG.Image = Animation.MG;
            RenderFG.Image = Animation.FG;
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dial = new SaveFileDialog();
            dial.Filter = "PNG Bytes Array files (*.pba)|*.pba|Background Animation files (*.ban)|*.ban|GIF files (*.gif)|*.gif";
            dial.InitialDirectory = Directory.GetCurrentDirectory();

            if (dial.ShowDialog() != DialogResult.OK)
                return;

            switch(Path.GetExtension(dial.FileName))
            {
                case ".ban": Animation.SaveAsBAN(dial.FileName); break;
                case ".gif": Animation.SaveAsGIF(dial.FileName); break;
                case ".pba": Animation.SaveAsPBA(dial.FileName); break;
            }
        }
    }
}
