using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3.res.ores
{
    public class Ores_Base : IUniqueRef
    {
        public Guid UniqueId { get; set; }
        public string Name;
        public float Rarity;
        public int ToolQuality;
        public int ColorDark, ColorMid, ColorLight, ColorParticles;
    }
}
