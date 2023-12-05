using ConfigureRoute.Obj;
using System.Drawing;
using System.Drawing.Drawing2D;
using Tooling;

namespace ConfigureRoute
{
    internal partial class Map
    {
        void draw_grid(Graphics g)
        {
            Pen pen = new Pen(Color.FromArgb(180, 190, 210));
            float modx = Core.Cam.X % Core.Cube;
            float mody = Core.Cam.Y % Core.Cube;
            for (float y= -1F; y < Core.RH/Core.Cube + 2; y++)
            {
                g.DrawLine(pen, - Core.Cube - modx, y * Core.Cube - mody, Core.RW - modx, y * Core.Cube - mody);
            }
            for (float x = -1F; x < Core.RW / Core.Cube + 2; x++)
            {
                g.DrawLine(pen, x * Core.Cube - modx, -Core.Cube - mody, x * Core.Cube - modx, Core.RH - mody);
            }
        }
        private void drawroad(Graphics g, Road t)
        {
            var pt = (t.x * Core.Cube, t.y * Core.Cube).P().Minus(Core.Cam);
            g.FillRectangle(Brushes.Gray, pt.X, pt.Y, Core.Cube, Core.Cube);
            if(Core.AltMode)
            {
                Matrix matrix = new Matrix();
                GraphicsPath path = (GraphicsPath)GPathArrow.Clone();
                matrix.Translate(pt.X + Core.Cube / 2 - GPathArrowSize / 2, pt.Y + Core.Cube / 2 - GPathArrowSize / 2);
                matrix.RotateAt(-90F * t.z, (GPathArrowSize / 2, GPathArrowSize / 2).P());
                path.Transform(matrix);
                g.FillPath(new SolidBrush(Color.FromArgb(100, Color.White)), path);
            }
        }
        private void drawsign(Graphics g, Sign s)
        {
            void drawit(int v, bool h, PointF A, PointF B)
            {
                switch(v)
                {
                    case int _ when v == Sign.LigneContinue:         g.DrawLine(new Pen(new SolidBrush(Color.WhiteSmoke), 2F), A, B); break;
                    case int _ when v == Sign.LigneDiscontinue:     g.DrawLine(new Pen(new HatchBrush(h ? HatchStyle.DashedHorizontal: HatchStyle.DashedVertical, Color.WhiteSmoke, Color.Transparent), 2F), A, B); break;
                    case int _ when v == Sign.Stop:                         g.DrawLine(new Pen(new SolidBrush(Color.WhiteSmoke), 4F), A, B); break;
                    case int _ when v == Sign.CederLePassage:       g.DrawLine(new Pen(new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.WhiteSmoke, Color.Transparent), 4F), A, B); break;
                }
            }
            var pt = (s.x * Core.Cube, s.y * Core.Cube).P().Minus(Core.Cam);
            drawit(s.t, true, pt, (pt.X + Core.Cube, pt.Y).P());
            drawit(s.l, false, pt, (pt.X, pt.Y + Core.Cube).P());
        }
        private void draw_entities(Graphics g)
        {
            Entities.ForEach(e => e.Draw(g));
        }
    }
}
