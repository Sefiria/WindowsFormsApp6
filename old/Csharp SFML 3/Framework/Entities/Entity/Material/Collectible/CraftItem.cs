using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._Entity._Material._Collectible
{
    public class CraftItem : Collectible
    {
        public new static string GetEntityPath()
        {
            return Collectible.GetEntityPath() + "/CraftItem";
        }
    }
}
