using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class GrayPlant : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.GrayPlant;
        public char RenderChar { get => '¥'; }
        public ConsoleColor RenderColor => ConsoleColor.Gray;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>();

        public void Action(ref Data data)
        {
            data.AddItemToInventory(new Item("GrayPlant"));
            data.SetTargetBlock(HelpBlock.Blocks.Floor);
        }
    }
}
