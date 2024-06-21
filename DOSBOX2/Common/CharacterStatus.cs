using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX2.Common
{
    internal class CharacterStatus
    {
        public enum Init
        {
            Bad, Mid, Good, Excellent
        }
        public byte HP = 3;
        public byte HGR = 5;
        public byte STR = 1;
        public CharacterStatus(Init init = Init.Mid)
        {
            switch(init)
            {
                case Init.Bad:
                    HP = 1;
                    HGR = 3;
                    STR = 1;
                    break;
                case Init.Mid:
                    HP = 3;
                    HGR = 5;
                    STR = 1;
                    break;
                case Init.Good:
                    HP = 5;
                    HGR = 10;
                    STR = 3;
                    break;
                case Init.Excellent:
                    HP = 10;
                    HGR = 20;
                    STR = 5;
                    break;
            }
        }
    }
}
