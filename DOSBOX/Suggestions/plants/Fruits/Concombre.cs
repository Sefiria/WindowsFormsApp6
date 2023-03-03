using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.plants.Fruits
{
    public class Concombre : Fruit
    {
        public Concombre(vecf vec) : base(vec)
        {
            CreateGraphics();
        }

        private void CreateGraphics()
        {
            g = new byte[7, 3]
            {
                { 2, 3, 2 },
                { 2, 1, 2 },
                { 2, 1, 2 },
                { 2, 1, 2 },
                { 2, 1, 2 },
                { 2, 1, 2 },
                { 2, 3, 2 },
            };
        }
    }
}
