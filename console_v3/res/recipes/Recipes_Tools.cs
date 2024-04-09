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
        public static Recipe CreateScytheA()
        {
            var needs = RecipeObj.Create3x3(@"
011
020
020
", (Types.Ore, 0, 0, 1), (0, Tex.WoodStick, 0, 4));
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.Scythe, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Scythe", RecipeMode.Static, needs, results);
        }
        public static Recipe CreateScytheB()
        {
            var needs = RecipeObj.Create3x3(@"
110
020
020
", (Types.Ore, 0, 0, 1), (0, Tex.WoodStick, 0, 4));
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.Scythe, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Scythe", RecipeMode.Static, needs, results);
        }
        public static Recipe CreateAxeA()
        {
            var needs = RecipeObj.Create3x3(@"
011
021
020
", (Types.Ore, 0, 0, 1), (0, Tex.WoodStick, 0, 4));
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.Axe, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Axe", RecipeMode.Static, needs, results);
        }
        public static Recipe CreateAxeB()
        {
            var needs = RecipeObj.Create3x3(@"
110
120
020
", (Types.Ore, 0, 0, 1), (0, Tex.WoodStick, 0, 4));
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.Axe, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Axe", RecipeMode.Static, needs, results);
        }
        public static Recipe CreatePickaxe()
        {
            var needs = RecipeObj.Create3x3(@"
111
020
020
", (Types.Ore, 0, 0, 1), (0, Tex.WoodStick, 0, 4));
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.Pickaxe, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Pickaxe", RecipeMode.Static, needs, results);
        }
        public static Recipe CreateShovel()
        {
            var needs = RecipeObj.Create3x3(@"
010
020
020
", (Types.Ore, 0, 0, 1), (0, Tex.WoodStick, 0, 4));
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.Shovel, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Shovel", RecipeMode.Static, needs, results);
        }
        public static Recipe CreateSword()
        {
            var needs = RecipeObj.Create3x3(@"
010
010
020
", (Types.Ore, 0, 0, 1), (0, Tex.WoodStick, 0, 4));
            var results = new List<RecipeObj> { new RecipeObj((int)Tex.Sword, 1) };
            return RecipeFactory.Create(Enum.GetName(typeof(Types), Types.Ore) + " Sword", RecipeMode.Static, needs, results);
        }
    }
}
