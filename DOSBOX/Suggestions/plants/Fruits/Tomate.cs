using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.plants.Fruits
{
    public class Tomate : Fruit
    {
        public Tomate(vecf vec) : base(vec)
        {
            CreateGraphics();
        }

        private void CreateGraphics()
        {
            g = new byte[5, 5]
            {
                { 2, 3, 3, 3, 2 },
                { 3, 1, 2, 3, 3 },
                { 3, 2, 3, 3, 3 },
                { 3, 3, 3, 3, 3 },
                { 2, 3, 3, 3, 2 },
            };
        }
    }
}
