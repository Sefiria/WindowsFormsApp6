using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFormsApp17
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(Keyboard.IsKeyDown(Key.LeftAlt))
            {
                var set = new FormConfig();
                Application.Run(set);
                set.BringToFront();
            }
            else
            {
                var run = new FormCatch();
                Application.Run(run);
                run.BringToFront();
            }
        }
    }
}
