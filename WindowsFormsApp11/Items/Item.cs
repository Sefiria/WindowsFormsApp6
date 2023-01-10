using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Items
{
    public abstract class Item
    {
        public int Count = 1;
        public int Color;

        public Item(int count)
        {
            Count = count;
            Color = System.Drawing.Color.White.ToArgb();
        }
    }
}
