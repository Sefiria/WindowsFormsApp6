using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsCardsApp15.Properties;

namespace WindowsCardsApp15.data
{
    internal class Rsx
    {
        public static Bitmap FromID(byte id)
        {
            switch(id)
            {
                default:
                case 0: return Resources.class_warrior;
                case 1: return Resources.class_mage;
                case 2: return Resources.class_rogue;
            }
        }
    }
}
