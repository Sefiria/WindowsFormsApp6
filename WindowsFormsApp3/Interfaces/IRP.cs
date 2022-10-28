using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp3.Entities;

namespace WindowsFormsApp3.Interfaces
{
    public interface IRP
    {
        int STR { get; set; }
        int STRMax { get; set; }
        int HP { get; set; }
        int HPMax { get; set; }

        void Hit(IRP From);
    }
}
