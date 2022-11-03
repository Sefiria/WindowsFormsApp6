using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;

namespace WindowsFormsApp6.World.Entities
{
    public class Unit : Entity
    {
        public Unit(float x, float y) : base(x, y, Resources.unit.Transparent())
        {
        }

        public override void Update()
        {
        }
    }
}
