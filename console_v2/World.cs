using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public class World
    {
        public Dictionary<vec, Dimension> Dimensions = new Dictionary<vec, Dimension>();
        public vec CurrentDimensionCoord = vec.Zero;
        public Dimension CurrentDimension => Dimensions[CurrentDimensionCoord];
        public Chunk GetChunk(vec dimention_coord, vec tile_coord) => Dimensions[dimention_coord].Chunks.FirstOrDefault(x => x.Value.Tiles.Any(t => t.Key == tile_coord)).Value;
        public Tile GetTile(vec tile_coords) => GetChunk(CurrentDimensionCoord, tile_coords)?.Tiles.FirstOrDefault(t => t.Value.Index == tile_coords).Value;
        public KeyValuePair<vec, Chunk> GetCurrentDimensionChunk(vec tile_coord) => CurrentDimension.Chunks.FirstOrDefault(c => c.Value.Tiles.ContainsKey(tile_coord));
        public World()
        {
        }

        public void Update()
        {
            CurrentDimension.Update();
        }

        public void TickSecond()
        {
            Dimensions.Keys.ToArray().ToList().ForEach(d => Dimensions[d].TickSecond());
        }

        public void Draw(Graphics g)
        {
            var tg = Core.Instance.TheGuy;
            Dimensions[tg.CurDimension].Chunks[tg.CurChunk].Draw(Core.Instance.g);
        }
        public bool IsBlocking(Tile tile) => ((int)tile.Sol).IsBlockingType();
        public bool IsBlocking(vec tile_coord) => ((int?)Tile.GetFromWorldLocation(tile_coord)?.Sol)?.IsBlockingType() ?? true;
        public bool IsntBlocking(Tile tile) => !IsBlocking(tile);
        public bool IsntBlocking(vec tile_coord, vec nxt_chunk) => !IsBlocking(tile_coord);

        internal bool IsntBlocking(vec dimension, vec chunk, vec tile)
        {
            if (!Dimensions.ContainsKey(dimension)) return false;
            if (!Dimensions[dimension].Chunks.ContainsKey(chunk)) return false;
            if (Dimensions[dimension].Chunks[chunk].Entities.FirstOrDefault(e => e.Position.ToTile() == tile) != null) return false;
            return ((int)Dimensions[dimension].Chunks[chunk].Tiles[tile].Sol).IsntBlockingType();
        }
        internal bool IsBlocking(vec d, vec c, vec t) => !IsBlocking(d, c, t);
    }
}
