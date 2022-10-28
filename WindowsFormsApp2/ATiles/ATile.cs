using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2.ATiles
{
    public class ATileBase
    {
        private static Timer TimerStep = new Timer() { Enabled = true, Interval = 200 };
        private static int Step = 0, StepMax = 4;
        private static List<Bitmap> Images;

        protected ATileBase(string ResourceName)
        {
            Bitmap source = (Bitmap)Resources.ResourceManager.GetObject(ResourceName);
            if (source == null) throw new ArgumentException($"Resource name '{ResourceName}' does not exist in the Resources");
            Images = Tools.SplitImage(source, 32, 32);
            TimerStep.Tick += Tick;
        }

        private static void Tick(object sender, EventArgs e)
        {
            Step++;
            if (Step >= StepMax)
                Step -= StepMax;
        }

        protected static void Draw(int X, int Y)
        {
            SharedCore.g.DrawImage(Images[Step], X, Y);
        }
    }
}
