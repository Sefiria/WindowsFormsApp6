using RetfarC.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC.UI.Build
{
    public class UICategory_Test1 : UICategory
    {
        public override List<UIStructure> Structures { get; set; } = new List<UIStructure>()
        {
            new UIStructure_Workbench(),
            new UIStructure_Workbench(),
            new UIStructure_Workbench(),
        };

        public UICategory_Test1()
        {
            Image = Resources.notex;
        }
    }
}
