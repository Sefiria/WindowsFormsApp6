using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp20
{
    public static class Common
    {
        public static Vector3 MultiplyPoint(this Matrix4x4 m, Vector3 point)
        {
            float x = m.M11 * point.X + m.M12 * point.Y + m.M13 * point.Z + m.M14 * 1;
            float y = m.M21 * point.X + m.M22 * point.Y + m.M23 * point.Z + m.M24 * 1;
            float z = m.M31 * point.X + m.M32 * point.Y + m.M33 * point.Z + m.M34 * 1;
            float w = m.M41 * point.X + m.M42 * point.Y + m.M43 * point.Z + m.M44 * 1;

            // If the w coordinate is not 1, divide the x, y, and z coordinates by w
            if (w != 1)
            {
                x /= w;
                y /= w;
                z /= w;
            }

            return new Vector3(x, y, z);
        }
    }
}
