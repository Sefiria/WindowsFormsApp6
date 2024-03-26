using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class GreenPlant : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.GreenPlant;
        public char RenderChar { get => '¥'; }
        public ConsoleColor RenderColor => ConsoleColor.DarkGreen;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>();

        public void Action(ref Data data)
        {
            data.AddItemToInventory(new Item("GreenPlant"));
            data.SetTargetBlock(HelpBlock.Blocks.Floor);
        }
    }
}
