using System;
using System.Drawing;

namespace WindowsFormsApp3.Entities
{
    public abstract class DrawableEntity : Entity, IDraw
    {
        public virtual Bitmap Image { get; set; }
        public int W => Image.Width;
        public int H => Image.Height;

        public DrawableEntity(float X = 0F, float Y = 0F) : base(X, Y)
        {
            if(!(this is Player))
                SharedData.Entities.Add(this);
        }

        public virtual void Draw()
        {
            if (Image != null)
            {
                SharedCore.g.DrawImage(Image, X, Y);
                //SharedCore.g.DrawRectangle(Pens.White, X, Y, W, H);
            }
        }

        public bool Collides(DrawableEntity e, float previsionX = -1F, float previsionY = -1F)
        {
            float ex = previsionX == -1F ? e.X : previsionX;
            float ey = previsionY == -1F ? e.Y : previsionY;
            float ew = e.W;
            float eh = e.H;
            return Maths.SQ(ex - X) + Maths.SQ(ey - Y) < Maths.SQ(Math.Min(ew, eh) / 2F + Math.Min(W, H) / 2F);
        }
    }
}
