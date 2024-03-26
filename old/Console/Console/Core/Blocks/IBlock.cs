using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Blocks
{
    public interface IBlock
    {
        HelpBlock.Blocks EnumValue { get; }
        char RenderChar { get; }
        ConsoleColor RenderColor { get; }
        Dictionary<HelpBlock.Blocks, int> Needs { get; }

        void Action(ref Data data);
    }
}
