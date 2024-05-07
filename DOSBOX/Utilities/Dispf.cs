using Tooling;

namespace DOSBOX.Utilities
{
    public abstract class Dispf
    {
        protected bool DisplayModuloScreen = false;
        protected bool DisplayCenterSprite = false;
        protected bool DisplayAlwaysExact = false;

        public vecf vec = new vecf{ x=0, y=0};
        public byte[,] g;
        public byte scale = 1;
        public int _w => g.GetLength(0);
        public int _h => g.GetLength(1);
        public void DisplayExact(int layer)
        {
            int w = Core.Layers[layer].GetLength(0);
            int h = Core.Layers[layer].GetLength(1);

            int x = (int) vec.x;
            int y = (int) vec.y;

            for (int i = 0; i < _w * scale; i++)
                for (int j = 0; j < _h * scale; j++)
                    if (x + i >= 0 && x + i < w && y + j >= 0 && y + j < h)
                        if (g[j / scale, i / scale] != (layer == 0 ? 4 : 0))
                            Core.Layers[layer][x + i, y + j] = g[j / scale, i / scale];
        }
        public void Display(int layer, vec srcLoc = null)
        {
            if(DisplayAlwaysExact)
            {
                DisplayExact(layer);
                return;
            }

            int w = Core.Layers[layer].GetLength(0);
            int h = Core.Layers[layer].GetLength(1);

            int x = (int) vec.x + (srcLoc != null ? srcLoc.x : 0);
            int y = (int) vec.y + (srcLoc != null ? srcLoc.y : 0);

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

            for (int i = 0; i < _w * scale; i++)
                for (int j = 0; j < _h * scale; j++)
                    if(x + _w - i - 1 >= 0 && x + _w - i - 1 < w && y + j >= 0 && y + j < h)
                        if(g[i / scale, j / scale] != (layer == 0 ? 4 : 0))
                            Core.Layers[layer][x + _w - i - 1, y + j] = g[i / scale, j / scale];
        }
    }
}
