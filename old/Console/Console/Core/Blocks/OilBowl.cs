using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class OilBowl : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.OilBowl;
        public char RenderChar { get => 'ᵿ'; }
        public ConsoleColor RenderColor => ConsoleColor.DarkYellow;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>()
        {
            [HelpBlock.Blocks.Bowl] = 1
        };

        public void Action(ref Data data)
        {
            if(ItemMng(ref data, Needs))
            {
                data.AddItemToInventory(new Item("OilBowl"));
                data.SetTargetBlock(HelpBlock.Blocks.Bowl);
            }
        }
    }
}
