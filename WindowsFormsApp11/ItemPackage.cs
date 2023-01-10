using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp11.Items;

namespace WindowsFormsApp11
{
    public class ItemPackage
    {
        public List<Item> Items;
        public int Count => Items.Count;

        public ItemPackage(List<Item> items)
        {
            Items = items;
        }

        public int CountOf(Type type)
        {
            return Items.FirstOrDefault(x => x.GetType().IsAssignableFrom(type))?.Count ?? 0;
        }

        public void Add(Item item)
        {
            var i = Items.FirstOrDefault(x => x.GetType().IsAssignableFrom(item.GetType()));
            if (i != null)
            {
                i.Count += item.Count;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void AddPackage(ItemPackage pck) => AddRange(pck.Items);
        public void AddRange(List<Item> items)
        {
            foreach (var item in items)
            {
                var i = Items.FirstOrDefault(x => x.GetType().IsAssignableFrom(item.GetType()));
                if (i != null)
                {
                    i.Count += item.Count;
                }
                else
                {
                    Items.Add(item);
                }
            }
        }

        public int CountOf<T>()
        {
            var i = Items.FirstOrDefault(x => x.GetType().IsAssignableFrom(typeof(T)));
            return i != null ? i.Count : 0;
        }

        public void Remove(Type type, int count = 1)
        {
            var i = Items.FirstOrDefault(x => x.GetType().IsAssignableFrom(type));
            if(i != null)
            {
                i.Count -= count;
                if (i.Count <= 0)
                    Items.Remove(i);
            }
        }
    }
}
