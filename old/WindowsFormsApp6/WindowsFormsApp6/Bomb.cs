using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public class Bomb
    {
        static public short HitsStart = 7;
        static public short MaxBombs = 5;
        public short HitsLeft = HitsStart;
        public int X, Y;
        public bool ShouldExplode = false;

        public Bomb(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public void Hit()
        {
            HitsLeft--;
            if (HitsLeft == 0)
                ShouldExplode = true;
        }

        public void Draw(Graphics g)
        {
            Rectangle bounds = MAIN.Instance.Mode.Grid.GetBlock(X, Y).Bounds;

            g.DrawEllipse(Pens.White, bounds.X, bounds.Y, bounds.Width, bounds.Height);

            Font font = null;
            if (HitsLeft < 4)   font = new Font(SystemFonts.DefaultFont.FontFamily, 12, FontStyle.Bold);
            else                font = new Font(SystemFonts.DefaultFont.FontFamily, 8, FontStyle.Bold);
            Size TextSize = TextRenderer.MeasureText(HitsLeft.ToString(), font);
            g.DrawString(HitsLeft.ToString(), font, Brushes.White, bounds.X + bounds.Width / 2 - TextSize.Width / 2 + 2, bounds.Y + bounds.Height / 2 - TextSize.Height / 2);
        }
    }
}
