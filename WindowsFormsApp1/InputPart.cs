using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public static class InputPart
    {
        public static void ManageMouse(MouseEventArgs e)
        {
            SharedData.Player.MouseDown(e);
        }
    }
}
