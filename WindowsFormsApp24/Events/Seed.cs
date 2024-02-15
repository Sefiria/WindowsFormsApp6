using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp24.Enumerations;
using System.Xml.Linq;

namespace WindowsFormsApp24.Events
{
    internal class Seed : Event
    {
        internal Seed(NamedObjects obj, long grow_ticks, int x, int y) : base(x, y) { Initialize(obj, grow_ticks); }
        internal Seed(NamedObjects obj, long grow_ticks, float x, float y) : base(0,0) { X = x; Y = y; Initialize(obj, grow_ticks); }

        internal void Initialize(NamedObjects obj, long grow_ticks)
        {
            var img = Core.NamedTextures[obj];
            Name = Enum.GetName(typeof(NamedObjects), obj) + "-seed";
            int w = (int)(img.Width * 0.5F);
            int sz = (int)(w * 0.8F);
            var min = new Bitmap(img, sz, sz);
            var tex = new Bitmap(w, w);
            Bounds = new RectangleF(0, 0, w, w);
            using (Graphics g = Graphics.FromImage(tex))
            {
                g.FillEllipse(Brushes.Maroon, 0, 0, w, w);
                g.DrawEllipse(new Pen(Color.Black, 2F), 0, 0, w, w);
                g.DrawImage(min, (w - sz) / 2, (w - sz) / 2);
            }
            LoadImage(tex, true);
            Data["grow_ticks"] = grow_ticks;
            OnPrimaryAction += () => { PredefinedActions[PredefinedAction.Plant](this); };
            OnSecondaryAction += () => { PredefinedActions[PredefinedAction.TakeDrop](this); };
        }
    }
}
