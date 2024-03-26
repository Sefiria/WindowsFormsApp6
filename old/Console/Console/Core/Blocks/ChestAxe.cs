﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class ChestAxe : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.ChestAxe;
        public char RenderChar { get => '◙'; }
        public ConsoleColor RenderColor => ConsoleColor.White;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>();

        public void Action(ref Data data)
        {
            data.AddItemToInventory(new Item("Axe"));
            data.SetTargetBlock(HelpBlock.Blocks.Floor);
        }
    }
}
