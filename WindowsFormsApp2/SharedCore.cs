using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.ATiles;

namespace WindowsFormsApp2
{
    public static class SharedCore
    {
        public static Graphics g;
        public static readonly int TileSize = 32;
        public static ATileWater ATileWater = new ATileWater();
        public static int RenderW, RenderH;
        public static Font Font = new Font("Segoe UI", 16F, FontStyle.Regular);
        public static Point MouseLocation;

        public static void Load(MainForm form)
        {
            g = Graphics.FromImage(form.Img);
            RenderW = form.Render.Width;
            RenderH = form.Render.Height;
        }

        public static void Dispose()
        {
            g.Dispose();
        }
    }
}
