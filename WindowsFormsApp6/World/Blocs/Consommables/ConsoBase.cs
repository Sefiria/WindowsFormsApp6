using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6.World.Blocs.Consommables
{
    public class ConsoBase
    {
        public int X, Y;
        public Bitmap Image = null, ImageUsed = null;
        public bool Used = false;

        public Bitmap GetImage() => Used ? ImageUsed : Image;

        public ConsoBase(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Draw()
        {
            if (Used == false)
            {
                if (Image != null)
                    Core.g.DrawImage(Image, X * Core.TileSz, Y * Core.TileSz - (Image.Height - Core.TileSz));
            }
            else
            {
                if (ImageUsed != null)
                    Core.g.DrawImage(ImageUsed, X * Core.TileSz, Y * Core.TileSz);
            }
        }
    }
}
