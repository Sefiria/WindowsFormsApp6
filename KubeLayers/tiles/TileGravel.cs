using System.Drawing;
using Tooling;

namespace KubeLayers.tiles
{
    internal class TileGravel : Tile
    {
        public TileGravel(vec v)
            : base(2, v)
        {
            using (Graphics g = Graphics.FromImage(Image))
            {
                g.Clear(Color.Gray.ChangeSaturation(-50));
            }
        }
    }
}
