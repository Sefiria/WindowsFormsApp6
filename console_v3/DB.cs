using console_v3.Properties;
using console_v3.res.DBResSpeDef;
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

        public static string DefineName(int _dbref) => Enum.GetName(typeof(TexName), _dbref);

        static DB()
        {
            Textures = Resources.textures.Split2D(16);
        }

        public static Bitmap[,] Textures;
        public enum TexName { UnbreakableRock = 0x0 + 0x0 * 16, Obsidian = 0x1 + 0x0 * 16, HardRock = 0x2 + 0x0 * 16, DeepRock = 0x3 + 0x0 * 16, Rock = 0x4 + 0x0 * 16,
            Dirt = 0x5 + 0x0 * 16, Grass = 0x6 + 0x0 * 16, Workbench2x2 = 0x8 + 0x0 * 16, Workbench3x3 = 0x9 + 0x0 * 16,
            CoalStone = 0x2 + 0x1 * 16, IronStone = 0x3 + 0x1 * 16, GoldStone = 0x4 + 0x1 * 16, DiamondStone = 0x5 + 0x1 * 16, EmeraldStone = 0x6 + 0x1 * 16,
            FurnaceOff = 0x8 + 0x1 * 16, FurnaceOn = 0x9 + 0x1 * 16,
            Coal = 0x2 + 0x2 * 16, Iron = 0x3 + 0x3 * 16, Gold = 0x4 + 0x4 * 16, Diamond = 0x5 + 0x5 * 16, Emerald = 0x6 + 0x6 * 16,
            WoodLog = 0x8 + 0x2 * 16, WoodPlank = 0x9 + 0x2 * 16, WoodStick = 0xA + 0x2 * 16, WoodMiniStick = 0xB + 0x2 * 16,
            WoodAxe = 0x0 + 0x3 * 16, WoodScythe = 0x1 + 0x3 * 16, WoodPickaxe = 0x2 + 0x3 * 16, WoodShovel = 0x3 + 0x3 * 16, WoodSword = 0x4 + 0x3 * 16,
            IronAxe = 0x0 + 0x4 * 16, IronScythe = 0x1 + 0x4 * 16, IronPickaxe = 0x2 + 0x4 * 16, IronShovel = 0x3 + 0x4 * 16, IronSword = 0x4 + 0x4 * 16,
            GoldAxe = 0x0 + 0x5 * 16, GoldScythe = 0x1 + 0x5 * 16, GoldPickaxe = 0x2 + 0x5 * 16, GoldShovel = 0x3 + 0x5 * 16, GoldSword = 0x4 + 0x5 * 16,
            DiamondAxe = 0x0 + 0x6 * 16, DiamondScythe = 0x1 + 0x6 * 16, DiamondPickaxe = 0x2 + 0x6 * 16, DiamondShovel = 0x3 + 0x6 * 16, DiamondSword = 0x4 + 0x6 * 16,
            EmeraldAxe = 0x0 + 0x7 * 16, EmeraldScythe = 0x1 + 0x7 * 16, EmeraldPickaxe = 0x2 + 0x7 * 16, EmeraldShovel = 0x3 + 0x7 * 16, EmeraldSword = 0x4 + 0x7 * 16,
            Purpila = 0x0 + 0x3 * 16, Blueseo = 0x1 + 0x3 * 16, Yellilea = 0x2 + 0x3 * 16, Blanca = 0x3 + 0x3 * 16, Whiteneo = 0x4 + 0x3 * 16, Redalis = 0x5 + 0x3 * 16, Orangeno = 0x6 + 0x3 * 16,
            PurpilaEssence = 0x0 + 0x4 * 16, BlueseoEssence = 0x1 + 0x4 * 16, YellileaEssence = 0x2 + 0x4 * 16, BlancaEssence = 0x3 + 0x4 * 16, WhiteneoEssence = 0x4 + 0x4 * 16, RedalisEssence = 0x5 + 0x4 * 16, OrangenoEssence = 0x6 + 0x4 * 16,
            Tree_Automn_A = 0x5 + 0x5 * 16, Tree_Automn_B = 0x6 + 0x5 * 16, Tree_Automn_C = 0x7 + 0x5 * 16,
            Tree_Summer_A = 0x5 + 0x6 * 16, Tree_Summer_B = 0x6 + 0x6 * 16, Tree_Summer_C = 0x7 + 0x6 * 16,
            Tree_Spring_A = 0x5 + 0x7 * 16, Tree_Spring_B = 0x6 + 0x7 * 16, Tree_Spring_C = 0x7 + 0x7 * 16,
            Chest = 0x5 + 0x8 * 16,
        }
        public static Bitmap GetTexture(int dbref) => Textures[dbref % 16, dbref / 16];
        public static List<int> Ores = new List<int>()
        {
            (int)TexName.Iron,
            (int)TexName.Gold,
            (int)TexName.Diamond,
            (int)TexName.Emerald,
        };
        public static List<int> Axes = new List<int>()
        {
            (int)TexName.WoodAxe,
            (int)TexName.IronAxe,
            (int)TexName.GoldAxe,
            (int)TexName.DiamondAxe,
            (int)TexName.EmeraldAxe,
        };
        public static List<int> Scythes = new List<int>()
        {
            (int)TexName.WoodScythe,
            (int)TexName.IronScythe,
            (int)TexName.GoldScythe,
            (int)TexName.DiamondScythe,
            (int)TexName.EmeraldScythe,
        };
        public static List<int> Pickaxes = new List<int>()
        {
            (int)TexName.WoodPickaxe,
            (int)TexName.IronPickaxe,
            (int)TexName.GoldPickaxe,
            (int)TexName.DiamondPickaxe,
            (int)TexName.EmeraldPickaxe,
        };
        public static List<int> Shovels = new List<int>()
        {
            (int)TexName.WoodShovel,
            (int)TexName.IronShovel,
            (int)TexName.GoldShovel,
            (int)TexName.DiamondShovel,
            (int)TexName.EmeraldShovel,
        };
        public static List<int> HarvestableGrounds = new List<int>
        {
            (int)TexName.Dirt,
            (int)TexName.Grass,
        };
        public static List<int> Plants = new List<int>
        {
            (int)TexName.Purpila,
            (int)TexName.Blueseo,
            (int)TexName.Yellilea,
            (int)TexName.Blanca,
            (int)TexName.Whiteneo,
            (int)TexName.Redalis,
            (int)TexName.Orangeno,
        };
        public static List<int> Structures = new List<int>
        {
            (int)TexName.Workbench2x2,
            (int)TexName.Workbench3x3,
            (int)TexName.FurnaceOff,
            (int)TexName.FurnaceOn,
        };// scierie ?
        public static List<int> Items = new List<int>
        {
            (int)TexName.WoodLog,
            (int)TexName.WoodPlank,
            (int)TexName.WoodStick,
            (int)TexName.WoodMiniStick,
            (int)TexName.PurpilaEssence,
            (int)TexName.BlueseoEssence,
            (int)TexName.YellileaEssence,
            (int)TexName.BlancaEssence,
            (int)TexName.WhiteneoEssence,
            (int)TexName.RedalisEssence,
            (int)TexName.OrangenoEssence,
        };
        public static List<int> Consommables = new List<int>()
        {
        };
        public static List<int> Tools = Axes.Concat(Scythes).Concat(Pickaxes).Concat(Shovels).ToList();// masse ?
        public static List<int> Collectibles = Items.Concat(Consommables).Concat(Structures).ToList();

        public static bool IsAxe(this int dbref) => Axes.Contains(dbref);
        public static bool IsScythe(this int dbref) => Scythes.Contains(dbref);
        public static bool IsPickaxe(this int dbref) => Pickaxes.Contains(dbref);
        public static bool IsShovel(this int dbref) => Shovels.Contains(dbref);
        public static bool IsOre(this int dbref) => Ores.Contains(dbref);
        public static bool IsPlant(this int dbref) => Plants.Contains(dbref);
        public static bool IsStructure(this int dbref) => Structures.Contains(dbref);
        public static bool IsItem(this int dbref) => Items.Contains(dbref);
        public static bool IsConsommable(this int dbref) => Consommables.Contains(dbref);
        public static bool IsTool(this int dbref) => Tools.Contains(dbref);
        public static bool IsCollectible(this int dbref) => Collectibles.Contains(dbref);


        public static Dictionary<string, Color> Colors = new Dictionary<string, Color>
        {
            ["Items"] = Color.Yellow,
            ["Tools"] = Color.Cyan,
        };
    }
}
