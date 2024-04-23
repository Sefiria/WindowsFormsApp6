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

        public static void Initialize()
        {
            Segments = new List<Segment>
            {
                new Segment(100, 200, 350, 220),
            };
            Matters = new List<IMatter>
            {
                new Controllable(120, 100, 16, Color.Cyan.ToArgb()),
                new Chest(300, 200, 20, 12, Segments[0].Angle, Color.Yellow.ToArgb(), ( 0x46, 0x01 )),
            };
        }
        public static void Update()
        {
            Matters.ForEach(e => e.Update());
        }
        public static void Draw(Graphics g)
        {
            Segments.ForEach(s => s.Draw(g));
            Matters.ForEach(e => e.Draw(g));
        }
        public static void DrawUI(Graphics g)
        {
            Matters.ForEach(e => e.DrawUI(g));
        }
    }
}
