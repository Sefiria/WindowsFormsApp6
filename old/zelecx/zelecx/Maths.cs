using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script
{
    static public class Maths
    {
        static _Main Global = _Main.Instance;

        static public float Lerp(float v0, float v1, float t) => v0 * t + v1 * (1F - t);
        static public Point Lerp2(Point p0, Point p1, float t) => new Point((int)Lerp(p0.X, p1.X, t), (int)Lerp(p0.Y, p1.Y, t));
        static public Point PositionTile(Point position) => new Point(position.X / _Main.TileSize, position.Y / _Main.TileSize);
        static public Point PositionSnap(Point position) => new Point((position.X / _Main.TileSize) * _Main.TileSize, (position.Y / _Main.TileSize) * _Main.TileSize);
        static public float Distance(Point A, Point B) => (float)Math.Sqrt(Math.Pow(B.X - A.X, 2D) + Math.Pow(B.Y - A.Y, 2D));
        static public bool FloatsComparison(float A, float B, float epsilon) => Math.Max(A, B) - Math.Min(A, B) < epsilon;
        /*static public (int x, int y) RotatePoint(float Px, float Py, float Ox, float Oy, int angle)
        {
            float a = (angle + 90 == 360 ? 0 : angle + 90) * (float)Math.PI / 180F;
            float Mx = Px - Ox;
            float My = Py - Oy;
            int X = (int)(Mx * Math.Cos(a) + My * Math.Sin(a) + Ox);
            int Y = (int)(-Mx * Math.Sin(a) + My * Math.Cos(a) + Oy);
            return (X, Y);
        }*/

    }
}
