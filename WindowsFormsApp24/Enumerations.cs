using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp24
{
    internal class Enumerations
    {
        public enum SceneStates
        {
            TitleScreen = 0,
            Menu,
            Map
        }
        public enum InputNames
        {
            Up=0,Left,Down,Right,
            Primary,Secondary,
            Ok,Cancel,
            Menu
        }
    }
}
