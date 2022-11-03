using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.World.Blocs.Consommables;

namespace WindowsFormsApp6.World.Entities
{
    public class Unit : Entity
    {
        public ConsoBase Target = null;

        public Unit(float x, float y) : base(x, y, "unit")
        {
        }

        public override void Update()
        {
            if(Target != null)
            {
                if (Maths.Distance(X, Y, Target.X.ToWorld(), Target.Y.ToWorld()) > 25)
                {
                    var look = Maths.GetLook(Target.X.ToWorld(), Target.Y.ToWorld(), X, Y);
                    X += look.X;
                    Y += look.Y;
                }
                else
                {
                    if (Target.Used)
                        Target = null;
                    else
                        Target.Life--;
                }
            }
        }
    }
}
