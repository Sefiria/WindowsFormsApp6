using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Behaviors.Skills
{
    public abstract class Armament : Entity, IDrawable, IUpdatable
    {
        public virtual Bitmap Texture { get; private set; } = null;

        public Rectangle Bounds = Rectangle.Empty;

        public Armament(string TexID)
        {
            Texture = ResourceMgr.ResourceManager.Instance.GetResource(TexID);
            Bounds = new Rectangle(0, 0, Texture.Width, Texture.Height);
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
