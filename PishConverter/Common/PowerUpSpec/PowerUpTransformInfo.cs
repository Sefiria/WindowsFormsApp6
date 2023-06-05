using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PishConverter.Common
{
    internal class PowerUpTransformInfo
    {
        public List<Vector2> ShotPorts;

        public PowerUpTransformInfo(List<Vector2> ShotPorts = null)
        {
            this.ShotPorts = ShotPorts;
        }
    }
}
