using System.Collections.Generic;
using System.Drawing;

namespace console_v3
{
    public class Scene
    {
        public Color BGColor;

        public Scene()
        {
        }

        public virtual void Initialize() { }

        public virtual void Update() { }
        public virtual void TickSecond() { }

        public virtual void Draw(Graphics g, Graphics gui)
        {
            g.Clear(BGColor);
        }
    }
}
