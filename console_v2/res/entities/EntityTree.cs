using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tooling;

namespace console_v2.res.entities
{
    internal class EntityTree : Entity
    {
        public EntityTree() : base() { }
        public EntityTree(vec tile, bool addToCurrentChunkEntities = true) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Name = "Arbre";
            CharToDisplay = 'ῼ';
            Stats = new Statistics(new Dictionary<Statistics.Stat, int> { [Statistics.Stat.HPMax] = 30, [Statistics.Stat.HP] = 30 });
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

            var axes = triggerer.Inventory.Tools.Where(tool => tool.DBRef == (int)Outils.Hache);
            var str = axes.Count() == 0 ? 1 : axes.Max(axe => axe.STR);
            Stats._Substract(Statistics.Stat.HP, str);
            if (Stats._Get(Statistics.Stat.HP) <= 0)
            {
                new Lootable(Position.ToTile(), true, new Item("Buche", (int)Objets.Buche, 1) { IsConsommable = false });
                Exists = false;
                ParticlesManager.Generate(Position + GraphicsManager.CharSize.Vf() / 2f, 5f, 10f, Color.White, 10, 100);
            }
            else
            {
                ParticlesManager.Generate(Position + GraphicsManager.CharSize.Vf() / 2f, 2f, 3f, Color.White, str, 100);
            }
        }
    }
}
