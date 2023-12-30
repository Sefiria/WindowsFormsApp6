using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp22.Entities
{
    public class Entity : ICoords, ISized
    {
        public bool Exist = true;
        public bool IsDrawable = true;
        public string tex = null;
        public Color color;
        private Entity m_Parent = null;
        public Entity Parent
        {
            get => m_Parent;
            set
            {
                if (HasParent(value)) return;
                if(m_Parent != null)
                    m_Parent.Children.Remove(this);
                m_Parent = value;
                m_Parent.Children.Add(this);
            }
        }
        public List<Entity> Children = new List<Entity>();

        public Guid Id;
        public string Name;
        public int weight = 0;
        public float X { get; set; }
        public float Y { get; set; }
        public Point iPos => new Point((int)X, (int)Y);
        public PointF Pos
        {
            get => new PointF(X, Y);
            set { X = value.X; Y = value.Y; }
        }
        public LoopValueF Angle = new LoopValueF(0F, 0F, 360F);

        public float m_W = 0F, m_H = 0F;
        public float W { get => m_W > 0 ? m_W : m_W = TexMgr.Load(tex).GetBounds().Width; set => m_W = value; }
        public float H { get => m_H > 0 ? m_H : m_H = TexMgr.Load(tex).GetBounds().Height; set => m_H = value; }
        public float RescaleW = 1F, RescaleH = 1F;
        public PointF DrawPoint => new PointF(Core.CenterPoint.X + Pos.X - Core.cam_ofs.x - Core.Player.X, Core.CenterPoint.Y + Pos.Y - Core.cam_ofs.y - Core.Player.Y);
        public virtual Rectangle Bounds => new Rectangle((int)Pos.X, (int)Pos.Y, (int)W, (int)H);
        public RectangleF BoundsF => new RectangleF(Pos.X, Pos.Y, W, H);
        public PointF Tile => Pos.DivF(Core.Cube);
        public virtual PointF CalculateLook() => Maths.AngleToPointF(Angle.Value).Round();
        public List<PointF> Edges => new List<PointF>() { Pos.MinusF(W/2, H/2), Pos.PlusF(W/2, -H/2), Pos.PlusF(-H/2, H/2), Pos.PlusF(W/2, H/2) };
        public bool IsVisible => Core.RenderBounds.Contains(DrawPoint.ToPoint());

        public Entity()
        {
            Ctor();
        }
        public Entity(string name)
        {
            Name = name;
            Ctor();
        }
        public Entity(string tex, Color color, int w, int h)
        {
            this.tex = Name = tex;
            this.color = color;
            W = w;
            H = h;
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
                var path = TexMgr.Load(tex);
                var matrix = new Matrix();
                var pt = DrawPoint;
                matrix.Translate(pt.X, pt.Y);
                matrix.Scale(W * RescaleW, H * RescaleH);
                matrix.Rotate(Angle.Value);
                path.Transform(matrix);
                g.FillPath(Brushes.Black, path);
                g.DrawPath(new Pen(color), path);
            }
        }

        public virtual void Update()
        {
            if (Exist == false)
                Core.Map.Entities.Remove(this);
        }

        public void Cascade(Action<List<object>> action, List<object> data)
        {
            data = data.Prepend( this ).ToList();
            action(data);
            new List<Entity>(Children).ForEach(e => { if (!e.Exist) Children.Remove(e); else e.Cascade(action, data); });
            if (Exist == false)
                Core.Map.Entities.Remove(this);
        }
        public bool HasParent(Entity entity)
        {
            var parent = Parent;
            while(parent != null)
            {
                if(parent == entity) return true;
                parent = parent.Parent;
            }
            return false;
        }
    }
}
