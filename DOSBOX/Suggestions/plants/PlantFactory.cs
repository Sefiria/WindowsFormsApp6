using DOSBOX.Suggestions.plants.Fruits;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions
{
    public class PlantFactory
    {
        public static ClassIPlant CreateRandom(vecf v)
        {
            switch (Core.RND.Next(3))
            {
                default:
                case 0: return new Plant<Pomme>(v);
                case 1: return new Plant<Tomate>(v);
                case 2: return new Plant<Concombre>(v);
            };
        }
        public static ClassIPlant Create(string kind, vecf v)
        {
            string name = kind.Split('_')[0];

            switch (name)
            {
                default:
                case "Pomme": return new Plant<Pomme>(v);
                case "Tomate": return new Plant<Tomate>(v);
                case "Concombre": return new Plant<Concombre>(v);
                case "OGM": return CreateOGM(kind, v);
            };
        }

        public static ClassIPlant CreateOGM(string kind, vecf v)
        {
            var plant = new Plant<OGM>(v);
            string dna = kind.Split('_')[1];
            string[] mutators = dna.Split(',');
            plant.ForceMaxBranches = sbyte.Parse(mutators[0]);
            plant.ForceMaxLeaves = sbyte.Parse(mutators[1]);
            plant.ForceMaxFruits = sbyte.Parse(mutators[2]);
            plant.DNA = dna;
            return plant;
        }
    }
}
