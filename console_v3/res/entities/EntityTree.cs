using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tooling;

namespace console_v3.res.entities
{
    internal class EntityTree : Entity
    {
        public EntityTree() : base() { }
        public EntityTree(vec tile, bool addToCurrentChunkEntities = true) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            Name = "Arbre";
            DBRef = RandomThings.arnd((int)DB.Tex.Tree_Spring_A, (int)DB.Tex.Tree_Spring_C);
            Stats = new Statistics(new Dictionary<Statistics.Stat, int> { [Statistics.Stat.HPMax] = 1, [Statistics.Stat.HP] = 1 });
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

            var axes = triggerer.Inventory.Tools.Where(tool => tool.DBRef.IsAxe());
            var str = axes.Count() == 0 ? 1 : axes.Max(axe => axe.STR);
            Stats._Substract(Statistics.Stat.HP, str);
            if (Stats._Get(Statistics.Stat.HP) <= 0)
            {
                new Lootable(Position.ToTile(), true, new Item("Buche", (int)DB.Tex.WoodLog, 1) { IsConsommable = false });
                Exists = false;
                ParticlesManager.Generate(Position + GraphicsManager.TileSize / 2f, 10f, 10f, Color.FromArgb(100, 50, 0), 10, 100);
            }
            else
            {
                ParticlesManager.Generate(Position + GraphicsManager.TileSize / 2f, 5f, 3f, Color.FromArgb(100, 50, 0), str, 100);
            }
        }
    }
}
