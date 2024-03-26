using Framework;
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
            public EntityProperties entity;

            public DropDownItem(Bitmap img, string text, EntityProperties _entity)
            {
                Image = img;
                Text = text;
                entity = _entity;
            }
            public override string ToString()
            {
                return Text;
            }
        }

        public ComboBoxIconed()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;

        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index == -1 || e.Index >= Items.Count)
                return;

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
