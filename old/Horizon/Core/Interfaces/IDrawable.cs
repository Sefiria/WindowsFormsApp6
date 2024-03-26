using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDrawable
    {
        Bitmap Texture { get; }

        void Render(Graphics g);
    }
}
