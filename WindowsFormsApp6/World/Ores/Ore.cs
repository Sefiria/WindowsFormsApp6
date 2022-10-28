using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;

namespace WindowsFormsApp6.World.Ores
{
    public enum OreType
    {
        None = -1,
        Bronze = 0,
        Silver,
        Gold,
        Titanium,
        Diamond
    }
    public class Ore
    {
        public int Count;
        public OreType OreType;
        [JsonIgnore] public Bitmap Image;

        public Ore(OreType oreType, int count)
        {
            OreType = oreType;
            Count = count;
            Image = GetOreRes(OreType);
        }

        public static Bitmap GetOreRes(OreType oreType)
        {
            Bitmap b;
            switch (oreType)
            {
                default:
                case OreType.Bronze: b = Resources.ore_bronze; b.MakeTransparent(); return b;
                case OreType.Silver: b = Resources.ore_silver; b.MakeTransparent(); return b;
                case OreType.Gold: b = Resources.ore_gold; b.MakeTransparent(); return b;
                case OreType.Titanium: b = Resources.ore_titanium; b.MakeTransparent(); return b;
                case OreType.Diamond: b = Resources.ore_diamond; b.MakeTransparent(); return b;
            }
        }

        public static OreType GetOre(int layer)
        {
            layer -= 6;
            int maxlayer = 25;
            if (layer > maxlayer) layer = maxlayer;
            Dictionary<OreType, (int Luck, int Weight)> ores_info = new Dictionary<OreType, (int Luck, int Weight)>()
            {
                [OreType.Bronze] = (50, layer + 10),
                [OreType.Silver] = (20, layer + 40),
                [OreType.Gold] = (5, layer + 100),
                [OreType.Titanium] = (3, layer + 250),
                [OreType.Diamond] = (1, layer + 1000),
            };

            int ore_quality = 0;

            int _min, _max, _value;
            foreach (var info in ores_info)
            {
                _min = Tools.RND.Next(100 - info.Value.Luck);
                _max = _min + info.Value.Luck;
                _value = Tools.RND.Next(100);
                ore_quality += (_value >= _min && _value <= _max ? 1 : 0) * info.Value.Weight;
            }

            if (ore_quality >= ores_info[OreType.Diamond].Weight)
                return OreType.Diamond;

            if (ore_quality >= ores_info[OreType.Titanium].Weight)
                return OreType.Titanium;

            if (ore_quality >= ores_info[OreType.Gold].Weight)
                return OreType.Gold;

            if (ore_quality >= ores_info[OreType.Silver].Weight)
                return OreType.Silver;

            if (ore_quality >= ores_info[OreType.Bronze].Weight)
                return OreType.Bronze;

            return OreType.None;
        }
    }
}
