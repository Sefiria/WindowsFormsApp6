using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using static System.Net.Mime.MediaTypeNames;

namespace console_v3.res.ores
{
    public class Ores_Base : IUniqueRef
    {
        public Guid UniqueId { get; set; }
        public string Name;
        public float Rarity;
        public int ToolQuality;
        public int ColorDark, ColorMid, ColorLight, ColorParticles;
        public Bitmap OreStoneImage;

        public Ores_Base(int ore)
        {
        }

        public static Bitmap ResetGraphics(int dbref, int dbref_ore)
        {
            Bitmap Image = null;
            var ore = DB.GetOre(dbref_ore);
            if (ore != null)
            {
                Image = DB.GetTextureSource(dbref)
                                  .ChangeColor(Color.FromArgb(1, 0, 0), Color.FromArgb(ore.ColorDark))
                                  .ChangeColor(Color.FromArgb(0, 1, 0), Color.FromArgb(ore.ColorMid))
                                  .ChangeColor(Color.FromArgb(0, 0, 1), Color.FromArgb(ore.ColorLight))
                                  .Resize(Core.TILE_SIZE + 1);
            }
            return Image ?? DB.GetTexture(dbref);
        }
    }
}
