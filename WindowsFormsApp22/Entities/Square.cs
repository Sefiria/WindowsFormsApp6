using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp22.Entities
{
    public class Square : Shape
    {
        public Square() : base("square", Color.Yellow, 32, 32) { }

        public override void Update()
        {
            base.Update();
            weight = 30;
        }
    }
}
