using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public class TheRecipes
    {
        public enum RecipeMode
        {
            Chaos=0, ToolsOnTop, Static,
        }
        public class RecipeObj
        {
            public int DBRef, Count;
            public RecipeObj()
            {
            }
            public RecipeObj(int DBRef, int Count)
            {
                this.DBRef = DBRef;
                this.Count = Count;
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
            public int SatisfiedCountBy(RecipeObj[,] slots)
            {
                switch(Mode)
                {
                    default:
                    case RecipeMode.Chaos: return SatisfiedCountBy_Chaos(slots);
                    case RecipeMode.ToolsOnTop: return SatisfiedCountBy_ToolsOnTop(slots);
                    case RecipeMode.Static: return SatisfiedCountBy_Static(slots);
                }
            }
            private int SatisfiedCountBy_Chaos(RecipeObj[,] slots)
            {
                var listA = _2DToList(Needs);
                var listB = _2DToList(slots);
                return listA.Count(a => listB.Where(b => b.DBRef == a.DBRef).Any());

            }
            private int SatisfiedCountBy_ToolsOnTop(RecipeObj[,] slots)
            {
                return 0;
            }
            private int SatisfiedCountBy_Static(RecipeObj[,] slots)
            {
                return 0;
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
                        ret.Add(array[i, j]);
                    }
                }
                return ret;
            }
        }
        public class RecipeFactory
        {
            public static Recipe Create(string name, RecipeMode mode, RecipeObj[,] needs, List<RecipeObj> results)
            {
                RecipeObj[,] inter = new RecipeObj[needs.GetLength(0), needs.GetLength(1)];
                for (int y = 0; y < needs.GetLength(1); y++)
                    for (int x = 0; x < needs.GetLength(0); x++)
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
                Recipe recipe = new Recipe(name, RecipeMode.Chaos);
                return recipe;
            }
            private static Recipe CreateToolsOnTop(string name, RecipeObj[,] needs, List<RecipeObj> results)
            {
                Recipe recipe = new Recipe(name, RecipeMode.ToolsOnTop);
                return recipe;
            }
            private static Recipe CreateStatic(string name, RecipeObj[,] needs, List<RecipeObj> results)
            {
                Recipe recipe = new Recipe(name, RecipeMode.Static);
                return recipe;
            }
        }

        public static List<Recipe> Recipes = new List<Recipe>
        {
            RecipeFactory.Create("Bois de chauffe", RecipeMode.ToolsOnTop, new RecipeObj[,]
            {
                { new RecipeObj((int)Outils.Hache, 1), new RecipeObj((int)Objets.Buche, 1) }
            }, new List<RecipeObj>
            {
                new RecipeObj((int)Objets.BoisDeChauffe, 1)
            }),
            RecipeFactory.Create("Test Chaos", RecipeMode.Chaos, new RecipeObj[,]
            {
                { new RecipeObj((int)Outils.Faux, 1), new RecipeObj((int)Objets.Buche, 1) }
            }, new List<RecipeObj>
            {
                new RecipeObj((int)Objets.BoisDeChauffe, 1)
            }),
            RecipeFactory.Create("Test Static", RecipeMode.Static, new RecipeObj[,]
            {
                { null, new RecipeObj((int)Objets.Buche, 1), null },
                { null, new RecipeObj((int)Outils.Pelle, 1), null }
            }, new List<RecipeObj>
            {
                new RecipeObj((int)Objets.BoisDeChauffe, 1)
            }),
        };
    }
}
