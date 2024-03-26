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
        Color RectangleColor = Color.Black;
        int RectangleWidth = 2;
        bool rectangle_holding = false;
        Point rectangle_PreviousMouse = Point.Empty;

        private void ToolRectangle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                NumericUpDown Item_Width_Set = new NumericUpDown();
                Item_Width_Set.Minimum = 1;
                Item_Width_Set.Maximum = 20;
                Item_Width_Set.Value = RectangleWidth;
                Item_Width_Set.ValueChanged += delegate { RectangleWidth = (int)Item_Width_Set.Value; };
                ToolStripMenuItem item_Width = new ToolStripMenuItem("Width →", null, new ToolStripControlHost(Item_Width_Set));

                ToolStripButton item_Color = new ToolStripButton("Color...");
                item_Color.Click += delegate { SetRectangleColor(); };

                ToolStripDropDown dropdown = new ToolStripDropDown();
                dropdown.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
                dropdown.Items.Add(item_Width);
                dropdown.Items.Add(item_Color);
                //dropdown.Items.Add(new ToolStripSeparator());
                dropdown.Show(MousePosition);

                return;
            }

            rectangle_holding = true;
            rectangle_PreviousMouse = e.Location;
        }
        private void ToolRectangle_MouseUp(object sender, MouseEventArgs e)
        {
            if(rectangle_holding && TemporaryObjectToDraw != null)
            {
                using (Graphics g = Graphics.FromImage(DrawLayer_Content))
                    g.DrawImage(TemporaryObjectToDraw, 0, 0);
            }
            rectangle_holding = false;
        }
        private void ToolRectangle_MouseLeave()
        {
        }
        private void ToolRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (rectangle_holding)
            {
                TemporaryObjectToDraw = new Bitmap(DrawLayer_Content.Width, DrawLayer_Content.Height);
                using (Graphics g = Graphics.FromImage(TemporaryObjectToDraw))
                {
                    int MinX = Math.Min(rectangle_PreviousMouse.X, e.X);
                    int MinY = Math.Min(rectangle_PreviousMouse.Y, e.Y);
                    int MaxX = Math.Max(rectangle_PreviousMouse.X, e.X);
                    int MaxY = Math.Max(rectangle_PreviousMouse.Y, e.Y);
                    g.DrawRectangle(new Pen(RectangleColor, RectangleWidth), MinX, MinY, MaxX - MinX, MaxY - MinY);

                    if (CustomTempalte_AddButton != null)
                        g.FillRectangle(new TextureBrush(ActivityPB.Image), CustomTempalte_AddButton.Bounds);
                    if (CustomTempalte_RemoveButton != null)
                        g.FillRectangle(new TextureBrush(ActivityPB.Image), CustomTempalte_RemoveButton.Bounds);
                }
                draw_PreviousMouse = e.Location;
            }
        }
        private void ToolRectangle_MouseWheel(object sender, MouseEventArgs e)
        {
            if (AltKeyHolding)
            {
                if (e.Delta > 0F)
                {
                    RectangleWidth = (int)(RectangleWidth * 1.5F);
                    if (RectangleWidth > 20)
                        RectangleWidth = 20;
                }
                else
                {
                    RectangleWidth = (int)(RectangleWidth / 1.5F);
                    if (RectangleWidth < 1)
                        RectangleWidth = 1;
                }
            }
        }
        private void SetRectangleColor()
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = RectangleColor;
            if (dialog.ShowDialog() == DialogResult.OK)
                RectangleColor = dialog.Color;
        }
    }
}
