using RetfarC.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC
{
    public class Stone : Resource
    {
        public Stone(float x, float y) : base(x, y)
        {
            Image = Resources.r_stone;
            Image.MakeTransparent();
            Items.Add(new Cobblestone(1, x, y));
            HP = HPMax = 10;
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
