using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2.res.entities
{
    internal class EntityPlant : Entity
    {
        public int DBPlant;
        public EntityPlant() : base() { }
        public EntityPlant(vec tile, Plantes plant, bool addToCurrentChunkEntities = true) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            DBPlant = (int)plant;
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
            g.DrawImage(DB.ResourcesSpecials[DBPlant], SceneAdventure.DrawingRect.Location.Plus(Position.pt));
            var ui = new Bitmap(200, 200);
            using (var u = Graphics.FromImage(ui))
            {
                u.Clear(Color.Black);
                u.DrawImage(DB.ResourcesSpecials[DBPlant], Point.Empty);
            }
        }
        public override void Action(Entity triggerer)
        {
            if (!Exists) return;

            var scythes = triggerer.Inventory.Tools.Where(tool => tool.DBRef == Outils.Faux);
            if (scythes.Count() == 0)
                return;
            var str = scythes.Max(scythe => scythe.STR);
            Stats._Substract(Statistics.Stat.HP, str);
            if (Stats._Get(Statistics.Stat.HP) <= 0)
            {
                triggerer.Inventory.Add((int)Objets.EssenceViolys - (int)Plantes.Violys + DBPlant);
                Exists = false;
                ParticlesManager.Generate(Position + GraphicsManager.CharSize.Vf() / 2f, 3f, 4f, Color.White, 3, 100);
            }
            else
            {
                ParticlesManager.Generate(Position + GraphicsManager.CharSize.Vf() / 2f, 1f, 2f, Color.White, str, 100);
            }
        }
    }
}
