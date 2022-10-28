using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WindowsFormsApp6.World.Ores;

namespace WindowsFormsApp6.World.Blocs.Tool
{
    public interface ITool
    {
        [JsonIgnore] Bitmap Image { get; set; }
        int BaseQuality { get; set; }
        OreType Quality { get; set; }
        int GetQualitySTR();
    }
}
