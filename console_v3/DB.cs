using console_v3.Properties;
using console_v3.res.ores;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;

namespace console_v3
{
    /*
    ᴗѼѽѿ◊

    obj de Scan pour hint sur entities &| déjà découverts
    */
    public enum GenerationMode
    {
        Cave = 0, Rocky, Plain, Forest
    }

    public static class DB
    {
        public static bool Is<T>(this int value) where T: Enum => Enum.IsDefined(typeof(T), value);
        public static bool Isnt<T>(this int value) where T: Enum => !value.Is<T>();
        public static IEnumerable<int> GetValues<T>() where T: Enum => Enum.GetValues(typeof(T)).Cast<int>();

        public static Dictionary<GenerationMode, Color> ChunkLayerColor = new Dictionary<GenerationMode, Color>
        {
            [GenerationMode.Cave] = Color.FromArgb(80,80,80),
            [GenerationMode.Rocky] = Color.DimGray,
            [GenerationMode.Plain] = Color.Green,
            [GenerationMode.Forest] = Color.DarkGreen,
        };

        public static string DefineName(int _dbref) => Enum.GetName(typeof(Tex), _dbref);

        static DB()
        {
            TexturesSources = Resources.textures.Split2DAndResize(16, 16, true, transparent_color: Color.FromArgb(0, 255, 0));
            Textures = Resources.textures.Split2DAndResize(16, Core.TILE_SIZE, true, transparent_color: Color.FromArgb(0, 255, 0));

            Ores_Wood.DefineInstance();
            Ores_Coal.DefineInstance();
            Ores_Iron.DefineInstance();
            Ores_Gold.DefineInstance();
            Ores_Diamond.DefineInstance();
            Ores_Emerald.DefineInstance();

            PxColors = new Dictionary<int, int>
            {
                [(int)Tex.UnbreakableRock] = Color.FromArgb(0, 0, 0).ToArgb(),
                [(int)Tex.Obsidian] = Color.FromArgb(30, 10, 30).ToArgb(),
                [(int)Tex.HardRock] = Color.FromArgb(50, 50, 50).ToArgb(),
                [(int)Tex.DeepRock] = Color.FromArgb(70, 70, 70).ToArgb(),
                [(int)Tex.Rock] = Color.FromArgb(100, 100, 100).ToArgb(),
                [(int)Tex.SoftRock] = Color.FromArgb(135, 115, 110).ToArgb(),
                [(int)Tex.Dirt] = Color.FromArgb(175, 115, 0).ToArgb(),
                [(int)Tex.Grass] = Color.FromArgb(50, 150, 0).ToArgb(),

                [(int)Tex.IronStone] = Ores_Iron.Instance.ColorMid,
                [(int)Tex.GoldStone] = Ores_Gold.Instance.ColorMid,
                [(int)Tex.DiamondStone] = Ores_Diamond.Instance.ColorMid,
                [(int)Tex.EmeraldStone] = Ores_Emerald.Instance.ColorMid,
            };

            Ores_Wood.DefineOreStoneImage();
            Ores_Coal.DefineOreStoneImage();
            Ores_Iron.DefineOreStoneImage();
            Ores_Gold.DefineOreStoneImage();
            Ores_Diamond.DefineOreStoneImage();
            Ores_Emerald.DefineOreStoneImage();
        }

