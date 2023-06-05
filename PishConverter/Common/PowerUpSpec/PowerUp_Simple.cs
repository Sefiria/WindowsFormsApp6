using PishConverter.Common.MapSpec;
using PishConverter.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PishConverter.Common
{
    internal class PowerUpSimple : PowerUp
    {
        public static PowerUpTransformInfo Info = new PowerUpTransformInfo
        {
            ShotPorts =  new List<Vector2>()
            {
                new Vector2(0, -H / 2F - 2F),
            }
        };
        public PowerUpSimple(Vector2 position, Vector2 look) : base(Info, position, look)
        {
        }
    }
}
