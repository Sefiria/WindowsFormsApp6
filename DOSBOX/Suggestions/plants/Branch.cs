using DOSBOX.Suggestions.plants;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Tooling;
using Maths = DOSBOX.Utilities.Maths;

namespace DOSBOX.Suggestions
{
    public class Branch
    {
        public Guid OwnerPlantGuid { get; set; }
        public ClassIPlant OwnerPlant => Data.Garden.ScenePlants.FirstOrDefault(p => p.Guid == OwnerPlantGuid);
        public Branch OwnerBranch { get; set; }
        public vecf endvec { get; set; }
        public vecf startvecfrombranch { get; set; } = vecf.Zero;
        public int maxbranches { get; set; }
        public int maxleaves { get; set; }
        public int maxfruits { get; set; } = 1;

        public vecf startvec => OwnerBranch != null ? startvecfrombranch : OwnerPlant?.vec;
        public bool IsMasterBranch => this == OwnerPlant?.masterbranch;

        public int depttree()
        {
            int level = 0;
            Branch pos = this;
            while (pos.OwnerBranch != null)
            {
                pos = new Branch(pos.OwnerBranch);
                level++;
            }
            return level;
        }

        public List<Branch> tree_branches { get; set; } = new List<Branch>();
        public List<Leaf> tree_leaves { get; set; } = new List<Leaf>();
        public List<Fruit> fruits { get; set; } = new List<Fruit>();

        public Branch() { }
        public Branch(Branch copy)
        {
            OwnerPlantGuid = copy.OwnerPlantGuid;
            OwnerBranch = copy.OwnerBranch;
        }
        public Branch(Guid ownerPlantGuid, Branch ownerBranch)
        {
            OwnerPlantGuid = ownerPlantGuid;
            OwnerBranch = ownerBranch;
            if (ownerBranch != null)
            {
                startvecfrombranch = new vecf(ownerBranch.endvec);
                endvec = new vecf(startvec);
                if (IsMasterBranch)
                    endvec += new vecf((Core.RND.Next(41) - 20F) / 20F, -Core.RND.Next(4, 11) / 10F);
                else
                {
                    vecf ownerlook = Maths.Normalized(OwnerBranch.endvec - OwnerBranch.startvec);
                    endvec += new vecf(ownerlook.x + (Core.RND.Next(41) - 20F) / 25F, ownerlook.y - (Core.RND.Next(41) - 20F) / 25F);
                }
            }
            else
            {
                endvec = new vecf(startvec);
                endvec.y--;
            }
            maxbranches = OwnerPlant.ForceMaxBranches == 0 ? OwnerPlant.ForceMaxBranches : (OwnerPlant.ForceMaxBranches > -1 ? Core.RND.Next(OwnerPlant.ForceMaxBranches) : Core.RND.Next(20, 50) / 10);
            maxleaves = OwnerPlant.ForceMaxLeaves == 0 ? OwnerPlant.ForceMaxLeaves : (OwnerPlant.ForceMaxLeaves > -1 ? Core.RND.Next(OwnerPlant.ForceMaxLeaves) : Core.RND.Next(10, 60) / 10);
            maxfruits = OwnerPlant.ForceMaxFruits == 0 ? OwnerPlant.ForceMaxFruits : (OwnerPlant.ForceMaxFruits > -1 ? Core.RND.Next(OwnerPlant.ForceMaxFruits) : Core.RND.Next(300) / 100);
        }

        public void Update()
        {
            new List<Branch>(tree_branches).ForEach(branch => branch.Update());
            new List<Leaf>(tree_leaves).ForEach(leaf => leaf.Update());
        }
        public void Display(int layer)
        {
            Graphic.DisplayLine(startvec, endvec, (byte) (OwnerPlant.water < OwnerPlant.waterneed ? 3 : 1), layer);
            new List<Branch>(tree_branches).ForEach(branch => branch.Display(layer));
        }
        public void DisplayLeaves(int layer)
        {
            new List<Leaf>(tree_leaves).ForEach(leaf => leaf.Display(layer));
            new List<Branch>(tree_branches).ForEach(branch => branch.DisplayLeaves(layer));
        }
        public void DisplayFruits(int layer)
        {
            new List<Fruit>(fruits).ForEach(fruit => fruit.Display(layer));
            new List<Branch>(tree_branches).ForEach(branch => branch.DisplayFruits(layer));
        }

        public void Grow()
        {
            var length = Maths.Length(startvec - endvec);
            var lengthmax = 28 - depttree() * 8;

            if (length < lengthmax)
            {
                vecf look = Maths.Normalized(endvec - startvec);
                endvec += look;
            }

            if(IsMasterBranch ? endvec.y < Data.Garden.FloorLevel - 4 : length > 4)
            {
                if(tree_branches.Count < maxbranches && length < lengthmax && Core.RND.Next(5) == 0)
                {
                    tree_branches.Add(new Branch(OwnerPlantGuid, this));
                }

                if (tree_leaves.Count < maxleaves && !IsMasterBranch && Core.RND.Next(8) == 0)
                    tree_leaves.Add(new Leaf(this));

                if (!IsMasterBranch && Core.RND.Next(120) == 60 && fruits.Count < maxfruits)
                    fruits.Add(OwnerPlant.CreateFruit(endvec, OwnerPlant.DNA));
            }

            tree_branches.ForEach(branch => branch.Grow());
        }

        internal int GetPotential() => maxfruits + tree_branches.Select(b => b.GetPotential()).Sum();
    }
}
