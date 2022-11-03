using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6.World.Blocs.Consommables
{
    public interface IReGrowable
    {
        int Timer { get; set; }
        void Tick();
    }
}
