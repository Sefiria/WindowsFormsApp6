using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp28_px
{
    public class Item
    {
        public int Id, Count;
        public Item()
        {
            Id = 0x00;
            Count = 0x00;
        }
        public Item(int Id, int Count)
        {
            this.Id = Id;
            this.Count = Count;
        }
        public Item((int id, int count) item)
        {
            Id = item.id;
            Count = item.count;
        }
    }
}
