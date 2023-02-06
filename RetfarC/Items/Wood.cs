﻿using RetfarC.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC
{
    public class Wood : Item
    {
        public Wood(int count, float x = 0F, float y = 0F) : base(x, y, count)
        {
            Image = Resources.i_wood;
            Image.MakeTransparent();
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
