using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp21.Entities
{
    public class Entity
    {
        public bool IsDrawable = false;

        public Guid Id;
        public string Name;
        public PointF Pos;
        public PointF Look;

        public PointF DrawPoint => Core.CenterPoint.PlusF(Pos);

        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
