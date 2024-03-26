using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._Entity._Equipment
{
    class Craftable : Equipment
    {
        public new static string GetEntityPath()
        {
            return Equipment.GetEntityPath() + "/Craftable";
        }
    }
}
