using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp7.UI
{
    public class UIButton : UIBase
    {
        public static Regex RxAlphaNum = new Regex(@"^[a-zA-Z0-9\s,]*$");

        public string Text;
        public Bitmap Icon;
        public int Margin { get; private set; } = 5;
        public override event EventHandler OnClick;
        public int LinkedPixelId = -1, LinkedPixelGradiantId = -1;
        public float BoundWidth = 1F;
        public Color FillColor, TextColor;

        public UIButton()
        {
        }
        public UIButton(string text, int x, int y, Color? textColor = null, Color? fillColor = null, int w = 0, int h = 0, int margin = 5) : base(x, y)
        {
            Text = text;
            var sz = Core.g.MeasureString(Text, Font);
            W = w != 0 ? w : Margin + (int)Math.Floor(sz.Width) + Margin;
            H = h != 0 ? h : Margin + (int)Math.Floor(sz.Height) + Margin;
            Margin = margin;
            FillColor = fillColor == null ? FillColor = Color.LightGray : fillColor.Value;
            TextColor = textColor == null ? TextColor = Color.Black : textColor.Value;
            Image = GenerateButtonImage();
        }
        public UIButton(Bitmap icon, int x, int y) : base(x, y)
        {
            Icon = icon;
            W = Icon.Width;
            H = Icon.Height;
            Image = GenerateButtonImage();
        }

        public override void Draw(Graphics g = null, Color? bounds_color = null)
        {
            if(Image == null)
                Image = GenerateButtonImage();

            var _g = g ?? Core.g;
            _g.DrawImage(Image, X, Y);
            if (bounds_color != null)
                _g.DrawRectangle(new Pen(bounds_color.Value, BoundWidth), X, Y, Image.Width, Image.Height);

        }
        public override void Update() { }

        public Bitmap GenerateButtonImage()
        {
            Bitmap result = new Bitmap(W, H);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.FillRectangle(new SolidBrush(FillColor), 0, 0, W, H);
                g.DrawRectangle(new Pen(Color.DodgerBlue, 2F), 0, 0, W, H);
                if(Icon != null)
                    g.DrawImage(Icon, 0, 0);
                else
                    g.DrawString(Text, Font, new SolidBrush(TextColor), Margin, Text.Length == 1 && !RxAlphaNum.IsMatch(Text) ? -5 : Margin);
            }
            return result;
        }

        public override void Clicked() => OnClick?.Invoke(this, null);
    }
}
