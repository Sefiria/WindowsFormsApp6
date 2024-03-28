using console_v2.res.entities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public class Chunk
    {
        public enum GenerationMode
        {
            Mine=0, Rocailleux, Plaine, Foret
        }

        public static vec ChunkSize = (24, 16).V();
        public static Dictionary<GenerationMode, Dictionary<float, Sols>> LayeredGrounds = new Dictionary<GenerationMode, Dictionary<float, Sols>>
        {
            [GenerationMode.Mine] = new Dictionary<float, Sols>{
                [0.0f] = Sols.Pierre,
                [0.8f] = Sols.Terre,
            },
            [GenerationMode.Rocailleux] = new Dictionary<float, Sols>
            {
                [0.0f] = Sols.Pierre,
                [0.6f] = Sols.Terre,
                [0.95f] = Sols.Herbe,
            },
            [GenerationMode.Plaine] = new Dictionary<float, Sols>
            {
                [0.0f] = Sols.Pierre,
                [0.1f] = Sols.Terre,
                [0.7f] = Sols.Herbe,
            },
            [GenerationMode.Foret] = new Dictionary<float, Sols>
            {
                [0.0f] = Sols.Terre,
                [0.4f] = Sols.Herbe,
            },
        };

        public vec Index;
        public Dictionary<vec, Tile> Tiles = new Dictionary<vec, Tile>();
        public List<Entity> Entities = new List<Entity>();
        public Chunk(vec index)
        {
            Index = index;
        }

        public void Update()
        {
            var entities = new List<Entity>(Entities);
            entities.ForEach(e => { if (e.Exists == false) Entities.Remove(e); else e.Update(); });
        }

        public void TickSecond()
        {
            new List<Entity>(Entities).ForEach(e => { if (e.Exists == false) Entities.Remove(e); else e.TickSecond(); });
        }

        public void Draw(Graphics g)
        {
            string result = "";
            for (int y = 0; y < ChunkSize.y; y++)
                for (int x = 0; x < ChunkSize.x; x++)
                    result += (char)DB.Resources[(int)Tiles[(x, y).V()].Sol];
            GraphicsManager.DrawString(g, result, Brushes.DimGray, vec.Zero);

            var entities = new List<Entity>(Entities);
            entities.ForEach(e => { if (e.Exists) e.Draw(g); });
        }


        public static Chunk Generate(GenerationMode mode, vec chunk_index)
        {
            Chunk chunk = new Chunk(chunk_index);

            int w = ChunkSize.x;
            int h = ChunkSize.y;
            int SEED = (int)DateTime.UtcNow.Ticks;
            System.Threading.Thread.Sleep(10);
            int FREQ = 10;
            INoise noise = new ValueNoise(SEED, FREQ);
            float[,] arr = new float[w, h];
            vec index;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    float fx = x / (w - 1.0f);
                    float fy = y / (h - 1.0f);

                    arr[x, y] = noise.Sample2D(fx, fy);
                }
            }

            ValueNoise.NormalizeArray(ref arr, w, h);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    index = (x, y).V();
                    chunk.Tiles[index] = new Tile(index) { Sol = GetLayeredGrounds(mode, arr[x, y]) };
                }
            }

            GenerateEntities(mode, chunk);

            return chunk;
        }

        private static void GenerateEntities(GenerationMode mode, Chunk chunk)
        {
            vec v;

            // Generate Trees

            var treesCount = RandomThings.rnd(Maths.Sq((int)mode) * 5);
            for (int i = 0; i < treesCount; i++)
            {
                do { v = (RandomThings.rnd(ChunkSize.x), RandomThings.rnd(ChunkSize.y)).V();}
                while (chunk.Entities.At(v) != null);
                chunk.Entities.Add(new EntityTree(v, false));
            }
        }

        private static Sols GetLayeredGrounds(GenerationMode mode, float layer)
        {
            var layers = LayeredGrounds[mode].Keys.ToList();
            var result = layers[0];
            for (int i = 0; i < layers.Count; i++)
                if (layer > layers[i]) result = layers[i];
            return LayeredGrounds[mode][result];
        }
    }
}
