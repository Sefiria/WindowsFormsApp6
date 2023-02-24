using System;
using System.Drawing;

namespace WindowsCardsApp15.UI
{
    public interface IUI
    {
        string ID { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int W { get; set; }
        int H { get; set; }
        Bitmap Image { get; }
        event EventHandler OnClick;

        void Update();
        void Draw(Graphics g = null, Color? bounds_color = null);
        void Clicked();
    }
}
