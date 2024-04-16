﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3
{
    public class Tile
    {
        public static Tile GetFromWorldLocation(vec tile_coords) => Core.Instance.SceneAdventure.World.GetTile(tile_coords);

        public vec Index;
        public Guid Seed;
        public vecf Coords => Index.f * GraphicsManager.TileSize;
        public int Resistance;
        public int Value;
        public int DBRef_Ore;
        public Tile(vec index, int value, int resistance = -1)
        {
            Index = index;
            Value = value;
            Seed = Guid.NewGuid();
            DBRef_Ore = Value == (int)DB.Tex.Rock ? OreCalculation() : - 1;
            Resistance = resistance;
            if (Resistance == -1)
                ResetAutoResistance();
        }
        public void ResetAutoResistance()
        {
            switch (Value)
            {
                case (int)DB.Tex.Grass: Resistance = 10; break;
                case (int)DB.Tex.Dirt: Resistance = 20; break;
                case (int)DB.Tex.Rock: Resistance = 50; break;
                case (int)DB.Tex.DeepRock: Resistance = 100; break;
                case (int)DB.Tex.HardRock: Resistance = 500; break;
                case (int)DB.Tex.Obsidian: Resistance = 5000; break;
                case (int)DB.Tex.UnbreakableRock: Resistance = int.MaxValue; break;
            }
        }
        public void Update()
        {
        }
        public Bitmap GetTexture()
        {
            // if tile value is rock, draw ore seed-related (take only the X in "00000000-XXXX-..." as integer)
            return (Value == (int)DB.Tex.Rock && DBRef_Ore > 17) ? DB.GetOre(DBRef_Ore + 0x1 * 16).OreStoneImage : DB.GetTexture(Value);
        }
        public int OreCalculation()
        {
            var ores = DB.Ores.Skip(2).ToList();

            int key = Convert.ToInt32($"0x{string.Concat(Seed.ToString().Skip(9).Take(4))}", 16);
            var ores_rarity = ores.Select(o => DB.GetOre(o).Rarity).ToList();
            ores_rarity.OrderByDescending(i => i);
            int id = -1, gap, rnd;
            for(int i=0; i<ores_rarity.Count && id == -1; i++)
            {
                gap = (ushort)(ores_rarity[i] * ushort.MaxValue);
                rnd = RandomThings.arnd(0, ushort.MaxValue - gap);
                if (key >= rnd && key < rnd + gap)
                    id = i;
            }
            return id == -1 ? (int)DB.Tex.Rock : ores[id] - 0x1 * 16;
        }
    }
}
