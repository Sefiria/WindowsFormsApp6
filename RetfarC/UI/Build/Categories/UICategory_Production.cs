using RetfarC.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC.UI.Build
{
    public class UICategory_Production : UICategory
    {
        public override List<UIStructure> Structures { get; set; } = new List<UIStructure>()
        {
            new UIStructure_Workbench(),
            //new UIStructure_Workbench(),
            //new UIStructure_Workbench(),
        };

        public UICategory_Production()
        {
            Image = Resources.uicategory_production;
            Image.MakeTransparent(Color.Black);
        }
    }
}
