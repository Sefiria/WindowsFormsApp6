using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp14
{
    internal class Map
    {
        public int w => 256;
        public int h => 256;
        public int hw => w / 2;
        public int hh => h / 2;
        public Bitmap Image;

        public Map()
        {
            Image = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(Image))
            {
                g.Clear(Color.Black);
                g.DrawLine(new Pen(Color.Red, 2F), w * .65F, hh - 48, w * .75F, hh - 32);
                g.DrawLine(new Pen(Color.Green, 2F), w * .75F, hh - 32, w * .75F, hh + 32);
                g.DrawLine(new Pen(Color.Blue, 2F), w * .75F, hh + 32, w * .65F, hh + 48);
            }
        }
    }
}
