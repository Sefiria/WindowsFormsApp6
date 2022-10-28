using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.World.Ores;

namespace WindowsFormsApp6.World.Blocs
{
    public class BlocStone : BlocBase
    {
        public BlocStone(int x, int y, int layer = 0) : base(x, y, Resources.stone, layer)
        {
            Life = 10;
        }
    }
}
