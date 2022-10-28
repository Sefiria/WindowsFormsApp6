using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WindowsFormsApp6.World.Ores;
using WindowsFormsApp6.World.Structures;

namespace WindowsFormsApp6.World.Blocs
{
    public interface IBloc
    {
        int X { get; set; }
        int Y { get; set; }
        [JsonIgnore] Bitmap Image { get; }
        int Layer { get; set; }
        int Life { get; set; }
        Ore Ore { get; set; }
        IStructure OwnerStructure { get; set; }

        void Update();
        void Draw(Bitmap image, int tilesz);
        void Draw();
    }
}
