using MiniKube.Entities;
using MiniKube.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniKube.Structures
{
    public class ConveyorBase : StructureBase
    {
        public ConveyorBase(string resource, int split_x = 0, int split_y = 0) : base(resource, split_x, split_y)
        {
        }
    }
    public class ConveyorMk1 : ConveyorBase
    {
        public override Anim Anim { get; set; } = AnimMgr.Load("conveyor_mk1");
        public ConveyorMk1() : base("item_conveyor_mk1", 24, 24)
        {
        }
    }
}
