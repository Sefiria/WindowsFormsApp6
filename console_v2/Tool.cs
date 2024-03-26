using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v2
{
    internal class Tool
    {
        public string Name = "Unnamed_Tool";
        public float Duration;
        public int Count;
        public Tool()
        {
        }
        public Tool(string name)
        {
            Name = name;
            Duration = 1f;
            Count = 1;
        }
    }
}
