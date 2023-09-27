using MiniKube.Items;
using MiniKube.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniKube
{
    internal partial class Map
    {
        Input<ItemIronPlate> input  = new Input<ItemIronPlate>();
        public Map()
        {
        }

        public void Update()
        {
            input.Pos = Core.TargetPoint;
        }

        public void Draw()
        {
            draw_grid();
            input.Draw();
        }
    }
}
