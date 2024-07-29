using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.fusion
{
    internal class RoomTiles
    {
        public static List<Tile> RefTiles = null;
        public static void Load()
        {
            if (RefTiles == null)
                DefineTiles();
        }
        public static void DefineTiles()
        {
            var humanreadableTiles = new List<Tile>
                {
                    new Tile() { Type = Tile.TYPE.EMPTY, Pixels = new byte[8, 8] },
                    new Tile() { Type = Tile.TYPE.SOLID, Pixels = new byte[8, 8]
                    {
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 3, 0, 0, 0, 0, 0, 0, 0 },
                        { 3, 1, 1, 1, 1, 1, 1, 0 },
                        { 3, 1, 1, 1, 1, 1, 1, 0 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 0, 0, 0, 0, 3, 0, 0, 0 },
                        { 1, 1, 1, 0, 3, 1, 1, 1 },
                        { 1, 1, 1, 0, 3, 1, 1, 1 },
                    } },
                    new Tile() { Type = Tile.TYPE.SOLID, Pixels = new byte[8, 8]
                    {
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 3, 2, 0, 0, 0, 0, 2, 3 },
                        { 3, 0, 2, 0, 0, 2, 0, 3 },
                        { 3, 0, 0, 2, 2, 0, 0, 3 },
                        { 3, 0, 0, 2, 2, 0, 0, 3 },
                        { 3, 0, 2, 0, 0, 2, 0, 3 },
                        { 3, 2, 0, 0, 0, 0, 2, 3 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                    } },
                    new Tile() { Type = Tile.TYPE.FRONT, Pixels = new byte[8, 8]
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 3, 3, 0, 0, 0 },
                        { 0, 3, 3, 3, 3, 3, 3, 0 },
                        { 3, 3, 4, 4, 4, 4, 3, 3 },
                        { 3, 4, 3, 3, 3, 3, 4, 3 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                    } },
                };

            RefTiles = new List<Tile>();
            foreach (var t in humanreadableTiles)
            {
                var tile = new Tile() { Type = t.Type, Pixels = new byte[8, 8] };
                for (int x = 0; x < 8; x++)
                    for (int y = 0; y < 8; y++)
                        tile.Pixels[x, y] = t[y, x];
                RefTiles.Add(tile);
            }
        }
    }
}
