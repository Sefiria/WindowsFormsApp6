using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities
{
    class Equipment : Entity
    {
        public new static string GetEntityPath()
        {
            return Entity.GetEntityPath() + "/Equipment";
        }
    }
}
