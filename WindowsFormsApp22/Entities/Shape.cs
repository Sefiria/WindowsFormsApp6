using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp22.Entities
{
    public class Shape : Entity
    {
        public Shape(string tex, Color color, int w, int h) : base(tex, color, w, h) { }
    }
}
