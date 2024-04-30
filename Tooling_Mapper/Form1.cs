using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
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
        (int, bool, bool) SelectedSegment;
        PointF SelectedOffset;
        PointF Center, Cam;
        float CamSpeed = 5F;

        public Form1()
        {
            InitializeComponent();

            MouseStates.Initialize(Render);
            KB.Init();

            init_draw();

            Cam = PointF.Empty;
            Center = (Render.Width / 2F, Render.Height / 2F).P();
            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);

            Cursor.Hide();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private static string ToValidJson(string input) => input.Replace("=", ":").Replace("X", "\"X\"").Replace("Y", "\"Y\"");
        private void Update(object sender, EventArgs e)
        {
            if (!Render.Parent.Focused)
                return;

            (bool _z, bool _q, bool _s, bool _d) = KB.ZQSD();
            if (_z) Cam.Y -= CamSpeed;
            if (_q) Cam.X -= CamSpeed;
            if (_s) Cam.Y += CamSpeed;
            if (_d) Cam.X += CamSpeed;

            if (KB.LeftCtrl)
            {
                if (KB.IsKeyPressed(KB.Key.Z) && Segments.Count > 0)
                    Segments.RemoveAt(Segments.Count - 1);
                else if (KB.IsKeyPressed(KB.Key.C) && Segments.Count > 0)
                {
                    try
                    {
                        var data = ToValidJson(string.Join("|", Segments.Select(s => string.Join("&", new[] { s.A.ToPoint().ToString(), s.B.ToPoint().ToString() }))));
                        Clipboard.SetText(data);
                        Notifs.Add(("Copied !", 0));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Clipboard Copy failed : {ex}");
                    }
                }
                else if (KB.IsKeyPressed(KB.Key.V))
                {
                    try
                    {
                        string data = Clipboard.GetText();
                        data = data.Replace("\\\"", "\"");
                        var segments = new List<Segment>();
                        string[] segs = data.Split('|');
                        foreach (var seg in segs)
                        {
                            string[] pts = seg.Split('&');
                            PointF A = JsonSerializer.Deserialize<PointF>(pts[0]);
                            PointF B = JsonSerializer.Deserialize<PointF>(pts[1]);
                            if (A == B)
                                continue;
                            segments.Add(new Segment(A, B));
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
            var ms = MouseStates.Position.MinusF(Center).PlusF(Cam);


            if (KB.LeftAlt)
            {
                bool col(PointF pt)
                {
                    return Maths.CollisionPointCercle(ms, pt.X, pt.Y, point_size);
                }
                var segs = Segments.Select(s => (Segments.IndexOf(s), col(s.A), col(s.B))).Where(i => i.Item2 || i.Item3);
                if (segs.Count() > 0)
                {
                    SelectedSegment = segs.MinBy(i => Maths.Distance(i.Item2 ? Segments[i.Item1].A : Segments[i.Item1].B, ms));
                    SelectedPointIndex = SelectedSegment.Item2 ? 0 : 1;
                    if (left_pressed)
                    {
                        SelectedSegmentIndex = SelectedSegment.Item1;
                        SelectedOffset = (SelectedSegment.Item2 ? Segments[SelectedSegmentIndex].A : Segments[SelectedSegmentIndex].B).MinusF(ms);
                        mode = SelectedSegment.Item2 ? 3 : 4;
                    }
                    else if (right_pressed)
                    {
                        Segments.RemoveAt(SelectedSegment.Item1);
                    }
                }
                else
                    SelectedPointIndex = -1;
            }

            if (left_pressed)
            {
                if (mode == 0)
                {
                    mode = 1;
                    A = B = ms;
                }
            }
            else if (left_down)
            {
                switch (mode)
                {
                    case 1: B = ms; break;
                    case 3: Segments[SelectedSegmentIndex].A = ms.PlusF(SelectedOffset); break;
                    case 4: Segments[SelectedSegmentIndex].B = ms.PlusF(SelectedOffset); break;
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
            g.DrawString(MouseStates.Position.MinusF(w/2F, h/2F).ToString(), fontNormal, b, MouseStates.Position.Plus(20));
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
            var wh = w / 2;
            var hh = h / 2;
            p = new Pen(c);

            void _draw_0(int x)
            {
                var str = "" + (x + Cam.X);
                strsz = g.MeasureString(str, fontMini);
                g.DrawLine(p, wh + x, hh -8, wh + x, hh + 8);
                g.DrawString(str, fontMini, (x + Cam.X) == 0 ? Brushes.Gray : b, wh + (x == 0 ? 4 : x - strsz.Width / 2F), hh + (x == 0 ? 4 : strsz.Height + 4));
            }
            void _draw_1(int x, int sz) => g.DrawLine(p, wh + x, hh - sz / 2, wh + x, hh + sz / 2);
            void _draw_2(int y)
            {
                var str = "" + (y + Cam.Y);
                strsz = g.MeasureString(str, fontMini);
                g.DrawLine(p, wh - 8, hh + y, wh + 8, hh + y);
                if (y != 0)
                    g.DrawString(str, fontMini, (y + Cam.Y) == 0 ? Brushes.Gray : b, wh + (y == 0 ? 4 : strsz.Width + 4), hh + (y == 0 ? 4 : y - strsz.Height / 2F));
            }
            void _draw_3(int y, int sz) => g.DrawLine(p, wh - sz / 2, hh + y, wh + sz / 2, hh + y);

            g.DrawLine(p, 0, hh, w, hh);
            for (int x = -(int)Cam.X; x > -wh; x -= 100) _draw_0(x);
            for (int x = -(int)Cam.X; x < wh; x += 100) _draw_0(x);
            for (int x = -(int)Cam.X - 50; x > -wh; x -= 100) _draw_1(x, 10);
            for (int x = -(int)Cam.X + 50; x < wh; x += 100) _draw_1(x, 10);
            for (int x = -(int)Cam.X - 25; x > -wh; x -= 50) _draw_1(x, 5);
            for (int x = -(int)Cam.X - 25; x < wh; x += 50) _draw_1(x, 5);

            g.DrawLine(p, wh, 0, wh, h);
            for (int y = -(int)Cam.Y; y > -hh; y -= 100) _draw_2(y);
            for (int y = -(int)Cam.Y; y < hh; y += 100) _draw_2(y);
            for (int y = -(int)Cam.Y - 50; y > -hh; y -= 100) _draw_3(y, 10);
            for (int y = -(int)Cam.Y + 50; y < hh; y += 100) _draw_3(y, 10);
            for (int y = -(int)Cam.Y - 25; y > -hh; y -= 50) _draw_3(y, 5);
            for (int y = -(int)Cam.Y + 25; y < hh; y += 50) _draw_3(y, 5);
        }
        private void DrawSegments()
        {
            var ms = Center.MinusF(MouseStates.Position).MinusF(Cam);
            var segments = new List<Segment>(Segments);
            foreach (var segment in segments)
            {
                PointF perp = Maths.Perpendiculaire(segment.A, segment.B);
                g.DrawLine(Pens.White, Center.PlusF(segment.A).MinusF(Cam), Center.PlusF(segment.B).MinusF(Cam));
                g.DrawLine(new Pen(Color.FromArgb(100, 0, 0), 8F), Center.PlusF(segment.A.MinusF(perp.x(5F)).MinusF(Cam)), Center.PlusF(segment.B.MinusF(perp.x(5F)).MinusF(Cam)));
                if (KB.LeftAlt)
                {
                    bool hover = SelectedSegment.Item1 != -1 && Segments[SelectedSegment.Item1] == segment;
                    g.DrawEllipse(hover && SelectedPointIndex == 0 ? Pens.White : Pens.DarkGray, Center.X + segment.A.X - Cam.X - point_size / 2F, Center.Y + segment.A.Y - Cam.Y - point_size / 2F, point_size, point_size);
                    g.DrawEllipse(hover && SelectedPointIndex == 1 ? Pens.White : Pens.DarkGray, Center.X + segment.B.X - Cam.X - point_size / 2F, Center.Y + segment.B.Y - Cam.Y - point_size / 2F, point_size, point_size);
                }
            }
        }
        private void DrawCreatingSegment()
        {
            if (mode != 1)
                return;
            PointF perp = Maths.Perpendiculaire(A, B);
            g.DrawLine(Pens.White, Center.PlusF(A).MinusF(Cam), Center.PlusF(B).MinusF(Cam));
            g.DrawLine(new Pen(Color.FromArgb(100, 0, 0), 8F), Center.PlusF(A.MinusF(perp.x(5F))).MinusF(Cam), Center.PlusF(B.MinusF(perp.x(5F))).MinusF(Cam));
        }
        private void DrawNotifs()
        {
            if(Notifs.Count == 0) return;
            var notif = Notifs.Last();
            var sz = g.MeasureString(notif.text, fontNormal);
            g.DrawString(notif.text, fontNormal, new SolidBrush(Color.FromArgb((byte)((notif_duration - notif.time) / (float)notif_duration * 255F), Color.Yellow)), w / 2 - sz.Width / 2F, h - 40 - sz.Height / 2F);
            if(notif.time + 1 >= notif_duration)
                Notifs.RemoveAt(Notifs.Count - 1);
            else
                Notifs[Notifs.Count - 1] = (notif.text, notif.time + 1);
        }
    }
}
