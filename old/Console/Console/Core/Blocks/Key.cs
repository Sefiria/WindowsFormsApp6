using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class Key : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.Key;
        public char RenderChar { get => '♀'; }
        public ConsoleColor RenderColor => ConsoleColor.White;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>();

        public void Action(ref Data data)
        {
            data.AddItemToInventory(new Item("Key"));
            data.SetTargetBlock(HelpBlock.Blocks.Floor);
        }
    }
}
