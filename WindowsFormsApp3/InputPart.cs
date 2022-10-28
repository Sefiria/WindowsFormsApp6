using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public static class InputPart
    {
        public static void ManageMouse(MouseEventArgs e)
        {
            SharedData.Player.MouseDown(e);
        }
        public static void ManageMouseMove(MouseEventArgs e)
        {
            SharedData.Player.MouseMove(e);
        }
        public static void ManageMouseUp()
        {
            SharedData.Player.MouseUp();
        }
        public static void ManageMouseLeave()
        {
            SharedData.Player.MouseLeave();
        }
    }
}
