using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolActivityMaker
{
    public class ToolStripTime : ToolStripControlHost
    {
        public TimeControl TimeControl
        {
            get
            {
                return Control as TimeControl;
            }
        }

        public ToolStripTime() : base(new TimeControl())
        {

        }
    }
}
