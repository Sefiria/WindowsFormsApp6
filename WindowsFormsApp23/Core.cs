using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp23
{
    internal class Core
    {
        internal static int RenderW, RenderH;
        internal static int SZ => 256;
        internal static Graphics render_g, g;
        internal static Map Map;

        static Core()
        {
            Map = new Map();
        }
    }
}
