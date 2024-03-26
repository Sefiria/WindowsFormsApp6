using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public abstract class BlockBase
    {
        protected bool ItemMng(ref Data data, Dictionary<HelpBlock.Blocks, int> Needs, bool removeItems = true)
        {
            foreach (var pair in Needs)
                if (data.GetItemCount(pair.Key.ToString()) < pair.Value)
                    return false;

            if (removeItems)
            {
                foreach (var pair in Needs)
                    data.RemoveItemToInventory(new Item(pair.Key.ToString()), pair.Value);
            }

            return true;
        }
        protected bool ItemMng(ref Data data, HelpBlock.Blocks block)
        {
            return data.GetItemCount(block.ToString()) > 0;
        }
        protected bool ItemMng(ref Data data, HelpBlock.Blocks[] blocks)
        {
            foreach (HelpBlock.Blocks block in blocks)
            {
                if(data.GetItemCount(block.ToString()) == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
