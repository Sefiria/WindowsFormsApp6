using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v3.res.ores
{
    internal class Ores_Wood : Ores_Base
    {
        public static Ores_Wood Instance;
        static Ores_Wood()
        {
            Instance = new Ores_Wood
            {
                UniqueId = Guid.NewGuid(),
                Name = "Wood",
                Rarity = 0F,
                ToolQuality = 2,
                ColorDark = Color.FromArgb(100, 50, 0).ToArgb(),
                ColorMid = Color.FromArgb(120, 70, 0).ToArgb(),
                ColorLight = Color.FromArgb(150, 100, 0).ToArgb(),
                ColorParticles = Color.FromArgb(100, 50, 0).ToArgb(),
            };
        }
    }
}
