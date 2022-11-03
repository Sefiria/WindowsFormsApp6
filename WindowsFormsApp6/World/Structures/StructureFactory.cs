using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;

namespace WindowsFormsApp6.World.Structures
{
    public class StructureFactory : StructureBase
    {
        public StructureFactory(int x, int y) : base(x, y, 4, 3, Resources.structure_factory)
        {
        }

        public override void MouseDown()
        {
            Data.Instance.Factory.PreviousState = Data.Instance.State;
            Data.Instance.Factory.RemoveUI();
            Data.Instance.Factory.AddCategoryUI();
            Data.Instance.State = Data.Instance.Factory;
        }
    }
}
