using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cast
{
    public static class Ext
    {
        #region Point, PointF

        public static PointF Add(this ref PointF pt, float x, float y) => pt = new PointF(pt.X + x, pt.Y + y);
        public static PointF Minus(this PointF A, PointF B) => new PointF(A.X - B.X, A.Y - B.Y);
        public static PointF Minus(this PointF A, float x, float y) => new PointF(A.X - x, A.Y - y);
        public static PointF Minus(this Point A, PointF B) => new PointF(A.X - B.X, A.Y - B.Y);
        public static PointF Minus(this Point A, float x, float y) => new PointF(A.X - x, A.Y -y);
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
        public static PointF PlusF(this PointF a, Point b) => new PointF(a.X + b.X, a.Y + b.Y);
        public static PointF PlusF(this Point a, PointF b) => new PointF(a.X + b.X, a.Y + b.Y);
        public static PointF PlusF(this int n, PointF pt) => new PointF(n + pt.X, n + pt.Y);
        public static PointF PlusF(this PointF pt, int n) => new PointF(n + pt.X, n + pt.Y);
        public static PointF PlusF(this int n, Point pt) => new PointF(n + pt.X, n + pt.Y);

        public static Point Plus(this PointF a, PointF b) => a.PlusF(b).ToPoint();
        public static Point Plus(this PointF a, Point b) => a.PlusF(b).ToPoint();
        public static Point Plus(this Point a, PointF b) => a.PlusF(b).ToPoint();
        public static Point Plus(this int n, PointF pt) => n.PlusF(pt).ToPoint();
        public static Point Plus(this PointF pt, int n) => pt.PlusF(n).ToPoint();
        public static Point Plus(this int n, Point pt) => n.PlusF(pt).ToPoint();

        #endregion


        public static PointF P(this (int X, int Y) data) => new PointF(data.X, data.Y); 
        public static PointF P(this (float X, float Y) data) => new PointF(data.X, data.Y); 
    }
}
