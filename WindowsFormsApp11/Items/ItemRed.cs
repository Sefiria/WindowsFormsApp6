using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Items
{
    public class ItemRed : Item
    {
        public ItemRed() : base(1) { Color = System.Drawing.Color.Red.ToArgb();}
        public ItemRed(int count = 1) : base(count)
        {
            Color = System.Drawing.Color.Red.ToArgb();
        }
    }
}
