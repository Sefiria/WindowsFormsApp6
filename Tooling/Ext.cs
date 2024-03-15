﻿using ILGPU.Runtime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Tooling
{
    public static class Ext
    {
        #region Point, PointF

        public static PointF Add(this ref PointF pt, float x, float y) => pt = new PointF(pt.X + x, pt.Y + y);
        public static Point Minus(this PointF A, PointF B) => new Point((int)(A.X - B.X), (int)(A.Y - B.Y));
        public static Point Minus(this PointF A, vec B) => new Point((int)(A.X - B.x), (int)(A.Y - B.y));
        public static Point Minus(this PointF A, vecf B) => new Point((int)(A.X - B.x), (int)(A.Y - B.y));
        public static Point Minus(this PointF A, float x, float y) => new Point((int)(A.X - x), (int)(A.Y - y));
        public static Point Minus(this PointF A, float n) => new Point((int)(A.X - n), (int) (A.Y - n));
        public static Point Minus(this Point A, PointF B) => new Point((int)(A.X - B.X), (int)(A.Y - B.Y));
        public static Point Minus(this Point A, vec B) => new Point((int)(A.X - B.x), (int) (A.Y - B.y));
        public static Point Minus(this Point A, vecf B) => new Point((int)(A.X - B.x), (int) (A.Y - B.y));
        public static Point Minus(this Point A, float x, float y) => new Point((int)(A.X - x), (int) (A.Y - y));
        public static Point Minus(this Point A, float n) => new Point((int)(A.X - n), (int) (A.Y - n));
        public static PointF MinusF(this PointF A, PointF B) => new PointF(A.X - B.X, A.Y - B.Y);
        public static PointF MinusF(this PointF A, vec B) => new PointF(A.X - B.x, A.Y - B.y);
        public static PointF MinusF(this PointF A, vecf B) => new PointF(A.X - B.x, A.Y - B.y);
        public static PointF MinusF(this PointF A, float x, float y) => new PointF(A.X - x, A.Y - y);
        public static PointF MinusF(this PointF A, float n) => new PointF(A.X - n, A.Y - n);
        public static PointF MinusF(this Point A, PointF B) => new PointF(A.X - B.X, A.Y - B.Y);
        public static PointF MinusF(this Point A, vec B) => new PointF(A.X - B.x, A.Y - B.y);
        public static PointF MinusF(this Point A, vecf B) => new PointF(A.X - B.x, A.Y - B.y);
        public static PointF MinusF(this Point A, float x, float y) => new PointF(A.X - x, A.Y -y);
        public static PointF MinusF(this Point A, float n) => new PointF(A.X - n, A.Y - n);
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
        public static Point x(this float n, Point pt) => new Point((int)(pt.X * n), (int)(pt.Y * n));
        public static Point x(this Point pt, float n) => new Point((int)(pt.X * n), (int)(pt.Y * n));
        public static Point x(this Point pt, Point o) => new Point(pt.X * o.X, pt.Y * o.Y);
        public static PointF x(this PointF pt, PointF o) => new PointF(pt.X * o.X, pt.Y * o.Y);
        public static PointF x(this PointF pt, Point o) => new PointF(pt.X * o.X, pt.Y * o.Y);
        public static Point x(this Point pt, PointF o) => new Point((int)(pt.X * o.X), (int)(pt.Y * o.Y));
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
        public static PointF PlusF(this PointF pt, float n) => new PointF(n + pt.X, n + pt.Y);
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
        public static Point iP(this (float X, float Y) data) => new Point((int)data.X, (int)data.Y); 
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
        public static int Snap(this int number, float n) => (int)(number / n) * (int)n;
        public static float SnapF(this int number, float n) => (int)(number / n) * n;
        public static int Snap(this float number, float n) => (int)(number / n) * (int)n;
        public static float SnapF(this float number, float n) => ((int)(number / n)) * n;

        public static (int x, int y) ToTupleInt(this Point pt) => (pt.X, pt.Y);
        public static (int x, int y) ToTupleInt(this PointF pt) => ((int)pt.X, (int)pt.Y);
        public static (float x, float y) ToTupleFloat(this Point pt) => (pt.X, pt.Y);
        public static (float x, float y) ToTupleFloat(this PointF pt) => (pt.X, pt.Y);

        public static bool IsDefined(this Guid guid) => guid != Guid.Empty;
        public static bool IsNotDefined(this Guid guid) => !guid.IsDefined();

        public static RectangleF ToF(this Rectangle rect) => new RectangleF(rect.Location, rect.Size);
        public static RectangleF WithOffset(this RectangleF rect, PointF offset)
        {
            RectangleF newRect = new RectangleF(rect.Location, rect.Size);
            newRect.Offset(offset);
            return newRect;
        }
        public static Rectangle WithOffset(this Rectangle rect, PointF offset) => rect.ToF().WithOffset(offset).ToIntRect();
        public static int GetEdgesAlignBitWise() => (int)(ContentAlignment.TopCenter | ContentAlignment.BottomCenter | ContentAlignment.MiddleLeft | ContentAlignment.MiddleRight);
        public static List<ContentAlignment> GetEdgesAlign() => new List<ContentAlignment> { ContentAlignment.TopCenter, ContentAlignment.BottomCenter, ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight };
        public static Line GetEdge(this Rectangle box, ContentAlignment @where)
        {
            switch(@where)
            {
                default: throw new ArgumentException($"ContentAlignment not expected, got value '{(int)@where}' where it expects [TopCenter (2), BottomCenter (512), MiddleLeft (16), MiddleRight (64)]");
                case ContentAlignment.TopCenter: return new Line(new Point(box.Left, box.Top), new Point(box.Right, box.Top));
                case ContentAlignment.MiddleRight: return new Line(new Point(box.Right, box.Top), new Point(box.Right, box.Bottom));
                case ContentAlignment.BottomCenter: return new Line(new Point(box.Left, box.Bottom), new Point(box.Right, box.Bottom));
                case ContentAlignment.MiddleLeft: return new Line(new Point(box.Left, box.Top), new Point(box.Left, box.Bottom));
            }
        }
        public static LineF GetEdge(this RectangleF box, ContentAlignment @where)
        {
            switch (@where)
            {
                default: throw new ArgumentException($"ContentAlignment not expected, got value '{(int)@where}' where it expects [TopCenter (2), BottomCenter (512), MiddleLeft (16), MiddleRight (64)]");
                case ContentAlignment.TopCenter: return new LineF(new PointF(box.Left, box.Top), new PointF(box.Right, box.Top));
                case ContentAlignment.MiddleRight: return new LineF(new PointF(box.Right, box.Top), new PointF(box.Right, box.Bottom));
                case ContentAlignment.BottomCenter: return new LineF(new PointF(box.Left, box.Bottom), new PointF(box.Right, box.Bottom));
                case ContentAlignment.MiddleLeft: return new LineF(new PointF(box.Left, box.Top), new PointF(box.Left, box.Bottom));
            }
        }
        public static List<Line> GetEdges(this Rectangle box) => GetEdgesAlign().Select(align => box.GetEdge(align)).ToList();
        public static List<LineF> GetEdges(this RectangleF box) => GetEdgesAlign().Select(align => box.GetEdge(align)).ToList();
        public static int GetCornersAlignBitWise() => (int)(ContentAlignment.TopLeft | ContentAlignment.TopRight | ContentAlignment.BottomLeft | ContentAlignment.BottomRight);
        public static List<ContentAlignment> GetCornersAlign() => new List<ContentAlignment> { ContentAlignment.TopLeft, ContentAlignment.TopRight, ContentAlignment.BottomLeft, ContentAlignment.BottomRight };
        public static Point GetCorner(this Rectangle box, ContentAlignment @where)
        {
            switch (@where)
            {
                default: throw new ArgumentException($"ContentAlignment not expected, got value '{(int)@where}' where it expects [TopLeft (!), TopRight ($), BottomLeft (256), BottomRight (1024)]");
                case ContentAlignment.TopLeft: return box.Location;
                case ContentAlignment.TopRight: return box.Location.Plus(new Point(box.Width, 0));
                case ContentAlignment.BottomLeft: return box.Location.Plus(new Point(0, box.Height));
                case ContentAlignment.BottomRight: return box.Location.Plus(new Point(box.Width, box.Height));
            }
        }
        public static PointF GetCorner(this RectangleF box, ContentAlignment @where)
        {
            switch (@where)
            {
                default: throw new ArgumentException($"ContentAlignment not expected, got value '{(int)@where}' where it expects [TopLeft (!), TopRight ($), BottomLeft (256), BottomRight (1024)]");
                case ContentAlignment.TopLeft: return box.Location;
                case ContentAlignment.TopRight: return box.Location.Plus(new PointF(box.Width, 0f));
                case ContentAlignment.BottomLeft: return box.Location.Plus(new PointF(0f, box.Height));
                case ContentAlignment.BottomRight: return box.Location.Plus(new PointF(box.Width, box.Height));
            }
        }
        public static List<Point> GetCorners(this Rectangle box) => GetCornersAlign().Select(align => box.GetCorner(align)).ToList();
        public static List<PointF> GetCorners(this RectangleF box) => GetCornersAlign().Select(align => box.GetCorner(align)).ToList();
    }
}
