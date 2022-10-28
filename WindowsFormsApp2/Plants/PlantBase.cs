using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Entities;

namespace WindowsFormsApp2.Plants
{
    public abstract class PlantBase : Item
    {
        public byte Alpha, Beta, Gamma, Teta, Zeta;
        public float Omega, Rho;

        public PlantBase(Items type, Point? coord = null, bool drawable = false) : base(type, coord, drawable)
        {
            Alpha = Beta = Gamma = Teta = Zeta = 0;
            Omega = Rho = 0F;
        }
    }
}
