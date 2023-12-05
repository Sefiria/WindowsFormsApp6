using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace Tooling
{
    public static class Maths
    {
        public static float g = 9.807F; // m/s

        public static float Smallest(ISized c) => Math.Min((int)c.W, (int)c.H);
        public static float SmallestSum(List<ISized> list)
        {
            float sum = 0;
            foreach (var c in list)
                sum += Smallest(c);
            return sum;
        }
        public static float Largest(ISized c) => Math.Max((int)c.W, (int)c.H);
        public static float LargestSum(List<ISized> list)
        {
            float sum = 0;
            foreach (var c in list)
                sum += Largest(c);
            return sum;
        }
        public static float Diagonal(int w, int h) => Sqrt(Sq(w) + Sq(h));
        public static float Diagonal(ISized c) => Diagonal((int)c.W, (int)c.H);
        public static float DiagonalSum(List<ISized> list)
        {
            float sum = 0;
            foreach (var c in list)
                sum +=Diagonal(c);
            return sum;
        }
        public static float Distance(float x1, float y1, float x2, float y2) => Sq(x2 - x1) + Sq(y2 - y1) == 0 ? 0F : Sqrt(Sq(x2 - x1) + Sq(y2 - y1));
        public static float Distance(float x1, float y1, int x2, int y2) => Distance(x1, y1, (float)x2, (float)y2);
        public static float Distance(int x1, int y1, float x2, float y2) => Distance((float)x1, (float)y1, x2, y2);
        public static float Distance(int x1, int y1, int x2, int y2) => Distance((float)x1, (float)y1, (float)x2, (float)y2);
        public static float Distance(PointF a, PointF b) => Distance(a.X, a.Y, b.X, b.Y);
        public static float Distance(vec a, vec b) => Distance(a.x, a.y, b.x, b.y);
        public static float Distance(vecf a, vecf b) => Distance(a.x, a.y, b.x, b.y);
        public static float Distance(ICoords a, ICoords b) => Distance(a.X, a.Y, b.X, b.Y);
        public static float Abs(float K) => Sqrt(Sq(K));
        public static float Sign(float K) => (float)Math.Round(K / Abs(K));
        public static float Normalize(float K) => K / Abs(K);
        public static PointF Normale(PointF a, PointF b) => new PointF(b.Y - b.Y, a.X - b.X).x(1F / Distance(a, b));
        public static float Sq(float i) => i * i;
        public static unsafe float Sqrt(float number)
        {
            float y = number;
            long i = 0x5f3759df - ((*(long*)&y) >> 1);
            y = *(float*)&i;
            y = y * (1.5F - (number * 0.5F * y * y));
            y = y * (1.5F - (number * 0.5F * y * y));
            return 1F / y * (1.5F - (number * 0.5F * y * y));
        }
        public static float Length(PointF v) => Sqrt(Sq(v.X) + Sq(v.Y));
        public static float Length(vecf v) => Sqrt(Sq(v.x) + Sq(v.y));
        public static PointF Normalized(PointF v)
        {
            float distance = Length(v);
            if (Math.Round(distance, 4) == 0F) return new PointF(0F, 0F);
            return new PointF(v.X / distance, v.Y / distance);
        }
        public static vecf Normalized(vecf v)
        {
            float distance = Length(v);
            if (Math.Round(distance, 4) == 0F) return new vecf(0F, 0F);
            return new vecf(v.x / distance, v.y / distance);
        }
        public static bool CollisionPointCercle(float x, float y, float CX, float CY, float CR)
        {
            float d2 = (x - CX) * (x - CX) + (y - CY) * (y - CY);
            if (d2 > CR * CR)
                return false;
            else
                return true;
        }
        public static bool CollisionDroite(PointF A, PointF B, float CX, float CY, float CR)
        {
            PointF u = PointF.Empty;
            u.X = B.X - A.X;
            u.Y = B.Y - A.Y;
            PointF AC = PointF.Empty;
            AC.X = CX - A.X;
            AC.Y = CY - A.Y;
            float numerateur = u.X * AC.Y - u.Y * AC.X;   // norme du vecteur v
            if (numerateur < 0F)
                numerateur = -numerateur;   // valeur absolue ; si c'est négatif, on prend l'opposé.
            float denominateur = Sqrt(u.X * u.X + u.Y * u.Y);  // norme de u
            float CI = numerateur / denominateur;
            return CI < CR;
        }
        public static bool CollisionSegment(float Ax, float Ay, float Bx, float By, float CX, float CY, float CR)
        {
            var A = new PointF(Ax, Ay);
            var B = new PointF(Bx, By);
            if (CollisionDroite(A, B, CX, CY, CR) == false)
                return false;  // si on ne touche pas la droite, on ne touchera jamais le segment
            PointF AB = PointF.Empty, AC = PointF.Empty, BC = PointF.Empty;
            AB.X = B.X - A.X;
            AB.Y = B.Y - A.Y;
            AC.X = CX - A.X;
            AC.Y = CY - A.Y;
            BC.X = CX - B.X;
            BC.Y = CY - B.Y;
            float pscal1 = AB.X * AC.X + AB.Y * AC.Y;  // pRoduit scalaiRe
            float pscal2 = (-AB.X) * BC.X + (-AB.Y) * BC.Y;  // pRoduit scalaiRe
            if (pscal1 >= 0F && pscal2 >= 0F)
                return true;   // I entRe A et B, ok.
                               // deRnièRe possibilité, A ou B dans le ceRcle
            if (CollisionPointCercle(A.X, A.Y, CX, CY, CR))
                return true;
            if (CollisionPointCercle(B.X, B.Y, CX, CY, CR))
                return true;
            return false;
        }
        public static bool ProjectionSurSegment(float Cx, float Cy, float Ax, float Ay, float Bx, float By)
        {
            float ACx = Cx - Ax;
            float ACy = Cy - Ay;
            float ABx = Bx - Ax;
            float ABy = By - Ay;
            float BCx = Cx - Bx;
            float BCy = Cy - By;
            float s1 = (ACx * ABx) + (ACy * ABy);
            float s2 = (BCx * ABx) + (BCy * ABy);
            if (s1 * s2 < 0F)
                return false;
            return true;
        }

        public static PointF Rotate(this PointF v, float degrees)
        {
            float rad = degrees.ToRadians();
            PointF result = PointF.Empty;
            result.X = v.X * (float)Math.Cos(rad) - v.Y * (float)Math.Sin(rad);
            result.Y = v.X * (float)Math.Sin(rad) + v.Y * (float)Math.Cos(rad);
            return result;
        }

        public static float ToRadians(this float degrees) => (float)Math.PI * degrees / 180F;
        public static float ToDegrees(this float radians) => radians / (float)Math.PI * 180F;
        public static PointF AngleToPointF(this float degrees) => new PointF((float)Math.Cos(degrees.ToRadians()), (float)Math.Sin(degrees.ToRadians()));
        public static float GetAngle(this PointF pt) => ((float)Math.Atan2(pt.Y, pt.X)).ToDegrees();

        public static float Lerp(float v0, float v1, double t)
        {
            return (float)((1D - t) * (double)v0 + t * (double)v1);
        }
        public static PointF Lerp(PointF v0, PointF v1, double t)
        {
            return new PointF(Lerp(v0.X, v1.X, t), Lerp(v0.Y, v1.Y, t));
        }

        public static PointF GetRayToolingLine(PointF v, PointF look, float bx, float by, float bw, float bh) => GetRayToolingLine(v, look, new RectangleF(bx, by, bw, bh));
        public static PointF GetRayToolingLine(PointF r, PointF look, RectangleF bounds)
        {
            if (look == PointF.Empty)
                return r;
            while (r.X > bounds.X && r.X < bounds.Width && r.Y > bounds.Y && r.Y < bounds.Height)
                r.Add(look.X, look.Y);
            if (r.X < bounds.X) r.X = bounds.X;
            if (r.Y < bounds.Y) r.Y = bounds.Y;
            if (r.X >= bounds.Width) r.X = bounds.Width - 1;
            if (r.Y >= bounds.Height) r.Y = bounds.Height - 1;
            return r;
        }
        public static PointF RayTooling(PointF s, PointF look, Bitmap map, Color color)
        {
            bool check(PointF _v) => _v.X >= 0F && _v.Y >= 0F && _v.X < map.Width && _v.Y < map.Height;

            PointF e = GetRayToolingLine(s, look, 0, 0, map.Width, map.Height);
            double length = new Vector(e.X - s.X, e.Y - s.Y).Length;
            PointF v, prevv = new PointF(float.NaN, float.NaN);
            for (double t = 0D; t <= 1D; t += 1D / length)
            {
                v = Lerp(s, e, t);
                if (prevv != new PointF(float.NaN, float.NaN) && prevv == v)
                    continue;
                prevv = v;
                if (check(v) && map.GetPixel((int)v.X, (int)v.Y).ToArgb() != color.ToArgb())
                    return v;
            }
            return PointF.Empty;
        }
        public static bool PointOnLine2D(this PointF _p, PointF _a, PointF _b, float t = 1E-03f)
        {
            PointF p = _p.ToPoint();
            PointF a = _a.ToPoint();
            PointF b = _b.ToPoint();

            // ensure points are collinear
            var zero = (b.X - a.X) * (p.Y - a.Y) - (p.X - a.X) * (b.Y - a.Y);
            if (zero > t || zero < -t) return false;

            // check if X-coordinates are not equal
            if (a.X - b.X > t || b.X - a.X > t)
                // ensure X is between a.X & b.X (use tolerance)
                return a.X > b.X
                    ? p.X + t > b.X && p.X - t < a.X
                    : p.X + t > a.X && p.X - t < b.X;

            // ensure Y is between a.Y & b.Y (use tolerance)
            return a.Y > b.Y
                ? p.Y + t > b.Y && p.Y - t < a.Y
                : p.Y + t > a.Y && p.Y - t < b.Y;
        }
        public static bool IsPointLeftToSegment(vecf a, vecf b, vecf o)
        {
            var vs = new Vector(b.x - a.x, b.y - a.y);
            var vo = new Vector(o.x - a.x, o.y - a.y);
            var product = Vector.CrossProduct(vs, vo);
            return product >= 0F;
        }
        public static PointF Round(this PointF pt) => new PointF((float)Math.Round(pt.X), (float)Math.Round(pt.Y));
        public static Point Round(this Point pt) => new Point((int)Math.Round((double)pt.X), (int)Math.Round((double)pt.Y));
        public static float Round(this float f, int digits) => (float)Math.Round(f, digits);

        /// <summary>
        ///  TODO: TO BE MODIFIED FOR ANGLES...
        /// </summary>
        /// <param name="src"></param>
        /// <param name="look"></param>
        /// <param name="speed"></param>
        /// <param name="max_distance"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static int SimpleRaycastHit(PointF src, PointF look, float speed, float max_distance, List<RectangleF> objects)
        {
            PointF pt = new PointF(src.X, src.Y);
            float d = 0F;
            RectangleF hit_rect;
            int hit = -1;
            int HitThat(float angle)
            {
                while (d < max_distance)
                {
                    pt = pt.PlusF(look.Rotate(angle).x(speed));
                    d += speed;
                    hit_rect = objects.FirstOrDefault(obj => obj.Contains(pt));
                    if (hit_rect == null)
                        continue;
                    return objects.IndexOf(hit_rect);
                }
                return -1;
            }

            hit = HitThat(0F);
            if(hit > -1) return hit;
            hit = HitThat(-5F);
            if(hit > -1) return hit;
            hit = HitThat(5F);
            if (hit > -1) return hit;
            hit = HitThat(-10F);
            if (hit > -1) return hit;
            hit = HitThat(10F);
            return hit;
        }
        public static bool CollisionSquareSquare(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            RectangleF box1 = new RectangleF(x1, y1, w1, h1);
            RectangleF box2 = new RectangleF(x2, y2, w2, h2);
            var a = box1.TopLeft();
            var z = box1.TopRight();
            var q = box1.BottomLeft();
            var s = box1.BottomRight();
            var e = box2.TopLeft();
            var r = box2.TopRight();
            var d = box2.BottomLeft();
            var f = box2.BottomRight();
            return
                box1.Contains(e) || box1.Contains(r) || box1.Contains(d) || box1.Contains(f) ||
                box2.Contains(a) || box2.Contains(z) || box2.Contains(q) || box2.Contains(s) ||
                box1.Contains(box2) || box2.Contains(box1);
        }
        public static bool CollisionSquareCircle(float x1, float y1, float w1, float h1, float x2, float y2, float r)
        {
            //RectangleF box1 = new RectangleF(x1, y1, w1, h1);
            //var a = box1.TopLeft();
            //var z = box1.TopRight();
            //var q = box1.BottomLeft();
            //var s = box1.BottomRight();
            //return Maths.Distance();
            return false;
        }
    }
}