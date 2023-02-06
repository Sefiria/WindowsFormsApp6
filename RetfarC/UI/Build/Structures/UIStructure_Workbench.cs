using RetfarC.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC.UI.Build
{
    public class UIStructure_Workbench : UIStructure
    {
        public UIStructure_Workbench()
        {
            Image = Resources.uistruct_workbench;
            Image.MakeTransparent(Color.Black);
        }

        public override List<Item> Needs { get; set; } = new List<Item>()
        {
            new Wood(10)
        };
    }
}
