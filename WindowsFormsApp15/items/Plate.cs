using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp15.structure;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15.items
{
    internal class Plate : Item
    {
        public Plate(Ores ore, vecf vec) : base(Items.Plate, vec)
        {
            OreType = ore;
            anim = new anim(0.2F, new List<Bitmap> { AnimRes.plates[OreType] });
        }
    }
}
