using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp15.utilities.tiles;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15.structure
{
    internal class StructureConveyor : Structure, IMoveStructure
    {
        public float Speed { get; set; } = 0.7F;
        private Way m_Way = (Way)2;
        public Way Way
        {
            get => m_Way;
            set
            {
                m_Way = value;
                anim = conveyors[Way];
            }
        }

        public StructureConveyor() : base(vecf.Zero)
        {
            Way = 0;
        }
        public StructureConveyor(vecf vec, int way) : base(vec)
        {
            Way = (Way)way;
        }
        public StructureConveyor(vecf vec, Way way) : base(vec)
        {
            Way = way;
        }
    }
}
