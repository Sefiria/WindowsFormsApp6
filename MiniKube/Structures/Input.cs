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
    public class Input<T> : StructureBase where T : Item
    {
        public override Anim Anim { get; set; } = AnimMgr.Load("struct_input");
        public Input() : base("item_struct_input")
        {
        }
    }
}
