using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public class Seed : BlockBase, IBlock
    {
        public HelpBlock.Blocks EnumValue => HelpBlock.Blocks.Seed;
        public char RenderChar { get => 'ꞈ'; }
        public ConsoleColor RenderColor => ConsoleColor.White;
        public Dictionary<HelpBlock.Blocks, int> Needs => new Dictionary<HelpBlock.Blocks, int>();

        public void Action(ref Data data)
        {
            if (ItemMng(ref data, HelpBlock.Blocks.WaterBowl))
            {
                if (ItemMng(ref data, HelpBlock.Blocks.OilBowl))
                {
                    if (ItemMng(ref data, HelpBlock.Blocks.MatterBowl))
                    {
                        data.RemoveItemToInventory(new Item("WaterBowl"));
                        data.RemoveItemToInventory(new Item("OilBowl"));
                        data.RemoveItemToInventory(new Item("MatterBowl"));
                        data.AddItemToInventory(new Item("Bowl", 3));
                        data.SetTargetBlock(HelpBlock.Blocks.MatterDrop);
                    }
                    else
                    {
                        data.RemoveItemToInventory(new Item("WaterBowl"));
                        data.RemoveItemToInventory(new Item("OilBowl"));
                        data.AddItemToInventory(new Item("Bowl", 2));
                        data.SetTargetBlock(HelpBlock.Blocks.GreenPlant);
                    }
                }
                else
                {
                    if (ItemMng(ref data, HelpBlock.Blocks.MatterBowl))
                    {
                        data.RemoveItemToInventory(new Item("WaterBowl"));
                        data.RemoveItemToInventory(new Item("MatterBowl"));
                        data.AddItemToInventory(new Item("Bowl", 2));
                        data.SetTargetBlock(HelpBlock.Blocks.DarkPlant);
                    }
                    else
                    {
                        data.RemoveItemToInventory(new Item("WaterBowl"));
                        data.AddItemToInventory(new Item("Bowl"));
                        data.SetTargetBlock(HelpBlock.Blocks.BluePlant);
                    }
                }
            }
            else
            {
                if (ItemMng(ref data, HelpBlock.Blocks.OilBowl))
                {
                    if (ItemMng(ref data, HelpBlock.Blocks.MatterBowl))
                    {
                        data.RemoveItemToInventory(new Item("OilBowl"));
                        data.RemoveItemToInventory(new Item("MatterBowl"));
                        data.AddItemToInventory(new Item("Bowl", 2));
                        data.SetTargetBlock(HelpBlock.Blocks.RedPlant);
                    }
                    else
                    {
                        data.RemoveItemToInventory(new Item("OilBowl"));
                        data.AddItemToInventory(new Item("Bowl"));
                        data.SetTargetBlock(HelpBlock.Blocks.GrayPlant);
                    }
                }
                else
                {
                    if (ItemMng(ref data, HelpBlock.Blocks.MatterBowl))
                    {
                        data.RemoveItemToInventory(new Item("MatterBowl"));
                        data.AddItemToInventory(new Item("Bowl"));
                        data.SetTargetBlock(HelpBlock.Blocks.MatterPlant);
                    }
                }
            }
        }
    }
}
