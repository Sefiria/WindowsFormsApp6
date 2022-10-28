using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2.Interfaces
{
    public interface IDamager
    {
        MaterialQuality Material { get; set; }
        int Damage { get; }
    }
}
