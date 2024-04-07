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
    public class Tool : ITool, IName, IDBItem, IUniqueRef
    {
        public Guid UniqueId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Unnamed_Tool";
        public int DBRef { get; set; }
        public float Duration;
        public int Count;
        public int STR;

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
            DBRef = (int)dbref;
            this.STR = STR;
            Count = 1;
        }

        public void Use(Entity triggerer)
        {
            switch((Outils)DBRef)
            {
                case Outils.Pelle: UseShovel(triggerer); break;
            }
        }

        private void UseShovel(Entity triggerer)
        {
            var tile = triggerer.Position.ToTile();
            var chunk = Core.Instance.SceneAdventure.World.GetChunk();
            if(chunk.Tiles[tile].Sol == Sols.Herbe)
            {
                var n = RandomThings.rnd(3);
                if (n > 0)
                    triggerer.Inventory.Add(new Item("Fibre De Plante", (int)Objets.FibreDePlante, n));
                chunk.Tiles[tile].Sol = Sols.Terre;
            }
            else if (chunk.Tiles[tile].Sol == Sols.Terre)
            {
                var n = RandomThings.rnd(2);
                if (n > 0)
                    triggerer.Inventory.Add(new Item("Boue", (int)Objets.Boue, n));
                chunk.Tiles[tile].Sol = Sols.Pierre;
            }
        }
    }
}
