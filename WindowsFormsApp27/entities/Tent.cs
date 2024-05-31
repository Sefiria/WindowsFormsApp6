using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp27.Properties;

namespace WindowsFormsApp27.entities
{
    public class Tent : Entity
    {
        public static Tent Create(float x, float y, string dna)
        {
            var e = new Tent();

            e.type = 1;
            e.x = x;
            e.y = y;

            e.Image = Resources.tent.Transparent().ResizeExact(Resources.tent.Width * 2, Resources.tent.Height * 2);
            e.w = e.Image.Width; e.h = e.Image.Height;

            e.ApplyDna(dna);

            Entities.Add(e);
            return e;
        }
    }
}
