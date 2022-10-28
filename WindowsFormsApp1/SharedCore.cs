using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class SharedCore
    {
        public static Graphics g;
        public static readonly int TileSize = 48;

        public static void Load(MainForm form)
        {
            g = Graphics.FromImage(form.Img);
        }
    }
}
