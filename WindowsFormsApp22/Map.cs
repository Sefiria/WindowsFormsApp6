using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;
using WindowsFormsApp22.Entities;
using static Tooling.RandomThings;

namespace WindowsFormsApp22
{
    internal partial class Map
    {
        public List<Entity> Entities = new List<Entity>();
        public List<Entity> OrphanEntities => Entities.Where(e => e.Parent == null).ToList();
        public List<Entity> VisibleEntities => Entities.Where(e => e.IsDrawable && Core.VisibleBounds.Contains(e.iPos)).ToList();
        public List<Behaviored> Bullets => Entities.Where(e => e is Behaviored &&  (e.Name?.Contains("bullet") ?? false)).Cast<Behaviored>().ToList();
        public Behaviored obj_colliders;

        public Map()
        {
        }
        public void InitializeInstance()
        {
            obj_colliders = new Behaviored("obj_colliders");
            obj_colliders.IsDrawable = false;
            Action<List<object>> behavior = list =>
            {
                var e = list[0] as Entity;
                if (e == obj_colliders)
                    return;
                if (VisibleEntities.Where(_e => !(_e is IAtk) && _e.HasParent(obj_colliders) && _e != e && ((e is IAtk && _e.Name != "atk_bullet") || (e is Player && _e.Name == "atk_bullet"))).FirstOrDefault(ve => { return Maths.Collides(ve, ve, e, e); }) != null) e.Exist = false;
            };
            obj_colliders.Behavior = behavior;
        }

        public void Update()
        {
            new List<Entity>(Entities).ForEach(x => x.Cascade(_e => { Entity e = _e[0] as Entity; if (!e.Exist) Entities.Remove(e); else e.Update(); }, new List<object>() { x }));
        }

        public void Draw(Graphics g)
        {
            draw_grid(g);
            Entities.ForEach(x => x.Cascade(_e => { Entity e = _e[0] as Entity; if (e.IsDrawable && Core.VisibleBounds.Contains(e.iPos)) e.Draw(g); }, new List<object>() { x }));
        }
    }
}
