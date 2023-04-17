using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp17.items;

namespace WindowsFormsApp17
{
    internal class fluidmgr
    {
        public List<fluid> fluids;

        public fluidmgr()
        {
            fluids = new List<fluid>();
        }
        public void AddFluid(int x, int y)
        {
            fluids.Add(new fluid(x, y, 4F));
        }
        public void AddFluid(Point loc)
        {
            fluids.Add(new fluid(loc.X, loc.Y, 4F));
        }

        internal void Update()
        {
            var _fluids = new List<fluid>(fluids);
            var _murs = new List<mur>(Data.Instance.murs);

            foreach (var fluid in _fluids)
            {
                if (fluid.Destroy)
                    fluids.Remove(fluid);
                else
                    fluid.Update(_fluids.Except(new List<fluid>() { fluid }).ToList(), _murs);
            }
        }
    }
}
