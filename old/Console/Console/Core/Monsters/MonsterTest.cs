using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Monsters
{
    public class MonsterTest : Monster
    {
        public MonsterTest(short indexPosition) : base("Test", indexPosition, 10, 0, 0, 0, 1, 0)
        {
            HasSpecial = false;
            HasRecover = false;

            // ─│┌┐└┘├┤┬┴┼
            Paint = "";
            Paint += @"
┌┬──┬─┐
├┼┐ ├─┘
└┴┴─┘  ";
        }
    }
}
