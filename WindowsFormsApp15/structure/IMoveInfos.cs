using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15.structure
{
    internal interface IMoveInfos
    {
        float Speed { get; set; }
        Way Way { get; set; }
    }
}
