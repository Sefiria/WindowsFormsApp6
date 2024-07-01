using DOSBOX2.Common;
using DOSBOX2.Scenes.ninja;
using System;
using System.Drawing;
using Tooling;
using static DOSBOX2.Common.EventHandlers;
using KeyBindings = DOSBOX2.Common.CharacterControls.KeyBindings;

namespace DOSBOX2.GameObjects
{
    internal class BasePlateformCharacter : BaseCharacter
    {
        protected float Jumping_incr_amount = 0F;

        public BasePlateformCharacter(
            byte faction,
            AnimController animController = null,
            Collider collider = null,
            CharacterStatus.Init status_init = CharacterStatus.Init.Mid,
            Inventory inventory = null
            )
            : base(faction, animController, collider, status_init, inventory)
        {
        }

        public override void Update()
        {
            UpdateShot();

            bool left = Controls.ForcedStates[KeyBindings.Left] || KB.IsKeyDown(Controls.KeyBinds[KeyBindings.Left]);
            bool right = Controls.ForcedStates[KeyBindings.Right] || KB.IsKeyDown(Controls.KeyBinds[KeyBindings.Right]);
            bool jump = IsOnGround && (Controls.ForcedStates[KeyBindings.Jump] || KB.IsKeyDown(Controls.KeyBinds[KeyBindings.Jump]));

            Look.x = left ? - 1F : (right ? 1F : 0F);

            if(Jumping_incr_amount > 0F)
            {
                if (Jumping_incr_amount < 1.2F)
                {
                    Jumping_incr_amount += 0.1F;
                    Look.y -= Jumping_incr_amount;
                }
                else
                {
                    Jumping_incr_amount = 0F;
                }
            }
            else if (jump)
            {
                Jumping_incr_amount = 0.5F;
                Look.y -= Jumping_incr_amount;
            }

            base.Update();

            Inventory.CleanEmptySlots();
        }
        private void UpdateShot()
        {
            if (KB.IsKeyPressed(Controls.KeyBinds[KeyBindings.Shot]))
            {
                if (Inventory.Contains(DB.REF.GUN) && Inventory.Substract(DB.REF.BULLET))
                {
                    new BaseBullet(this, 1, (Facing == Faces.Left ? -2 : 2, 0).Vf(), Faction) { Name = "Bullet", Facing = Facing, x = x + w / 2, y = y + h / 2 };
                }
            }
        }
    }
}
