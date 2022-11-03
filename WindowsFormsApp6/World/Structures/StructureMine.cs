using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;

namespace WindowsFormsApp6.World.Structures
{
    public class StructureMine : StructureBase
    {
        public StructureMine(int x, int y) : base(x, y, 4, 3, Resources.structure_mine)
        {
        }

        public override void MouseDown()
        {
            Data.Instance.Mine.PreviousState = Data.Instance.State;
            Data.Instance.State = Data.Instance.Mine;
        }
    }
}
