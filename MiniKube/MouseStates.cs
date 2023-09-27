using System.Windows.Forms;

namespace MiniKube
{
    internal class MouseStates
    {
        public static MouseButtons ButtonDown = MouseButtons.None;
        public static bool IsDown => ButtonDown != MouseButtons.None;
        public static bool IsUp => !IsDown;
    }
}
