using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp22.Entities
{
    internal class atk_circle : Circle
    {
        public atk_circle(float _x, float _y)
        {
            X = _x;
            Y = _y;
            weight = 30;
        }

        public override void Update()
        {
            var look = Pos.LookToPlayer();
            Pos = Pos.PlusF(look.x(0.01F));


        }
        public bool Collides(Entity e)
        {
            //if (e is Shape)
            //{
            //    switch (e)
            //    {
            //        case Square square: return Maths.coll;
            //        case Circle circle: return true;
            //    }
            //}
            return false;
        }
    }
}
