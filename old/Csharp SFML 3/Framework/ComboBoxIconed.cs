using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor
{
    public class ComboBoxIconed : ComboBox
    {
        public class DropDownItem
        {
            public Bitmap Image;
            public string Text;

            public DropDownItem(Bitmap img, string text)
            {
                Image = img;
                Text = text;
            }
            public override string ToString()
            {
                return Text;
            }
        }

        public ComboBoxIconed()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            DropDownItem item = (DropDownItem)Items[e.Index];
            // Draw the colored 16 x 16 square
            e.Graphics.DrawImage(item.Image, e.Bounds.Left, e.Bounds.Top);
            // Draw the value (in this case, the color name)
            e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor), e.Bounds.Left + item.Image.Width, e.Bounds.Top + 2);

            base.OnDrawItem(e);
        }
    }
}
