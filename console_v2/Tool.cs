using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tooling;

namespace console_v2
{
    public class Tool : IName, IDBItem
    {
        public string Name { get; set; } = "Unnamed_Tool";
        public Outils DBRef;
        public float Duration;
        public int Count;
        public int STR;

        public int DBItem => (int)DBRef;

        public Tool()
        {
        }
        public Tool(Tool copy)
        {
            Name = copy.Name;
            Duration = copy.Duration;
            DBRef = copy.DBRef;
            Count = copy.Count;
        }
        public Tool(string name, Outils dbref, int STR = 1)
        {
            Name = name;
            Duration = 1f;
            DBRef = dbref;
            this.STR = STR;
            Count = 1;
        }

        public void Use(Entity triggerer)
        {
            switch(DBRef)
            {
                case Outils.Pelle: UseShovel(triggerer); break;
            }
        }

        private void UseShovel(Entity triggerer)
        {
            // TODO -> TO TEST
            var tile = triggerer.Position.ToTile();
            var chunk = Core.Instance.SceneAdventure.World.GetCurrentDimensionChunk(tile).Value;
            if(chunk.Tiles[tile].Sol == Sols.Herbe)
            {
                triggerer.Inventory.Add(new Item("Fibre De Plante", Objets.FibreDePlante, RandomThings.rnd(5)));
                chunk.Tiles[tile].Sol = Sols.Terre;
            }
            else if (chunk.Tiles[tile].Sol == Sols.Terre)
            {
                triggerer.Inventory.Add(new Item("Boue", Objets.Boue));
                chunk.Tiles[tile].Sol = Sols.Pierre;
            }
        }
    }
}
