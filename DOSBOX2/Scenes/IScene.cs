﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX2.Scenes
{
    public interface IScene
    {
        void Init();
        void Update();
        void Dispose();
    }
}