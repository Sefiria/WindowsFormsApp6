using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotile
{
    public class Tile
    {
        public Bitmap Current;

        public Tile() { }
        public Tile(Bitmap img)
        {
            Current = new Bitmap(img, Data.TileSize, Data.TileSize);
        }
    }
}
