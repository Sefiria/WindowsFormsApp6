using System;
using Tooling;

namespace DOSBOX2.Common
{
    internal class ColliderEntity : Entity
    {
        public Collider Collider;

        public override float x { get => Collider?.x ?? 0F; set { if (Collider != null) Collider.x = value; } }
        public override float y { get => Collider?.y ?? 0F; set { if (Collider != null) Collider.y = value; } }
        public override float w
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
        public override float h
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

        public ColliderEntity(
            byte faction = 0,
            CharacterStatus.Init status_init = CharacterStatus.Init.Mid,
            Collider collider = null,
            Inventory inventory = null)
            : base(faction, status_init, inventory)
        {
            Collider = collider;
        }
        public override void Draw()
        {
            if (!IsKinetic && !IsTrigger && (PrevPosition?.i == (x, y).V() || !first_before_update_done) || Collider == null)
                return;
            switch (Collider)
            {
                case Circle c:
                    if(PrevPosition != null)
                        Graphic.FillCircle(3, (int)PrevPosition.x, (int)PrevPosition.y, (int)c.r);
                    Graphic.FillCircle(0, (int)c.x, (int)c.y, (int)c.r);
                    break;
                case Box b:
                    if (PrevPosition != null)
                        Graphic.FillRect(3, (int)PrevPosition.x, (int)PrevPosition.y, (int)b.w, (int)b.h);
                    Graphic.FillRect(0, (int)b.x, (int)b.y, (int)b.w, (int)b.h);
                    break;
            }
        }
        public override void ClearDraw()
        {
            if(Collider != null)
            switch (Collider)
            {
                case Circle c:
                    Graphic.FillCircle(3, (int)x, (int)y, (int)c.r);
                    break;
                case Box b:
                    Graphic.FillRect(3, (int)x, (int)y, (int)b.w, (int)b.h);
                    break;
            }
        }
    }
}
