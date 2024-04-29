using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Tooling;
using WindowsFormsApp28_px.matters.non_organic;
using WindowsFormsApp28_px.matters.organic;

namespace WindowsFormsApp28_px
{
    internal class Common
    {
        public static List<Segment> Segments;
        public static List<IMatter> Matters;
        public static List<Bullet> Bullets;
        public static Controllable Controllable;
        public static PointF Cam => new PointF(ScreenWidth / 2F, ScreenHeight / 2F).MinusF(Controllable.Point);
        public static int ScreenWidth, ScreenHeight;

        public static void Initialize(int w, int h)
        {
            ScreenWidth = w;
            ScreenHeight = h;

            //Segments = new List<Segment>
            //{
            //    new Segment(100, 50, 50, 100),
            //    new Segment(50, 100, 100, 200),
            //    new Segment(100, 200, 500, 300),
            //};
            var data = "{\"X\":-177,\"Y\":-203}&{\"X\":-200,\"Y\":-175}|{\"X\":-200,\"Y\":-175}&{\"X\":-200,\"Y\":173}|{\"X\":-200,\"Y\":173}&{\"X\":-173,\"Y\":198}|{\"X\":-173,\"Y\":198}&{\"X\":175,\"Y\":197}|{\"X\":175,\"Y\":197}&{\"X\":198,\"Y\":174}|{\"X\":198,\"Y\":174}&{\"X\":196,\"Y\":-172}|{\"X\":196,\"Y\":-172}&{\"X\":164,\"Y\":-198}|{\"X\":164,\"Y\":-198}&{\"X\":-180,\"Y\":-200}";
            LoadSegments(data);
            Controllable = new Controllable(0, 0, 16, Color.Cyan.ToArgb());
            Matters = new List<IMatter>
            {
                new Chest(150, 150, 20, 12, Segments[0].Angle, Color.Yellow.ToArgb(), ( 0x46, 0x01 )),
            };
            Bullets = new List<Bullet>();
        }
        /*

         */
        private static void LoadSegments(string data)
        {
            Segments = new List<Segment>();
            string[] segs = data.Split('|');
            foreach(var seg in segs)
            {
                string[] pts = seg.Split('&');
                PointF A = JsonSerializer.Deserialize<PointF>(pts[0]);
                PointF B = JsonSerializer.Deserialize<PointF>(pts[1]);
                if (A == B)
                    continue;
                Segments.Add(new Segment(A, B));
            }
        }

        public static void Update()
        {
            Controllable.Update();
            Matters.ForEach(e => e.Update());
            var bullets = new List<Bullet>(Bullets);
            bullets.ForEach(b => { if (!b.Exists) Bullets.Remove(b); else b.Update(); });
        }
        public static void Draw(Graphics g)
        {
            Controllable.Draw(g);
            Segments.ForEach(s => s.Draw(g, Cam));
            Matters.ForEach(e => e.Draw(g, Cam));
            Bullets.ForEach(b => b.Draw(g, Cam));
        }
        public static void DrawUI(Graphics g)
        {
            Matters.ForEach(e => e.DrawUI(g));
        }
    }
}
