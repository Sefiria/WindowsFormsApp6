﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defacto_rio
{
    public class ItemPrototype : Prototype
    {
        public string icon;
        public string icon_size;
        public string stack_size;

        public override string ToString()
        {
            return name;
        }
    }
}
