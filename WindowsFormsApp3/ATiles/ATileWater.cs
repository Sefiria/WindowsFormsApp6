using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp3.Properties;

namespace WindowsFormsApp3.ATiles
{
    public class ATileWater : ATileBase
    {
        public ATileWater() : base("atile_water")
        {
        }

        public new void Draw(int X, int Y)
        {
            ATileBase.Draw(X, Y);
        }
    }
}
