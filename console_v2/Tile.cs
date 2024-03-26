using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public class Tile
    {
        public static Tile GetFromWorldLocation(vec tile_coords) => Core.Instance.SceneAdventure.World.GetTile(tile_coords);

        public vec Index;
        public vecf Coords => Index.f * ((PointF)GraphicsManager.CharSize).vecf();
        public Sols Sol;
        public Murs Mur;
        public List<int> Content = new List<int>();
        public IEnumerable<int> Entities => Content.Where(x => x.IsntBlockingType());
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
