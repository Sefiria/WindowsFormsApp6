using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX2
{
    public class Item
    {
        public byte Id, Count;
        public Item()
        {
            Id = 0x00;
            Count = 0x00;
        }
        public Item(Item copy)
        {
            Id = copy.Id;
            Count = copy.Count;
        }
        public Item(byte Id, byte Count)
        {
            this.Id = Id;
            this.Count = Count;
        }
        public Item((byte id, byte count) item)
        {
            Id = item.id;
            Count = item.count;
        }
    }
}
