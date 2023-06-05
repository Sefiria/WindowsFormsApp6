using PishConverter.Common.MapSpec;
using PishConverter.Tools;
using System.Drawing;

namespace PishConverter.Common
{
    internal class Drawable : Entity
    {
        public Bitmap Texture { get; protected set; }
        public int X => (int)Position.X - W / 2;
        public int Y => (int)Position.Y - H / 2;
        public int W => Texture?.Width ?? 0;
        public int H => Texture?.Height ?? 0;
        public Rectangle Rect => new Rectangle(X, Y, W, H);

        public Drawable()
        {
            Map.Instance.Entities.Add(this);
        }

        public virtual void Draw()
        {
            if(Texture != null)
                Global.g.DrawImage(Texture, X, Y);
        }
    }
}
