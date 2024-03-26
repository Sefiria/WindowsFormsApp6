using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class CrackedTree : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.CrackedTree;
        public char RenderChar { get => 'Δ'; }
        public ConsoleColor RenderColor => ConsoleColor.White;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>()
        {
            [HelpBlock.Blocks.Axe] = 1
        };

        public void Action(ref Data data)
        {
            if (ItemMng(ref data, Needs, false))
            {
                data.SetTargetBlock(HelpBlock.Blocks.Wood);
            }
        }
    }
}
