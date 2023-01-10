using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Items
{
    public class ItemYellow : Item
    {
        public ItemYellow() : base(1) { Color = System.Drawing.Color.Yellow.ToArgb(); }
        public ItemYellow(int count = 1) : base(count)
        {
            Color = System.Drawing.Color.Yellow.ToArgb();
        }
    }
}
