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
        Guid Guid { get; set; }
        vecf vec { get; set; }
        Branch masterbranch { get; set; }
        byte waterneed { get; set; }
        byte water { get; set; }


        void Update(byte[,] ActiveBG = null);
        void Display(int layer);
        Fruit CreateFruit(vecf v);
        int GetPotential();
    }
    public class ClassIPlant : IPlant
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public vecf vec { get; set; }
        public Branch masterbranch { get; set; }
        public byte waterneed { get; set; }
        public byte water { get; set; }
        public List<vec> px_seed { get; set; } = new List<vec>() { new vec(0, 0) };

        public virtual void Update(byte[,] ActiveBG = null) { }
        public virtual void Display(int layer) { }
        public virtual Fruit CreateFruit(vecf v) { return null; }
        public virtual int GetPotential() => 0;
    }
}
