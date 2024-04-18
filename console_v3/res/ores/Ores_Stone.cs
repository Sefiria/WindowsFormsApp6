using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3.res.ores
{
    internal class Ores_Stone : Ores_Base
    {
        public static Ores_Stone Instance;
        public Ores_Stone() : base((int)DB.Tex.Stone) { }
        public static void DefineInstance()
        {
            Instance = new Ores_Stone
            {
                UniqueId = Guid.NewGuid(),
                Name = "Iron",
                Rarity = 0F,
                ToolQuality = 4,
                ColorDark = Color.FromArgb(50, 50, 50).ToArgb(),
                ColorMid = Color.FromArgb(100, 100, 100).ToArgb(),
                ColorLight = Color.FromArgb(180, 180, 180).ToArgb(),
                ColorParticles = Color.FromArgb(150, 150, 150).ToArgb(),
            };
        }
        public static void DefineOreStoneImage()
        {
            Instance.OreStoneImage = ResetGraphics(RandomThings.arnd((int)DB.Tex.OreStoneA, (int)DB.Tex.OreStoneB), (int)DB.Tex.Stone);
        }
    }
}
