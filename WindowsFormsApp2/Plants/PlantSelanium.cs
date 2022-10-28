using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Entities;

namespace WindowsFormsApp2.Plants
{
    public class PlantSelanium : PlantBase
    {
        public PlantSelanium(Point? coord = null, bool drawable = false) : base(Items.PlantSelanium, coord, drawable)
        {
            Alpha = 111;
            Beta = 222;
            Gamma = 24;
            Teta = 16;
            Zeta = 0;
            Omega = -0.951F;
            Rho = 0.123F;
        }

        public static List<Item> Repeat(int count, Point? Coord = null, bool Drawable = false) => Enumerable.Repeat(new PlantSelanium(Coord, Drawable), count).Cast<Item>().ToList();
    }
}
