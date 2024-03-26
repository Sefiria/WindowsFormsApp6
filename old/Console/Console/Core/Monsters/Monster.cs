using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Monsters
{
    public class Monster : Attacker
    {
        public int RenderPosX = 0;

        public Monster(string Name, int RenderPosX, int HP, int MP, int SP, int DEF, int STR, int INT, bool HasSpecial = false, bool HasRecover = false)
            : base(Name, HP, MP, SP, DEF, STR, INT, HasSpecial, HasRecover)
        {
            this.RenderPosX = RenderPosX;
        }
    }
}
