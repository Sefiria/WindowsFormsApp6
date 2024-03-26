using _Console.Core.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Skills
{
    public class ItemEffects
    {
        public static bool UseItem(Attacker source, Attacker target, Item item)
        {
            if (item == null)
                return false;

            MethodInfo method = typeof(ItemEffects).GetMethod(item.Name);

            if (method == null)
                return false;

            return (bool) method.Invoke(null, new[] { source, target });
        }

        public static bool DEBUG(Attacker source, Attacker target)
        {
            return true;
        }
    }
}
