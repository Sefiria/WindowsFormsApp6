using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    static public class Maths
    {
        static public double DistanceNonZero(Point A, Point B, double epsilon)
        {
            double value = Math.Sqrt(Math.Pow((A.X - B.X), 2) + Math.Pow((A.Y - B.Y), 2));
            return value < epsilon ? 1.0 : value;
        }
        static public double Distance(Point A, Point B)
        {
            return Math.Sqrt(Math.Pow((A.X - B.X), 2) + Math.Pow((A.Y - B.Y), 2));
        }
        static public double Lerp(double v0, double v1, double t)
        {
            return (1.0 - t) * v0 + t * v1;
        }
        static public Point Lerp2(Point v0, Point v1, double t)
        {
            return new Point((int)((1.0 - t) * v0.X + t * v1.X), (int)((1.0 - t) * v0.Y + t * v1.Y));
        }
        static public Point Lerp2(int v0_X, int v0_Y, int v1_X, int v1_Y, double t)
        {
            return Lerp2(new Point(v0_X, v0_Y), new Point(v1_X, v1_Y), t);
        }
        static public T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        static public (int X, int Y) ClampPositionInRectangle(this (int X, int Y) point, Rectangle bounds)
        {
            return (Clamp(point.X, bounds.X, bounds.Width), Clamp(point.Y, bounds.Y, bounds.Height));
        }
        static public (int X, int Y) ClampPositionInRectangle(int X, int Y, Rectangle bounds)
        {
            return ClampPositionInRectangle((X, Y), bounds);
        }
        static public (float X, float Y) ClampPositionInRectangle(this (float X, float Y) point, Rectangle bounds)
        {
            return (Clamp(point.X, bounds.X, bounds.Width), Clamp(point.Y, bounds.Y, bounds.Height));
        }
        static public (float X, float Y) ClampPositionInRectangle(float X, float Y, Rectangle bounds)
        {
            return ClampPositionInRectangle((X, Y), bounds);
        }
        static public Point ClampPositionInRectangle(this Point point, Rectangle bounds)
        {
            return new Point(Clamp(point.X, bounds.X, bounds.Width), Clamp(point.Y, bounds.Y, bounds.Height));
        }
        static public Rectangle RectangleF_To_Rectangle(Rectangle bounds)
        {
            return new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }
    }
}
