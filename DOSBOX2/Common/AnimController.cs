using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DOSBOX2.Common.Entity;

namespace DOSBOX2.Common
{
    internal class AnimController
    {
        public enum AnimType
        {
            Idle, Move, Shot
        }

        public Dictionary<AnimType, Anim> Animations;
        public float w = 0, h = 0;

        protected AnimType last_type = AnimType.Idle;

        public AnimController()
        {
            Animations = new Dictionary<AnimType, Anim>();
        }
        public AnimController(Dictionary<AnimType, Anim> animations)
        {
            Animations = animations;
        }

        public void Draw(AnimType type, Entity e)
        {
            if (!Animations.ContainsKey(type) || Animations[type] == null)
            {
                w = h = 0F;
                return;
            }
            else
            {
                w = Animations[type].w;
                h = Animations[type].h;
                Animations[type].Draw(e);
                last_type = type;
            }
        }
        public void ClearDraw(Entity e)
        {
            if (Animations.ContainsKey(last_type) && Animations[last_type] == null)
            {
                Animations[last_type].ClearDraw(e);
            }
        }
    }
}
