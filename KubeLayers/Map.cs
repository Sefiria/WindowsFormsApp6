using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using KubeLayers.entities.minable;

namespace KubeLayers
{
    public class Map
    {
        Dictionary<vec, Tile> Tiles = new Dictionary<vec, Tile>();

        public Map()
        {
        }

        public void Update()
        {
            Tiles.Keys.ToList().ForEach(x => { Tiles[x].Update(); });
        }
        public void Draw()
        {
            Tiles.Keys.ToList().ForEach(x => { Tiles[x].Draw(); });
        }

        public void Gen()
        {
            vec v;
            Tile tile;

            for(int j = -4; j <= 4; j++)
            {
                for (int i = -4; i <= 4; i++)
                {
                    v = (i, j).V();
                    tile = Tile.Random(v);
                    if(i < -1 || i > 1 || j < -1 || j > 1)
                        tile.Entity = new Rock() { TileLocation = v };
                    Tiles[v] = tile;
                }
            }
        }
    }
}
