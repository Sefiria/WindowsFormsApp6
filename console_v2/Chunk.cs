﻿using console_v2.res.entities;
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
            List<(vecf vf, int res)> specials = new List<(vecf vf, int res)>();
            string result = "";
            for (int y = 0; y < ChunkSize.y; y++)
            {
                for (int x = 0; x < ChunkSize.x; x++)
                {
                    var res = (int)Tiles[(x, y).V()].Sol;
                    var c = DB.Resources[res];
                    if(c >= short.MaxValue)
                    {
                        vecf _vf = ((int)(x * GraphicsManager.CharSize.Width), (int)(y * GraphicsManager.CharSize.Height)).Vf();
                        specials.Add((_vf, res));
                        result += ' ';
                    }
                    else
                        result += (char)c;
                }
            }
            GraphicsManager.DrawString(g, result, Brushes.DimGray, vec.Zero);
            specials.ForEach(spe => g.DrawImage(DB.ResourcesSpecials[spe.res], SceneAdventure.DrawingRect.Location.Plus(spe.vf.pt.MinusF(5, 0))));

            var entities = new List<Entity>(Entities);
            entities.ForEach(e => { if (e.Exists) e.Draw(g); });
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

            //// RESOURCES

            // Generate Plants

            var available_harvestableGrounds = chunk.Tiles.Where(tile => chunk.Entities.At(tile.Key) == null && DB.HarvestableGrounds.Contains(tile.Value.Sol)).Select(tile => tile.Key).ToList();
            var plantsCount = RandomThings.rnd(Math.Min(2 * (int)mode, available_harvestableGrounds.Count / 4));
            for (int i = 0; i < plantsCount; i++)
            {
                var rnd_id = RandomThings.rnd(available_harvestableGrounds.Count);
                var names = DB.Plants.Select(pl => Enum.GetName(typeof(Ressources), pl)).ToArray();
                var plant = (Ressources)Enum.Parse(typeof(Ressources), names[RandomThings.rnd(names.Length)]);
                chunk.Entities.Add(new EntityPlant(available_harvestableGrounds.ElementAt(rnd_id), plant, false));
                available_harvestableGrounds.RemoveAt(rnd_id);
            }

            // Generate Stones

            var available_stoneGrounds = chunk.Tiles.Where(tile => chunk.Entities.At(tile.Key) == null && tile.Value.Sol == Sols.Pierre).Select(tile => tile.Key).ToList();
            var stonesCount = RandomThings.rnd(Math.Min(2 * (int)mode, available_stoneGrounds.Count / 4));
            for (int i = 0; i < stonesCount; i++)
            {
                var rnd_id = RandomThings.rnd(available_stoneGrounds.Count);
                var results = new Dictionary<int, int> { [(int)Objets.Pierre] = 3, [(int)Objets.Cailloux] = 8, };
                chunk.Entities.Add(new EntityResource(available_stoneGrounds.ElementAt(rnd_id), (int)Ressources.Rocher, Outils.Masse, rndResults:results, addToCurrentChunkEntities:false));
                available_stoneGrounds.RemoveAt(rnd_id);
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
