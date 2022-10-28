using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WindowsFormsApp6.World.Structures
{
    public interface IStructure
    {
        int X { get; set; }
        int Y { get; set; }
        int W { get; set; }
        int H { get; set; }
        [JsonIgnore] Bitmap Image { get; }

        void Update();
        void Draw(Bitmap image, int tilesz);
        void Draw();
        void MouseDown();

        void RemoveStructureFromAllBlocks();
        void AddStructureToAllBlocks();
    }
}
