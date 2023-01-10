using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp11.Bullets;
using WindowsFormsApp11.Travelers;

namespace WindowsFormsApp11
{
    public static class Maths
    {
        public static bool IsColliding(Entity A, Entity B) => IsColliding(A.Rect, B.Rect);
        public static bool IsColliding(Rectangle A, Rectangle B)
        {
            bool res = A.IntersectsWith(B);
            return res;
        }
        public static unsafe float FSqrt(float number)
        {
            float y = number;
            long i = 0x5f3759df - ((*(long*)&y) >> 1);
            y = *(float*)&i;
            y = y * (1.5F - (number * 0.5F * y * y));
            return 1F / y * (1.5F - (number * 0.5F * y * y));
        }
        public static float ToRad(this float degrees) => (float)(degrees * Math.PI / 180D);
        public static float ToDeg(this float radians) => (float)(radians / Math.PI * 180D);
    }
}
