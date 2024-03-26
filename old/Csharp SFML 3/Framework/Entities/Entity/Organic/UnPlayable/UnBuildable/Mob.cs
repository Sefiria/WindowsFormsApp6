using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._Entity._Organic._UnPlayable._UnBuildable
{
    public class Mob : UnBuildable
    {
        public new static string GetEntityPath()
        {
            return UnBuildable.GetEntityPath() + "/Mob";
        }
    }
}
