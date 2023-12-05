using System.Drawing;
using Tooling;

namespace KubeLayers.tiles
{
    internal class TileDirt : Tile
    {
        public TileDirt(vec v)
            : base(3, v)
        {
            using (Graphics g = Graphics.FromImage(Image))
            {
                g.Clear(Color.DarkGoldenrod.ChangeSaturation(-50));
            }
        }
    }
}
