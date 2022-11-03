using System.Drawing;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.World.WorldResources;

namespace WindowsFormsApp6.World.Blocs.Consommables
{
    public class ConsoTree : ConsoBase, IReGrowable
    {
        public static readonly Bitmap Tree = Resources.conso_tree.Transparent();
        public static readonly Bitmap Tronc = Resources.conso_tronc.Transparent();
        public ConsoTree(int x, int y) : base(x, y)
        {
            Image = Tree;
            ImageUsed = Tronc;
            Life = 1000;
        }

        public int Timer { get; set; } = 0;

        public void Tick()
        {
            if (!Used)
            {
                Timer = 0;
            }
            else
            {
                if (Timer >= 5000)
                {
                    Timer = 0;
                    Life = 100;
                }
                else
                {
                    Timer++;
                }
            }
        }

        public override void GetConsoResource()
        {
            Data.Instance.StatInfo.Inventory.AddResource(new Tronc());
        }
    }
}
