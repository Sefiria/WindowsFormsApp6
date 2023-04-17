using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetMake.Utilities;

namespace AssetMake.utilities
{
    public class Collider
    {
        public vecf vec = vecf.Zero;

        public float x { get => vec.x; set => vec.x = value; }
        public float y { get => vec.y; set => vec.y = value; }

        public virtual vecf size { get; } = new vecf(1F, 1F);
    }
}
