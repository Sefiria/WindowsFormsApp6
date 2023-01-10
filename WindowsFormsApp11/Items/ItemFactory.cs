using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Items
{
    public class ItemFactory
    {
        public static Item Create()
        {
            int itemsCount = 6;
            switch(Var.Rnd.Next(itemsCount))
            {
                default:
                case 0: return new ItemBlue();
                case 1: return new ItemGreen();
                case 2: return new ItemRed();
                case 3: return new ItemYellow();
                case 4: return new ItemBlack();
                case 5: return new ItemWhite();
            }
        }
    }
}
