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
            switch (kind)
            {
                default:
                case "Pomme": return new Plant<Pomme>(v);
                case "Tomate": return new Plant<Tomate>(v);
                case "Concombre": return new Plant<Concombre>(v);
            };
        }
    }
}
