using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class MatterPlant : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.MatterPlant;
        public char RenderChar { get => '¥'; }
        public ConsoleColor RenderColor => ConsoleColor.DarkMagenta;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>();

        public void Action(ref Data data)
        {
            data.AddItemToInventory(new Item("MatterPlant"));
            data.SetTargetBlock(HelpBlock.Blocks.Floor);
        }
    }
}
