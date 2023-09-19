using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp21.Entities
{
    public class CollisionSquare : CollisionBase
    {
        public Entity Entity;
        public PointF Pos => Entity.Pos;
        public float X => Pos.X;
        public float Y => Pos.Y;
        public float Width = Core.Cube;
        public float Height = Core.Cube;
        public float W => Width;
        public float H => Height;

        public RectangleF RectF => new RectangleF(Entity.Pos, new SizeF(Width, Height));
        public Rectangle Rect => new Rectangle((int)RectF.X, (int)RectF.Y, (int)RectF.Width, (int)RectF.Height);

        public CollisionSquare()
            : base()
        {
            Type = CollisionType.Square;
        }
        public CollisionSquare(ref Entity e, float w, float h)
            : base()
        {
            Type = CollisionType.Square;
            Entity = e;
            Width = w;
            Height = h;
        }
    }
}
