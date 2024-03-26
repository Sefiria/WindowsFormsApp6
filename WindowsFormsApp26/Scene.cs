using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp26
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
        public virtual void DrawForeground(Graphics g)
        {
        }


        public void Collide(ref List<Entity> Entities)
        {
            CollisionsInfo infos;
            var entities = new List<Entity>(Entities);
            foreach (Entity e in entities)
            {
                infos = e.Collider?.GetCollidingEntities(entities.Except(new[] { e }).ToList());
                infos?.CollisionInfoList?.ForEach(info =>
                {
                    PointF e_Forces = e.Forces;
                    var keep = 0.8F;
                    var dist = new PointF(info.Entity.Forces.X - e.Forces.X, info.Entity.Forces.Y - e.Forces.Y).norm().x(1f);
                    e.Forces = e.Forces.x(keep).PlusF(dist);
                    info.Entity.Forces = info.Entity.Forces.x(keep).MinusF(dist);
                });
            }
        }
    }
}
