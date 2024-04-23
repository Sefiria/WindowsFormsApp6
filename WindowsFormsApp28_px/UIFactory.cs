using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp28_px
{
    public class UIContent
    {
        public float x, y, w, h;
        public delegate void UpdateHandler();
        public delegate void DrawHandler(Graphics g);
        public event UpdateHandler OnUpdate;
        public event DrawHandler OnDraw;
        public void Update()
        {
            OnUpdate?.Invoke();
        }
        public void Draw(Graphics g)
        {
            OnDraw?.Invoke(g);
        }
    }

    public class UIFactory
    {
        public static UIContent CreateItemContainer(List<Item> items, float x, float y)
        {
            var content = new UIContent();
            content.x = x;
            content.y = y;
            var m = 8;// margin
            var sqrt_count = Maths.Sqrt(items.Count);
            var sq_size = m + (DB.ItemSize + m) * sqrt_count;
            content.w = content.h = sq_size;
            content.OnDraw += (g) =>
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 100)), new RectangleF(content.x, content.y, content.w, content.h));
            };
            return content;
        }
    }
}
