using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v2
{
    internal class Inventory
    {
        public long Gils;
        public List<Item> Items;
        public List<Tool> Tools;
        public Inventory()
        {
            Gils = 0;
            Items = new List<Item>();
            Tools = new List<Tool>();
        }
    }
}
