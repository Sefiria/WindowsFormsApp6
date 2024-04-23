using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp28_px
{
    public static class InventoryManager
    {
        public static void Merge(this List<Item> inv, List<Item> content)
        {
            inv.AddRange(content);
        }
        public static void Add(this List<Item> inv, Item item)
        {
            if (item.Count == 0)
                return;
            var inv_it = inv.FirstOrDefault(it => it.Id == item.Id);
            if (inv_it == null)
                inv.Add(item);
            else
                inv[inv.IndexOf(inv_it)].Count += item.Count;
        }
        public static void Add(this List<Item> inv, int id, int count)
        {
            if (count == 0)
                return;
            var inv_it = inv.FirstOrDefault(it => it.Id == id);
            if (inv_it == null)
                inv.Add(new Item(id, count));
            else
                inv[inv.IndexOf(inv_it)].Count += count;
        }
        /// <summary>
        /// Removes instantly the item
        /// </summary>
        public static void Delete(this List<Item> inv, Item item)
        {
            var inv_it = inv.FirstOrDefault(it => it.Id == item.Id);
            if (inv_it != null)
                inv.Remove(inv_it);
        }
        /// <summary>
        /// Removes count item, removes instantly if reaches 0
        /// </summary>
        public static void Remove(this List<Item> inv, Item item)
        {
            if(item.Count == 0) return;
            var inv_it = inv.FirstOrDefault(it => it.Id == item.Id);
            if (inv_it != null)
            {
                inv[inv.IndexOf(inv_it)].Count -= item.Count;
                if(inv[inv.IndexOf(inv_it)].Count <= 0)
                    inv.Remove(inv_it);
            }
        }
        /// <summary>
        /// Removes count item, removes instantly if reaches 0
        /// </summary>
        public static void Remove(this List<Item> inv, int id, int count)
        {
            if(count == 0) return;
            var inv_it = inv.FirstOrDefault(it => it.Id == id);
            if (inv_it != null)
            {
                inv[inv.IndexOf(inv_it)].Count -= count;
                if (inv[inv.IndexOf(inv_it)].Count <= 0)
                    inv.Remove(inv_it);
            }
        }
        public static bool Contains(this List<Item> inv, int id)
        {
            return inv.Any(it => it.Id == id);
        }
    }
}
