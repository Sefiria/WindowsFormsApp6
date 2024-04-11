using console_v3.res.entities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3
{
    public class Chunk
    {
        public static vec ChunkSize = (24, 24).V();
        public static Dictionary<GenerationMode, Dictionary<float, int>> LayeredGrounds = new Dictionary<GenerationMode, Dictionary<float, int>>
        {
            [GenerationMode.Cave] = new Dictionary<float, int>
            {
                [0.0f] = (int)DB.Tex.Rock,
                [0.8f] = (int)DB.Tex.Dirt,
            },
            [GenerationMode.Rocky] = new Dictionary<float, int>
            {
                [0.0f] = (int)DB.Tex.Rock,
                [0.6f] = (int)DB.Tex.Dirt,
                [0.95f] = (int)DB.Tex.Grass,
            },
            [GenerationMode.Plain] = new Dictionary<float, int>
            {
                [0.0f] = (int)DB.Tex.Rock,
                [0.1f] = (int)DB.Tex.Dirt,
                [0.7f] = (int)DB.Tex.Grass,
            },
            [GenerationMode.Forest] = new Dictionary<float, int>
            {
                [0.0f] = (int)DB.Tex.Dirt,
                [0.4f] = (int)DB.Tex.Grass,
            },
        };

        public vec Index;
        public Dictionary<vec, Tile> Tiles = new Dictionary<vec, Tile>();
        public List<Entity> Entities = new List<Entity>();
        public bool AlreadyVisited = false;
        public GenerationMode Layer;

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
            const float LUM_DIST = 6F;
            const float LUM_BRIGHT = 0.4F;

            List<(vecf vf, int res)> specials = new List<(vecf vf, int res)>();
            Bitmap tex;
            float time = Core.Instance.SceneAdventure.Time;
            float timelum = LUM_BRIGHT * (((int)(time / 12) == 0 ? time : 12F - (time - 11F)) % 12F) / 12F;
            IEnumerable<float> torchs_near;
            Tile tile;
            
            // apply adjusted luminosity only on common temp tex (3 / 4 in the chunk) and then draw them
            var texs = Tiles.Values.Select(t => t.Value).Distinct().ToDictionary(t => t, t =>DB.GetTexture(t).GetAdjusted(1f - LUM_BRIGHT + timelum));

            // torchs locations
            var torchs_coords = Entities.Where(e => e.DBRef == (int)DB.Tex.Torch).Select(t => t.Tile);

            // opti : adjust for lum only single time, others just use cached tex
            // tile has to have same value and same luminosity
            var cached_adjusted_texs = new Dictionary<(int type, float lum), Bitmap>();

            for (int y = 0; y < ChunkSize.y; y++)
            {
                for (int x = 0; x < ChunkSize.x; x++)
                {
                    tile = Tiles[(x, y).V()];
                    tex = texs[tile.Value];
                    torchs_near = torchs_coords.Select(c => c.Distance(tile.Index)).Where(d => d <= LUM_DIST);
                    float lum = torchs_near.Count() == 0 ? 0F : (LUM_DIST - torchs_near.Min()) * (LUM_BRIGHT / LUM_DIST);
                    if (lum > 0f)
                    {
                        if (cached_adjusted_texs.ContainsKey((tile.Value, lum)))
                            tex = cached_adjusted_texs[(tile.Value, lum)];
                        else
                            tex = cached_adjusted_texs[(tile.Value, lum)] = tex.GetAdjusted(1.001f + lum * (LUM_BRIGHT - timelum) * 1.2f);
                    }
                    GraphicsManager.Draw(g, tex, (x * GraphicsManager.TileSize, y * GraphicsManager.TileSize).Vf());
                }
            }
            new List<Entity>(Entities).ForEach(e => { if (e.Exists) e.Draw(g); });
        }


        public static Chunk Generate(GenerationMode mode, vec chunk_index)
        {
            Chunk chunk = new Chunk(chunk_index);

            chunk.Layer = mode;
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
                    chunk.Tiles[index] = new Tile(index, GetLayeredGrounds(mode, arr[x, y]));
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

            //// RESOURCES

            // Generate Plants

            var available_harvestableGrounds = chunk.Tiles.Where(tile => chunk.Entities.At(tile.Key) == null && DB.HarvestableGrounds.Contains(tile.Value.Value)).Select(tile => tile.Key).ToList();
            var plantsCount = RandomThings.rnd(Math.Min(2 * (int)mode, available_harvestableGrounds.Count / 4));
            for (int i = 0; i < plantsCount; i++)
            {
                var rnd_id = RandomThings.rnd(available_harvestableGrounds.Count);
                var plant = DB.Plants[RandomThings.rnd(DB.Plants.Count)];
                chunk.Entities.Add(new EntityPlant(available_harvestableGrounds.ElementAt(rnd_id), plant, false));
                available_harvestableGrounds.RemoveAt(rnd_id);
            }

            // Generate Stones

            var available_stoneGrounds = chunk.Tiles.Where(tile => chunk.Entities.At(tile.Key) == null && tile.Value.Value == (int)DB.Tex.Rock).Select(tile => tile.Key).ToList();
            var stonesCount = RandomThings.rnd(Math.Min(2 * (int)mode, available_stoneGrounds.Count / 4));
            for (int i = 0; i < stonesCount; i++)
            {
                var rnd_id = RandomThings.rnd(available_stoneGrounds.Count);
                var results = new Dictionary<int, int> { [(int)DB.Tex.Stone] = 3, [(int)DB.Tex.Pebble] = 8, };
                chunk.Entities.Add(new EntityResource(available_stoneGrounds.ElementAt(rnd_id), (int)DB.Tex.Stone, (int)DB.Tex.Hammer, rndResults: results, addToCurrentChunkEntities: false));
                available_stoneGrounds.RemoveAt(rnd_id);
            }
        }

        private static int GetLayeredGrounds(GenerationMode mode, float layer)
        {
            var layers = LayeredGrounds[mode].Keys.ToList();
            var result = layers[0];
            for (int i = 0; i < layers.Count; i++)
                if (layer > layers[i]) result = layers[i];
            return LayeredGrounds[mode][result];
        }
    }
}
