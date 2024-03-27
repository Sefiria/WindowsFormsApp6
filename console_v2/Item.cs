using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public class Item : IName, IDBItem
    {
        public string Name { get; set; } = "Unnamed_Item";
        public Objets DBRef;
        public bool IsConsommable = true;
        public bool IsMenuConsommable = true;
        public int Count;

        public int DBItem => (int)DBRef;

        public Item()
        {
        }
        public Item(string name, Objets dbref, int count = 1)
        {
            Name = name;
            DBRef = dbref;
            Count = count;
        }
        public void Consume()
        {
            if (Count == 0) return;

            //

            Count--;
        }
    }
}
