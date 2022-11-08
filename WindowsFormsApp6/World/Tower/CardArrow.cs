using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.World.Entities;

namespace WindowsFormsApp6.World.Tower
{
    public class CardArrow : CardBase
    {
        public int LVL, Timer = 0;
        public CardArrow(int x, int y, int lvl = 1) : base(x, y)
        {
            LVL = lvl;
            Image = Resources.card_arrow.Transparent().Resized(Core.TileSz);
            int dx = Image.Width / 2, dy = Image.Height / 2;
            using (Graphics g = Graphics.FromImage(Image))
            {
                SizeF szf = g.MeasureString(LVL.ToString(), Control.DefaultFont);
                g.DrawString(LVL.ToString(), Control.DefaultFont, Brushes.Black, dx - szf.Width / 2, dy - szf.Height / 2);
            }
        }

        public override void Update(StateTower.TowerStats stats, Func<int, int, TowerUnit> GetClosestTarget, Action<Bullet> AddBullet)
        {
            if(Timer >= 100)
            {
                var x = (5 + X) * Core.TileSz + Core.TileSz / 2;
                var y = (7 + Y) * Core.TileSz + Core.TileSz / 2;
                var target = GetClosestTarget(x, y);
                if (target != null)
                    AddBullet(new Bullet(x, y, target, dmg: 1, spd: 1));
                Timer = 0;
            }
            else
            {
                Timer += stats.SPD;
            }
        }
    }
}
