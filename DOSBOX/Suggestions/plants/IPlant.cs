using System;
using System.Collections.Generic;
using Tooling;

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
        Fruit CreateFruit(vecf v, string dna = null);
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
        public int ForceMaxBranches = -1, ForceMaxLeaves = -1, ForceMaxFruits = -1;
        public string DNA = "";

        public virtual void Update(byte[,] ActiveBG = null) { }
        public virtual void Display(int layer) { }
        public virtual Fruit CreateFruit(vecf v, string dna = null) { return null; }
        public virtual int GetPotential() => 0;
    }
}
