using Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Sign
    {
        public int Index;
        public byte[] ImageBytes { get; set; }
        public InputScript script;

        public Sign()
        {
            Index = 0;
            ImageBytes = null;
        }
    }
}
