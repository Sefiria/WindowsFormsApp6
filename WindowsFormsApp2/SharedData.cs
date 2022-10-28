using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Entities;

namespace WindowsFormsApp2
{
    public static class SharedData
    {
        public static List<DrawableEntity> Entities => World?.Current?.Entities ?? new List<DrawableEntity>();
        public static Player Player = null;
        public static World World;
        public static List<string> Logs = new List<string>();
    }
}
