using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Utilities
{
    public abstract class Dispf
    {
        public vecf vec = new vecf{ x=0, y=0};
        public byte[,] g;
        public byte scale = 1;
        public int _w => g.GetLength(0);
        public int _h => g.GetLength(1);
        public void Display(int layer, vec srcLoc = null)
        {
            int w = Core.Layers[layer].GetLength(0);
            int h = Core.Layers[layer].GetLength(1);

            int x = (int) vec.x + (srcLoc != null ? srcLoc.x : 0);
            int y = (int) vec.y + (srcLoc != null ? srcLoc.y : 0);

            for (int i = 0; i < _w * scale; i++)
                for (int j = 0; j < _h * scale; j++)
                    if(x + _w - i - 1 >= 0 && x + _w - i - 1 < w && y + j >= 0 && y + j < h)
                        Core.Layers[layer][x + _w - i - 1, y + j] = g[i / scale, j / scale];
        }
    }
}
