using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp3.Entities;
using WindowsFormsApp3.Entities.Mobs;

namespace WindowsFormsApp3
{
    public static class SharedData
    {
        public static List<DrawableEntity> Entities = new List<DrawableEntity>();
        public static List<MobBase> Mobs => Entities.Where(x => x is MobBase).Cast<MobBase>().ToList();
        public static Player Player = null;
        public static List<string> Logs = new List<string>();
        public static int Wave = 0;
    }
}
