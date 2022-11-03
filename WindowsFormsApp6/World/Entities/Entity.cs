using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6.World.Entities
{
    public class Entity
    {
        public float X, Y;
        public bool LookAtRight = false;
        public Bitmap ImageLeft = null;
        public Bitmap ImageRight = null;

        public Entity(float x, float y, Bitmap image)
        {
            X = x;
            Y = y;
            ImageLeft = image;
            ImageRight = new Bitmap(image);
            ImageRight.RotateFlip(RotateFlipType.RotateNoneFlipX);
        }

        public virtual void Update() { }
        public virtual void Draw()
        {
            Core.g.DrawImage(LookAtRight ? ImageRight : ImageLeft, X, Y);
        }
    }
}
