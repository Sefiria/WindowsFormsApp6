using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsCardsApp15.scenes;

namespace WindowsCardsApp15
{
    internal class Core
    {
        public static int W, H, CardW = 96, CardH = 128;
        public static Graphics g;
        public static Scene Scene;
        public static bool Quitter = false;
        public static Font Font = new Font("Segoe UI", 12F, FontStyle.Regular);
    }
}
