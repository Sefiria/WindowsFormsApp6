using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp17
{
    internal class enums
    {
        public enum TileName
        {
            Air = 0,
            Grass,
            DirtyGrass,
            Dirt,
            DirtyStone,
            Stone,
        }
        //public enum Ores
        //{
        //    Iron = 0, Copper, Coal, Gold, Diamond
        //}
        //public static Ores GetRandomOre() => (Ores)Core.RND.Next(Enum.GetNames(typeof(Ores)).Length);
    }
}
