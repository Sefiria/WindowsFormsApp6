using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Items
{
    public class ItemGreen : Item
    {
        public ItemGreen() : base(1) { Color = System.Drawing.Color.Lime.ToArgb(); }
        public ItemGreen(int count = 1) : base(count)
        {
            Color = System.Drawing.Color.Lime.ToArgb();
        }
    }
}