        public static Bitmap[,] Textures, TexturesSources;
        public enum Tex { UnbreakableRock = 0x0 + 0x0 * 16, Obsidian = 0x1 + 0x0 * 16, HardRock = 0x2 + 0x0 * 16, DeepRock = 0x3 + 0x0 * 16,
            SoftRock = 0x5 + 0x0 * 16, Dirt = 0x6 + 0x0 * 16, Grass = 0x7 + 0x0 * 16, Sawmill = 0x8 + 0x0 * 16, Workbench = 0x9 + 0x0 * 16,
            Rock = 0x1 + 0x1 * 16, CoalStone = 0x2 + 0x1 * 16, IronStone = 0x3 + 0x1 * 16, GoldStone = 0x4 + 0x1 * 16, DiamondStone = 0x5 + 0x1 * 16, EmeraldStone = 0x6 + 0x1 * 16,
            Furnace = 0x8 + 0x1 * 16, FurnaceOn = 0x9 + 0x1 * 16,
            Wood = 0x0 + 0x2 * 16, Stone = 0x1 + 0x2 * 16, Coal = 0x2 + 0x2 * 16, Iron = 0x3 + 0x2 * 16, Gold = 0x4 + 0x2 * 16, Diamond = 0x5 + 0x2 * 16, Emerald = 0x6 + 0x2 * 16,
            WoodPlank = 0x9 + 0x2 * 16, WoodStick = 0xA + 0x2 * 16, WoodMiniStick = 0xB + 0x2 * 16,
            Axe = 0x0 + 0x3 * 16, Scythe = 0x1 + 0x3 * 16, Pickaxe = 0x2 + 0x3 * 16, Shovel = 0x3 + 0x3 * 16, Sword = 0x4 + 0x3 * 16, Hammer = 0x5 + 0x3 * 16,
            Purpila = 0x8 + 0x3 * 16, Blueseo = 0x9+ 0x3 * 16, Yellilea = 0xA + 0x3 * 16, Blanca = 0xB + 0x3 * 16, Whiteneo = 0xC + 0x3 * 16, Redalis = 0xD + 0x3 * 16, Orangeno = 0xE + 0x3 * 16,
            PurpilaEssence = 0x8 + 0x4 * 16, BlueseoEssence = 0x9 + 0x4 * 16, YellileaEssence = 0xA + 0x4 * 16, BlancaEssence = 0xB + 0x4 * 16, WhiteneoEssence = 0xC + 0x4 * 16, RedalisEssence = 0xD + 0x4 * 16, OrangenoEssence = 0xE + 0x4 * 16,
            Tree_Automn_A = 0x8 + 0x5 * 16, Tree_Automn_B = 0x9 + 0x5 * 16, Tree_Automn_C = 0xA + 0x5 * 16,
            Tree_Summer_A = 0x8 + 0x6 * 16, Tree_Summer_B = 0x9 + 0x6 * 16, Tree_Summer_C = 0xA + 0x6 * 16,
            Tree_Spring_A = 0x8 + 0x7 * 16, Tree_Spring_B = 0x9 + 0x7 * 16, Tree_Spring_C = 0xA + 0x7 * 16,
            Chest = 0x8 + 0x8 * 16, Torch = 0x9 + 0x8 * 16, TorchOn = 0xA + 0x8 * 16,
            PlantFiber = 0x0 + 0x4 * 16, Pebble = 0x1 + 0x4 * 16,

            Mud = 0x5 + 0x0 * 16,// dirt

