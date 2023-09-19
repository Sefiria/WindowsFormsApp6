using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WindowsFormsApp21.Entities
{
    public class CollisionCircle : CollisionBase
    {
        public Entity Entity;
        public PointF Pos => Entity.Pos;
        public float X => Pos.X;
        public float Y => Pos.Y;
        public float Radius = Core.Cube;
        public float R => Radius;

        public RectangleF RectF => new RectangleF(Entity.Pos, new SizeF(Radius, Radius));
        public Rectangle Rect => new Rectangle((int)RectF.X, (int)RectF.Y, (int)RectF.Width, (int)RectF.Height);

        public CollisionCircle()
            : base()
        {
            Type = CollisionType.Circle;
        }
        public CollisionCircle(ref Entity e, float radius)
            : base()
        {
            Type = CollisionType.Circle;
            Entity = e;
            Radius = radius;
        }
    }
}
