using ConfigureRoute.Entities;
using ConfigureRoute.Obj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using Tooling;
using static Tooling.RandomThings;

namespace ConfigureRoute
{
    internal partial class Map
    {
        public List<Road> Roads = new List<Road>();
        public List<Sign> Signs = new List<Sign>();
        public List<Entity> Entities = new List<Entity>();
        public List<Car> Cars => Entities.OfType<Car>().ToList();
        public List<Car> CarsExcept(Car car) => Entities.OfType<Car>().Except(new Car[] {car}).ToList();

        int GPathArrowSize;
        GraphicsPath GPathArrow;

        public Road GetTargetRoad()
        {
            var pt = Core.TargetPoint.PlusF(Core.Cam).Div(Core.Cube);
            return Core.Map.Roads.FirstOrDefault(r => r.x == pt.X && r.y == pt.Y);
        }
        public Sign GetTargetSign()
        {
            var pt = Core.TargetPoint.PlusF(Core.Cam).Div(Core.Cube);
            return Core.Map.Signs.FirstOrDefault(s => s.x == pt.X && s.y == pt.Y);
        }

        public Map()
        {
            int sz = GPathArrowSize = (int)(Core.Cube * 0.70F);
            GPathArrow = new GraphicsPath(
                new Point[]{
                        (sz/4, sz/2).iP(), (0, sz/2).iP(), (sz/2, 0).iP(), (sz, sz/2).iP(), (sz/2+sz/4, sz/2).iP(), (sz/2+sz/4, sz).iP(), (sz/4, sz).iP(), (sz/4, sz/2).iP()
                },
                new byte[]
                {
                        0, 1, 1, 1, 1, 1, 1, 1
                });
        }

        public void Update()
        {
            var list = new List<Entity>(Entities);
            foreach(var entity in list)
            {
                if (!entity.Exist)
                {
                    Entities.Remove(entity);
                    continue;
                }
                else
                    entity.Update();
            }
            CarLogic();
        }

        private void CarLogic()
        {
            if(Core.Auto && Cars.Count < 1 && Roads.Count > 0 && rnd(50) == 25)
            {
                int where = rnd(4);
                Road r = null;
                switch(where)
                {
                    case 0: r = Roads.Aggregate((i1, i2) => i1.y < i2.y ? i1 : i2); break;
                    case 1: r = Roads.Aggregate((i1, i2) => i1.x < i2.x ? i1 : i2); break;
                    case 2: r = Roads.Aggregate((i1, i2) => i1.y > i2.y ? i1 : i2); break;
                    case 3: r = Roads.Aggregate((i1, i2) => i1.x > i2.x ? i1 : i2); break;
                }
                if (r != null)
                    CreateCar(r);
            }
        }

        public void CreateCar(Road r)
        {
            var pos = (r.WorldX + Core.Cube / 2, r.WorldY + Core.Cube / 2).P();
            var angle = new LoopValueF(r.Angle, 0F, 360F);
            new Car() { Pos = pos, Angle = angle };
        }

        public void Draw(Graphics g)
        {
            draw_grid(g);
            Roads.Where(t => Core.VisibleBounds.Contains((t.x, t.y).P().ToPoint())).ToList().ForEach(t => drawroad(g, t));
            Signs.Where(s => Core.VisibleBounds.Contains((s.x, s.y).P().ToPoint())).ToList().ForEach(s => drawsign(g, s));
            draw_entities(g);
        }

        public Road FindRoad(PointF pos) => FindRoad((int)pos.X / Core.Cube, (int)pos.Y / Core.Cube);
        public Road FindRoad(int x, int y) => Roads.FirstOrDefault(r => r.x == x && r.y == y);
        public Sign FindSign(int x, int y) => Signs.FirstOrDefault(r => r.x == x && r.y == y);
    }
}
