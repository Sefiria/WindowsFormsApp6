using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class Door : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.Door;
        public char RenderChar { get => '◘'; }
        public ConsoleColor RenderColor => ConsoleColor.White;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>()
        {
            [HelpBlock.Blocks.Key] = 1
        };

        public void Action(ref Data data)
        {
            if (ItemMng(ref data, Needs))
            {
                data.SetTargetBlock(HelpBlock.Blocks.Floor);
            }
        }
    }
}
