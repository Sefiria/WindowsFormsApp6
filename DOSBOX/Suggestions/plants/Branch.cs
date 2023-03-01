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
        public Plant OwnerPlant;
        public Branch OwnerBranch;
        public vecf endvec;
        public bool HasFlower = false;
        public vecf startvecfrombranch = vecf.Zero;

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

        List<Branch> tree_branches = new List<Branch>();
        List<Leaf> tree_leaves = new List<Leaf>();

        public Branch(Branch copy)
        {
            OwnerPlant = copy.OwnerPlant;
            OwnerBranch = copy.OwnerBranch;
        }
        public Branch(Plant ownerPlant, Branch ownerBranch)
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
        }

        public void Update()
        {
            new List<Branch>(tree_branches).ForEach(branch => branch.Update());
            new List<Leaf>(tree_leaves).ForEach(leaf => leaf.Update());
        }
        public void Display(int layer)
        {
            Graphic.DisplayLine(startvec, endvec, 1, layer);

            new List<Branch>(tree_branches).ForEach(branch => branch.Display(layer));
            new List<Leaf>(tree_leaves).ForEach(leaf => leaf.Display(layer));
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
                if(length < lengthmax && Core.RND.Next(5) == 0 && tree_branches.Count < 8)
                {
                    tree_branches.Add(new Branch(OwnerPlant, this));
                }

                if(!IsMasterBranch && Core.RND.Next(8) == 0 && tree_leaves.Count < 4)
                {
                    tree_leaves.Add(new Leaf(this));
                }
            }

            tree_branches.ForEach(branch => branch.Grow());
        }
    }
}
