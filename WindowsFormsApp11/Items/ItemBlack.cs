using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Items
{
    public class ItemBlack : Item
    {
        public ItemBlack() : base(1) { Color = System.Drawing.Color.Black.ToArgb(); }
        public ItemBlack(int count = 1) : base(count)
        {
            Color = System.Drawing.Color.Black.ToArgb();
        }
    }
}
