﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions
{
    public interface ISuggestion
    {
        bool ShowHowToPlay { get; set; }

        void Init();
        void Update();
        void HowToPlay();
    }
}
