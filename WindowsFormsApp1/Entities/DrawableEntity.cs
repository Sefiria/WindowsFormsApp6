using System.Drawing;

namespace WindowsFormsApp1.Entities
{
    public abstract class DrawableEntity : Entity, IDraw
    {
        public Bitmap Image { get; set; }
        public int W => Image.Width;
        public int H => Image.Height;

        public DrawableEntity(int X = 0, int Y = 0) : base(X, Y)
        {
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
    }
}
