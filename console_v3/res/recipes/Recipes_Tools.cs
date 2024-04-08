using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static console_v3.DB;
using static console_v3.TheRecipes;

namespace console_v3.res.recipes.tools
{
    internal class Recipes_Tools
    {
        public static Recipe GetAxes()
        {
            var needs = new RecipeObj[,] {
                        { null, new RecipeObj((int)Types.Ore, 1), new RecipeObj((int)Types.Ore, 1) },
                        { null, new RecipeObj((int)Tex.WoodMiniStick, 1), new RecipeObj((int)Types.Ore, 1) },
                        { null, new RecipeObj((int)Tex.WoodMiniStick, 1), null },
                };
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.WoodScythe, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Axe", RecipeMode.Static, needs, results);
        }
        public static Recipe GetScythes()
        {
            var needs = new RecipeObj[,] {
                        { null, new RecipeObj((int)Types.Ore, 1), new RecipeObj((int)Types.Ore, 1) },
                        { null, new RecipeObj((int)Tex.WoodMiniStick, 1), null },
                        { null, new RecipeObj((int)Tex.WoodMiniStick, 1), null },
                };
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.WoodScythe, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Scythe", RecipeMode.Static, needs, results);
        }
    }
}
