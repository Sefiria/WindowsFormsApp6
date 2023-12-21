using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp17
{
    public class Entity : ICoords, ISized
    {
        public bool Exist = true;
        public bool IsDrawable = true;
        public List<Bitmap> Images = null;
        public float anim_spd = 0.1F;
        private int anim_frame = 0, anim_last_dir = 0;
        private float anim_frame_time = 0F;
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
        public float W { get => m_W > 0 ? m_W : m_W = Images[0].Width; set => m_W = value; }
        public float H { get => m_H > 0 ? m_H : m_H = Images[0].Height; set => m_H = value; }
        public float RescaleW = 1F, RescaleH = 1F;
        public virtual PointF DrawPoint => Utils.DrawPoint(Pos);
        public virtual Rectangle Bounds => new Rectangle((int)Pos.X, (int)Pos.Y, (int)W, (int)H);
        public RectangleF BoundsF => new RectangleF(Pos.X, Pos.Y, W, H);
        public virtual Point Tile => Pos.Div(Core.TSZ);
        public PointF DrawTile => Utils.DrawPoint(Pos.PlusF(W / 2, H / 2).Snap(Core.TSZ));
        //public Point DrawTile => DrawPoint.Snap(Core.TSZ);
        public virtual PointF CalculateLook() => Maths.AngleToPointF(Angle.Value).Round();
        public List<PointF> Edges => new List<PointF>() { Pos.Minus(W/2, H/2), Pos.PlusF(W/2, -H/2), Pos.PlusF(-H/2, H/2), Pos.PlusF(W/2, H/2) };
        public bool IsVisible => Core.RenderBounds.Contains(DrawPoint.ToPoint());

        public Entity()
        {
        }
        public Entity(string name, Bitmap img, int splitx = 0, int splity = 0, float resize_factor = 0F)
        {
            Images = (splitx > 0 && splity > 0) ? img.Split(0, 0, splitx, splity) : new List<Bitmap> { img };
            for (int i = 0; i < Images.Count; i++)
            {
                Images[i].MakeTransparent(Color.White);
                if (resize_factor != 0F)
                {
                    var _img = Images[i];
                    Images[i] = new Bitmap(_img, (int)(_img.Width * resize_factor), (int)(_img.Height * resize_factor));
                }
            }
            Images.ForEach(x =>
            {
            });
            Name = name;
            Ctor();
        }
        public void Ctor()
        {
            Id = Guid.NewGuid();
            W = Images[0].Width;
            H = Images[0].Height;
            Data.Instance.Entities.Add(this);
        }

        public virtual void Draw(Graphics g, PointF? position = null)
        {
            var img = new Bitmap(Images[anim_frame]);
            if (anim_last_dir < 0) img.RotateFlip(RotateFlipType.RotateNoneFlipX);
            g.DrawImage(img, DrawPoint);
        }

        public virtual void Update()
        {
            if (Exist == false)
                Data.Instance.Entities.Remove(this);
        }

        public void Cascade(Action<List<object>> action, List<object> data)
        {
            data = data.Prepend( this ).ToList();
            action(data);
            new List<Entity>(Children).ForEach(e => { if (!e.Exist) Children.Remove(e); else e.Cascade(action, data); });
            if (Exist == false)
                Data.Instance.Entities.Remove(this);
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

        public void Animate(int direction)
        {
            anim_last_dir = direction;
            if (anim_last_dir == 0)
            {
                anim_frame_time = 0F;
                anim_frame = 0;
            }
            else
            {
                anim_frame_time += anim_spd;
                if (anim_frame_time >= 1F) { anim_frame+=direction; anim_frame_time = 0F; }
                if (anim_frame < 0) { anim_frame += Images.Count; }
                if (anim_frame >= Images.Count) { anim_frame -= Images.Count; }
            }
        }
    }
}
