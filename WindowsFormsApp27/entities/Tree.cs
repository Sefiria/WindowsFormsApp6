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
    public class Tree : Entity
    {
        public static Tree Create(float x, float y, string dna)
        {
            var e = new Tree();

            e.type = 2;
            e.x = x;
            e.y = y;

            e.Image = Resources.tree.Transparent();
            e.w = e.Image.Width; e.h = e.Image.Height;

            e.ApplyDna(dna);

            Entities.Add(e);
            return e;
        }
    }
}
