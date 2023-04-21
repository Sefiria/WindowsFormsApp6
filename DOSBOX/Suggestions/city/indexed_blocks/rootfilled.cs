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
        static List<Tile> GetRootFilled()
        {
            var humanreadableTiles = new List<Tile>
                {
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]  // 1
                    {
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 2
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 2, 2, 2, 2, 2, 2, 2, 2 },
                        { 2, 2, 2, 2, 2, 2, 2, 2 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 3
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 2, 2, 2, 2 },
                        { 0, 0, 0, 2, 2, 2, 2, 2 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 4
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 2, 2, 2, 2, 0, 0, 0, 0 },
                        { 2, 2, 2, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 5
                    {
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 2, 2, 2 },
                        { 0, 0, 0, 0, 2, 2, 2, 2 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 6
                    {
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 2, 2, 2, 2, 2, 0, 0, 0 },
                        { 2, 2, 2, 2, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                };

            var tiles = new List<Tile>();
            foreach (var t in humanreadableTiles)
            {
                var tile = new Tile() { Type = t.Type, Pixels = new byte[8, 8] };
                for (int x = 0; x < 8; x++)
                    for (int y = 0; y < 8; y++)
                        tile.Pixels[x, y] = t[y, x];
                tiles.Add(tile);
            }
            return tiles;
        }
    }
}
