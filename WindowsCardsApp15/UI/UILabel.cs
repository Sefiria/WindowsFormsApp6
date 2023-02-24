using System;
using System.Drawing;

namespace WindowsCardsApp15.UI
{
    public class UILabel : UIBase
    {
        public string Text;

        public UILabel()
        {
        }
        public UILabel(string text, int x, int y) : base(x, y)
        {
            Text = text;
            var sz = Core.g.MeasureString(Text, Font);
            W = (int)Math.Floor(sz.Width);
            H = (int)Math.Floor(sz.Height);
            Image = GenerateButtonImage();
        }

        public override void Draw(Graphics g = null, Color? bounds_color = null)
        {
            (g ?? Core.g).DrawImage(Image, X, Y);
        }

        public Bitmap GenerateButtonImage()
        {
            Bitmap result = new Bitmap(W, H);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawString(Text, Font, Brushes.White, 0, 0);
            return result;
        }
    }
}
