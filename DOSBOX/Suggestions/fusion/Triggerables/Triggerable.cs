using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.fusion.Triggerables
{
    public interface ITriggerable
    {
        void Trigger(Dispf by);
    }
}
