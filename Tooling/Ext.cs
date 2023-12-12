using System.Drawing;

namespace Tooling
{
    public static class Ext
    {
        #region Point, PointF

        public static PointF Add(this ref PointF pt, float x, float y) => pt = new PointF(pt.X + x, pt.Y + y);
        public static PointF Minus(this PointF A, PointF B) => new PointF(A.X - B.X, A.Y - B.Y);
        public static PointF Minus(this PointF A, vec B) => new PointF(A.X - B.x, A.Y - B.y);
        public static PointF Minus(this PointF A, vecf B) => new PointF(A.X - B.x, A.Y - B.y);
        public static PointF Minus(this PointF A, float x, float y) => new PointF(A.X - x, A.Y - y);
        public static PointF Minus(this PointF A, float n) => new PointF(A.X - n, A.Y - n);
        public static PointF Minus(this Point A, PointF B) => new PointF(A.X - B.X, A.Y - B.Y);
        public static PointF Minus(this Point A, vec B) => new PointF(A.X - B.x, A.Y - B.y);
        public static PointF Minus(this Point A, vecf B) => new PointF(A.X - B.x, A.Y - B.y);
        public static PointF Minus(this Point A, float x, float y) => new PointF(A.X - x, A.Y -y);
        public static PointF Minus(this Point A, float n) => new PointF(A.X - n, A.Y - n);
        public static Point Snap(this Point pt, int n) => pt.Div(n).x(n);
        public static PointF Snap(this PointF pt, int n) => pt.Div(n).x(n);
        public static Point Div(this Point pt, int n) => new Point(pt.X  / n, pt.Y  / n);
        public static Point Div(this PointF pt, int n) => new Point((int)pt.X  / n, (int)pt.Y  / n);
        public static PointF DivF(this Point pt, int n) => new Point(pt.X / n, pt.Y / n);
        public static PointF DivF(this Point pt, float n) => new Point((int)(pt.X / n), (int)(pt.Y / n));
        public static PointF DivF(this PointF pt, int n) => new PointF(pt.X / n, pt.Y / n);
        public static PointF DivF(this PointF pt, float n) => new PointF(pt.X / n, pt.Y / n);
        public static PointF x(this int n, PointF pt) => new PointF(pt.X * n, pt.Y * n);
        public static PointF x(this PointF pt, int n) => new PointF(pt.X * n, pt.Y * n);
        public static PointF x(this PointF pt, float n) => new PointF(pt.X * n, pt.Y * n);
        public static Point x(this int n, Point pt) => new Point(pt.X * n, pt.Y * n);
        public static Point x(this Point pt, int n) => new Point(pt.X * n, pt.Y * n);
        public static PointF Mod(this PointF A, int B) => new PointF(A.X % B, A.Y % B);
        public static PointF Mod(this PointF A, float B) => new PointF(A.X % B, A.Y % B);

        public static PointF ToPointF(this Point pt) => new PointF(pt.X, pt.Y);
        public static Point ToPoint(this PointF pt) => new Point((int)pt.X, (int)pt.Y);

        public static PointF PlusF(this PointF a, PointF b) => new PointF(a.X + b.X, a.Y + b.Y);
        public static PointF PlusF(this PointF a, vec b) => new PointF(a.X + b.x, a.Y + b.y);
        public static PointF PlusF(this PointF a, vecf b) => new PointF(a.X + b.x, a.Y + b.y);
        public static PointF PlusF(this PointF a, Point b) => new PointF(a.X + b.X, a.Y + b.Y);
        public static PointF PlusF(this Point a, PointF b) => new PointF(a.X + b.X, a.Y + b.Y);
        public static PointF PlusF(this Point a, vec b) => new PointF(a.X + b.x, a.Y + b.y);
        public static PointF PlusF(this Point a, vecf b) => new PointF(a.X + b.x, a.Y + b.y);
        public static PointF PlusF(this int n, PointF pt) => new PointF(n + pt.X, n + pt.Y);
        public static PointF PlusF(this PointF pt, int n) => new PointF(n + pt.X, n + pt.Y);
        public static PointF PlusF(this int n, Point pt) => new PointF(n + pt.X, n + pt.Y);
        public static PointF PlusF(this PointF pt, float x, float y) => new PointF(pt.X + x, pt.Y + y);

        public static Point Plus(this PointF a, PointF b) => a.PlusF(b).ToPoint();
        public static Point Plus(this PointF a, Point b) => a.PlusF(b).ToPoint();
        public static Point Plus(this Point a, PointF b) => a.PlusF(b).ToPoint();
        public static Point Plus(this int n, PointF pt) => n.PlusF(pt).ToPoint();
        public static Point Plus(this PointF pt, int n) => pt.PlusF(n).ToPoint();
        public static Point Plus(this int n, Point pt) => n.PlusF(pt).ToPoint();
        public static Point Plus(this Point pt, int x, int y) => new Point(pt.X + x, pt.Y + y);

        #endregion


        public static Point iP(this (int X, int Y) data) => new Point(data.X, data.Y); 
        public static PointF P(this (int X, int Y) data) => new PointF(data.X, data.Y); 
        public static PointF P(this (float X, float Y) data) => new PointF(data.X, data.Y);
        public static PointF P(this float n) => new PointF(n, n);
        public static Point iP(this float n) => new Point((int)n, (int)n);
        public static float ToAngle(this PointF pt) => Maths.GetAngle(pt);
        public static Size iSz(this (int X, int Y) data) => new Size(data.X, data.Y);
        public static SizeF Sz(this (int X, int Y) data) => new SizeF(data.X, data.Y);
        public static SizeF Sz(this (float X, float Y) data) => new SizeF(data.X, data.Y);
        public static SizeF Sz(this float n) => new SizeF(n, n);
        public static Size iSz(this float n) => new Size((int)n, (int)n);
        public static vec V(this (int X, int Y) data) => new vec(data.X, data.Y);
        public static vec V(this (float X, float Y) data) => new vec(data.X, data.Y);
        public static vecf Vf(this (int X, int Y) data) => new vecf(data.X, data.Y);
        public static vecf Vf(this (float X, float Y) data) => new vecf(data.X, data.Y);
        public static vecf Vf(this SizeF size) => new vecf(size.Width, size.Height);
        public static vecf Vf(this Size size) => new vecf(size.Width, size.Height);
        public static vec V(this SizeF size) => new vec(size.Width, size.Height);
        public static vec V(this Size size) => new vec(size.Width, size.Height);
        public static ICoords IC(this (int X, int Y) data) => ICoordsFactory.Create(data.X, data.Y);
        public static ICoords IC(this (float X, float Y) data) => ICoordsFactory.Create(data.X, data.Y);
        public static float DistanceFromLine(this PointF self, float ax, float by, float c) => Maths.DistanceFromLine(self, ax, by, c);


        public static Rectangle ToIntRect(this RectangleF rect) => new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);


        public static Color Mod(this Color color, int r, int g, int b) => Color.FromArgb(Tools.Range(0, color.R + r, 255), Tools.Range(0, color.G + g, 255), Tools.Range(0, color.B + b, 255));
        public static Color Mod(this Color color, int v) => Color.FromArgb(Tools.Range(0, color.R + v, 255), Tools.Range(0, color.G + v, 255), Tools.Range(0, color.B + v, 255));
        public static Color Reversed(this Color c) => Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);

        public static PointF norm(this PointF v) => Maths.Normalized(v);
        public static float Length(this PointF self) => Maths.Length(self);


        public static int ToInt(this bool boolean) => boolean ? 1 : 0;
    }
}
