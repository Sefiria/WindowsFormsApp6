using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;

namespace WindowsFormsApp6.World.Structures
{
    public class StructureTower : StructureBase
    {
        public StructureTower(int x, int y) : base(x, y, 2, 2, Resources.structure_tower)
        {
        }

        public override void MouseDown()
        {
            Data.Instance.Tower.PreviousState = Data.Instance.State;
            Data.Instance.State = Data.Instance.Tower;
        }
    }
}
