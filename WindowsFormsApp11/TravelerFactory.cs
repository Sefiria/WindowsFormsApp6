using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp11.Travelers;

namespace WindowsFormsApp11
{
    public class TravelerFactory
    {
        public static Traveler CreateTiny()
        {
            return new TravelerTiny();
        }
    }
}
