using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public class Inputs
    {
        [System.Runtime.InteropServices.DllImport("USER32.dll")]
        static private extern short GetKeyState(int nVirtKey);

        public enum Key
        {
            Z = 90,
            Q = 83,
            S = 81,
            D = 68,
            LAlt = 164
        }

        static public Dictionary<Key, bool> IsDown = new Dictionary<Key, bool>();
        static public bool KeyDown(Key key) => IsDown[key];

        static public void Initialize()
        {
            string[] list = Enum.GetNames(typeof(Key));
            foreach (string key in list)
                IsDown[(Key)Enum.Parse(typeof(Key), key)] = false;
        }
        static public void Update()
        {
            Key[] keys = IsDown.Keys.ToArray();
            foreach (Key key in keys)
                IsDown[key] = GetKeyState((int)key) < 0;
        }
    }
}
