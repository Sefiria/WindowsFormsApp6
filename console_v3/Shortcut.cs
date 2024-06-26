﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3
{
    public class Shortcut
    {
        public int Index;
        public KB.Key Key;
        public IUniqueRef Ref;
        public Shortcut()
        {
        }
        public Shortcut(int index, KB.Key key, IUniqueRef @ref)
        {
            Index = index;
            Key = key;
            Ref = @ref;
        }
        public bool IsPressed() => KB.IsKeyPressed(Key);
        public bool IsDown() => KB.IsKeyDown(Key);
    }
}
