using System;

namespace WindowsFormsApp3
{
    public class Maths
    {
        public readonly static double PI = 3.14159265359;
        public static float SQ(float i) => i * i;
        public static unsafe float FSqrt(float number)
        {
            float y = number;
            long i = 0x5f3759df - ((*(long*)&y) >> 1);
            y = *(float*)&i;
            y = y * (1.5F - (number * 0.5F * y * y));
            return 1F / y * (1.5F - (number * 0.5F * y * y));
        }
        public static Vecf GetLook(Vecf B, Vecf A) => GetLook(B.X, B.Y, A.X, A.Y);
        public static Vecf GetLook(float Bx, float By, float Ax, float Ay) => new Vecf(Normalize(Bx - Ax), Normalize(By - Ay));
        public static float Normalize(float K) => K / FSqrt(SQ(K));
        public static double DegToRad(double deg) => deg / 180D * PI;
        public static double RadToDeg(double deg) => deg / 180D * PI;
        public static float AngleFromLook(Vecf look)
        {
            float dx = look.X;
            float dy = look.Y;
            return (float)Math.Atan2(dy, dx);
        }
    }
}
