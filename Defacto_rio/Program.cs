using System;
using System.Windows.Forms;

namespace Defacto_rio
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
            int crashcount = 0;
            while (crashcount < 4)
            {
                try
                {
                    Application.Run(new FormMain());
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}{Environment.NewLine}{Environment.NewLine}{ex.InnerException}", "Fatal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    crashcount++;
                }
            }
        }
    }
}
