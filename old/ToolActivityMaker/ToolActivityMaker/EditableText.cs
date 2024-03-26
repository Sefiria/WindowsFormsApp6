using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolActivityMaker
{
    [Serializable]
    public class EditableText
    {
        public string Text;
        public Point Location;
        public int Padding = 5;
        public Color BackColor = Color.Transparent;
        public Color ForeColor = Color.Black;
        public Size TextSize => TextRenderer.MeasureText(Text, SystemFonts.DefaultFont);
        public Size Size { get { Size size = TextSize; return new Size(size.Width + Padding * 2, size.Height + Padding * 2); } }
        public Rectangle Bounds => new Rectangle(Location, Size);
        public Point DrawPointLocation => new Point(Location.X + Padding, Location.Y + Padding);
        public int X { get { return Location.X; } set { Location = new Point(value, Location.Y); } }
        public int Y { get { return Location.Y; } set { Location = new Point(Location.X, value); } }
        public int Width => Size.Width;
        public int Height => Size.Height;

        public bool Hover = false;
        public bool Editing = false;

        public void MouseClick(object sender, MouseEventArgs e)
        {
            Editing = true;
        }

        public void Edition(KeyEventArgs e)
        {
            if (!Editing)
                return;

            if (e.Control || e.Alt)
                return;

            switch(e.KeyCode)
            {
                case Keys.Enter: Editing = false; return;
                case Keys.Back: if (string.IsNullOrWhiteSpace(Text)) return; Text = Text.Remove(Text.Length - 1); return; 
                case Keys.ShiftKey: return;
                case Keys.Delete: MainForm.Instance.DeleteText(this); return;
            }

            Text += Tools.GetCharFromKey(e.KeyCode);
        }


        public void Draw(Graphics g)
        {
            g.FillRectangle(new TextureBrush(MainForm.Instance.ActivityPB.Image), Bounds);
            
            if(BackColor.ToArgb() != Color.Transparent.ToArgb())
                g.FillRectangle(new SolidBrush(BackColor), Bounds);

            if (Hover)
            {
                g.DrawRectangle(Pens.Black, Bounds);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Text))
                    g.DrawRectangle(Pens.LightGray, Bounds);
            }

            if (Editing)
                g.DrawRectangle(new Pen(Color.Black, 2F), Bounds);

            g.DrawString(Text, SystemFonts.DefaultFont, new SolidBrush(ForeColor), DrawPointLocation);
        }
        public void LimitToBounds(int X, int Y, int Width, int Height)
        {
            LimitToBounds(new Point(X, Y), new Size(Width, Height));
        }
        public void LimitToBounds(Point Location, Size Size)
        {
            if (X < 0) X = 0;
            if (Y < 0) Y = 0;
            if (X + Width >= Size.Width) X = Size.Width - Width;
            if (Y + Height >= Size.Height) Y = Size.Height - Height;
        }

        public void LimitToBounds(Rectangle Bounds)
        {
            LimitToBounds(new Point(Bounds.X, Bounds.Y), new Size(Bounds.Width, Bounds.Height));
        }
    }
}
