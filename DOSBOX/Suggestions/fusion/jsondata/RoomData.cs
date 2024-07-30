using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.fusion.jsondata
{
    internal class RoomData
    {
        public RoomData_doors[] doors { get; set; }
        public RoomData_warps[] warps { get; set; }
        public RoomData_mobs[] mobs { get; set; }
        public RoomData_objects[] objects { get; set; }
        public bool HasFlies { get; set; } = true;
    }
}
