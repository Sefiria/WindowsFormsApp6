using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v3.res.ores
{
    internal class Ores_Emerald : Ores_Base
    {
        public static Ores_Emerald Instance;
        static Ores_Emerald()
        {
            Instance = new Ores_Emerald
            {
                UniqueId = Guid.NewGuid(),
                Name = "Emerald",
                Rarity = 0.001F,
                ToolQuality = 200,
                ColorDark = Color.FromArgb(0, 123, 24).ToArgb(),
                ColorMid = Color.FromArgb(23, 221, 98).ToArgb(),
                ColorLight = Color.FromArgb(217, 255, 235).ToArgb(),
                ColorParticles = Color.FromArgb(23, 221, 98).ToArgb(),
            };
        }
    }
}
