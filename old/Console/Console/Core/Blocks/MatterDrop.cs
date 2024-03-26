using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class MatterDrop : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.MatterDrop;
        public char RenderChar { get => 'º'; }
        public ConsoleColor RenderColor => ConsoleColor.Magenta;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>();

        public void Action(ref Data data)
        {
            data.AddItemToInventory(new Item("MatterDrop"));
            data.SetTargetBlock(HelpBlock.Blocks.Floor);
        }
    }
}
