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
        static List<Tile> GetExt()
        {
            var humanreadableTiles = new List<Tile>
                {
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]
                    {
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]
                    {
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]
                    {
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]
                    {
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]
                    {
                        { 3, 3, 3, 3, 3, 3, 3, 0 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]
                    {
                        { 0, 3, 3, 3, 3, 3, 3, 3 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]
                    {
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 1, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]
                    {
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 1, 1, 1, 1, 1, 1, 1, 3 },
                        { 3, 3, 3, 3, 3, 3, 3, 0 },
                    } },
                    new Tile() { Type = Tile.TYPE.GROUND, Pixels = new byte[8, 8]
                    {
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 3, 1, 1, 1, 1, 1, 1, 1 },
                        { 0, 3, 3, 3, 3, 3, 3, 3 },
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
