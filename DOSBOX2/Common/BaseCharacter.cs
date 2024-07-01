using DOSBOX2.Common;
using DOSBOX2.Scenes.ninja;
using System.Drawing;
using Tooling;
using static DOSBOX2.Common.EventHandlers;
using KeyBindings = DOSBOX2.Common.CharacterControls.KeyBindings;

namespace DOSBOX2.GameObjects
{
    internal class BaseCharacter : AnimEntity
    {
        public BaseCharacter(
            byte faction,
            AnimController animController = null,
            Collider collider = null,
            CharacterStatus.Init status_init = CharacterStatus.Init.Mid,
            Inventory inventory = null
            )
            : base(faction, status_init, collider, animController, inventory)
        {
            Faction = faction;
            Controls = new CharacterControls();
            Status = new CharacterStatus(status_init);
            Inventory = inventory ?? new Inventory();
        }
    }
}
