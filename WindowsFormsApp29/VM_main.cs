using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp29.Controls;

namespace WindowsFormsApp29
{
    internal class VM_main : IDisposable
    {
        List<UIElement> uIElements = new List<UIElement>();
        ConsoleControl.ConsoleControl console = new ConsoleControl.ConsoleControl();
        public VM_main()
        {
            MainForm.Ref.Controls.Add(console);
            console.Dock = DockStyle.Fill;
            console.StartProcess("cmd /k echo test", null);
            //console.ProcessCommand += (sender, e) =>
            //{
            //    console.WriteOutput(e.Command);
            //};

            //uIElements.Add(new CMD());
        }
        public void Update()
        {
            uIElements.ForEach(x => x.Update());
        }
        public void Draw(Graphics g)
        {
            uIElements.ForEach(x => x.Draw(g));
        }

        public void Dispose()
        {
            console.Dispose();
        }
    }
}
