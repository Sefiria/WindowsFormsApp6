using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions
{
    public interface IState
    {
        void Init();
        void Update();
    }
}
