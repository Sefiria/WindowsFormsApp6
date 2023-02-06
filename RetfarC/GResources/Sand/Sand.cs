using RetfarC.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC
{
    public class Sand : Resource
    {
        public Sand(float x, float y) : base(x, y)
        {
            Image = Resources.r_sand;
            Image.MakeTransparent();
            Items.Add(new SandDust(1, x, y));
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
