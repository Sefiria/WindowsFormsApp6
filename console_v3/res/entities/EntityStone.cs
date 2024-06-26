﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3.res.entities
{
    internal class EntityResource : Entity
    {
        public int NeededTool;
        public Dictionary<int, int> Results;
        public Dictionary<int, int> RndResults;
        public EntityResource() : base() { }
        public EntityResource(vec tile, int dbref, int neededTool, Dictionary<int, int> results = null, bool addToCurrentChunkEntities = true, Dictionary<int, int> rndResults = null) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            DBRef = dbref;
            Name = DB.DefineName(dbref);
            NeededTool = neededTool;
            Results = results;
            RndResults = rndResults;
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

            var hammers = triggerer.Inventory.Tools.Where(tool => tool.DBRef == NeededTool);
            if (hammers.Count() == 0)
                return;
            var str = hammers.Max(hammer => hammer.STR);
            Stats._Substract(Statistics.Stat.HP, str);
            if (Stats._Get(Statistics.Stat.HP) <= 0)
            {
                if(Results != null)
                    foreach(var result in Results)
                        triggerer.Inventory.AddItem(((result.Key, result.Value)));
                int count;
                if(RndResults != null)
                    foreach (var rndResult in RndResults)
                        if ((count = RandomThings.rnd(rndResult.Value)) > 0)
                        triggerer.Inventory.AddItem((rndResult.Key, count));
                Exists = false;
                ParticlesManager.Generate(Position + GraphicsManager.TileSize / 2f, 3f, 4f, Color.White, 3, 100);
            }
            else
            {
                ParticlesManager.Generate(Position + GraphicsManager.TileSize / 2f, 1f, 2f, Color.White, str, 100);
            }
        }
    }
}
