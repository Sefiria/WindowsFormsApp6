using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6.UI
{
    public class UIButton : UIBase
    {
        public string Text;
        [JsonIgnore] public Bitmap Icon;
        private int Margin = 5;
        public override event EventHandler OnClick;

        [JsonConstructor]
        public UIButton()
        {
        }
        public UIButton(string text, int x, int y) : base(x, y)
        {
            Text = text;
            var sz = Core.g.MeasureString(Text, Font);
            W = Margin + (int)Math.Floor(sz.Width) + Margin;
            H = Margin + (int)Math.Floor(sz.Height) + Margin;
            Image = GenerateButtonImage();
        }
        public UIButton(Bitmap icon, int x, int y) : base(x, y)
        {
            Icon = icon;
            W = Margin + Icon.Width + Margin;
            H = Margin + Icon.Height + Margin;
            Image = GenerateButtonImage();
        }

        public override void Draw(Graphics g = null, Color? bounds_color = null)
        {
            if(Image == null)
                Image = GenerateButtonImage();

            var _g = g ?? Core.g;
            _g.DrawImage(Image, X, Y);
            if (bounds_color != null)
                _g.DrawRectangle(new Pen(bounds_color.Value, 2F), X, Y, Image.Width, Image.Height);

        }
        public override void Update() { }

        public Bitmap GenerateButtonImage()
        {
            Bitmap result = new Bitmap(W, H);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.FillRectangle(Brushes.LightGray, 0, 0, W, H);
                g.DrawRectangle(new Pen(Color.DodgerBlue, 2F), 0, 0, W, H);
                if(Icon != null)
                    g.DrawImage(Icon, Margin, Margin);
                else
                    g.DrawString(Text, Font, Brushes.Black, Margin, Margin);
            }
            return result;
        }

        public override void Clicked() => OnClick?.Invoke(null, null);
    }
}
