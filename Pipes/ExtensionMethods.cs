using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipes
{
    public static class ExtensionMethods
    {
        public static Point ToTile(this Point pt) => new Point((pt.X / Core.TSZ) * Core.TSZ, (pt.Y / Core.TSZ) * Core.TSZ);
    }
}
