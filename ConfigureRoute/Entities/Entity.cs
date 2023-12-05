using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace ConfigureRoute.Entities
{
    public class Entity
    {
        public bool Exist = true;
        public bool IsDrawable = true;
        public Bitmap tex = null;
        //public virtual Anim Anim { get; set; } = null;

        public Guid Id;
        public string Name;
        public PointF Pos;
        public LoopValueF Angle = new LoopValueF(0F, 0F, 360F);

        public float m_W = 0F, m_H = 0F;
        public float W { get => m_W > 0 ? m_W : tex.Width; set => m_W = value; }
        public float H { get => m_H > 0 ? m_H : tex.Height; set => m_H = value; }
        public PointF DrawPoint => Core.CenterPoint.PlusF(Pos);
        public virtual Rectangle Bounds => new Rectangle((int)Pos.X, (int)Pos.Y, (int)W, (int)H);
        public RectangleF BoundsF => new RectangleF(Pos.X, Pos.Y, W, H);
        public PointF Tile => Pos.DivF(Core.Cube);
        public PointF CalculateLook() => Maths.AngleToPointF(Angle.Value).Round();
        public List<PointF> Edges => new List<PointF>() { Pos.Minus(W/2, H/2), Pos.PlusF(W/2, -H/2), Pos.PlusF(-H/2, H/2), Pos.PlusF(W/2, H/2) };

    public Entity()
        {
            Ctor();
        }
        public Entity(string tex)
        {
            this.tex = TexMgr.Load(tex);
            Ctor();
        }
        public void Ctor()
        {
            Id = Guid.NewGuid();
            Core.Map.Entities.Add(this);
        }

        public virtual void Draw(Graphics g, PointF? position = null)
        {
            if (tex != null)
            {
                Core.g.DrawImage(tex, position ?? PointF.Empty);
            }
        }

        public virtual void Update() { }
    }
}
