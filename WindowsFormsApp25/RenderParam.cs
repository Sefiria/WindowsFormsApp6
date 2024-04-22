using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp25
{
    public class RenderParam
    {
        public Graphics g = null;
        public Bitmap img = null;
        public bool auto_clear = true;
        public RenderParam()
        {
        }
        public void ResetGraphics() => g = Graphics.FromImage(img);
    }
}
