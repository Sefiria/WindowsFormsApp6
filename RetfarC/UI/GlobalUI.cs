using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC
{
    public static class GlobalUI
    {
        public static bool DisplayBuild { get => UIBuild.Display; set => UIBuild.Display = value; }

        public static void Draw()
        {
            UIBuild.Draw();
        }
    }
}
