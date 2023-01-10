using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Items
{
    public class ItemWhite : Item
    {
        public ItemWhite() : base(1) { Color = System.Drawing.Color.White.ToArgb(); }
        public ItemWhite(int count = 1) : base(count)
        {
            Color = System.Drawing.Color.White.ToArgb();
        }
    }
}
