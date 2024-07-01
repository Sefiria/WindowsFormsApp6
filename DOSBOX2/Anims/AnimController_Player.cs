using DOSBOX2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX2.Anims
{
    internal class AnimController_Player : AnimController
    {
        public AnimController_Player() : base()
        {
            Animations.Add(AnimType.Idle, new Anim_Player_Idle());
            Animations.Add(AnimType.Move, new Anim_Player_Move());
        }
    }
}
