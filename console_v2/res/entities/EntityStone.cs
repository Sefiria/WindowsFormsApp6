using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2.res.entities
{
    internal class EntityResource : Entity
    {
        public int DBRef;
        public Outils NeededTool;
        public Dictionary<int, int> Results;
        public Dictionary<int, int> RndResults;
        public EntityResource() : base() { }
        public EntityResource(vec tile, int dbref, Outils neededTool, Dictionary<int, int> results = null, bool addToCurrentChunkEntities = true, Dictionary<int, int> rndResults = null) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            DBRef = dbref;
            NeededTool = neededTool;
            Results = results;
            RndResults = rndResults;
            (CharToDisplay, DBResSpe) = DB.RetrieveDBResOrSpe(dbref);
            Stats = new Statistics(new Dictionary<Statistics.Stat, int> { [Statistics.Stat.HPMax] = 20, [Statistics.Stat.HP] = 20 });
        }

        public override void Update()
        {
        }
        public override void TickSecond()
        {
        }
        public override void Action(Entity triggerer)
        {
            if (!Exists) return;

            var scythes = triggerer.Inventory.Tools.Where(tool => tool.DBRef == NeededTool);
            if (scythes.Count() == 0)
                return;
            var str = scythes.Max(scythe => scythe.STR);
            Stats._Substract(Statistics.Stat.HP, str);
            if (Stats._Get(Statistics.Stat.HP) <= 0)
            {
                if(Results != null)
                    foreach(var result in Results)
                        triggerer.Inventory.Add((result.Key, result.Value));
                int count;
                if(RndResults != null)
                    foreach (var rndResult in RndResults)
                        if ((count = RandomThings.rnd(rndResult.Value)) > 0)
                        triggerer.Inventory.Add((rndResult.Key, count));
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
