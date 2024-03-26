using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities
{
    [Serializable]
    public class Material : Entity
    {
        public Material()
        {

        }

        public new static string GetEntityPath()
        {
            return Entity.GetEntityPath() + "/Material";
        }
    }
}
