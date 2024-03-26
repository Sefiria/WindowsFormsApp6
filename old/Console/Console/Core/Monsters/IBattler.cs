using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Monsters
{
    public interface IBattler
    {
        string Name { get; set; }
        int MaxHP { get; set; }
        int MaxMP { get; set; }
        int MaxSP { get; set; }
        int HP { get; set; } // Health Points
        int MP { get; set; } // Magical Points
        int SP { get; set; } // Special Points (auto charge)
        int DEF { get; set; } // Defense
        int STR { get; set; } // Strong (physical)
        int INT { get; set; } // Intelligence : Strong (magic)
        bool HasSpecial { get; set; }
        bool HasRecover { get; set; }
    }
}
