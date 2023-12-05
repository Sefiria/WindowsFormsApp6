using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cast
{
    public class Entity
    {
        public bool Exists = true;
        public float X = 0, Y = 0;
        public Color C;

        public PointF Position => (X, Y).P();
        public virtual RectangleF Bounds => RectangleF.Empty;

        public Entity()
        {
            Core.Entities.entities.Add(this);
        }
        public virtual void Update()
        {
        }
        public virtual void Draw(Graphics g, PointF? offset = null)
        {
        }
        public virtual void DrawMinimap(Graphics g)
        {
        }
    }
}
