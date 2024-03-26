﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._AbstractObject
{
    class CraftAttribute : AbstractObject
    {
        public new static string GetEntityPath()
        {
            return AbstractObject.GetEntityPath() + "/CraftAttribute";
        }
    }
}
