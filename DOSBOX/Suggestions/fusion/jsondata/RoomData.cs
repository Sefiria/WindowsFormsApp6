﻿using System;
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
        public bool HasFlies { get; set; } = false;
    }
}
