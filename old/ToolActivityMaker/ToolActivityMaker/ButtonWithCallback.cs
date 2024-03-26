using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolActivityMaker
{
    public class ButtonWithCallback
    {
        public Point Location = Point.Empty;
        public Image Icon = null;
        public Action Callback = null;
        public object UserData = null;

        public Size Size => Icon.Size;
        public Rectangle Bounds => new Rectangle(Location, Size);

        public ButtonWithCallback(Point Location, Image Icon, Action Callback, object UserData = null)
        {
            this.Location = Location;
            this.Icon = Icon;
            this.Callback = Callback;
            this.UserData = UserData;
        }
        public void OnAnyClick(MouseEventArgs e)
        {
            if (Bounds.Contains(e.Location))
            {
                Callback?.Invoke();
            }
        }
        public void Draw(Graphics g)
        {
            g.DrawImage(Icon, Location);
        }
    }
}
