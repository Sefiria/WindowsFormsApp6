using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public static class Extensions
    {
        public static vec ToTile(this vec v) => v / GraphicsManager.CharSize.V();
    }
}
