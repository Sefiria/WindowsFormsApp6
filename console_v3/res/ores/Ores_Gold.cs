using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v3.res.ores
{
    internal class Ores_Gold : Ores_Base
    {
        public static Ores_Gold Instance;
        static Ores_Gold()
        {
            Instance = new Ores_Gold
            {
                UniqueId = Guid.NewGuid(),
                Name = "Gold",
                Rarity = 0.6F,
                ToolQuality = 4,
                ColorDark = Color.FromArgb(248, 175, 43).ToArgb(),
                ColorMid = Color.FromArgb(252, 238, 75).ToArgb(),
                ColorLight = Color.FromArgb(255, 255, 181).ToArgb(),
                ColorParticles = Color.FromArgb(252, 238, 75).ToArgb(),
            };
        }
    }
}
