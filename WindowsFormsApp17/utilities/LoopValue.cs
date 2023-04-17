﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp17.utilities
{
    internal class LoopValue
    {
        public int min, max;

        int m_value = default;
        public int Value
        {
            get => m_value;
            set
            {
                m_value = value;
                while (m_value < min) m_value += max;
                while (m_value >= max) m_value -= max;
            }
        }
        public LoopValue()
        {
        }
        public LoopValue(int v, int min, int max)
        {
            Value = v;
            this.min = min;
            this.max = max;
        }
    }

    internal class LoopValueF
    {
        public float min, max;

        float m_value = default;
        public float Value
        {
            get => m_value;
            set
            {
                m_value = value;
                while (m_value < min) m_value += max;
                while (m_value >= max) m_value -= max;
            }
        }
        public LoopValueF()
        {
        }
        public LoopValueF(float v, float min, float max)
        {
            Value = v;
            this.min = min;
            this.max = max;
        }
    }
}
