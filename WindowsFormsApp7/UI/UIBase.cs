using System;
using System.Drawing;

namespace WindowsFormsApp7.UI
{
    public class UIBase : IUI
    {
        public string Tag { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public bool Hover { get; set; } = false;
        public Bitmap Image { get; set; }

        public static Font Font = new Font("Segoe UI", 12F);
        public virtual event EventHandler OnClick;

        public UIBase()
        {
            X = 0;
            Y = 0;
            W = 0;
            H = 0;
            Image = null;
        }
        public UIBase(int x, int y, Bitmap img = null, int w = 0, int h = 0)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            Image = img;
        }

        public virtual void Draw(Graphics g = null, Color? bounds_color = null) { }
        public virtual void Update() { }
        public virtual void Clicked() { }
    }
}
