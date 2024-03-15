using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ToolingGranularity
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            if (args.Length == 0 || !int.TryParse(args[0], out int mode) || mode < 0 || mode > 0)
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form form = null;
            switch(mode)
            {
                case 0: form = new FormMain(); break;
            }
            Application.Run(form);
        }

        //internal static string RunCommand(string cmd)
        //{
        //    string output = "";
        //    if (File.Exists("____templogs.txt")) File.Delete("____templogs.txt");
        //    if (File.Exists("____temperr.txt")) File.Delete("____temperr.txt");
        //    Process.Start(new ProcessStartInfo("CMD.exe", $"{cmd} 1>\"____templogs.txt\"  2>\"____temperr.txt\"") { CreateNoWindow = true, UseShellExecute = false });
        //    Stopwatch timeout = new Stopwatch();
        //    timeout.Start();
        //    do { } while (!File.Exists("____templogs.txt") && timeout.Elapsed.TotalSeconds < 10D && !File.Exists("____temperr.txt"));
        //    if (File.Exists("____templogs.txt"))
        //        output = File.ReadAllText("____templogs.txt");
        //    return output;
        //}
    }
}
