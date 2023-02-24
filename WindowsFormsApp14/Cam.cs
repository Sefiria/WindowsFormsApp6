using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp14
{
    internal class Cam
    {
        public vecf vec;
        public float angle, fov = 25F;

        public vecf look => angle.AngleToVecf();

        public Cam(float x = float.NaN, float y = float.NaN, float angle = 0F)
        {
            vec = new vecf(float.IsNaN(x) ? Core.Map.hw : x, float.IsNaN(y) ? Core.Map.hh : y);
            this.angle = angle;
        }

        public void Update()
        {
            while (angle < 0F) angle += 360;
            while (angle >= 360F) angle -= 360;
        }


        internal void StrafeLeft()
        {
            vec += look.Rotate(-90);
        }
        internal void StrafeRight()
        {
            vec += look.Rotate(90);
        }
        internal void MoveForward()
        {
            vec += look;
        }
        internal void MoveBackward()
        {
            vec -= look;
        }
    }
}
