using System.Drawing;
using Tooling;

namespace KubeLayers.tiles
{
    internal class TileGrass : Tile
    {
        public TileGrass(vec v)
            : base(1, v)
        {
            using (Graphics g = Graphics.FromImage(Image))
            {
                g.Clear(Color.Green.ChangeSaturation(-50));
            }
        }
    }
}
