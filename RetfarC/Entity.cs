using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetfarC
{
    public class Entity
    {
        public VecF Pos, RndPos, Look;
        public Bitmap Image;
        public bool Visible = true, ToVisible = false;
        public bool Destroy = false;
        public Rectangle Bounds => new Rectangle((int)Pos.X * Core.TSZ, (int)(Pos.Y - (Image.Height > 24 ? 1 : 0)) * Core.TSZ, Core.TSZ, (Image.Height > 24 ? 2:1)*Core.TSZ);
        public bool MouseHover => Bounds.Contains(Core.MousePos);

        public Entity(float x, float y, float lx = 0F, float ly = 0F)
        {
            Pos = new VecF { X=x, Y=y };
            RndPos = new VecF { X=Core.RND.Next(8), Y= Core.RND.Next(8) };
            Look = new VecF { X= lx, Y=ly };
            EntityMgr.Entities.Add(this);
        }
        public Entity(VecF pos, float lx = 0F, float ly = 0F)
        {
            Pos = pos;
            Look = new VecF { X = lx, Y = ly };
            EntityMgr.Entities.Add(this);
        }
        public Entity(float x, float y, VecF look)
        {
            Pos = new VecF { X = x, Y = y };
            Look = look;
            EntityMgr.Entities.Add(this);
        }
        public Entity(VecF pos, VecF? look = null)
        {
            Pos = pos;
            Look = look ?? VecF.Empty;
            EntityMgr.Entities.Add(this);
        }

        public virtual void Update()
        {
        }

        public virtual void Draw()
        {
            if (Visible)
            {
                Core.g.DrawImage(Image, Pos.X * Core.TSZ + RndPos.X - (Image.Width - 24), Pos.Y * Core.TSZ + RndPos.Y - (Image.Height - 24));
                if (MouseHover)
                    Core.g.DrawRectangle(Pens.Cyan, Bounds);
            }
        }

        public virtual void Click(MouseEventArgs e)
        {
        }
    }
}
