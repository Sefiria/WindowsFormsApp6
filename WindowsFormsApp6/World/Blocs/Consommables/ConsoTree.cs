using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;

namespace WindowsFormsApp6.World.Blocs.Consommables
{
    public class ConsoTree : ConsoBase
    {
        public static readonly Bitmap Tree = Resources.conso_tree.Transparent();
        public static readonly Bitmap Tronc = Resources.conso_tronc.Transparent();
        public ConsoTree(int x, int y) : base(x, y)
        {
            Image = Tree;
            ImageUsed = Tronc;
        }
    }
}
