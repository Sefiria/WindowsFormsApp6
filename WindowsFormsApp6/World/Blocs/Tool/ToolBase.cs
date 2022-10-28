using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WindowsFormsApp6.World.Ores;

namespace WindowsFormsApp6.World.Blocs.Tool
{
    public class ToolBase : ITool
    {
        [JsonIgnore] public Bitmap Image { get; set; }
        public int BaseQuality { get; set; }
        public OreType Quality { get; set; }
        public int Radius { get; set; }

        public int GetQualitySTR() => Quality == OreType.None ? 1 : ((int)Quality + 1) * 3;//BaseQuality * ((int)Quality + 1);//devient Enchant
    }
}
