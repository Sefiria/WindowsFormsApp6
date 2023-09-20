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

        public class Ingredient : Property
        {
            public string type = "", name = "";
            public string amount = "";
            public Ingredient() { }
            public Ingredient(string type, string name, string amount)
            {
                this.type = type;
                this.name = name;
                this.amount = amount;
            }
        }
        public class Result : Ingredient { public Result() { } public Result(string type, string name, string amount) : base(type, name, amount) { } }
    }
}
