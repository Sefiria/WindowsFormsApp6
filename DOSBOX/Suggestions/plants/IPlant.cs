using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions
{
    public interface IPlant
    {
        vecf vec { get; set; }
        Branch masterbranch { get; set; }
        byte waterneed { get; set; }
        byte water { get; set; }


        void Update();
        void Display(int layer);
        Fruit CreateFruit(vecf v);
    }
}
