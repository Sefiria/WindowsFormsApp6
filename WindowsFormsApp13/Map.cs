using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp13.Properties;

namespace WindowsFormsApp13
{
    internal class Map
    {
        public int w = 512;
        public int h = 512;
        public int hw => w / 2;
        public int hh => h / 2;
        public Bitmap Image;

        public Map()
        {
            //Image = new Bitmap(w, h);
            //using (Graphics g = Graphics.FromImage(Image))
            //{
            //    g.Clear(Color.Black);
            //    g.DrawLine(Pens.White, w * .75F, hh - 32, w * .75F, hh + 32);
            //}
            Image = new Bitmap(Resources.map1);
            w = Image.Width;
            h = Image.Height;
        }
    }
}
