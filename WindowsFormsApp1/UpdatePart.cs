using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public static class UpdatePart
    {
        public static Timer TimerPlantGrow = new Timer() { Enabled = true, Interval = 1000 };

        public static void Update(MainForm form)
        {
            foreach (var entity in SharedData.Entities)
            {
                if (entity.Exists)
                {
                    entity.Update();
                }
            }
        }
    }
}
