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
    public class TowerUnit : Entity
    {
        public int LookX = 0, LookY = 0;
        public int CheckpointID = 0;
        public float Speed = 0.05F;
        public TowerUnit(float x, float y, int lookX = 1, int lookY = 0) : base(x, y, "unit")
        {
            LookX = lookX;
            LookY = lookY;
        }

        public override void Update()
        {
            X += LookX * Speed;
            Y += LookY * Speed;
        }
    }
}
