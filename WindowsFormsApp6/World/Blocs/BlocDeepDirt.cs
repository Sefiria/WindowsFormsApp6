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
    public class BlocDeepDirt : BlocBase
    {
        public BlocDeepDirt(int x, int y, int layer = 0) : base(x, y, Resources.deepdirt, layer)
        {
            Life = 5;
        }
    }
}
