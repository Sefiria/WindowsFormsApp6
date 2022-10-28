using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;

namespace WindowsFormsApp6.World.Structures
{
    public class StructureShop : StructureBase
    {
        public StructureShop(int x, int y) : base(x, y, 4, 3, Resources.structure_shop)
        {
        }

        public override void MouseDown()
        {
            Data.Instance.Shop.PreviousState = Data.Instance.State;
            Data.Instance.Shop.RemoveAcheterAndVendreUI();
            Data.Instance.State = Data.Instance.Shop;
        }
    }
}
