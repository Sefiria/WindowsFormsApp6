using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp24
{
    internal class Cam
    {
        internal float X, Y;
        internal Point Position => (X, Y).iP();

        internal void Update()
        {
        }
    }
}
