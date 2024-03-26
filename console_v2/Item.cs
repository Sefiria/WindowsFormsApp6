using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v2
{
    internal class Item
    {
        public string Name = "Unnamed_Item";
        public bool IsConsommable = true;
        public bool IsMenuConsommable = true;
        public int Count;
        public Item()
        {
        }
        public Item(string name, int count = 1)
        {
            Name = name;
            Count = count;
        }
    }
}
