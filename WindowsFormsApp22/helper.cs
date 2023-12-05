using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp22
{
    public static class helper
    {
        public static PointF LookToPlayer(this PointF from) => Core.Player.Pos.Minus(from).norm();
    }
}
