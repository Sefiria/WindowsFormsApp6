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
        public EntityTree() : base() { }
        public EntityTree(vec tile, bool addToCurrentChunkEntities = true) : base(tile.ToWorld(), addToCurrentChunkEntities)
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
                new Lootable(Position.ToTile(), true, new Item("Buche", Objets.Buche, 1) { IsConsommable = false });
                Exists = false;
                ParticlesManager.Generate(Position + GraphicsManager.CharSize.Vf() / 2f, 5f, 10f, Color.White, 10, 100);
            }
            else
            {
                ParticlesManager.Generate(Position + GraphicsManager.CharSize.Vf() / 2f, 2f, 3f, Color.White, 3, 100);
            }
        }
    }
}
