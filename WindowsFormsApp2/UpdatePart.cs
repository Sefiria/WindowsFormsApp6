using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Entities;

namespace WindowsFormsApp2
{
    public static class UpdatePart
    {
        public static Timer TimerPlantGrow = new Timer() { Enabled = true, Interval = 1000 };

        public static void Update(MainForm form)
        {
            SharedData.World.Current.Update();

            var Entities = new List<Entities.DrawableEntity>(SharedData.Entities);
            foreach (var entity in Entities)
            {
                if (!entity.Exists)
                {
                    SharedData.Entities.Remove(entity);
                    continue;
                }

                if (entity.Exists)
                {
                    entity.Update();
                }
            }

            if (SharedData.Player.Exists)
                SharedData.Player.Update();
        }
    }
}
