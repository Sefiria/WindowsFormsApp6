using System.Collections.Generic;
using System.Drawing;

namespace console_v2
{
    public class Scene
    {
        public Color BGColor;
        public List<Entity> Entities = new List<Entity>();

        public Scene()
        {
        }

        public virtual void Initialize() { }

        public virtual void Update()
        {
        }

        public virtual void Draw(Graphics g)
        {
            g.Clear(BGColor);
        }
    }
}
