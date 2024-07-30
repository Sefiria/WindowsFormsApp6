using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace DOSBOX.Suggestions.fusion.jsondata
{
    public class RoomData_coord
    {
        public int x { get; set; }
        public int y { get; set; }

        public vec AsVec() => new vec(x, y);
    }
}
