using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class WaterBowl : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.WaterBowl;
        public char RenderChar { get => 'ᵿ'; }
        public ConsoleColor RenderColor => ConsoleColor.DarkCyan;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>()
        {
            [HelpBlock.Blocks.Bowl] = 1
        };

        public void Action(ref Data data)
        {
            if(ItemMng(ref data, Needs))
            {
                data.AddItemToInventory(new Item("WaterBowl"));
                data.SetTargetBlock(HelpBlock.Blocks.Bowl);
            }
        }
    }
}
