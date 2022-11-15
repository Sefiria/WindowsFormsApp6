using System;
using System.Drawing;

namespace WindowsFormsApp7.UI
{
    public interface IUI
    {
        string Tag { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int W { get; set; }
        int H { get; set; }
        bool Hover { get; set; }
        Bitmap Image { get; }
        event EventHandler OnClick;

        void Update();
        void Draw(Graphics g = null, Color? bounds_color = null);
        void Clicked();
    }
}
