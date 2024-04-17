using console_v3.res.recipes.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tooling;
using static console_v3.DB;
using static console_v3.TheRecipes;

namespace console_v3
{
    public class TheRecipes
    {
        public enum RecipeMode
        {
            Chaos=0, ToolsOnTop, Static,
        }
        public class RecipeObj
        {
            public int DBRef, DBRef_Ore, Count;
            public Types Type = Types.Undefined;
            public RecipeObj()
            {
            }
            public RecipeObj(RecipeObj clone)
            {
                DBRef = clone.DBRef;
                DBRef_Ore = clone.DBRef_Ore;
                Count = clone.Count;
                Type = clone.Type;
            }
            public RecipeObj(int DBRef, int Count, int DBRef_Ore = -1)
            {
                this.DBRef = DBRef;
                this.DBRef_Ore = DBRef_Ore;
                this.Count = Count;
            }
            public RecipeObj(Types Type, int Count, int DBRef_Ore = -1)
            {
                this.Type = Type;
                this.DBRef_Ore = DBRef_Ore;
                this.Count = Count;
            }
            public RecipeObj Clone()
            {
                return new RecipeObj
                {
                    DBRef = DBRef,
                    DBRef_Ore = DBRef_Ore,
                    Count = Count,
                    Type = Type,
                };
            }
            public static RecipeObj[,] Create3x3(string format, params (Types type, Tex dbref, Tex dbref_ore, int count)[] args)
            {
                var objs = new RecipeObj[3, 3];
                var lines = format.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int id;
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        id = int.Parse("" + lines[x][y]) - 1;
                        if (id == -1)
                            objs[x, y] = null;
                        else
                            objs[x, y] = args[id].type != Types.Undefined ? new RecipeObj(args[id].type, args[id].count) : new RecipeObj((int)args[id].dbref, args[id].count);
                    }
                }
                return objs;
            }
        }
        public class Recipe : IUniqueRef
        {
            public Guid UniqueId { get; set; }
            public string Name;
            public RecipeMode Mode;
            public RecipeObj[,] Needs;
            public List<RecipeObj> Results;
            public Recipe(string Name, RecipeMode Mode)
            {
                UniqueId = Guid.NewGuid();
                this.Name = Name;
                this.Mode = Mode;
            }
            public bool SatisfiedBy(RecipeObj[,] slots)
            {
                switch(Mode)
                {
                    default:
                    case RecipeMode.Chaos: return SatisfiedBy_Chaos(slots);
                    case RecipeMode.ToolsOnTop: return SatisfiedBy_ToolsOnTop(slots);
                    case RecipeMode.Static: return SatisfiedBy_Static(slots);
                }
            }
            /*
            private int SatisfiedCountBy_Chaos(RecipeObj[,] slots)
            {
                var listA = _2DToList(Needs);
                var listB = _2DToList(slots);
                int count = 0;
                RecipeObj match;
                while (true)
                {
                    for (int a = 0; a < listA.Count; a++)
                    {
                        match = listB.FirstOrDefault(slot => slot?.Count > 0 && slot?.DBRef == listA[a]?.DBRef);
                        if (match == null)
                            return count;
                        if (listB[listB.IndexOf(match)].DBRef.Isnt<Outils>())
                            listB[listB.IndexOf(match)].Count--;
                    }
                    count++;
                }
            }
            */
            private bool SatisfiedBy_Chaos(RecipeObj[,] slots)
            {
                List<RecipeObj> slot_already_used = new List<RecipeObj>();
                var listA = _2DToList(Needs);
                var listB = _2DToList(slots);
                var allNeeds = listA.SelectMany(a => DB.GetListByType(a?.Type ?? DB.Types.Undefined).Concat(new List<int> { a.DBRef }));
                RecipeObj match;
                if (listB.Where(b => b != null).Any(b => !allNeeds.ToList().Contains(b.DBRef)))
                    return false;
                for (int a = 0; a < listA.Count; a++)
                {
                    match = listB.Except(slot_already_used).FirstOrDefault(slot => slot?.Count >= listA[a]?.Count && slot?.DBRef == listA[a]?.DBRef);
                    if (match == null)
                        return false;
                    if (listB[listB.IndexOf(match)].DBRef.IsTool() == false)
                    {
                        listB[listB.IndexOf(match)].Count--;
                        slot_already_used.Add(listB[listB.IndexOf(match)]);
                    }
                }
                return true;
            }
            private bool SatisfiedBy_ToolsOnTop(RecipeObj[,] slots)
            {
                var listA = _2DToList(Needs);
                var listB = _2DToList(slots);
                RecipeObj match;
                int layer, layer_tool = 0, layer_item = int.MaxValue;
                for (int a = 0; a < listA.Count; a++)
                {
                    var allNeeds = DB.GetListByType(listA[a]?.Type ?? DB.Types.Undefined).Concat((new List<int> { listA[a].DBRef })).ToList();
                    match = listB.FirstOrDefault(slot => slot != null && slot.Count >= allNeeds.Count && allNeeds.Contains(slot.DBRef));
                    if (match == null)
                        return false;
                    layer = -1;
                    for (int i = 0; i < slots.GetLength(0) && layer == -1; i++)
                        for (int j = 0; j < slots.GetLength(1) && layer == -1; j++)
                            if (slots[i, j]?.DBRef == match.DBRef)
                                layer = j;
                    if (layer == -1)
                        return false;
                    if (listB[listB.IndexOf(match)].DBRef.IsTool() == false)
                    {
                        listB[listB.IndexOf(match)].Count--;
                        if (layer < layer_item)
                            layer_item = layer;
                    }
                    else
                    {
                        if (layer > layer_tool)
                            layer_tool = layer;
                    }
                    if (listB[listB.IndexOf(match)].DBRef.IsTool() == false)
                        listB[listB.IndexOf(match)].Count--;
                }

                return layer_tool < layer_item;
            }
            private bool SatisfiedBy_Static(RecipeObj[,] slots)
            {
                var _slots = Duplicate(slots);
                if (Needs.GetLength(0) == 0 || Needs.GetLength(1) == 0 || Needs.GetLength(0) != slots.GetLength(0) || Needs.GetLength(1) != slots.GetLength(1))
                    return false;
                var single_need = -1;
                for (int i = 0; i < Needs.GetLength(0); i++)
                {
                    for (int j = 0; j < Needs.GetLength(1); j++)
                    {
                        var allNeeds = DB.GetListByType(Needs[i, j]?.Type ?? DB.Types.Undefined);
                        if(allNeeds.Count > 0 && single_need == -1)
                        {
                            if (!(allNeeds.Contains(_slots[i, j]?.DBRef ?? -1) && _slots[i, j]?.Count >= Needs[i,j].Count))
                                return false;
                            if (_slots[i, j] != null)
                            {
                                _slots[i, j].Count -= Needs[i, j].Count;
                                single_need = _slots[i, j].DBRef;
                            }
                        }
                        else
                        {
                            if ((allNeeds.Count > 0 && single_need > -1 ? single_need : (Needs[i, j]?.DBRef ?? -1)) != (_slots[i, j]?.DBRef ?? -1) || _slots[i, j]?.Count < Needs[i, j]?.Count)
                                return false;
                            if (_slots[i, j] != null && Needs[i, j] != null)
                                _slots[i, j].Count -= Needs[i, j].Count;
                        }
                        if (_slots[i, j]?.Count <= 0)
                            _slots[i, j] = null;
                    }
                }
                return true;
            }

            static RecipeObj[,] Duplicate(RecipeObj[,] array)
            {
                int width = array.GetLength(0);
                int height = array.GetLength(1);
                RecipeObj[,] result = new RecipeObj[width,height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        result[i, j] = array[i, j] != null ? new RecipeObj(array[i,j]) : null;
                    }
                }
                return result;
            }

            static List<RecipeObj> _2DToList(RecipeObj[,] array)
            {
                int width = array.GetLength(0);
                int height = array.GetLength(1);
                List<RecipeObj> ret = new List<RecipeObj>(width * height);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        ret.Add(array[i, j]?.Clone() ?? null);
                    }
                }
                return ret;
            }
        }
        public class RecipeFactory
        {
            public RecipeFactory()
            {
            }
            static RecipeFactory()
            {
                Recipes = new List<Recipe>();
                RecipeObj[,] needs;
                List<RecipeObj> results;
                Recipe recipe;

                #region 3x3
                Recipes.AddRange(new[]
                {
                    Recipes_Tools.CreateScytheA(),
                    Recipes_Tools.CreateScytheB(),
                    Recipes_Tools.CreateAxeA(),
                    Recipes_Tools.CreateAxeB(),
                    Recipes_Tools.CreatePickaxe(),
                    Recipes_Tools.CreateShovel(),
                    Recipes_Tools.CreateSword(),
                    Recipes_Tools.CreateHammer(),
                });

                needs = RecipeObj.Create3x3(@"
001
010
100", (0, Tex.WoodMiniStick, 0, 1));
                results = new List<RecipeObj> { new RecipeObj((int)DB.Tex.WoodStick, 1) };
                recipe = Create("WoodStick", RecipeMode.Static, needs, results);
                Recipes.Add(recipe);
                #endregion

                #region 2x2
                needs = new RecipeObj[,] { { new RecipeObj((int)DB.Tex.Wood, 1), new RecipeObj((int)DB.Tex.Wood, 1), new RecipeObj((int)DB.Tex.Wood, 1), new RecipeObj((int)DB.Tex.Wood, 1) } };
                results = new List<RecipeObj> { new RecipeObj((int)DB.Tex.Workbench, 1) };
                recipe = Create("Workbench", RecipeMode.Chaos, needs, results);
                Recipes.Add(recipe);

                needs = new RecipeObj[,] { { new RecipeObj((int)DB.Tex.Wood, 1) } };
                results = new List<RecipeObj> { new RecipeObj((int)DB.Tex.WoodMiniStick, 16) };
                recipe = Create("WoodMiniStick", RecipeMode.Chaos, needs, results);
                Recipes.Add(recipe);

                needs = new RecipeObj[,] { { new RecipeObj((int)DB.Tex.Pebble, 1), new RecipeObj((int)DB.Tex.Pebble, 1) },
                                                          { new RecipeObj((int)DB.Tex.Pebble, 1), new RecipeObj((int)DB.Tex.Pebble, 1) }};
                results = new List<RecipeObj> { new RecipeObj((int)DB.Tex.Stone, 1) };
                recipe = Create("Stone", RecipeMode.Chaos, needs, results);
                Recipes.Add(recipe);

                needs = new RecipeObj[,] { { new RecipeObj((int)DB.Tex.Stone, 1) }};
                results = new List<RecipeObj> { new RecipeObj((int)DB.Tex.Pebble, 4) };
                recipe = Create("Pebble", RecipeMode.Chaos, needs, results);
                Recipes.Add(recipe);
                #endregion
            }
            public static Recipe Create(string name, RecipeMode mode, RecipeObj[,] needs, List<RecipeObj> results)
            {
                RecipeObj[,] inter = new RecipeObj[needs.GetLength(1), needs.GetLength(0)];
                for (int y = 0; y < needs.GetLength(0); y++)
                    for (int x = 0; x < needs.GetLength(1); x++)
                        inter[x, y] = needs[y, x];
                needs = inter;

                switch (mode)
                {
                    default:
                    case RecipeMode.Chaos: return CreateChaos(name, needs, results);
                    case RecipeMode.ToolsOnTop: return CreateToolsOnTop(name, needs, results);
                    case RecipeMode.Static: return CreateStatic(name, needs, results);
                }
            }

            private static Recipe CreateChaos(string name, RecipeObj[,] needs, List<RecipeObj> results)
            {
                Recipe recipe = new Recipe(name, RecipeMode.Chaos) { Needs = needs, Results = results };
                return recipe;
            }
            private static Recipe CreateToolsOnTop(string name, RecipeObj[,] needs, List<RecipeObj> results)
            {
                Recipe recipe = new Recipe(name, RecipeMode.ToolsOnTop) { Needs = needs, Results = results};
                return recipe;
            }
            private static Recipe CreateStatic(string name, RecipeObj[,] needs, List<RecipeObj> results)
            {
                Recipe recipe = new Recipe(name, RecipeMode.Static) { Needs = needs, Results = results };
                return recipe;
            }
        }

        public static List<Recipe> Recipes;
    }
}
