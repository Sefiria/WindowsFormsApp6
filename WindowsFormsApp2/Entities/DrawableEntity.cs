using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp2.Entities.Enumerations;

namespace WindowsFormsApp2.Entities
{
    public abstract class DrawableEntity : Entity, IDraw
    {
        public virtual Bitmap Image { get; set; }
        public int W => Image.Width;
        public int H => Image.Height;

        public DrawableEntity(int X = 0, int Y = 0) : base(X, Y)
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
    }
}
