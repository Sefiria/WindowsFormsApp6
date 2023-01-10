using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp10
{
    public class Plant
    {
        public int X, Y;
        public float Size;

        public Plant(int x, int y, float size)
        {
            X = x;
            Y = y;
            Size = size >= 1F ? size : 1F;
        }
    }
}
