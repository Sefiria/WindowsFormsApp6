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
        static List<Tile> GetRootLight()
        {
            var humanreadableTiles = new List<Tile>
                {
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 7
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 8
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 2, 2, 0 },
                        { 0, 2, 2, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 9
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 10
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 11
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 12
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 13
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 2, 2, 0 },
                        { 0, 2, 2, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 14
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 2, 2, 0 },
                        { 0, 2, 2, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 15
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 16
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8] // 17
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 2, 2, 0, 0, 2, 2, 0 },
                        { 0, 2, 2, 0, 0, 2, 2, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 2, 0, 0, 0 },
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
