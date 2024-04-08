using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static console_v3.DB;
using static console_v3.TheRecipes;

namespace console_v3.res.recipes.tools
{
    internal class Recipes_Scythes
    {
        public static Recipe Create()
        {
            var needs = RecipeObj.Create3x3(@"
011
020
020
", (Types.Ore, 0, 1), (0, Tex.WoodStick, 4));
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.WoodScythe, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Scythe", RecipeMode.Static, needs, results);
        }
    }
}
