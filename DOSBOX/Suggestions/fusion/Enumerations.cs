using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.fusion
{
    public class Enumerations
    {
        public enum PhysicalObjectType
        {
            button=0,
        }
        public enum PhysicalObjectSide
        {
            Top = 0,
            Left,
            Bottom,
            Right,
        }
    }
}
