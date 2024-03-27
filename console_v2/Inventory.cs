using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public class Inventory
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

        public bool Contains(Objets item) => Items?.ContainsItem((int)item) ?? false;
        public bool Contains(Outils tool) => Tools?.ContainsItem((int)tool) ?? false;
        public bool Contains(int dbref)
        {
            if (dbref.Is<Objets>()) return Contains((Objets)dbref);
            if (dbref.Is<Outils>()) return Contains((Outils)dbref);
            return false;
        }

        public void Add(params Item[] items) => Items?.AddRange(items);
        public void Add(params Tool[] tools) => Tools?.AddRange(tools);
        public void Add(params int[] dbrefs)
        {
            foreach (int dbref in dbrefs)
            {
                if (dbref.Is<Objets>()) _addItem(dbref);
                else if (dbref.Is<Outils>()) _addTool(dbref);
            }
        }
        private void _addItem(int dbref)
        {
            if (Contains((Objets)dbref))
                Items[Items.IndexOf(Items.First(i => i.DBItem == dbref))].Count++;
            else
                Add(new Item(Enum.GetName(typeof(Objets), dbref), (Objets)dbref));
        }
        private void _addTool(int dbref)
        {
            if (Contains((Outils)dbref))
                Tools[Tools.IndexOf(Tools.First(i => i.DBItem == dbref))].Count++;
            else
                Add(new Tool(Enum.GetName(typeof(Outils), dbref), (Outils)dbref));
        }

        public void Remove(params Item[] items) => Items?.AddRange(items);
        public void Remove(params Tool[] tools) => Tools?.AddRange(tools);
        public void Remove(params int[] dbrefs)
        {
            foreach (int dbref in dbrefs)
            {
                if (dbref.Is<Objets>()) Contains((Objets)dbref);
                else if (dbref.Is<Outils>()) Contains((Outils)dbref);
            }
        }
    }
}
