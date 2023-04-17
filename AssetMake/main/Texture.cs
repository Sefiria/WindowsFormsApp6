using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMake.main
{
    internal class Texture
    {
        public Bitmap Image { get; set; }

        public Texture()
        {

        }
        public Texture(Bitmap image)
        {
            Image = image;
        }
    }
}
