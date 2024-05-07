using Tooling;

namespace DOSBOX.Utilities
{
    public abstract class Disp
    {
        protected bool DisplayAlwaysExact = false;

        public vec vec = new vec{ x=0, y=0};
        public byte[,] g;
        public byte scale = 1;
        public int _w => g.GetLength(0);
        public int _h => g.GetLength(1);
        public virtual void DisplayExact(int layer, vec srcLoc = null)
        {
            int w = Core.Layers[layer].GetLength(0);
            int h = Core.Layers[layer].GetLength(1);

            int x = vec.x + (srcLoc != null ? srcLoc.x : 0);
            int y = vec.y + (srcLoc != null ? srcLoc.y : 0);

            for (int i = 0; i < _w * scale; i++)
                for (int j = 0; j < _h * scale; j++)
                    if (x + i >= 0 && x + i < w && y + j >= 0 && y + j < h)
                        if (g[i / scale, j / scale] != (layer == 0 ? 4 : 0))
                            Core.Layers[layer][x + i, y + j] = g[i / scale, j / scale];
        }
        public virtual void Display(int layer, vec srcLoc = null, byte[,] ActiveBG = null)
        {
            if (DisplayAlwaysExact)
            {
                DisplayExact(layer);
                return;
            }

            int w = (ActiveBG == null ? Core.Layers[layer] : ActiveBG).GetLength(0);
            int h = (ActiveBG == null ? Core.Layers[layer] : ActiveBG).GetLength(1);

            int x = vec.x + (srcLoc != null ? srcLoc.x : 0);
            int y = vec.y + (srcLoc != null ? srcLoc.y : 0);

            for (int i = 0; i < _w * scale; i++)
                for (int j = 0; j < _h * scale; j++)
                    if(x + _w - i - 1 >= 0 && x + _w - i - 1 < w && y + j >= 0 && y + j < h)
                        if(g[i / scale, j / scale] != (layer == 0 ? 4 : 0))
                            (ActiveBG == null ? Core.Layers[layer] : ActiveBG)[x + _w - i - 1, y + j] = g[i / scale, j / scale];
        }
    }
}
