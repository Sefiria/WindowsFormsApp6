using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp21.Entities
{
    public interface ICollision
    {
        CollisionBase.CollisionType Type { get; set; }
    }
}