using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class Bowl : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.Bowl;
        public char RenderChar { get => 'ʊ'; }
        public ConsoleColor RenderColor => ConsoleColor.White;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>();

        public void Action(ref Data data)
        {
            data.AddItemToInventory(new Item("Bowl"));
            data.SetTargetBlock(HelpBlock.Blocks.Floor);
        }
    }
}
