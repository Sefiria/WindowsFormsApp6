using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointAnima
{
    public static class Ext
    {
        #region Point, PointF

        public static PointF Add(this ref PointF pt, float x, float y) => pt = new PointF(pt.X + x, pt.Y + y);
        public static PointF Minus(this PointF A, PointF B) => new PointF(A.X - B.X, A.Y - B.Y);
        public static Point Snap(this Point pt, int n) => pt.Div(n).x(n);
        public static Point Div(this Point pt, int n) => new Point(pt.X  / n, pt.Y  / n);
        public static PointF Div(this PointF pt, float n) => new PointF(pt.X  / n, pt.Y  / n);
        public static PointF x(this int n, PointF pt) => new PointF(pt.X * n, pt.Y * n);
        public static PointF x(this PointF pt, int n) => new PointF(pt.X * n, pt.Y * n);
        public static PointF x(this PointF pt, float n) => new PointF(pt.X * n, pt.Y * n);
        public static Point x(this int n, Point pt) => new Point(pt.X * n, pt.Y * n);
        public static Point x(this Point pt, int n) => new Point(pt.X * n, pt.Y * n);

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

        public static PointF MinusF(this PointF a, PointF b) => new PointF(a.X - b.X, a.Y - b.Y);
        public static PointF MinusF(this PointF a, Point b) => new PointF(a.X - b.X, a.Y - b.Y);
        public static PointF MinusF(this Point a, PointF b) => new PointF(a.X - b.X, a.Y - b.Y);
        public static PointF MinusF(this int n, PointF pt) => new PointF(n - pt.X, n - pt.Y);
        public static PointF MinusF(this PointF pt, int n) => new PointF(n - pt.X, n - pt.Y);
        public static PointF MinusF(this int n, Point pt) => new PointF(n - pt.X, n - pt.Y);

        public static Point Minus(this PointF a, Point b) => a.MinusF(b).ToPoint();
        public static Point Minus(this Point a, PointF b) => a.MinusF(b).ToPoint();
        public static Point Minus(this int n, PointF pt) => n.MinusF(pt).ToPoint();
        public static Point Minus(this PointF pt, int n) => pt.MinusF(n).ToPoint();
        public static Point Minus(this int n, Point pt) => n.MinusF(pt).ToPoint();

        #endregion
    }
}
