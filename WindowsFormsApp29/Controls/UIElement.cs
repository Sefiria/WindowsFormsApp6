using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp29.Controls
{
    internal abstract class UIElement
    {
        public Guid ID;
        public UIElement()
        {
            ID = Guid.NewGuid();
        }

        public abstract void Update();
        public abstract void Draw(Graphics g);
    }
}
