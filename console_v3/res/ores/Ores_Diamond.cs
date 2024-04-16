using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v3.res.ores
{
    internal class Ores_Diamond : Ores_Base
    {
        public static Ores_Diamond Instance;
        public Ores_Diamond() : base((int)DB.Tex.Diamond) { }
        public static void DefineInstance()
        {
            Instance = new Ores_Diamond
            {
                UniqueId = Guid.NewGuid(),
                Name = "Diamond",
                Rarity = 0.001F,
                ToolQuality = 50,
                ColorDark = Color.FromArgb(93, 236, 245).ToArgb(),
                ColorMid = Color.FromArgb(119, 206, 251).ToArgb(),
                ColorLight = Color.FromArgb(93, 236, 245).ToArgb(),
                ColorParticles = Color.FromArgb(119, 206, 251).ToArgb(),
            };
        }
        public static void DefineOreStoneImage()
        {
            Instance.OreStoneImage = ResetGraphics((int)DB.Tex.OreStone, (int)DB.Tex.Diamond);
        }
    }
}
