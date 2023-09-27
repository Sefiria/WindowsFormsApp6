using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniKube.Entities
{
    public class Entity
    {
        public bool IsDrawable = true;
        public Bitmap tex = null;
        public virtual Anim Anim { get; set; } = null;

        public Guid Id;
        public string Name;
        public PointF Pos;

        public float W => tex.Width;
        public float H => tex.Height;
        public PointF DrawPoint => Core.CenterPoint.PlusF(Pos);

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
        }

        public virtual void Draw(PointF? position = null)
        {
            if (tex != null)
            {
                Core.g.DrawImage(tex, position ?? PointF.Empty);
            }
        }
    }
}
