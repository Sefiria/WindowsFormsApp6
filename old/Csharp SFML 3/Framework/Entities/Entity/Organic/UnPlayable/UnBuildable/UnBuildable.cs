using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._Entity._Organic._UnPlayable
{
    public class UnBuildable : UnPlayable
    {
        public new static string GetEntityPath()
        {
            return UnPlayable.GetEntityPath() + "/UnBuildable";
        }
    }
}
