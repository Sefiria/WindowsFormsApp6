using DOSBOX.Utilities;
using System.Collections.Generic;
using Tooling;

namespace DOSBOX.Suggestions
{
    internal class Fiole : Disp
    {
        public static readonly int w = 8, h = 32;
        public static int MaxIngredients => 8;
        List<string> Ingredients;

        public Fiole(vec vec)
        {
            this.vec = new vec(vec);
            CreateGraphics();
        }

        private void CreateGraphics()
        {
            byte c = 2;
            g = new byte[w, h];
            for (int x = 2; x <w - 2; x++)
            {
                g[x, h-1] = c;
            }
            for (int y = 0; y < h - 1; y++)
            {
                g[0, y] = c;
                g[w-1, y] = c;
            }
            for (int y = h - 2; y < h - 1; y++)
            {
                g[0, y] = 0;
                g[1, y] = c;
                g[w - 1, y] = 0;
                g[w - 2, y] = c;
            }
        }

        public void Display(List<string> Ingredients)
        {
            this.Ingredients = Ingredients;
            Display(2);
        }
        /// <summary>
        /// Call directly Display(List<string> Ingredients), not this
        /// </summary>
        public override void Display(int layer, vec srcLoc = null, byte[,] ActiveBG = null)
        {
            for (int y = 16; y < 48; y++)
                for (int x = 0; x < 64; x++)
                    Core.Layers[2][x, y] = (byte)((x+y)%3 == 0 ? 1 : 0);

            byte IngredientColor(int i)
            {
                switch(Ingredients[i])
                {
                    default: return 1;
                    case "S": return 1;
                    case "X": return 3;
                    case "O": return 4;
                    case "K": return 2;
                    case "235": return 3;
                }
            }

            List<(int x, int y, byte c)> outfiolepx = new List<(int x, int y, byte c)>();
            void add(int x, int y) => outfiolepx.Add((x, y, Core.Layers[2][vec.x + x, vec.y + y]));
            add(0, h - 2);
            add(w - 1, h - 2);
            add(0, h - 1);
            add(1, h - 1);
            add(w - 2, h - 1);
            add(w - 1, h - 1);

            int hsz = h / MaxIngredients;
            for (int i=0; i< Ingredients.Count; i++)
            {
                for (int y = 0; y < hsz; y++)
                    for (int x = 0; x < w; x++)
                        Core.Layers[2][vec.x + x, vec.y + h - (i+1) * hsz + y] = IngredientColor(i);
            }

            foreach (var px in outfiolepx)
                Core.Layers[2][vec.x + px.x, vec.y + px.y] = px.c;

            base.Display(layer, srcLoc, ActiveBG);
        }
    }
}
