using RetfarC.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC
{
    public class Plant : Resource
    {
        public Plant(float x, float y) : base(x, y)
        {
            Image = Resources.r_plant;
            Image.MakeTransparent();
            Items.Add(new Fiber(1, x, y));
            HP = HPMax = 5;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
