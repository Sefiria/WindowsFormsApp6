using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp10
{
    public class Peix
    {
        public float X, Y;
        public float Diameter = 1F;
        public float Radius => Diameter / 2F;

        public Peix(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Update()
        {
            if (Data.Plants.Count == 0)
                return;

            var list = new List<Plant>(Data.Plants);
            float d;
            float min_d = float.MaxValue;
            int target = -1;
            for (int i=0; i < list.Count; i++)
            {
                d = Maths.Distance(X, Y, list[i].X, list[i].Y);
                if(d < min_d)
                {
                    min_d = d;
                    target = i;
                }
            }

            X += Maths.Normalize(list[target].X - X);
            Y += Maths.Normalize(list[target].Y - Y);

            if((int)Maths.Distance(X, Y, list[target].X, list[target].Y) < (Radius < 1F ? 1F : Radius) + list[target].Size)
            {
                Data.Plants.RemoveAt(target);
                Diameter += list[target].Size / 2F;
            }
        }
    }
}
