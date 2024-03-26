using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.Maps
{
    public abstract class Map : IDrawable, IUpdatable
    {
        public Bitmap Texture => null;
        public List<Entity> entities = new List<Entity>();
        public Size size;

        public void Render(Graphics g)
        {
            foreach (Entity entity in entities)
                if (entity is IDrawable)
                    (entity as IDrawable).Render(g);
        }

        public void Update()
        {
            foreach (Entity entity in entities)
                if (entity is IUpdatable)
                    (entity as IUpdatable).Update();
        }

        public void Dispose()
        {

        }
    }
}
