using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace DOSBOX2.Common
{
    internal class CharacterControls
    {
        public KB.Key Left = KB.Key.Q;
        public KB.Key Right = KB.Key.D;
        public KB.Key Jump = KB.Key.Space;
        public KB.Key Crouch = KB.Key.LeftCtrl;
        public KB.Key Shot = KB.Key.Numpad0;
        public KB.Key Action = KB.Key.LeftAlt;
        public KB.Key LookUp = KB.Key.Up;
        public CharacterControls()
        {
        }
    }
}
