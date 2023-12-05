using ConfigureRoute.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using static ConfigureRoute.Enumerations;

namespace ConfigureRoute.Obj
{
    public class Road
    {
        public int x, y, z;
        // t:top, b:bottom, l:left, r:right
        public Road m_t, m_l, m_b, m_r;
        public Road t => m_t ?? (m_t = Core.Map.FindRoad(x, y-1));
        public Road l => m_l ?? (m_l = Core.Map.FindRoad(x-1, y));
        public Road b => m_b ?? (m_b = Core.Map.FindRoad(x, y+1));
        public Road r => m_r ?? (m_r = Core.Map.FindRoad(x+1, y));

        public vec Position => (x, y).V();
        public float WorldX => x * Core.Cube;
        public float WorldY => y * Core.Cube;
        public vec WorldPosition => (WorldX, WorldY).V();
        public RectangleF Bounds => new RectangleF(WorldX, WorldY, Core.Cube, Core.Cube);
        /// <summary>
        /// Get the Road next to this one. Amount (in tiles) of x and y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Road Next(int x, int y) => Core.Map.FindRoad(this.x + x, this.y + y);
        public Road NextFromV(int z)
        {
            switch(z)
            {
                default: return null;
                case 0:return t;
                case 1:return l;
                case 2:return b;
                case 3:return r;
            }
        }
        public Road NextFromWay(Ways way) => NextFromV(Direction.RotateDirection(way));
        public Road NextFromZ() => NextFromV(z);
        public Road Behind() => NextFromV(OppositeDirection);
        public Sign Sign => Core.Map.FindSign(x, y);
        public List<Car> GetCarsOnIt(Car except = null) => Core.Map.Cars.Except(new List<Car>() { except }).Where(c => c.ContainedByRoad(this)).ToList();

        /// <summary>
        /// 0 up, 1 left, 2 down, 3 right
        /// </summary>
        public int Direction => z;
        public int OppositeDirection => z == 0 ? 2 : (z == 1 ? 3 : (z == 2 ? 0 : 1));
        public float Angle => 270F - Direction * 90F;
        public Road()
        {
        }

        public static Road At(PointF pos) => Core.Map.FindRoad(pos);
    }
}
