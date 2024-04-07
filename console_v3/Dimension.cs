using System;
using System.Collections.Generic;
using System.Linq;
using Tooling;
using static console_v3.Chunk;

namespace console_v3
{
    public class Dimension
    {
        public vec Index;
        public Dictionary<vec, Chunk> Chunks = new Dictionary<vec, Chunk>();
        public vec CurrentChunkCoord = vec.Zero;
        public Chunk CurrentChunk => Chunks[CurrentChunkCoord];
        public KeyValuePair<vec, Chunk> GetChunk(vec tile_coord) => Chunks.FirstOrDefault(c => c.Value.Tiles.ContainsKey(tile_coord - c.Key * Chunk.ChunkSize));
        public int GEN_SEED = (int)DateTime.UtcNow.Ticks;
        public int GEN_FREQ = 20;
        public INoise GEN_NOISE;

        vec[,] m = new vec[3, 3]{
                { (-1, -1).V(), (0, -1).V(), (1, -1).V() },
                { (-1,  0).V(), (0,  0).V(), (1,  0).V() },
                { (-1,  1).V(), (0,  1).V(), (1,  1).V() }
        };

        public Chunk this[vec chunk] => Chunks[chunk];

        public Dimension(vec index)
        {
            Index = index;
            GEN_NOISE = new ValueNoise(GEN_SEED, GEN_FREQ);
            CreateChunks();
        }

        public void Update()
        {
            Chunks.Keys.ToArray().ToList().ForEach(c => Chunks[c].Update());
            if (Core.Instance.TheGuy.HasMoved)
            {
                UpdateMoveCreateChunks();
            }
        }
        public void TickSecond()
        {
            Chunks.Keys.ToArray().ToList().ForEach(c => Chunks[c].TickSecond());
        }

        private void CreateChunks()
        {
            var tg = Core.Instance.TheGuy;
            var chunk = tg?.CurChunk ?? vec.Zero;
            vec v;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    v = chunk + (x, y).V();
                    if (!Chunks.ContainsKey(v))
                    {
                        var gen = (GEN_NOISE.Sample2D(v.x / 40f, v.y / 40f) + 1f) / 2f;
                        Chunks[v] = Generate((GenerationMode)((Enum.GetNames(typeof(GenerationMode)).Length - 1) * gen), v);
                    }
                }
            }
        }
        private void UpdateMoveCreateChunks()
        {
            var tg = Core.Instance.TheGuy;
            var chunk = tg?.CurChunk ?? vec.Zero;
            vec d = tg.Direction;
            vec v = chunk + d;
            if (d.x != -1 && d.x != 1 && d.y != -1 && d.y != 1)
                return;
            if (!Chunks.ContainsKey(v))
            {
                var gen = (GEN_NOISE.Sample2D(v.x / 99f, v.y / 99f) + 1f) / 2f;
                Chunks[v] = Generate((GenerationMode)(Enum.GetNames(typeof(GenerationMode)).Length * gen), v);
            }
        }
    }
}
