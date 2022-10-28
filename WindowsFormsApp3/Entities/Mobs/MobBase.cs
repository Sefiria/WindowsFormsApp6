using System.Drawing;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Entities.Mobs
{
    public abstract class MobBase : DrawableEntity, IRP
    {
        public int STRMax { get; set; }
        public int STR { get; set; }
        public int HPMax { get; set; }
        public int HP { get; set; }
        protected MobBase(Bitmap Image, float X = 0F, float Y = 0F) : base(X, Y)
        {
            this.Image = Image;
        }
        public void Hit(IRP From)
        {
            HP -= From.STR;
            if (HP <= 0)
            {
                HP = 0;
                Exists = false;
            }
        }
    }
}
