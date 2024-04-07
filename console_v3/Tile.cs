using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3
{
    public class Tile
    {
        public static Tile GetFromWorldLocation(vec tile_coords) => Core.Instance.SceneAdventure.World.GetTile(tile_coords);

        public vec Index;
        public vecf Coords => Index.f * GraphicsManager.TileSize;
        public int Value;
        public Tile(vec index)
        {
            Index = index;
        }
        public void Update()
        {
        }
        public void Draw(Graphics g)
        {
            GraphicsManager.DrawTile(g, this, Coords.i);
        }
    }
}
