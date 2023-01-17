using System;
using System.Drawing;

namespace WindowsFormsApp12
{
    public class Maths
    {
        public readonly static double PI = 3.14159265359;

        public static float Distance(float x1, float y1, int x2, int y2) => Distance(x1, y1, (float)x2, (float)y2);
        public static float Distance(int x1, int y1, float x2, float y2) => Distance((float)x1, (float)y1, x2, y2);
        public static float Distance(int x1, int y1, int x2, int y2) => Distance((float)x1, (float)y1, (float)x2, (float)y2);
        public static float Distance(float x1, float y1, float x2, float y2) => FSqrt(SQ(x2 -x1) + SQ(y2 - y1));

        public static float SQ(float i) => i * i;
        public static unsafe float FSqrt(float number)
        {
            float y = number;
            long i = 0x5f3759df - ((*(long*)&y) >> 1);
            y = *(float*)&i;
            y = y * (1.5F - (number * 0.5F * y * y));
            return 1F / y * (1.5F - (number * 0.5F * y * y));
        }
        public static PointF GetLook(PointF B, PointF A) => GetLook(B.X, B.Y, A.X, A.Y);
        public static PointF GetLook(int Bx, int By, int Ax, int Ay) => GetLook((float)Bx, (float)By, (float)Ax, (float)Ay);
        public static PointF GetLook(float Bx, float By, int Ax, int Ay) => GetLook(Bx, By, (float)Ax, (float)Ay);
        public static PointF GetLook(int Bx, int By, float Ax, float Ay) => GetLook((float)Bx, (float)By, Ax, Ay);
        public static PointF GetLook(float Bx, float By, float Ax, float Ay) => new PointF(Normalize(Bx - Ax), Normalize(By - Ay));
        public static float Normalize(float K) => K / FSqrt(SQ(K));
        public static double DegToRad(double deg) => deg / 180D * PI;
        public static double RadToDeg(double deg) => deg / 180D * PI;
        public static float AngleFromLook(PointF look)
        {
            float dx = look.X;
            float dy = look.Y;
            return (float)Math.Atan2(dy, dx);
        }

        public static float Lerp(float v0, float v1, float t)
        {
            return (1F - t) * v0 + t * v1;
        }
    }
}
