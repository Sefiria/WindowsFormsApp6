using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static _Console.Core.Utils.Maths;

namespace Core.Entities
{
    public abstract class Ship : Entity, IDrawable, IUpdatable
    {
        public virtual Bitmap Texture { get; private set; } = null;

        public Rectangle Bounds = Rectangle.Empty;
        public Vec Center = Vec.Zero;

        public Ship(string TexID)
        {
            Texture = ResourceMgr.ResourceManager.Instance.GetResource(TexID);
            Bounds = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Center = new Vec(Bounds.Width / 2, Bounds.Height / 2);
        }
        public virtual void Render(Graphics g)
        {
            g.DrawImage(Texture, Position.AsPointF);
        }
        public virtual void Update()
        {
        }
    }
}
