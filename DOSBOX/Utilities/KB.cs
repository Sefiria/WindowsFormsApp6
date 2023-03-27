﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DOSBOX.Utilities
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
            Z,Q,S,D
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
        };

        public static void Init() => Update();
        public static void Update() => AvailableKeys.ForEach(k => Released[k] = !Keyboard.IsKeyDown(k));
        public static bool IsKeyDown(Key k) => Keyboard.IsKeyDown(AvailableKeys[(int)k]);
        public static bool IsKeyPressed(Key k) => Keyboard.IsKeyDown(AvailableKeys[(int)k]) && Released[AvailableKeys[(int)k]];
        public static bool IsKeyUp(Key k) => !IsKeyDown(k);
    }
}