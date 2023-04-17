using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMake.utilities
{
    internal class RangeValue
    {
        public int min, max;
        public bool max_included;

        int m_value = default;
        public int Value
        {
            get => m_value;
            set
            {
                m_value = value;
                if (m_value < min) m_value = min;
                if(max_included)
                    if (m_value > max) m_value = max;
                else
                    if (m_value >= max) m_value = max - 1;
            }
        }
        public RangeValue()
        {
        }
        public RangeValue(int v, int min, int max, bool max_included = true)
        {
            Value = v;
            this.min = min;
            this.max = max;
            if(this.max < this.min) this.max = max_included ? this.min : this.min + 1;
            this.max_included = max_included;
        }
    }

    internal class RangeValueF
    {
        public float min, max;
        public bool max_included;

        float m_value = default;
        public float Value
        {
            get => m_value;
            set
            {
                m_value = value;
                if (max_included)
                    if (m_value > max) m_value = max;
                    else
                    if (m_value >= max) m_value = max - 1;
            }
        }
        public RangeValueF()
        {
        }
        public RangeValueF(float v, float min, float max, bool max_included = true)
        {
            Value = v;
            this.min = min;
            this.max = max;
            if(this.max < this.min) this.max = max_included ? this.min : this.min + 1;
            this.max_included = max_included;
        }
    }
}
