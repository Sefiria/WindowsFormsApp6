using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11
{
    public class Entity
    {
        public float X, Y;
        public int W, H;
        public virtual Rectangle Rect => new Rectangle((int)(X - W/2), (int)(Y - H/2), W, H);

        public virtual void Update() { }
        public virtual void Draw() { }
    }
}
