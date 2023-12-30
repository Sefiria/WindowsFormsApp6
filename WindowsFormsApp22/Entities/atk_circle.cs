using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp22.Entities
{
    internal class atk_circle : Circle, IAtk
    {
        public static List<Entity> AtkEntities => Core.Map.VisibleEntities.Where(e => e is IAtk).ToList();

        private float base_distance_to_player = 50F + RandomThings.rnd(70);
        private float distance_to_player => base_distance_to_player + (float)Math.Sin(atick.Value.ToRadians()) * 20F;
        private LoopValueF atick = new LoopValueF(0F, 0F, 360F);
        public int cooldownA = 0, cooldown_maxA = 400;
        public int cooldownB = 0, cooldown_maxB = 1000;

        public atk_circle(float _x, float _y)
        {
            X = _x;
            Y = _y;
            weight = 30;
            cooldownA = RandomThings.rnd(cooldown_maxA);
            cooldownB = RandomThings.rnd(cooldown_maxB);
        }

        public override void Update()
        {
            var look = Pos.LookToPlayer();
            if (Maths.Distance(Core.Player.Pos, Pos) > (Math.Max(W, H) + Math.Max(Core.Player.W, Core.Player.H)) / 2F + distance_to_player)
                Pos = Pos.PlusF(look.x(IsVisible ? 0.5F : 2F));
            else
                Pos = Pos.MinusF(look.x(IsVisible ? 0.5F : 2F));
            atick.Value += RandomThings.rnd(3F);

            AtkEntities.ForEach(e => { if (Maths.Distance(e.Pos, Pos) <= (Math.Max(W, H) + Math.Max(e.W, e.H)) / 2F) Pos = Pos.MinusF(e.Pos.MinusF(Pos).norm()); });


            if (IsVisible)
            {
                void shot(float a)
                {
                    var _look = look.Rotate(a);
                    var bullet = new Behaviored("atk_bullet", Color.White, Pos.X + _look.X * W * 1.1F, Pos.Y + _look.Y * H * 1.1F, 2, 2, Behaviored.Default_AddLook(_look, 1F));
                    bullet.Parent = Core.Map.obj_colliders;
                    bullet.weight = 10;
                }

                if (cooldownA == 0)
                {
                    shot(0);
                    cooldownA += cooldown_maxA;
                }
                else cooldownA--;

                if (cooldownB == 0)
                {
                    shot(-10);
                    shot(0);
                    shot(10);
                    cooldownB += cooldown_maxB;
                }
                else cooldownB--;
            }
        }
        public bool Collides(Entity e)
        {
            //if (e is Shape)
            //{
            //    switch (e)
            //    {
            //        case Square square: return Maths.coll;
            //        case Circle circle: return true;
            //    }
            //}
            return false;
        }
    }
}
