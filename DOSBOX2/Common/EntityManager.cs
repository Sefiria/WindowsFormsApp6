using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX2.Common
{
    internal class EntityManager
    {
        public List<Entity> Entities;
        public EntityManager()
        {
            Entities = new List<Entity>();
        }
        public void BeforeUpdate()
        {
            var copy = new List<Entity>(Entities);
            foreach (Entity e in copy)
            {
                if (e.Exists)
                    e.BeforeUpdate();
                else
                    Entities.Remove(e);
            }
        }
        public void Update()
        {
            var copy = new List<Entity>(Entities);
            foreach (Entity e in copy)
            {
                if (e.Exists)
                    e.Update();
                else
                    Entities.Remove(e);
            }
        }
        public void AfterUpdate()
        {
            var copy = new List<Entity>(Entities);
            foreach (Entity e in copy)
            {
                if (e.Exists)
                    e.AfterUpdate();
                else
                    Entities.Remove(e);
            }
        }
        public void Draw()
        {
            var copy = new List<Entity>(Entities);
            foreach (Entity e in copy)
                if (e.Exists)
                    e.Draw();
        }

        internal void Add(Entity entity)
        {
            Entities.Add(entity);
        }
        internal void Remove(Entity entity)
        {
            Entities.Remove(entity);
        }
        internal Entity GetByID(Guid ID) => Entities.FirstOrDefault(e => e.ID == ID);
        internal Entity GetByName(string name) => Entities.FirstOrDefault(e => e.Name == name);
    }
}
