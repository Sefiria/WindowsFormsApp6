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
        Color CircleColor = Color.Black;
        int CircleWidth = 2;
        bool Circle_holding = false;
        Point Circle_PreviousMouse = Point.Empty;

        private void ToolCircle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                NumericUpDown Item_Width_Set = new NumericUpDown();
                Item_Width_Set.Minimum = 1;
                Item_Width_Set.Maximum = 20;
                Item_Width_Set.Value = CircleWidth;
                Item_Width_Set.ValueChanged += delegate { CircleWidth = (int)Item_Width_Set.Value; };
                ToolStripMenuItem item_Width = new ToolStripMenuItem("Width →", null, new ToolStripControlHost(Item_Width_Set));

                ToolStripButton item_Color = new ToolStripButton("Color...");
                item_Color.Click += delegate { SetCircleColor(); };

                ToolStripDropDown dropdown = new ToolStripDropDown();
                dropdown.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
                dropdown.Items.Add(item_Width);
                dropdown.Items.Add(item_Color);
                //dropdown.Items.Add(new ToolStripSeparator());
                dropdown.Show(MousePosition);

                return;
            }

            Circle_holding = true;
            Circle_PreviousMouse = e.Location;
        }
        private void ToolCircle_MouseUp(object sender, MouseEventArgs e)
        {
            if (Circle_holding && TemporaryObjectToDraw != null)
            {
                using (Graphics g = Graphics.FromImage(DrawLayer_Content))
                    g.DrawImage(TemporaryObjectToDraw, 0, 0);
            }
            Circle_holding = false;
        }
        private void ToolCircle_MouseLeave()
        {
        }
        private void ToolCircle_MouseMove(object sender, MouseEventArgs e)
        {
            if (Circle_holding)
            {
                TemporaryObjectToDraw = new Bitmap(DrawLayer_Content.Width, DrawLayer_Content.Height);
                using (Graphics g = Graphics.FromImage(TemporaryObjectToDraw))
                {
                    int MinX = Math.Min(Circle_PreviousMouse.X, e.X);
                    int MinY = Math.Min(Circle_PreviousMouse.Y, e.Y);
                    int MaxX = Math.Max(Circle_PreviousMouse.X, e.X);
                    int MaxY = Math.Max(Circle_PreviousMouse.Y, e.Y);
                    g.DrawEllipse(new Pen(CircleColor, CircleWidth), MinX, MinY, MaxX - MinX, MaxY - MinY);

                    if (CustomTempalte_AddButton != null)
                        g.FillRectangle(new TextureBrush(ActivityPB.Image), CustomTempalte_AddButton.Bounds);
                    if (CustomTempalte_RemoveButton != null)
                        g.FillRectangle(new TextureBrush(ActivityPB.Image), CustomTempalte_RemoveButton.Bounds);
                }
                draw_PreviousMouse = e.Location;
            }
        }
        private void ToolCircle_MouseWheel(object sender, MouseEventArgs e)
        {
            if (AltKeyHolding)
            {
                if (e.Delta > 0F)
                {
                    CircleWidth = (int)(CircleWidth * 1.5F);
                    if (CircleWidth > 20)
                        CircleWidth = 20;
                }
                else
                {
                    CircleWidth = (int)(CircleWidth / 1.5F);
                    if (CircleWidth < 1)
                        CircleWidth = 1;
                }
            }
        }
        private void SetCircleColor()
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = CircleColor;
            if (dialog.ShowDialog() == DialogResult.OK)
                CircleColor = dialog.Color;
        }
    }
}
