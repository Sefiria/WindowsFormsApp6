using System;
using System.Collections.Generic;
using System.Linq;
using Tooling;
using static DOSBOX2.Common.AnimController;

namespace DOSBOX2.Common
{
    internal class AnimEntity : ColliderEntity
    {
        public AnimController AnimController;

        public AnimEntity(
            byte faction = 0,
            CharacterStatus.Init status_init = CharacterStatus.Init.Mid,
            Collider collider = null,
            AnimController animController = null,
            Inventory inventory = null)
            : base(faction, status_init, collider, inventory)
        {
            AnimController = animController;
        }
        public override void Draw()
        {
            if (AnimController == null || !first_before_update_done)
                return;
            if(IsMoving)
                AnimController.Draw(AnimType.Move, this);
            else
                AnimController.Draw(AnimType.Idle, this);
        }
        public override void ClearDraw()
        {
            AnimController?.ClearDraw(this);
        }
    }
}
