using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions
{
    public class Branch
    {
        public IPlant OwnerPlant;
        public Branch OwnerBranch;
        public vecf endvec;
        public vecf startvecfrombranch = vecf.Zero;
        public int maxbranches, maxleaves;

        public vecf startvec => OwnerBranch != null ? startvecfrombranch : OwnerPlant.vec;
        public bool IsMasterBranch => this == OwnerPlant.masterbranch;

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

        public List<Branch> tree_branches = new List<Branch>();
        public List<Leaf> tree_leaves = new List<Leaf>();
        public List<Fruit> fruits = new List<Fruit>();

        public Branch(Branch copy)
        {
            OwnerPlant = copy.OwnerPlant;
            OwnerBranch = copy.OwnerBranch;
        }
        public Branch(IPlant ownerPlant, Branch ownerBranch)
        {
            OwnerPlant = ownerPlant;
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
            maxbranches = Core.RND.Next(10, 50) / 10;
            maxleaves = Core.RND.Next(0, 40) / 10;
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

            if(IsMasterBranch ? endvec.y < Garden.FloorLevel - 4 : length > 4)
            {
                if(tree_branches.Count < maxbranches && length < lengthmax && Core.RND.Next(5) == 0)
                {
                    tree_branches.Add(new Branch(OwnerPlant, this));
                }

                if (tree_leaves.Count < maxleaves && !IsMasterBranch && Core.RND.Next(8) == 0)
                    tree_leaves.Add(new Leaf(this));

                if (!IsMasterBranch && Core.RND.Next(256) == 128 && fruits.Count < 2)
                    fruits.Add(OwnerPlant.CreateFruit(endvec));
            }

            tree_branches.ForEach(branch => branch.Grow());
        }
    }
}
