using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tooling;

namespace Tooling_Mapper
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };
        Bitmap RenderImage;
        Graphics g;

        Font fontNormal = new Font("Courrier New", 12F);
        Font fontMini = new Font("Courrier New", 8F);

        List<Segment> Segments = new List<Segment>();
        List<(string text, int time)> Notifs = new List<(string text, int time)>();
        int SelectedSegmentIndex = -1, SelectedPointIndex = -1;
        PointF SelectedOffset;

        public Form1()
        {
            InitializeComponent();

            MouseStates.Initialize(Render);
            KB.Init();

            init_draw();

            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);

            Cursor.Hide();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            if(KB.LeftCtrl)
            {
                if(KB.IsKeyPressed(KB.Key.Z) && Segments.Count > 0)
                    Segments.RemoveAt(Segments.Count - 1);
                else if (KB.IsKeyPressed(KB.Key.C) && Segments.Count > 0)
                {
                    try
                    {
                        Clipboard.SetText(string.Join("|", Segments.Select(s => string.Join("&", new[] { s.A.ToString(), s.B.ToString() }))));
                        Notifs.Add(("Copied !", 0));
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Clipboard Copy failed : {ex}");
                    }
                }
                else if (KB.IsKeyPressed(KB.Key.V))
                {
                    try
                    {
                        string str = Clipboard.GetText();
                        List<Segment> segments = new List<Segment>();
                        var segs = str.Split('|');
                        string[] ab, a_parts, b_parts;
                        int ax, ay, bx, by;
                        foreach (var seg in segs )
                        {
                            ab = seg.Split('&');
                            a_parts = ab[0].Split(new[] { ", " }, StringSplitOptions.None);
                            ax = int.Parse(string.Concat(string.Concat(a_parts[0].SkipWhile(c => c != '=')).Skip(1)));
                            ay = int.Parse(string.Concat(a_parts[1].Skip(2).TakeWhile(c => c != '}')));
                            b_parts = ab[1].Split(new[] { ", " }, StringSplitOptions.None);
                            bx = int.Parse(string.Concat(string.Concat(b_parts[0].SkipWhile(c => c != '=')).Skip(1)));
                            by = int.Parse(string.Concat(b_parts[1].Skip(2).TakeWhile(c => c != '}')));
                            segments.Add(new Segment(new PointF(ax, ay), new PointF(bx, by)));
                        }
                        if (segments.Count > 0)
                        {
                            Segments = new List<Segment>(segments);
                            Notifs.Add(("Pasted !", 0));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Clipboard Paste failed : {ex}");
                    }
                }
            }
            else
            {
                if (KB.IsKeyPressed(KB.Key.C))
                    Segments.Clear();
            }

            UpdateMouse();

            KB.Update();
            MouseStates.Update();
        }
        private void Draw(object sender, EventArgs e)
        {
            g.Clear(Color.Black);

            DrawCrossGrid();
            DrawRule();

            DrawSegments();
            DrawCreatingSegment();

            DrawNotifs();

            DrawCursor();

            Render.Image = RenderImage;
        }



        int mode = 0;
        PointF A, B;
        private void UpdateMouse()
        {
            bool left_pressed = MouseStates.IsButtonPressed(MouseButtons.Left);
            bool right_pressed = MouseStates.IsButtonPressed(MouseButtons.Right);
            bool left_down = MouseStates.IsButtonDown(MouseButtons.Left);


            if (KB.LeftCtrl)
            {
                bool col(PointF pt)
                {
                    return Maths.CollisionPointCercle(MouseStates.Position, pt.X - point_size / 2F, pt.Y - point_size / 2F, point_size);
                }
                var segs = Segments.Select(s => (Segments.IndexOf(s), col(s.A), col(s.B))).Where(i => i.Item2 || i.Item3);
                if (segs.Count() > 0)
                {
                    var SelectedSegment = segs.MinBy(i => Maths.Distance(i.Item2 ? Segments[i.Item1].A : Segments[i.Item1].B, MouseStates.Position));
                    if (left_pressed)
                    {
                        SelectedSegmentIndex = SelectedSegment.Item1;
                        SelectedPointIndex = SelectedSegment.Item2 ? 0 : 1;
                        SelectedOffset = (SelectedSegment.Item2 ? Segments[SelectedSegmentIndex].A : Segments[SelectedSegmentIndex].B).MinusF(MouseStates.Position);
                        mode = SelectedSegment.Item2 ? 3 : 4;
                    }
                    else if(right_pressed)
                    {
                        Segments.RemoveAt(SelectedSegment.Item1);
                    }
                }
            }

            if (left_pressed)
            {
                if (mode == 0)
                {
                    mode = 1;
                    A = MouseStates.Position;
                }
            }
            else if (left_down)
            {
                switch (mode)
                {
                    case 1: B = MouseStates.Position; break;
                    case 3: Segments[SelectedSegmentIndex].A = MouseStates.Position.PlusF(SelectedOffset); break;
                    case 4: Segments[SelectedSegmentIndex].B = MouseStates.Position.PlusF(SelectedOffset); break;
                }
            }
            else
            {
                if (mode == 1)
                    Segments.Add(new Segment(A, B));
                mode = 0;
            }
        }



        SizeF strsz;
        byte color_brightness;
        Color c;
        Pen p;
        Brush b;
        int w, h, notif_duration, point_size = 16;
        private void init_draw()
        {
            color_brightness = 64;
            c = Color.FromArgb(color_brightness, color_brightness, color_brightness);
            p = new Pen(c);
            b = new SolidBrush(c);
            w = Render.Width;
            h = Render.Height;
            notif_duration = 50;
        }
        private void DrawCursor()
        {
            g.DrawLine(Pens.Cyan, MouseStates.Position, MouseStates.Position.Plus(10));
            g.FillEllipse(Brushes.Cyan, MouseStates.Position.X + 10, MouseStates.Position.Y + 10, 8, 8);
            g.DrawString(MouseStates.Position.ToString(), fontNormal, b, MouseStates.Position.Plus(20));
        }
        private void DrawCrossGrid()
        {
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            p.DashPattern = new[] { 6F, 4F };
            g.DrawLine(p, MouseStates.Position.X, 0, MouseStates.Position.X, h);
            g.DrawLine(p, 0, MouseStates.Position.Y, w, MouseStates.Position.Y);
        }
        private void DrawRule()
        {
            p = new Pen(c);
            for (int x = 0; x < w; x += 100)
            {
                strsz = g.MeasureString(x.ToString(), fontMini);
                g.DrawLine(p, x, 0, x, 16);
                g.DrawString(x.ToString(), fontMini, b, x == 0 ? 4 : x - strsz.Width / 2F, x == 0 ? 4 : strsz.Height + 4);
            }
            for (int x = 50; x < w; x += 100)
            {
                g.DrawLine(p, x, 0, x, 10);
                g.DrawLine(p, x, 0, x, 10);
            }
            for (int x = 25; x < w; x += 50)
            {
                g.DrawLine(p, x, 0, x, 4);
            }

            for (int y = 0; y < h; y += 100)
            {
                strsz = g.MeasureString(y.ToString(), fontMini);
                g.DrawLine(p, 0, y, 16, y);
                g.DrawString(y.ToString(), fontMini, b, y == 0 ? 4 : strsz.Width + 4, y == 0 ? 4 : y - strsz.Height / 2F);
            }
            for (int y = 50; y < h; y += 100)
            {
                g.DrawLine(p, 0, y, 10, y);
            }
            for (int y = 25; y < h; y += 50)
            {
                g.DrawLine(p, 0, y, 4, y);
            }
        }
        private void DrawSegments()
        {
            var segments = new List<Segment>(Segments);
            foreach (var segment in segments)
            {
                PointF perp = Maths.Perpendiculaire(segment.A, segment.B);
                g.DrawLine(Pens.White, segment.A, segment.B);
                g.DrawLine(new Pen(Color.FromArgb(100, 0, 0), 8F), segment.A.MinusF(perp.x(5F)), segment.B.MinusF(perp.x(5F)));
                if (KB.LeftCtrl)
                {
                    bool col(PointF pt)
                    {
                        return Maths.CollisionPointCercle(MouseStates.Position, pt.X - point_size / 2F, pt.Y - point_size / 2F, point_size);
                    }
                    var segs = Segments.Select(s => (Segments.IndexOf(s), col(s.A), col(s.B))).Where(i => i.Item2 || i.Item3);
                    (int, bool, bool) SelectedSegment = (-1, false, false);
                    if (segs.Count() > 0)
                    {
                        SelectedSegment = segs.MinBy(i => Maths.Distance(i.Item2 ? Segments[i.Item1].A : Segments[i.Item1].B, MouseStates.Position));
                        SelectedSegmentIndex = SelectedSegment.Item1;
                        SelectedPointIndex = SelectedSegment.Item2 ? 0 : 1;
                        SelectedOffset = (SelectedSegment.Item2 ? Segments[SelectedSegmentIndex].A : Segments[SelectedSegmentIndex].B).MinusF(MouseStates.Position);
                        mode = SelectedSegment.Item2 ? 3 : 4;
                    }
                    bool hover = SelectedSegment.Item1 > -1 && Segments[SelectedSegment.Item1] == segment;
                    g.DrawEllipse(hover && SelectedPointIndex == 0 ? Pens.White : Pens.DarkGray, segment.A.X - point_size / 2F, segment.A.Y - point_size / 2F, point_size, point_size);
                    g.DrawEllipse(hover && SelectedPointIndex == 1 ? Pens.White : Pens.DarkGray, segment.B.X - point_size / 2F, segment.B.Y - point_size / 2F, point_size, point_size);
                }
            }
        }
        private void DrawCreatingSegment()
        {
            if (mode != 1)
                return;
            PointF perp = Maths.Perpendiculaire(A, B);
            g.DrawLine(Pens.White, A, B);
            g.DrawLine(new Pen(Color.FromArgb(100, 0, 0), 8F), A.MinusF(perp.x(5F)), B.MinusF(perp.x(5F)));
        }
        private void DrawNotifs()
        {
            if(Notifs.Count == 0) return;
            var notif = Notifs.Last();
            g.DrawString(notif.text, fontNormal, new SolidBrush(Color.FromArgb((byte)((notif_duration - notif.time) / (float)notif_duration * 255F), Color.Yellow)), w / 2, h - 40);
            if(notif.time + 1 >= notif_duration)
                Notifs.RemoveAt(Notifs.Count - 1);
            else
                Notifs[Notifs.Count - 1] = (notif.text, notif.time + 1);
        }
    }
}
