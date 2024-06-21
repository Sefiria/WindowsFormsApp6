using DOSBOX2.Common;
using DOSBOX2.Scenes.ninja;
using System.Drawing;
using Tooling;

namespace DOSBOX2.GameObjects
{
    internal class BasePlateformCharacter
    {
        public byte Faction; // group of colliders
        public CharacterControls Controls;
        public CharacterStatus Status;
        public Collider Collider;
        public Inventory Inventory;

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
        {
            Faction = faction;
            Controls = new CharacterControls();
            Status = new CharacterStatus(status_init);
            Collider = collider;
            Inventory = inventory ?? new Inventory();
        }

        public virtual void Update()
        {
            UpdateShot();
            Inventory.CleanEmptySlots();
        }
        private void UpdateShot()
        {
            if (KB.IsKeyPressed(Controls.Shot))
            {
                if (Inventory.Substract(DB.REF.GUN))
                {
                    // create bullet
                }
            }
        }
        public virtual void Colliding()
        {
        }
        public virtual void Draw(Graphics g)
        {
        }
    }
}
