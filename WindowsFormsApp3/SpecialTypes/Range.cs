using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3.SpecialTypes
{
    public class Range
    {
        private int _value = 0;

        public int Min { get; private set; } = 0;
        public int Max { get; private set; } = 0;
        public int Value { get => _value; private set { _value = value; CheckValue(); } }

        private void CheckValue()
        {
            if (_value < Min) _value = Min;
            if (_value > Max) _value = Max;
        }

        public Range(int Min, int Max, int? Value = null)
        {
            if (Max < Min) Max = Min;
            this.Min = Min;
            this.Max = Max;
            this.Value = Value ?? this.Min;
        }

        public void Set(int Value)
        {
            this.Value = Value;
        }
        public void Increase()
        {
            Value++;
        }
        public void Decrease()
        {
            Value--;
        }
        public void Reset()
        {
            Value = Min;
        }
    }
}
