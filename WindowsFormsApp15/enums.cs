using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp15
{
    internal class enums
    {
        public enum Way
        {
            Down = 0, Left, Up, Right
        }
        public enum Ores
        {
            Iron = 0, Copper, Coal, Gold, Diamond
        }
        public enum Items
        {
            Ore = 0, Plate,
        }
        public static Ores GetRandomOre() => (Ores)Core.RND.Next(Enum.GetNames(typeof(Ores)).Length);
    }
}
