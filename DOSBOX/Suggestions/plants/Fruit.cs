using DOSBOX.Utilities;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class Fruit : Dispf
    {
        public bool IsHover(vec v) => IsHover(v.f);
        public bool IsHover(vecf v) => v.x >= vec.x && v.y >= vec.y && v.x < vec.x + _w && v.y < vec.y + _h;
        public Fruit(vecf vec)
        {
            this.vec = new vecf(vec);
        }
    }
}
