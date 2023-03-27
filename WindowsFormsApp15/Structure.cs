using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp15.utilities.tiles;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;

namespace WindowsFormsApp15
{
    internal class Structure
    {
        public anim anim, frontanim = null;
        public vecf vec;

        public List<Item> ItemsOnTile => Data.Instance.Items.Where(i => i.rect.IntersectsWith(new RectangleF(vec.x, vec.y, Core.TSZ, Core.TSZ))).ToList();

        protected Structure(vecf vec = null)
        {
            this.vec = new vecf(vec);
        }

        public virtual void Update() { }

        public void Display()
        {
            anim?.Display(vec.snap(Core.TSZ));
        }
        public void FrontDisplay()
        {
            frontanim?.Display(vec.snap(Core.TSZ));
        }
    }
}
