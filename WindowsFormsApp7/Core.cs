using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp7
{
    public class Core
    {
        public static Form1 MainForm;
        public static int TileSz, Zoom;
        public static Point MousePosition = new Point(-1, -1);
        public static Bitmap Image, ImageUI;
        public static int IterationsCount = 4;
        public static int TileSzZoom =>TileSz * Zoom;

        public static Graphics gui;

        public static Graphics g;
        public static int ExactRenderW, ExactRenderH;
        public static int RW, RH;
        public static int RWT => RW / TileSz;
        public static int RHT => RH / TileSz;
        public static int RWTZ => RWT / Zoom;
        public static int RHTZ => RHT / Zoom;
        public static int RWxZ => RWT * Zoom;
        public static int RHxZ => RHT * Zoom;

        public static Graphics gp;
        public static int RPW, RPH;
        public static int RPWT => RPW / TileSz;
        public static int RPHT => RPH / TileSz;
    }
}
