using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX2.Common
{
    internal class EventHandlers
    {
        public delegate void EmptyHandler();
        public delegate void CollisionHandler(Entity collider);
        //public delegate void UpdateHandler();
        //public delegate void DrawHandler(Graphics g);
    }
}
