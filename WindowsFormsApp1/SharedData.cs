using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Entities;

namespace WindowsFormsApp1
{
    public static class SharedData
    {
        public static List<DrawableEntity> Entities = new List<DrawableEntity>();
        public static Player Player = null;

        public static void Clear()
        {
            Entities.Clear();
        }
    }
}
