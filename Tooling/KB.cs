using System.Collections.Generic;
using System.Windows.Input;

namespace Tooling
{
    public class KB
    {
        public enum Key
        {
            Escape = 0,
            Space,
            Enter,
            Up,
            Down,
            Left,
            Right,
            Z,Q,S,D,
            A, E, R, C,
            Num1, Num2,Num3,Num4,Num5,Num6,Num7,Num8,Num9, Num0, CharDegree, CharPlus,
            LeftAlt, RightAlt, LeftShift, RightShift, LeftCtrl, RightCtrl,
            Tab, Supr,
        }

        static Dictionary<System.Windows.Input.Key, bool> Released = new Dictionary<System.Windows.Input.Key, bool>()
        {
            [System.Windows.Input.Key.Escape] = true,
            [System.Windows.Input.Key.Space] = true,
            [System.Windows.Input.Key.Enter] = true,
            [System.Windows.Input.Key.Up] = true,
            [System.Windows.Input.Key.Down] = true,
            [System.Windows.Input.Key.Left] = true,
            [System.Windows.Input.Key.Right] = true,
            [System.Windows.Input.Key.Z] = true,
            [System.Windows.Input.Key.Q] = true,
            [System.Windows.Input.Key.S] = true,
            [System.Windows.Input.Key.D] = true,
            [System.Windows.Input.Key.A] = true,
            [System.Windows.Input.Key.E] = true,
            [System.Windows.Input.Key.R] = true,
            [System.Windows.Input.Key.C] = true,
            [System.Windows.Input.Key.D1] = true,
            [System.Windows.Input.Key.D2] = true,
            [System.Windows.Input.Key.D3] = true,
            [System.Windows.Input.Key.D4] = true,
            [System.Windows.Input.Key.D5] = true,
            [System.Windows.Input.Key.D6] = true,
            [System.Windows.Input.Key.D7] = true,
            [System.Windows.Input.Key.D8] = true,
            [System.Windows.Input.Key.D9] = true,
            [System.Windows.Input.Key.D0] = true,
            [System.Windows.Input.Key.Oem4] = true,
            [System.Windows.Input.Key.OemPlus] = true,
            [System.Windows.Input.Key.LeftAlt] = true,
            [System.Windows.Input.Key.RightAlt] = true,
            [System.Windows.Input.Key.LeftShift] = true,
            [System.Windows.Input.Key.LeftCtrl] = true,
            [System.Windows.Input.Key.RightCtrl] = true,
            [System.Windows.Input.Key.Tab] = true,
            [System.Windows.Input.Key.Delete] = true,
        };

        static readonly List<System.Windows.Input.Key> AvailableKeys = new List<System.Windows.Input.Key>()
        {
            System.Windows.Input.Key.Escape,
            System.Windows.Input.Key.Space,
            System.Windows.Input.Key.Enter,
            System.Windows.Input.Key.Up,
            System.Windows.Input.Key.Down,
            System.Windows.Input.Key.Left,
            System.Windows.Input.Key.Right,
            System.Windows.Input.Key.Z,
            System.Windows.Input.Key.Q,
            System.Windows.Input.Key.S,
            System.Windows.Input.Key.D,
            System.Windows.Input.Key.A,
            System.Windows.Input.Key.E,
            System.Windows.Input.Key.R,
            System.Windows.Input.Key.C,
            System.Windows.Input.Key.D1,
            System.Windows.Input.Key.D2,
            System.Windows.Input.Key.D3,
            System.Windows.Input.Key.D4,
            System.Windows.Input.Key.D5,
            System.Windows.Input.Key.D6,
            System.Windows.Input.Key.D7,
            System.Windows.Input.Key.D8,
            System.Windows.Input.Key.D9,
            System.Windows.Input.Key.D0,
            System.Windows.Input.Key.Oem4,
            System.Windows.Input.Key.OemPlus,
            System.Windows.Input.Key.LeftAlt,
            System.Windows.Input.Key.RightAlt,
            System.Windows.Input.Key.LeftShift,
            System.Windows.Input.Key.RightShift,
            System.Windows.Input.Key.LeftCtrl,
            System.Windows.Input.Key.RightCtrl,
            System.Windows.Input.Key.Tab,
            System.Windows.Input.Key.Delete,
        };

        public static void Init() => Update();
        public static void Update() => AvailableKeys.ForEach(k => Released[k] = !Keyboard.IsKeyDown(k));
        public static bool IsKeyDown(Key k) => Keyboard.IsKeyDown(AvailableKeys[(int)k]);
        public static bool IsKeyPressed(Key k) => Keyboard.IsKeyDown(AvailableKeys[(int)k]) && Released[AvailableKeys[(int)k]];
        public static bool IsKeyUp(Key k) => !IsKeyDown(k);
        public static bool LeftShift => Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift);
    }
}
