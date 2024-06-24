using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using DOSBOX2.Common;

namespace DOSBOX2.GameObjects
{
    internal class BaseBullet : Entity
    {
        public BaseBullet(int diameter, vecf look, byte faction = 0) : base(faction, CharacterStatus.Init.Mid, new Circle(0F, 0F, diameter / 2F))
        {
            Look = look;
        }
    }
}
