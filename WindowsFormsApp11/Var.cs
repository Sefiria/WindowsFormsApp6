using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11
{
    public class Var
    {
        public static Data Data;

        public static int W, H;
        public static Graphics g;
        public static Random Rnd = new Random((int)DateTime.UtcNow.Ticks);

        public static void Initialize()
        {
            Data = new Data();
            CraftingTable.Initialize();
        }
    }
}
