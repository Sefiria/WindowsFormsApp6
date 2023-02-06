using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC
{
    public static class ResourceFactory
    {
        public static Resource Create(int x, int y)
        {
            switch(WorldMgr.Tiles[x, y])
            {
                default:
                case TileTypes.Grass: return CreateOnGrass(x, y);
                case TileTypes.Stone: return CreateOnStone(x, y);
                case TileTypes.Sand: return CreateOnSand(x, y);
            }
        }
        public static Resource CreateOnGrass(int x, int y)
        {
            switch (Core.RND.Next(2))
            {
                default:
                case 0: return new Plant(x, y);
                case 1: return new Tree(x, y);
            }
        }
        public static Resource CreateOnStone(int x, int y)
        {
            switch (Core.RND.Next(1))
            {
                default:
                case 0: return new Stone(x, y);
            }
        }
        public static Resource CreateOnSand(int x, int y)
        {
            switch (Core.RND.Next(1))
            {
                default:
                case 0: return new Sand(x, y);
            }
        }
    }
}
