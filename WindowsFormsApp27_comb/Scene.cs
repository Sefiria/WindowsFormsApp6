using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp27_comb
{
    internal abstract class Scene
    {
        public abstract void Initialize();
        public abstract void Update();
        public abstract void Draw(Graphics g);
        public abstract void DrawUI(Graphics g);
    }
}
