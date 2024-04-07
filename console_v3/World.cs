using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3
{
    public class World
    {
        public Dictionary<vec, Dimension> Dimensions = new Dictionary<vec, Dimension>();
        public vec CurrentDimensionCoord => Core.Instance.TheGuy?.CurDimension ?? vec.Zero;
        public vec CurrentChunkCoord => Core.Instance.TheGuy?.CurChunk ?? vec.Zero;
        public Dimension CurrentDimension => Dimensions[CurrentDimensionCoord];
        public Dimension GetDimension(vec dimension_coord)
        {
            if (Dimensions.ContainsKey(dimension_coord))
                return Dimensions[dimension_coord];
            return null;
        }
        public Chunk GetChunk(vec dimension_coord, vec chunk_coord)
        {
            var dimension = GetDimension(dimension_coord);
            if (dimension.Chunks.ContainsKey(chunk_coord))
                return dimension.Chunks[chunk_coord];
            return null;
        }
        public Dimension GetDimension() => Dimensions[CurrentDimensionCoord];
        public Chunk GetChunk(vec chunk_coord) => GetChunk(CurrentDimensionCoord, chunk_coord);
        public Chunk GetChunk() => GetChunk(CurrentChunkCoord);
        public Tile GetTile(vec tile_coords) => GetChunk()?.Tiles.FirstOrDefault(t => t.Value.Index == tile_coords).Value;
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

        public void Draw(Graphics g, Graphics gui)
        {
            var tg = Core.Instance.TheGuy;
            Dimensions[tg.CurDimension].Chunks[tg.CurChunk].Draw(Core.Instance.g);
            MinimapManager.Draw(gui);
        }

        internal bool IsntBlocking(vec dimension, vec chunk, vec tile)
        {
            if (!Dimensions.ContainsKey(dimension)) return false;
            if (!Dimensions[dimension].Chunks.ContainsKey(chunk)) return false;
            if (Dimensions[dimension].Chunks[chunk].Entities.FirstOrDefault(e => e.Position.ToTile() == tile) != null) return false;
            return true;
        }
        internal bool IsBlocking(vec d, vec c, vec t) => !IsBlocking(d, c, t);
    }
}
