using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp24.Events;

namespace WindowsFormsApp24
{
    internal class Fruit
    {
        internal OldCrop Owner;
        internal Root Parent;
        internal bool NeedGeneration = true;

        public Fruit(OldCrop owner, Root parent)
        {
            Owner = owner;
            Parent = parent;
        }
        internal void Update()
        {
        }
        internal void GenerateDraw(Graphics g)
        {
            if (NeedGeneration)
            {
                NeedGeneration = false;
            }
        }
    }
}
