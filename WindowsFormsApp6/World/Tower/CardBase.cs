using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.World.Entities;

namespace WindowsFormsApp6.World.Tower
{
    public class CardBase
    {
        public int X, Y;
        public Bitmap Image;
        public CardBase(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void Draw()
        {
            Core.g.DrawImage(Image, (5 + X) * Core.TileSz, (7 + Y) * Core.TileSz);
        }

        public virtual void Update(StateTower.TowerStats stats, Func<int, int, TowerUnit> GetClosestTarget, Action<Bullet> AddBullet) { }
    }
}
