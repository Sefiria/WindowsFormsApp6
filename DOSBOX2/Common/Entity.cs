using System;
using Tooling;

namespace DOSBOX2.Common
{
    internal class Entity
    {
        public enum Faces
        {
            Left, Right, Top, Bottom
        }

        public static EntityManager Manager = null;

        public bool Exists = true;
        public Guid ID;
        public string Name;
        public byte Faction; // group of colliders
        public vecf Look = vecf.Zero;
        public Faces Facing = Faces.Right;
        public CharacterControls Controls;
        public CharacterStatus Status;
        public Collider Collider;
        public Inventory Inventory;
        public byte[,] Texture = null;
        public vecf PrevPosition;

        private bool first_before_update_done = false;

        public float Speed = 0.5F;

        public float x { get => Collider?.x ?? 0F; set { if (Collider != null) Collider.x = value; } }
        public float y { get => Collider?.y ?? 0F; set { if (Collider != null) Collider.y = value; } }
        public float w
        {
            get
            {
                if (Collider == null) return 0F;
                switch (Collider)
                {
                    default: return 0F;
                    case Circle c: return c.diameter;
                    case Box b: return b.w;
                }
            }
        }
        public float h
        {
            get
            {
                if (Collider == null) return 0F;
                switch (Collider)
                {
                    default: return 0F;
                    case Circle c: return c.diameter;
                    case Box b: return b.h;
                }
            }
        }

        public Entity(
            byte faction = 0,
            CharacterStatus.Init status_init = CharacterStatus.Init.Mid,
            Collider collider = null,
            Inventory inventory = null)
        {
            ID = Guid.NewGuid();

            Faction = faction;
            Status = new CharacterStatus(status_init);
            Collider = collider;
            Inventory = inventory;

            Manager.Add(this);
        }

        public virtual void BeforeUpdate()
        {
            PrevPosition = (x, y).Vf();
            first_before_update_done = true;
        }
        public virtual void Update()
        {
            x += Look.x * Speed * Core.Speed;
            y += Look.y * Speed * Core.Speed;
        }
        public virtual void AfterUpdate()
        {
            Controls?.Update();
        }
        public virtual void Colliding()
        {
        }
        public virtual void Draw(bool force = false, bool reset_prev = true)
        {
            if (!force && (PrevPosition?.i == (x, y).V() || !first_before_update_done))
                return;
            if(Texture != null)
            {
                if(reset_prev)Graphic.SetBatch(Texture, (int)PrevPosition.x, (int)PrevPosition.y, Graphic.BatchMode.Reset, Facing == Faces.Left);
                Graphic.SetBatch(Texture, (int)x, (int)y, Graphic.BatchMode.Raw, Facing == Faces.Left);
            }
            else if(Collider != null)
            {
                switch (Collider)
                {
                    case Circle c:
                        if (reset_prev) Graphic.FillCircle(3, (int)PrevPosition.x, (int)PrevPosition.y, (int)c.r);
                        Graphic.FillCircle(0, (int)c.x, (int)c.y, (int)c.r);
                        break;
                    case Box b:
                        if (reset_prev) Graphic.FillRect(3, (int)PrevPosition.x, (int)PrevPosition.y, (int)b.w, (int)b.h);
                        Graphic.FillRect(0, (int)b.x, (int)b.y, (int)b.w, (int)b.h);
                        break;
                }
            }
        }
    }
}
