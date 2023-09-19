using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishConverter.Common
{
    internal class PowerUpTransformInfo : StatsInfo
    {
        public void Transform(ref StatsInfo statsInfo)
        {
            statsInfo.HP += HP;
            statsInfo.MoveSpeed += MoveSpeed;
        }
    }
}
