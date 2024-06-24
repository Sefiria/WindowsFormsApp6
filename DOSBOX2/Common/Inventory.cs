using System;
using System.Collections.Generic;
using System.Linq;

namespace DOSBOX2.Common
{
    internal class Inventory
    {
        public List<Item> Items;
        public Inventory(Inventory copy)
        {
            Items = new List<Item>(copy.Items);
        }
        public Inventory(List<Item> copy)
        {
            Items = new List<Item>(copy);
        }
        public Inventory()
        {
            Items = new List<Item>();
        }

        public bool Contains(byte id) => Items.Any(x => x.Id == id);
        public bool Contains(Item item) => Contains(item.Id);
        public bool Contains<T>(T item) where T : Enum => Contains(Convert.ToByte(item));
        public byte Count(byte id) => Contains(id) ? Items.First(x => x.Id == id).Count : (byte)0;
        public byte Count(Item item) => Count(item.Id);
        public byte Count<T>(T item) where T : Enum => Count(Convert.ToByte(item));
        public void Add(byte id, byte count)
        {
            if (Contains(id))
                Items.First(x => x.Id == id).Count += count;
            else
                Items.Add(new Item(id, count));
        }
        public void Add(Item item) => Add(item.Id, item.Count);
        public void Add<T>(T item, byte count) where T : Enum => Add(Convert.ToByte(item), count);
        public bool Substract(byte id, byte count = 1)
        {
            if (Count(id) > 0)
            {
                Items.First(x => x.Id == id).Count -= count;
                return true;
            }
            return false;
        }
        public bool Substract(Item item) => Substract(item.Id, item.Count);
        public bool Substract<T>(T item, byte count = 1) where T : Enum => Substract(Convert.ToByte(item), count);

        public void CleanEmptySlots()
        {
            Items.RemoveAll(x => x.Count <= 0);
        }
    }
}
