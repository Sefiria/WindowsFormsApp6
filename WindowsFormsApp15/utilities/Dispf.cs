using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp15.Utilities
{
    public abstract class Dispf
    {
        protected bool DisplayModuloScreen = false;
        protected bool DisplayCenterSprite = false;
        protected bool DisplayAlwaysExact = false;

        public vecf vec = new vecf{ x=0, y=0};
        public Bitmap g;
        public byte scale = 1;
        public int _w => g.Width;
        public int _h => g.Height;
        public void Display(vecf srcLoc = null)
        {
            int w = Core.rw;
            int h = Core.rh;

            int x = (int) (vec.x + (srcLoc != null ? srcLoc.x : 0));
            int y = (int) (vec.y + (srcLoc != null ? srcLoc.y : 0));

            if(DisplayCenterSprite)
            {
                x -= _w / 2;
                y -= _h / 2;
            }

            if(DisplayModuloScreen)
            {
                x = x % 64;
                y = y % 64;
            }

            Core.g.DrawImage(g, x, y);
        }
    }
}
