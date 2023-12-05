using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cast
{
    public class Entities
    {
        public List<Entity> entities = new List<Entity>();
        public Entities()
        {
        }
        public void Update()
        {
            var list = new List<Entity>(entities);
            foreach (Entity e in list)
            {
                if (e.Exists) e.Update();
                else entities.Remove(e);
            }
        }
        public void Draw(Graphics g, PointF? offset = null)
        {
            entities.ForEach(x => x.Draw(g, offset));
        }
        public void DrawMinimap(Graphics g)
        {
            entities.ForEach(x => x.DrawMinimap(Core.Map.gmini));
        }
        
    }
}
