using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public static class KeysReleased
    {
        public static Dictionary<Keys, bool> Dict = new Dictionary<Keys, bool>()
        {
            [Keys.Enter] = true,
        };
        public static bool IsReleased(Keys key)=> Dict.ContainsKey(key) ? Dict[key] : Dict[key] = true;
    }
}
