using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tooling;

namespace console_v2
{
    public class Tool : IName, IDBItem
    {
        public string Name { get; set; } = "Unnamed_Tool";
        public Outils DBRef;
        public float Duration;
        public int Count;
        public int STR = 1;

        public int DBItem => (int)DBRef;

        public Tool()
        {
        }
        public Tool(Tool copy)
        {
            Name = copy.Name;
            Duration = copy.Duration;
            DBRef = copy.DBRef;
            Count = copy.Count;
        }
        public Tool(string name, Outils dbref)
        {
            Name = name;
            Duration = 1f;
            DBRef = dbref;
            Count = 1;
        }

        public void Use(Entity triggerer)
        {
        }
    }
}