            OreStone = 0x7 + 0x1 * 16,
        }
        public static Dictionary<int, int> PxColors;
        public static int GetPxColor(int dbref)
        {
            if(PxColors.ContainsKey(dbref))
                return PxColors[dbref];
            return Color.Black.ToArgb();
        }
        public static Bitmap GetTextureSource(int dbref, int new_size_in_px = -1) => (new_size_in_px <= 0 && new_size_in_px != Core.TILE_SIZE )? TexturesSources[dbref % 16, dbref / 16] : new Bitmap(TexturesSources[dbref % 16, dbref / 16], new_size_in_px, new_size_in_px);
        public static Bitmap GetTexture(int dbref, int new_size_in_px = -1) => new_size_in_px <= 0 ? Textures[dbref % 16, dbref / 16] : new Bitmap(Textures[dbref % 16, dbref / 16], new_size_in_px, new_size_in_px);
        public static List<int> Ores = new List<int>()
        {
            (int)Tex.Wood,
            (int)Tex.Stone,
            (int)Tex.Coal,
            (int)Tex.Iron,
            (int)Tex.Gold,
            (int)Tex.Diamond,
            (int)Tex.Emerald,
        };
        public static List<int> HarvestableGrounds = new List<int>
        {
            (int)Tex.Dirt,
            (int)Tex.Grass,
        };
        public static List<int> Plants = new List<int>
        {
            (int)Tex.Purpila,
            (int)Tex.Blueseo,
            (int)Tex.Yellilea,
            (int)Tex.Blanca,
            (int)Tex.Whiteneo,
            (int)Tex.Redalis,
            (int)Tex.Orangeno,
        };
        public static List<int> Structures = new List<int>
        {
            (int)Tex.Sawmill,
            (int)Tex.Workbench,
            (int)Tex.Furnace,
            (int)Tex.Torch,
        };// scierie ?
        public static List<int> Items = new List<int>
        {
            (int)Tex.Wood,
            (int)Tex.WoodPlank,
            (int)Tex.WoodStick,
            (int)Tex.WoodMiniStick,
            (int)Tex.PurpilaEssence,
            (int)Tex.BlueseoEssence,
            (int)Tex.YellileaEssence,
            (int)Tex.BlancaEssence,
            (int)Tex.WhiteneoEssence,
            (int)Tex.RedalisEssence,
            (int)Tex.OrangenoEssence,
            (int)Tex.PlantFiber,
            (int)Tex.Stone,
            (int)Tex.Pebble,
        };
        public static List<int> Consumables = new List<int>()
        {
        };
        public static List<int> Tools = new List<int> { (int)Tex.Axe, (int)Tex.Pickaxe, (int)Tex.Shovel, (int)Tex.Scythe, (int)Tex.Hammer };
        public static List<int> Collectibles = Items.Concat(Consumables).Concat(Structures).Concat(Ores).ToList();

        public static bool IsAxe(this int dbref) => dbref == (int)Tex.Axe;
        public static bool IsScythe(this int dbref) => dbref == (int)Tex.Scythe;
        public static bool IsPickaxe(this int dbref) => dbref == (int)Tex.Pickaxe;
        public static bool IsShovel(this int dbref) => dbref == (int)Tex.Shovel;
        public static bool IsHammer(this int dbref) => dbref == (int)Tex.Hammer;
        public static bool IsOre(this int dbref) => Ores.Contains(dbref);
        public static bool IsPlant(this int dbref) => Plants.Contains(dbref);
        public static bool IsStructure(this int dbref) => Structures.Contains(dbref);
        public static bool IsItem(this int dbref) => Items.Contains(dbref);
        public static bool IsConsumable(this int dbref) => Consumables.Contains(dbref);
        public static bool IsTool(this int dbref) => Tools.Contains(dbref);
        public static bool IsCollectible(this int dbref) => Collectibles.Contains(dbref);

        public enum Types
        {
            Undefined = 0, Ore, Item, Tool, Consumable, Plant, Structure, Collectible
        }
        public static List<int> GetListByType(Types type)
        {
            switch(type)
            {
                default: return new List<int>();
                case Types.Ore: return Ores;
                case Types.Item: return Items;
                case Types.Tool: return Tools;
                case Types.Consumable: return Consumables;
                case Types.Plant: return Plants;
                case Types.Structure: return Structures;
                case Types.Collectible: return Collectibles;
            }
        }

        public static Dictionary<string, Color> Colors = new Dictionary<string, Color>
        {
            ["Items"] = Color.Yellow,
            ["Tools"] = Color.Cyan,
        };

        public static Ores_Base GetOre(int dbref_ore)
        {
            switch(dbref_ore)
            {
                default: return null;
                case (int)Tex.Wood: return Ores_Wood.Instance;
                case (int)Tex.Coal: return Ores_Wood.Instance;
                case (int)Tex.Stone: return Ores_Stone.Instance;
                case (int)Tex.Iron: return Ores_Iron.Instance;
                case (int)Tex.Gold: return Ores_Gold.Instance;
                case (int)Tex.Diamond: return Ores_Diamond.Instance;
                case (int)Tex.Emerald: return Ores_Emerald.Instance;
            }
        }
    }
}
