using System;
using System.Drawing;
using System.Windows;

namespace WindowsFormsApp15.Utilities
{
    public static class Maths
    {
        public static float Distance(float x1, float y1, int x2, int y2) => Distance(x1, y1, (float)x2, (float)y2);
        public static float Distance(int x1, int y1, float x2, float y2) => Distance((float)x1, (float)y1, x2, y2);
        public static float Distance(int x1, int y1, int x2, int y2) => Distance((float)x1, (float)y1, (float)x2, (float)y2);
        public static float Distance(float x1, float y1, float x2, float y2) => Sq(x2 - x1) + Sq(y2 - y1) == 0 ? 0F : Sqrt(Sq(x2 - x1) + Sq(y2 - y1));
        public static float Normalize(float K) => K / Sqrt(Sq(K));
        public static float Sq(float i) => i * i;
        public static unsafe float Sqrt(float number)
        {
            float y = number;
            long i = 0x5f3759df - ((*(long*)&y) >> 1);
            y = *(float*)&i;
            y = y * (1.5F - (number * 0.5F * y * y));
            return 1F / y * (1.5F - (number * 0.5F * y * y));
        }
        public static float Length(vecf v) => Sqrt(Sq(v.x) + Sq(v.y));
        public static vecf Normalized(vecf v)
        {
            float distance = Length(v);
            if (Math.Round(distance, 4) == 0F) return new vecf(0F, 0F);
            return new vecf(v.x / distance, v.y / distance);
        }
        public static bool CollisionPointBox(float curseur_x, float curseur_y, Box box)
        {
            if (curseur_x >= box.x
                && curseur_x < box.x + box.w
                && curseur_y >= box.y
                && curseur_y < box.y + box.h)
                return true;
            else
                return false;
        }
        public static bool CollisionBoxBox(Box box1, Box box2)
        {
            if ((box2.x >= box1.x + box1.w) // trop à droite
                || (box2.x + box2.w <= box1.x) // trop à gauche
                || (box2.y >= box1.y + box1.h)  // trop en bas
                || (box2.y + box2.h <= box1.y)) // trop en haut
                return false;
            else
                return true;
        }
        public static bool CollisionPointCercle(float x, float y, Circle C)
        {
            float d2 = (x - C.x) * (x - C.x) + (y - C.y) * (y - C.y);
            if (d2 > C.r * C.r)
                return false;
            else
                return true;
        }
        public static bool CollisionDroite(vecf A, vecf B, Circle C)
        {
            vecf u = vecf.Zero;
            u.x = B.x - A.x;
            u.y = B.y - A.y;
            vecf AC = vecf.Zero;
            AC.x = C.x - A.x;
            AC.y = C.y - A.y;
            float numerateur = u.x * AC.y - u.y * AC.x;   // norme du vecteur v
            if (numerateur < 0F)
                numerateur = -numerateur;   // valeur absolue ; si c'est négatif, on prend l'opposé.
            float denominateur = Sqrt(u.x * u.x + u.y * u.y);  // norme de u
            float CI = numerateur / denominateur;
            return CI < C.r;
        }
        public static bool CollisionSegment(float Ax, float Ay, float Bx, float By, float Cx, float Cy, float Cr) => CollisionSegment(new vecf(Ax, Ay), new vecf(Bx, By), new Circle(Cx, Cy, Cr));
        public static bool CollisionSegment(float Ax, float Ay, float Bx, float By, Circle C) => CollisionSegment(new vecf(Ax, Ay), new vecf(Bx, By), C);
        public static bool CollisionSegment(vecf A, vecf B, Circle C)
        {
            if (CollisionDroite(A, B, C) == false)
                return false;  // si on ne touche pas la droite, on ne touchera jamais le segment
            vecf AB = vecf.Zero, AC = vecf.Zero, BC = vecf.Zero;
            AB.x = B.x - A.x;
            AB.y = B.y - A.y;
            AC.x = C.x - A.x;
            AC.y = C.y - A.y;
            BC.x = C.x - B.x;
            BC.y = C.y - B.y;
            float pscal1 = AB.x * AC.x + AB.y * AC.y;  // produit scalaire
            float pscal2 = (-AB.x) * BC.x + (-AB.y) * BC.y;  // produit scalaire
            if (pscal1 >= 0F && pscal2 >= 0F)
                return true;   // I entre A et B, ok.
                                // dernière possibilité, A ou B dans le cercle
            if (CollisionPointCercle(A.x, A.y, C))
                return true;
            if (CollisionPointCercle(B.x, B.y, C))
                return true;
            return false;
        }
        /// <returns>
        /// <para>0 : no collision</para> 
        /// <para>1 : corner top-left</para> 
        /// <para>2 : corner bottom-left</para> 
        /// <para>3 : corner top-right</para> 
        /// <para>4 : corner bottom-right</para> 
        /// <para>5 : one inside the other</para> 
        /// <para>6 : segment top</para> 
        /// <para>7 : segment right</para> 
        /// <para>8 : segment bottom</para> 
        /// <para>9 : segment left</para> 
        /// </returns>
        public static int CollisionCercleBox(Circle C1, Box box)
        {
            Box boxCercle = new Box(C1);  // retourner la bounding box de l'image porteuse, ou calculer la bounding box.
                                            // premier test :
            if (CollisionBoxBox(box, boxCercle) == false)
                return 0;// no collision
                            // deuxieme test :
            if (CollisionPointCercle(box.x, box.y, C1)) return 1;// corner top-left
            if (CollisionPointCercle(box.x, box.y + box.h, C1)) return 2;// corner bottom-left
            if (CollisionPointCercle(box.x + box.w, box.y, C1)) return 3;// corner top-right
            if (CollisionPointCercle(box.x + box.w, box.y + box.h, C1)) return 4;// corner bottom-right
                                                                                    // cas E :
            if (CollisionSegment(box.x + box.w, box.y, box.x, box.y, C1.x, C1.y + C1.r, C1.r)) return 6;// segment top
            if (CollisionSegment(box.x + box.w, box.y + box.h, box.x + box.w, box.y, C1.x - (int)(C1.r / 2), C1.y, C1.r)) return 7;// segment right
            if (CollisionSegment(box.x, box.y + box.h, box.x + box.w, box.y + box.h, C1.x, C1.y - (int)(C1.r / 2), C1.r)) return 8;// segment bottom
            if (CollisionSegment(box.x, box.y, box.x, box.y + box.h, C1.x + C1.r, C1.y, C1.r)) return 9;// segment left
                                                                                                        // troisieme test :
            if (CollisionPointBox(C1.x, C1.y, box)) return 5;// one inside the other
                                                                // cas B :
            return 0;// no collision
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

        public static vecf Rotate(this vecf v, float degrees)
        {
            float rad = degrees.ToRadians();
            vecf result = vecf.Zero;
            result.x = v.x * (float)Math.Cos(rad) - v.y * (float)Math.Sin(rad);
            result.y = v.x * (float)Math.Sin(rad) + v.y * (float)Math.Cos(rad);
            return result;
        }

        public static float ToRadians(this float degrees) => (float)Math.PI * degrees / 180F;
        public static vecf AngleToVecf(this float degrees) => new vecf((float)Math.Cos(degrees.ToRadians()), (float)Math.Sin(degrees.ToRadians()));

        public static float Lerp(float v0, float v1, double t)
        {
            return (float)((1D - t) * (double)v0 + t * (double)v1);
        }
        public static vecf Lerp(vecf v0, vecf v1, double t)
        {
            return new vecf(Lerp(v0.x, v1.x, t), Lerp(v0.y, v1.y, t));
        }

        public static vecf GetRaycastLine(vecf v, vecf look, float bx, float by, float bw, float bh) => GetRaycastLine(v, look, new RectangleF(bx, by, bw, bh));
        public static vecf GetRaycastLine(vecf v, vecf look, RectangleF bounds)
        {
            if (look == vecf.Zero)
                return v;
            vecf r = new vecf(v);
            while (r.x > bounds.X && r.x < bounds.Width && r.y > bounds.Y && r.y < bounds.Height)
                r += look;
            if (r.x < bounds.X) r.x = bounds.X;
            if (r.y < bounds.Y) r.y = bounds.Y;
            if (r.x >= bounds.Width) r.x = bounds.Width - 1;
            if (r.y >= bounds.Height) r.y = bounds.Height - 1;
            return r;
        }
        public static vecf Raycast(vecf s, vecf look, Bitmap map, Color color)
        {
            bool check(vecf _v) => _v.x >= 0F && _v.y >= 0F && _v.x < map.Width && _v.y < map.Height;

            vecf e = GetRaycastLine(s, look, 0, 0, map.Width, map.Height);
            double length = new Vector(e.x - s.x, e.y - s.y).Length;
            vecf v, prevv = new vecf(float.NaN, float.NaN);
            for (double t = 0D; t <= 1D; t += 1D / length)
            {
                v = Lerp(s, e, t);
                if (prevv != new vecf(float.NaN, float.NaN) && prevv == v)
                    continue;
                prevv = v;
                if (check(v) && map.GetPixel((int)v.x, (int)v.y).ToArgb() != color.ToArgb())
                    return v;
            }
            return vecf.Zero;
        }
    }
}