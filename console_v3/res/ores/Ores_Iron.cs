using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v3.res.ores
{
    internal class Ores_Iron : Ores_Base
    {
        public static Ores_Iron Instance;
        static Ores_Iron()
        {
            Instance = new Ores_Iron
            {
                UniqueId = Guid.NewGuid(),
                Name = "Iron",
                Rarity = 0.8F,
                ToolQuality = 8,
                ColorDark = Color.FromArgb(175, 142, 119).ToArgb(),
                ColorMid = Color.FromArgb(216, 175, 147).ToArgb(),
                ColorLight = Color.FromArgb(226, 192, 170).ToArgb(),
                ColorParticles = Color.FromArgb(175, 142, 119).ToArgb(),
            };
        }
    }
}
