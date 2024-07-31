using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.fusion
{
    internal class RoomTiles
    {
        public static List<Tile> RefTiles = new List<Tile>()
        {
            new Tile() { Type = Tile.TYPE.EMPTY, Pixels = new byte[8, 8] },
            new Tile(Tile.TYPE.SOLID, "8,8,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,3,1,1,1,1,1,1,0,3,1,1,1,1,1,1,0,3,3,3,3,3,3,3,3,0,0,0,0,3,0,0,0,1,1,1,0,3,1,1,1,1,1,1,0,3,1,1,1" ),
            new Tile(Tile.TYPE.SOLID, "8,8,3,3,3,3,3,3,3,3,3,2,0,0,0,0,2,3,3,0,2,0,0,2,0,3,3,0,0,2,2,0,0,3,3,0,0,2,2,0,0,3,3,0,2,0,0,2,0,3,3,2,0,0,0,0,2,3,3,3,3,3,3,3,3,3" ),
            new Tile(Tile.TYPE.FRONT, "8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,0,0,0,0,3,3,3,3,3,3,0,3,3,4,4,4,4,3,3,3,4,3,3,3,3,4,3,3,3,3,3,3,3,3,3" ),
        };
    }
}
