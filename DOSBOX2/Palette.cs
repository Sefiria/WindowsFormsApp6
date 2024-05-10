using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX2
{
    public class Palette
    {
        public int size;
        public int[] colors;
        public Palette(){}
        public Palette(int[] colors)
        {
            size = colors.Length;
            this.colors = colors;
        }
        public Color this[int index] => Color.FromArgb(colors[index]);
    }
}
