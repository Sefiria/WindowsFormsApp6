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
        private bool draw_holding = false;
        private Point draw_PreviousMouse = Point.Empty;
        private enum DrawType { Pen, Eraser };
        private DrawType draw_type = DrawType.Pen;
        private float draw_size = 1F;
        private Color PenColor = Color.Black;

        private void ToolDraw_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ToolStripMenuItem item_Type = new ToolStripMenuItem("Type →");
                ToolStripButton item_Type_Pen = new ToolStripButton("Pen");
                item_Type_Pen.Click += delegate { draw_type = DrawType.Pen; };
                ToolStripButton item_Type_Eraser = new ToolStripButton("Eraser");
                item_Type_Eraser.Click += delegate { draw_type = DrawType.Eraser; };
                item_Type.DropDownItems.Add(item_Type_Pen);
                item_Type.DropDownItems.Add(item_Type_Eraser);

                ToolStripButton item_PenColor = new ToolStripButton("Pen Color...");
                item_PenColor.Click += delegate { SetPenColor(); };

                ToolStripDropDown dropdown = new ToolStripDropDown();
                dropdown.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
                dropdown.Items.Add(item_Type);
                dropdown.Items.Add(item_PenColor);
                //dropdown.Items.Add(new ToolStripSeparator());
                dropdown.Show(MousePosition);

                return;
            }

            draw_holding = true;
            draw_PreviousMouse = e.Location;
        }
        private void ToolDraw_MouseUp(object sender, MouseEventArgs e)
        {
            draw_holding = false;
        }
        private void ToolDraw_MouseLeave()
        {
        }
        private void ToolDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw_holding)
            {
                using (Graphics g = Graphics.FromImage(DrawLayer_Content))
                {
                    float radius = draw_size / 2F;

                    switch (draw_type)
                    {
                        case DrawType.Pen:
                            for (double t = 0D; t <= 1D; t += radius / 2D / Math.Sqrt(Math.Pow(draw_PreviousMouse.X - e.X, 2D) + Math.Pow(draw_PreviousMouse.Y - e.Y, 2D)))
                                g.FillEllipse(new SolidBrush(PenColor), (float)Tools.Lerp(draw_PreviousMouse.X - radius, e.X - radius, t), (float)Tools.Lerp(draw_PreviousMouse.Y - radius, e.Y - radius, t), draw_size, draw_size);
                            break;

                        case DrawType.Eraser:
                            for (double t = 0D; t <= 1D; t += radius / 2D / Math.Sqrt(Math.Pow(draw_PreviousMouse.X - e.X, 2D) + Math.Pow(draw_PreviousMouse.Y - e.Y, 2D)))
                                g.FillEllipse(new TextureBrush(ActivityPB.Image), (float)Tools.Lerp(draw_PreviousMouse.X - radius, e.X - radius, t), (float)Tools.Lerp(draw_PreviousMouse.Y - radius, e.Y - radius, t), draw_size, draw_size);
                            break;
                    }

                    if(CustomTempalte_AddButton != null)
                        g.FillRectangle(new TextureBrush(ActivityPB.Image), CustomTempalte_AddButton.Bounds);
                    if(CustomTempalte_RemoveButton != null)
                        g.FillRectangle(new TextureBrush(ActivityPB.Image), CustomTempalte_RemoveButton.Bounds);
                }
                draw_PreviousMouse = e.Location;
            }
        }
        private void ToolDraw_MouseWheel(object sender, MouseEventArgs e)
        {
            if (AltKeyHolding)
            {
                if (e.Delta > 0F)
                {
                    draw_size *= 1.5F;
                    if (draw_size > 50F)
                        draw_size = 50F;
                }
                else
                {
                    draw_size /= 1.5F;
                    if (draw_size < 0F)
                        draw_size = 0F;
                }
            }
        }

        private void SetPenColor()
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = PenColor;
            if (dialog.ShowDialog() == DialogResult.OK)
                PenColor = dialog.Color;
        }
    }
}
