using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp22.Entities;

namespace WindowsFormsApp22
{
    public class EvtMgr
    {
        List<Entity> Atks = new List<Entity>();
        public int cooldown = 0, max_cooldown=200;
        public int wave = 1, count = 0;
        public List<Entity> Pending = new List<Entity>();

        public EvtMgr()
        {
        }

        public void Update()
        {
            Pending.RemoveAll(e => e.Exist == false);

            if (cooldown <= 0)
            {
                if (count <= 10)
                {
                    cooldown = max_cooldown - count * 5 * wave;
                    var what = What();
                    Pending.Add(what);
                    var where = Where(what);
                    what.X = where.x;
                    what.Y = where.y;
                    float rescale = 0.5F + RandomThings.rnd(5) / 10F;
                    what.RescaleW = rescale;
                    what.RescaleH = rescale;
                    what.weight = (int)(rescale * 60F);
                    what.Parent = Core.Map.obj_colliders;
                    count++;
                }
                else
                {
                    if(Pending.Count == 0)
                    {
                        count -= 10;
                        wave++;
                    }
                }
            }
            else cooldown--;
        }

        (float x, float y) Where(Entity what)
        {
            float x, y, w = Core.RW, h = Core.RH;
            int side = RandomThings.rnd(4);
            switch (side)
            {
                default:
                case 0: x = Core.Player.X - w / 2 + RandomThings.rnd(w); y = Core.Player.Y - h  / 2 - what.H * 10F; break;
                case 1: y = Core.Player.Y - h  / 2 + RandomThings.rnd(h); x = Core.Player.X - w / 2 - what.W * 10F; break;
                case 2: x = Core.Player.X - w / 2 + RandomThings.rnd(w); y = Core.Player.Y + h / 2 + what.H * 10F; break;
                case 3: y = Core.Player.Y - h  / 2 + RandomThings.rnd(h); x = Core.Player.X + w / 2 + what.W * 10F; break;
            }
            return (x, y);
        }
        Entity What()
        {
            int TYPES_COUNT = 1;
            int rnd = RandomThings.rnd(TYPES_COUNT);
            switch (rnd)
            {
                default:
                case 0: return new atk_circle(0, 0);
            }
        }
    }
}
