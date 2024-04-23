using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp27_comb
{
    internal class UIBitmapButton : UIButton
    {
        public Bitmap Image;
        public UIBitmapButton(Bitmap Image, int X, int Y)
        {
            this.Image = Image;
            this.X = X;
            this.Y = Y;
        }
        protected UIBitmapButton(UIBitmapButton clone)
        {
            Image = new Bitmap(clone.Image);
        }
        public UIBitmapButton Clone()
        {
            return new UIBitmapButton(this);
        }
        public override void Draw(Graphics g)
        {
            g.DrawImage(IsDisabled ? Image.GetAdjusted(brightness:0.75F, contrast:0.5F) : Image, X, Y);
        }
    }
}
