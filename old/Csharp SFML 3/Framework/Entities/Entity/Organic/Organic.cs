using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities
{
    public class Organic : Entity
    {
        public int HP = 1;
        public bool indestrucible = false;

        public new static string GetEntityPath()
        {
            return Entity.GetEntityPath() + "/Organic";
        }
    }
}
