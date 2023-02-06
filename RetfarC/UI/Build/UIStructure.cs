using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC.UI.Build
{
    public abstract class UIStructure
    {
        public Bitmap Image;
        public abstract List<Item> Needs { get; set; }
    } 
}
