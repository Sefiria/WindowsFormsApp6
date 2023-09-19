using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Defacto_rio
{
    public class PropertyTypes
    {
        public class Property { }
        public class PropertyArray<T> where T : Property
        {
            public string Name { get; set; }
            public List<T> Properties = new List<T>();
            public PropertyArray(string name)
            {
                Name = name;
            }
            public override string ToString()
            {
                return Name;
            }
        }

        public class Ingredient : Property
        {
            public string type, name;
            public int amount;
            public Ingredient(string type, string name, int amount)
            {
                this.type = type;
                this.name = name;
                this.amount = amount;
            }
        }
        public class Result : Ingredient { public Result(string type, string name, int amount) : base(type, name, amount) { } }
    }
}
