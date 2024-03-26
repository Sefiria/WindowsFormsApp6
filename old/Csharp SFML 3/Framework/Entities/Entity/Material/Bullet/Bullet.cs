using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entities._Entity._Material.Bullet
{
    public class Bullet : Material
    {
        public new static string GetEntityPath()
        {
            return Material.GetEntityPath() + "/Bullet";
        }
    }
}
