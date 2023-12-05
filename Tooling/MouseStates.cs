using System.Drawing;
using System.Windows.Forms;

namespace Tooling
{
    public class MouseStates
    {
        public static MouseButtons ButtonDown = MouseButtons.None;
        public static bool IsDown => ButtonDown != MouseButtons.None;
        public static bool IsUp => !IsDown;
        public static PointF Position = PointF.Empty;
    }
}
