using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3.res.ores
{
    internal class Ores_Coal : Ores_Base
    {
        public static Ores_Coal Instance;
        public Ores_Coal() : base((int)DB.Tex.Coal) { }
        public static void DefineInstance()
        {
            Instance = new Ores_Coal
            {
                UniqueId = Guid.NewGuid(),
                Name = "Coal",
                Rarity = 1.5F,
                ToolQuality = 1,
                ColorDark = Color.FromArgb(0, 0, 0).ToArgb(),
                ColorMid = Color.FromArgb(10, 10, 10).ToArgb(),
                ColorLight = Color.FromArgb(20, 20, 20).ToArgb(),
                ColorParticles = Color.FromArgb(20, 20, 20).ToArgb(),
            };
        }
        public static void DefineOreStoneImage()
        {
            Instance.OreStoneImage = ResetGraphics(RandomThings.arnd((int)DB.Tex.OreStoneA, (int)DB.Tex.OreStoneB), (int)DB.Tex.Coal);
        }
    }
}
