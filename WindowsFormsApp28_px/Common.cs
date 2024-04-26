using System.Collections.Generic;
using System.Drawing;
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

        public static void Initialize()
        {
            Segments = new List<Segment>
            {
                new Segment(100, 50, 50, 100),
                new Segment(50, 100, 100, 200),
                new Segment(100, 200, 500, 300),
            };
            Matters = new List<IMatter>
            {
                new Controllable(120, 100, 16, Color.Cyan.ToArgb()),
                new Chest(150, 150, 20, 12, Segments[0].Angle, Color.Yellow.ToArgb(), ( 0x46, 0x01 )),
            };
            Bullets = new List<Bullet>();
        }
        public static void Update()
        {
            Matters.ForEach(e => e.Update());
            var bullets = new List<Bullet>(Bullets);
            bullets.ForEach(b => { if (!b.Exists) Bullets.Remove(b); else b.Update(); });
        }
        public static void Draw(Graphics g)
        {
            Segments.ForEach(s => s.Draw(g));
            Matters.ForEach(e => e.Draw(g));
            Bullets.ForEach(b => b.Draw(g));
        }
        public static void DrawUI(Graphics g)
        {
            Matters.ForEach(e => e.DrawUI(g));
        }
    }
}
