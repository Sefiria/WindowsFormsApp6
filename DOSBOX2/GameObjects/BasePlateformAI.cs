using DOSBOX2.Common;
using DOSBOX2.Scenes.ninja;
using System;
using System.Drawing;
using Tooling;
using static DOSBOX2.Common.EventHandlers;
using KeyBindings = DOSBOX2.Common.CharacterControls.KeyBindings;

namespace DOSBOX2.GameObjects
{
    internal class BasePlateformAI : BasePlateformCharacter
    {
        public AIBehavior AI;

        public BasePlateformAI(
            byte faction,
            AIBehavior ai,
            AnimController animController = null,
            Collider collider = null,
            CharacterStatus.Init status_init = CharacterStatus.Init.Mid,
            Inventory inventory = null
            )
            : base(faction, animController, collider, status_init, inventory)
        {
            AI = ai;
        }
        public override void Update()
        {
            AI.Behave(this, Manager);
            base.Update();
        }
    }
}
