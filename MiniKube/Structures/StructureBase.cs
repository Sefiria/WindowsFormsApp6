using MiniKube.Entities;
using MiniKube.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniKube.Structures
{
    public class StructureBase : Item
    {
        public StructureBase() : base()
        {
        }
        public StructureBase(string resource) : base(resource)
        {
        }
        public override void Draw(PointF? offset = null)
        {
            if(offset != null)
                Core.g.DrawImage(Anim.CurrentTexture, Pos.Minus(Anim.CollisionBounds.X, Anim.CollisionBounds.Y).PlusF(offset.Value));
            else
                Core.g.DrawImage(Anim.CurrentTexture, Pos.Minus(Anim.CollisionBounds.X, Anim.CollisionBounds.Y));
        }
    }
}
