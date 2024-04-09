using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3.res.entities
{
    internal class EntityPlant : Entity
    {
        public EntityPlant() : base() { }
        public EntityPlant(vec tile, int plant, bool addToCurrentChunkEntities = true) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            DBRef = plant;
            Stats = new Statistics(new Dictionary<Statistics.Stat, int> { [Statistics.Stat.HPMax] = 3, [Statistics.Stat.HP] = 3 });
        }

        public override void Update()
        {
        }
        public override void TickSecond()
        {
        }
        public override void Draw(Graphics g)
        {
            GraphicsManager.Draw(g, DB.GetTexture(DBRef), Position);
        }
        public override void Action(Entity triggerer)
        {
            if (!Exists) return;

            var scythes = triggerer.Inventory.Tools.Where(tool => tool.DBRef.IsScythe());
            if (scythes.Count() == 0)
                return;
            var str = scythes.Max(scythe => scythe.STR);
            Stats._Substract(Statistics.Stat.HP, str);
            if (Stats._Get(Statistics.Stat.HP) <= 0)
            {
                triggerer.Inventory.AddItem((DBRef + 16, 1));
                Exists = false;
                ParticlesManager.Generate(Position + GraphicsManager.TileSize / 2 / 2f, 3f, 4f, Color.White, 3, 100);
            }
            else
            {
                ParticlesManager.Generate(Position + GraphicsManager.TileSize / 2 / 2f, 1f, 2f, Color.White, str, 100);
            }
        }
    }
}
