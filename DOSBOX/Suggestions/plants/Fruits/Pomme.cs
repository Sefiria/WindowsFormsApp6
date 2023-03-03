using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.plants.Fruits
{
    public class Pomme : Fruit
    {
        public Pomme(vecf vec) : base(vec)
        {
            DisplayAlwaysExact = true;
            CreateGraphics();
        }

        private void CreateGraphics()
        {
            g = new byte[8, 8]
            {
                { 0, 2, 3, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 3, 0, 3, 0, 0 },
                { 0, 0, 2, 2, 2, 2, 0, 0 },
                { 0, 2, 1, 4, 2, 1, 2, 0 },
                { 0, 2, 4, 1, 1, 1, 2, 0 },
                { 0, 2, 1, 1, 1, 1, 2, 0 },
                { 0, 2, 1, 1, 1, 1, 2, 0 },
                { 0, 0, 2, 2, 2, 2, 0, 0 },
            };
        }
    }
}
