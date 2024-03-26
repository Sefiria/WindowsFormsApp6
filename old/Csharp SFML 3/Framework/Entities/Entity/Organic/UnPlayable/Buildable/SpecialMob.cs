using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._Entity._Organic._UnPlayable._Buildable
{
    public class SpecialMob : Buildable
    {
        public new static string GetEntityPath()
        {
            return Buildable.GetEntityPath() + "/SpecialMob";
        }
    }
}
