using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.plants
{
    public class Data
    {
        public static Dictionary<string, int> Fruits;
        public static Dictionary<string, int> Seeds;
        public static string SelectedSeed = "";

        public static void Init()
        {
            Fruits = new Dictionary<string, int>();
            Seeds = new Dictionary<string, int>();

            //debug
            Fruits["Pomme"] = 1;
            Fruits["Tomate"] = 10;
            Fruits["Concombre"] = 100;
            Seeds["Pomme"] = 10;
            Seeds["Tomate"] = 10;
            Seeds["Concombre"] = 10;
        }

        public static int SelectedSeedCount => Seeds.ContainsKey(SelectedSeed) ? Seeds[SelectedSeed] : 0;
        public static IPlant DropSeed(vecf v)
        {
            if (SelectedSeed == "")
                return null;
            Seeds[SelectedSeed]--;
            if (Seeds[SelectedSeed] == 0)
            {
                Seeds.Remove(SelectedSeed);
                SelectedSeed = "";
            }
            return PlantFactory.Create(SelectedSeed, v);
        }
    }
}
