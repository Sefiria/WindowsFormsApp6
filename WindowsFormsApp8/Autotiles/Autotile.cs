using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp8.Autotiles
{
    public class Autotile
    {
        private Bitmap Resource;
        private Dictionary<string, List<Bitmap>> ResSplit = new Dictionary<string, List<Bitmap>>();
        private int Type;

        public Autotile(Bitmap res)
        {
            int w = res.Width;
            int h = res.Height;
            int s = Core.TileSz;
            int wt = w / s;
            int ht = h / s;
            int t;

            if (wt == 1 && ht == 1)
                t = 0;
            else if (wt == 4 && ht == 6)
                t = 1;
            else if (wt == 4 && ht == 8)
                t = 2;
            else throw new Exception("Autotile.cs L26 : Resource has incorrect size");


            Resource = res;
            Type = t;

            AutoSplit();
        }

        private void AutoSplit()
        {
            switch(Type)
            {
                default:
                case 0: AutoSplit0(); return;
                case 1: AutoSplit1(); return;
                case 2: AutoSplit2(); return;
            }
        }
        private void AutoSplit0()
        {
            Bitmap res = Resource;
            ResSplit["single"] = new List<Bitmap() { res };// et les gradients coco ????
        }
        private void AutoSplit1()
        {
            Bitmap res = Resource;
        }
        private void AutoSplit2()
        {
        }

        public Bitmap GetTileImage(int gradientIndex)
        {
            if (ResSplit.Count == 1)
                return ResSplit["single"][0];


        }
    }
}
