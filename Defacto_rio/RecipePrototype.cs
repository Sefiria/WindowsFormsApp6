using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Defacto_rio.Common;
using static Defacto_rio.PropertyTypes;

namespace Defacto_rio
{
    public class RecipePrototype
    {
        public string name;
        public string type;
        public List<Ingredient> ingredients;
        public List<Result> results;
        public int energy_required;
        public string localised_description;
        public string category;
        public string group;
        public string subgroup;
        public string order;

        public override string ToString()
        {
            return name;
        }
    }
}
