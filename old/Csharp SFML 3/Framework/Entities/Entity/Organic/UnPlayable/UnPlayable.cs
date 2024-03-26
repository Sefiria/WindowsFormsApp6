using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._Entity._Organic
{
    public class UnPlayable : Organic
    {
        public new static string GetEntityPath()
        {
            return Organic.GetEntityPath() + "/UnPlayable";
        }
    }
}
