using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DOSBOX.Suggestions.city.User;

namespace DOSBOX.Suggestions.city
{
    internal partial class IndexedBlocks
    {
        public static List<Tile> RefTiles = new List<Tile>();

        static IndexedBlocks()
        {
            RefTiles.Add(new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] });
            RefTiles.AddRange(GetRootFilled());
            RefTiles.AddRange(GetRootLight());
            RefTiles.AddRange(GetExt());
            RefTiles.AddRange(GetStructures());
        }
    }
}
