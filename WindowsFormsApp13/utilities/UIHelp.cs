using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp13.utilities
{
    internal class UIHelp
    {
        internal static void DrawSuper(Graphics gui, int w, int super, Color color, int y)
        {
            int sz = 20;
            if (super == Cam.super_min)
            {
                gui.FillEllipse(new SolidBrush(color), w - sz - 5, 5 + (sz + 5) * y, sz, sz);
                gui.DrawEllipse(Pens.White, w - sz - 5, 5 + (sz + 5) * y, sz, sz);
            }
            else if (super >= 0F)
            {
                float prct = super / (float)Cam.super_max;

                gui.FillEllipse(new SolidBrush(color), w - sz - 5, 5 + (sz + 5) * y, sz, sz);
                gui.FillRectangle(Brushes.Black, w - 5 - (int)(sz * (1F - prct)), 5 + (sz + 5) * y, sz, sz);
                gui.DrawEllipse(new Pen(color), w - sz - 5, 5 + (sz + 5) * y, sz, sz);
            }
            else
            {
                float prct = super / (float)Cam.super_min;

                gui.FillEllipse(new SolidBrush(Color.FromArgb(150, color)), w - sz - 5, 5 + (sz + 5) * y, sz, sz);
                gui.FillRectangle(Brushes.Black, w - 5 - (int)(sz * (1F - prct)), 5 + (sz + 5) * y, sz, sz);
                gui.DrawEllipse(Pens.Gray, w - sz - 5, 5 + (sz + 5) * y, sz, sz);
            }
        }
    }
}
