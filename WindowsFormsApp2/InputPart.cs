using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public static class InputPart
    {
        public static void ManageMouse(MouseEventArgs e)
        {
            SharedData.Player.MouseDown(e);
        }
        public static void ManageMouseMove(MouseEventArgs e)
        {
            SharedCore.MouseLocation = e.Location;
        }
    }
}
