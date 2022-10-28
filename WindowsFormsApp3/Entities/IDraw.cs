using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3.Entities
{
    public interface IDraw
    {
        float X { get; set; }
        float Y { get; set; }
        Bitmap Image { get; set; }
    }
}
