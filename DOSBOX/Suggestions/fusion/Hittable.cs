using DOSBOX.Suggestions.fusion.jsondata;
using DOSBOX.Suggestions.fusion.Triggerables;
using DOSBOX.Utilities;

namespace DOSBOX.Suggestions.fusion
{
    public class Hittable : PhysicalObject
    {
        public Hittable()
        {
        }
        public virtual void Hit(Harmful by) {}
    }
}
