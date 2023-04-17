using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMake.main
{
    internal class Animation
    {
        public List<Bitmap> Frames = new List<Bitmap>();

        public Animation()
        {

        }
        public Animation(List<Bitmap> src)
        {
            Frames = new List<Bitmap>(src);
        }
    }
}
