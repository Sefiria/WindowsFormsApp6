using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Defacto_rio.Common;
using static Defacto_rio.PropertyTypes;

namespace Defacto_rio
{
    public class RecipePrototype : Prototype
    {
        public string ingredients;
        public string results;
        public string energy_required;
        public string category;
        public string group;
        public string subgroup;
        public string order;

        public override string ToString()
        {
            return name ?? "";
        }
    }
}
