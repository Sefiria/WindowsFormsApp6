using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2.res.entities
{
    internal class EntityTree : Entity
    {
        public EntityTree(vec tile) : base(tile.ToWorld())
        {
            CharToDisplay = 'ῼ';
            Stats = new Statistics(new Dictionary<Statistics.Stat, int> { [Statistics.Stat.HPMax] = 10, [Statistics.Stat.HP] = 10 });
        }

        public override void Update()
        {
        }
        public override void TickSecond()
        {
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }
        public override void Action(Entity triggerer)
        {
            if (!Exists) return;

            var axes = triggerer.Inventory.Tools.Where(tool => tool.DBRef == Outils.Hache);

            if (axes.Count() == 0)
                return;

            var str = axes.Max(tool => tool.STR);
            Stats._Substract(Statistics.Stat.HP, str);
            if (Stats._Get(Statistics.Stat.HP) <= 0)
            {
                new Lootable(Position.i, true, new Item("Buche", Objets.Buche, 1));
                Exists = false;
            }
        }
    }
}
