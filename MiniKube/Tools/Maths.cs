using System;
using System.Drawing;
using System.Windows;

namespace MiniKube.Utilities
{
    public static class Maths
    {
        public static float g = 9.807F; // m/s

        public static float Distance(float x1, float y1, int x2, int y2) => Distance(x1, y1, (float)x2, (float)y2);
        public static float Distance(int x1, int y1, float x2, float y2) => Distance((float)x1, (float)y1, x2, y2);
        public static float Distance(int x1, int y1, int x2, int y2) => Distance((float)x1, (float)y1, (float)x2, (float)y2);
        public static float Distance(float x1, float y1, float x2, float y2) => Sq(x2 - x1) + Sq(y2 - y1) == 0 ? 0F : Sqrt(Sq(x2 - x1) + Sq(y2 - y1));
        public static float Distance(PointF a, PointF b) => Distance(a.X, a.Y, b.X, b.Y);
        public static float Abs(float K) => Sqrt(Sq(K));
        public static float Normalize(float K) => K / Abs(K);
        public static PointF Normale(PointF a, PointF b) => new PointF(b.Y - b.Y, a.X - b.X).x(1F / Distance(a, b));
        public static float Sq(float i) => i * i;
        public static unsafe float Sqrt(float number)
        {
            float y = number;
            long i = 0x5f3759df - ((*(long*)&y) >> 1);
            y = *(float*)&i;
            y = y * (1.5F - (number * 0.5F * y * y));
            return 1F / y * (1.5F - (number * 0.5F * y * y));
        }
        public static float Length(PointF v) => Sqrt(Sq(v.X) + Sq(v.Y));
        public static PointF Normalized(PointF v)
        {
            float distance = Length(v);
            if (Math.Round(distance, 4) == 0F) return new PointF(0F, 0F);
            return new PointF(v.X / distance, v.Y / distance);
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
        public static PointF AngleToPointF(this float degrees) => new PointF((float)Math.Cos(degrees.ToRadians()), (float)Math.Sin(degrees.ToRadians()));

        public static float Lerp(float v0, float v1, double t)
        {
            return (float)((1D - t) * (double)v0 + t * (double)v1);
        }
        public static PointF Lerp(PointF v0, PointF v1, double t)
        {
            return new PointF(Lerp(v0.X, v1.X, t), Lerp(v0.Y, v1.Y, t));
        }

        public static PointF GetRaycastLine(PointF v, PointF look, float bx, float by, float bw, float bh) => GetRaycastLine(v, look, new RectangleF(bx, by, bw, bh));
        public static PointF GetRaycastLine(PointF r, PointF look, RectangleF bounds)
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
        public static PointF Raycast(PointF s, PointF look, Bitmap map, Color color)
        {
            bool check(PointF _v) => _v.X >= 0F && _v.Y >= 0F && _v.X < map.Width && _v.Y < map.Height;

            PointF e = GetRaycastLine(s, look, 0, 0, map.Width, map.Height);
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
    }
}