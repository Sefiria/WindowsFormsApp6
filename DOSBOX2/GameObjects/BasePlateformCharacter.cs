using DOSBOX2.Common;
using DOSBOX2.Scenes.ninja;
using Tooling;
using KeyBindings = DOSBOX2.Common.CharacterControls.KeyBindings;

namespace DOSBOX2.GameObjects
{
    internal class BasePlateformCharacter : Entity
    {
        #region Events
        public delegate void EmptyHandler();
        //public delegate void UpdateHandler();
        //public delegate void DrawHandler(Graphics g);
        public event EmptyHandler OnAction;
        //public event UpdateHandler OnUpdate;
        //public event DrawHandler OnDraw;
        #endregion

        public BasePlateformCharacter(
            byte faction,
            CharacterStatus.Init status_init = CharacterStatus.Init.Mid,
            Collider collider = null,
            Inventory inventory = null
            )
            : base(faction, status_init, collider, inventory)
        {
            Faction = faction;
            Controls = new CharacterControls();
            Status = new CharacterStatus(status_init);
            Collider = collider;
            Inventory = inventory ?? new Inventory();
        }

        public override void Update()
        {
            UpdateShot();
            Inventory.CleanEmptySlots();
        }
        private void UpdateShot()
        {
            if (KB.IsKeyPressed(Controls.KeyBinds[KeyBindings.Shot]))
            {
                if (Inventory.Contains(DB.REF.GUN) && Inventory.Substract(DB.REF.BULLET))
                {
                    new BaseBullet(1, (Facing == Faces.Left ? -1 : 1, 0).Vf(), Faction) { Name = "Bullet (Player)", Facing = Facing, x = x + w / 2, y = y + h / 2 };
                }
            }

            if (KB.IsKeyDown(Controls.KeyBinds[KeyBindings.Left]))
                x -= Speed;
            if (KB.IsKeyDown(Controls.KeyBinds[KeyBindings.Right]))
                x += Speed;
        }
    }
}
