using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Items
{
    public class ItemBlue : Item
    {
        public ItemBlue() : base(1) { Color = System.Drawing.Color.Blue.ToArgb(); }
        public ItemBlue(int count = 1) : base(count)
        {
            Color = System.Drawing.Color.Blue.ToArgb();
        }
    }
}
