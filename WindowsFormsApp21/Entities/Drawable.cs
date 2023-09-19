using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp21.Animations;

namespace WindowsFormsApp21.Entities
{
    public class Drawable : Entity
    {
        public ICollision Collision;
        public Animation Anim;

        public Drawable()
            : base()
        {
            IsDrawable = true;
        }
        public Drawable(ICollision Collision, string animationName)
            : base()
        {
            IsDrawable = true;
            Anim = Animation.Load(animationName);
        }

        public void Draw()
        {
            Animator.Draw(Anim, Core.g);
        }
    }
}
