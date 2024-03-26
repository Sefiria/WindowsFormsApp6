using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolActivityMaker
{
    partial class MainForm
    {
        EditableText text_clicked = null;
        Point MouseTextOffset = Point.Empty;
        private bool text_mouseholding = false;
        private void ToolText_MouseDown(object sender, MouseEventArgs e)
        {
            text_clicked = null;
            Texts.ForEach(x => x.Hover = x.Editing = false);

            foreach (EditableText text in Texts)
            {
                if (text.Bounds.Contains(e.Location))
                {
                    text_clicked = text;
                    break;
                }
            }

            if (text_clicked == null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    EditableText text = new EditableText();
                    text.Location = e.Location;
                    text.Hover = text.Editing = true;
                    Texts.Add(text);
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    ToolStripButton item_TransparentBackColor = new ToolStripButton("Set Transparent BackColor");
                    item_TransparentBackColor.Click += delegate { text_clicked.BackColor = Color.Transparent; };
                    ToolStripButton item_BackColor = new ToolStripButton("BackColor...");
                    item_BackColor.Click += delegate { SetTextBackColor(text_clicked); };
                    ToolStripButton item_ForeColor = new ToolStripButton("ForeColor...");
                    item_ForeColor.Click += delegate { SetTextForeColor(text_clicked); };

                    ToolStripDropDown dropdown = new ToolStripDropDown();
                    dropdown.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
                    dropdown.Items.Add(item_TransparentBackColor);
                    dropdown.Items.Add(item_BackColor);
                    dropdown.Items.Add(item_ForeColor);

                    dropdown.Show(MousePosition);
                }
                else
                {
                    text_clicked.MouseClick(sender, e);
                    text_mouseholding = true;
                    MouseTextOffset = new Point(e.X - text_clicked.X, e.Y - text_clicked.Y);
                }
            }
        }
        private void ToolText_MouseUp(object sender, MouseEventArgs e)
        {
            text_mouseholding = false;
        }
        private void ToolText_MouseLeave()
        {
        }
        private void ToolText_MouseMove(object sender, MouseEventArgs e)
        {
            if (text_mouseholding)
            {
                if (text_clicked == null)
                {
                    text_mouseholding = false;
                    return;
                }
                else
                {
                    text_clicked.Location = new Point(e.X - MouseTextOffset.X, e.Y - MouseTextOffset.Y);
                    text_clicked.LimitToBounds(Point.Empty, ActivityPB.Size);
                }
            }
            else
            {
                foreach (EditableText text in Texts)
                    text.Hover = text.Bounds.Contains(e.Location);
            }
        }

        private void SetTextBackColor(EditableText text)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = text.BackColor;
            if(dialog.ShowDialog() == DialogResult.OK)
                text.BackColor = dialog.Color;
        }
        private void SetTextForeColor(EditableText text)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = text.ForeColor;
            if (dialog.ShowDialog() == DialogResult.OK)
                text.ForeColor = dialog.Color;
        }
    }
}
