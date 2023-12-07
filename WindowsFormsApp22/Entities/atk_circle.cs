using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp22.Entities
{
    internal class atk_circle : Circle, IAtk
    {
        public static List<Entity> AtkEntities => Core.Map.VisibleEntities.Where(e => e is IAtk).ToList();

        private float base_distance_to_player = 50F + RandomThings.rnd(70);
        private float distance_to_player => base_distance_to_player + (float)Math.Sin(atick.Value.ToRadians()) * 20F;
        private LoopValueF atick = new LoopValueF(0F, 0F, 360F);

        public atk_circle(float _x, float _y)
        {
            X = _x;
            Y = _y;
            weight = 30;
        }

        public override void Update()
        {
            var look = Pos.LookToPlayer();
            if (Maths.Distance(Core.Player.Pos, Pos) > (Math.Max(W, H) + Math.Max(Core.Player.W, Core.Player.H)) / 2F + distance_to_player)
                Pos = Pos.PlusF(look.x(1F));
            else
                Pos = Pos.Minus(look.x(1F));
            atick.Value += RandomThings.rnd(3F);

            AtkEntities.ForEach(e => { if (Maths.Distance(e.Pos, Pos) <= (Math.Max(W, H) + Math.Max(e.W, e.H)) / 2F) Pos = Pos.Minus(e.Pos.Minus(Pos).norm()); });
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
