using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tooling.UI
{
    public static class UIMgt
    {
        public static UI CurrentClicked, CurrentHover;
        public static List<UI> UI = new List<UI>();

        static UIMgt() { }

        public static void Update()
        {
            CurrentClicked = null;
            CurrentHover = null;
            UI.ForEach(ui => ui.Update());

            if (MouseStatesV1.IsDown)
                UI.Where(ui => ui.IsHover).ToList().ForEach(ui => ui.Click());
        }

        public static void Draw(Graphics g)
        {
            UI.ForEach(ui => ui.Draw(g));
        }

        // moved to Update
        //public static void MouseDown()
        //{
        //    if (MouseStates.IsDown && MouseStates.ButtonDown == MouseButtons.Left)
        //        UI.Where(ui => ui.Bounds.Contains(MouseStates.Position)).ToList().ForEach(ui => ui.Click());
        //}
    }
}
