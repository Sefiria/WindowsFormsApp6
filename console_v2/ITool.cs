using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v2
{
    public interface ITool
    {
        void Use(Entity triggerer);
    }
}
