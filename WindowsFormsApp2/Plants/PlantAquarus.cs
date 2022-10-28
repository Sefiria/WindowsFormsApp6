using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Entities;

namespace WindowsFormsApp2.Plants
{
    public class PlantAquarus : PlantBase
    {
        public PlantAquarus(Point? coord = null, bool drawable = false) : base(Items.PlantAquarus, coord, drawable)
        {
            Alpha = 10;
            Beta = 200;
            Gamma = 128;
            Teta = 88;
            Zeta = 255;
            Omega = 0.128F;
            Rho = -0.800F;
        }

        public static List<Item> Repeat(int count, Point? Coord = null, bool Drawable = false) => Enumerable.Repeat(new PlantAquarus(Coord, Drawable), count).Cast<Item>().ToList();
    }
}
