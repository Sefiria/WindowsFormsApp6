using DOSBOX2.GameObjects;
using System;
using System.Drawing;
using System.Linq;
using Tooling;
using static DOSBOX2.Common.EventHandlers;

namespace DOSBOX2.Common
{
    internal class Entity
    {
        public enum Faces
        {
            Left, Right, Top, Bottom
        }

        public static EntityManager Manager = null;

        #region events
        public event EmptyHandler OnGroundHit;
        public event CollisionHandler OnHit;
        #endregion

        public bool Exists = true;
        public Guid ID;
        public string Name;
        public byte Faction; // group of colliders
        public vecf Look = vecf.Zero;
        public Faces Facing = Faces.Right;
        public CharacterControls Controls;
        public CharacterStatus Status;
        public Inventory Inventory;
        public vecf PrevPosition;
        public Entity Owner = null;
        /// <summary>
        /// Prevent Gravity to apply if true
        /// </summary>
        public bool IsKinetic = false;
        /// <summary>
        /// Indicates no physical collision with entities, but keep triggering hit events
        /// IsKinetic = true is implicit
        /// </summary>
        public bool IsTrigger = false;

        public float Gravity = 0.75F;
        public bool IsMoving = false;
        public bool IsOnGround = false;

        public virtual float x { get; set; }
        public virtual float y { get; set; }
        public virtual float w { get; protected set; }
        public virtual float h { get; protected set; }

        public float cx => x + w / 2F;
        public float cy => y + h / 2F;
        public vecf center => (cx, cy).Vf();


        public Rectangle Rect => new Rectangle((int)x, (int)y, (int)w, (int)h);

        protected bool first_before_update_done = false;

        public float Speed = 0.5F;

        public Entity(
            byte faction = 0,
            CharacterStatus.Init status_init = CharacterStatus.Init.Mid,
            Inventory inventory = null)
        {
            ID = Guid.NewGuid();

            Faction = faction;
            Status = new CharacterStatus(status_init);
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
            if (Look.x < 0F) Facing = Faces.Left;
            if (Look.x > 0F) Facing = Faces.Right;

            Entity ground_contact_collider = null;
            bool touches_ground = IsKinetic || IsTrigger || (ground_contact_collider = Manager.Entities.Except(this).ToList().FirstOrDefault(e => new RectangleF(x, y + h - 2, w, 4).IntersectsWith(e.Rect))) != null;
            if (!IsOnGround && touches_ground)
            {
                OnGroundHit?.Invoke();
                Look.y = Math.Min(0F, Look.y);
            }
            IsOnGround = touches_ground;

            if (!IsOnGround)
                Look.y += !IsKinetic && !IsTrigger && !touches_ground ? Gravity : 0F;

            if(Manager.Entities.Except(this).Except(ground_contact_collider).ToList().FirstOrDefault(e => new RectangleF(x + Look.x * Speed * Core.Speed, y + h - 2, w, 4).IntersectsWith(e.Rect)) == null)
                x += Look.x * Speed * Core.Speed;
            y += Look.y * Speed * Core.Speed;
        }
        public virtual void AfterUpdate()
        {
            Controls?.Update();
            IsMoving = Controls?.IsMoving ?? false;
        }
        public virtual void Colliding()
        {
            if (IsTrigger) return;
            var entities = Manager.Entities.Except(this).ToList().Where(e => !e.IsTrigger);
            vecf n;

            foreach (Entity other in entities)
            {
                if (!other.Rect.IntersectsWith(Rect)) continue;
                n = (other.center - center).Normalized();

                if (other.IsKinetic && !IsKinetic)
                {
                    do { x -= n.x * Core.Speed * 2F; y -= n.y * Core.Speed * 2F; } while (other.Rect.IntersectsWith(Rect));
                }
                else if (!other.IsKinetic && IsKinetic)
                {
                    do { other.y -= Speed * Core.Speed * 2F; } while (other.Rect.IntersectsWith(Rect));
                }
                else if (!other.IsKinetic && !IsKinetic)
                {
                    do
                    {
                        if (other.y < y) { x -= n.x * Core.Speed * 2F; y -= n.y * Core.Speed * 2F; }
                        else { other.x -= n.x * Core.Speed * 2F; other.y -= n.y * Core.Speed * 2F; }
                    } while (other.Rect.IntersectsWith(Rect));
                }
            }
        }

        public virtual void Draw()
        {
        }
        public virtual void ClearDraw()
        {
        }
    }
}
