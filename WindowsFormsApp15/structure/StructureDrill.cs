using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp15.items;
using WindowsFormsApp15.utilities.tiles;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15.structure
{
    internal class StructureDrill : Structure, IMoveStructure
    {
        public Way Way { get; set; } = Way.Down;
        public float Speed { get; set; } = 1F;
        private float tick, tickmax = 100F;
        public StructureDrill() : base(vecf.Zero)
        {
            anim = drill;
        }
        public StructureDrill(vecf vec, int way) : base(vec)
        {
            anim = drill;
            Way = (Way)way;
        }
        public StructureDrill(vecf vec, Way way) : base(vec)
        {
            anim = drill;
            Way = way;
        }
        public override void Update()
        {
            if (tick >= tickmax)
            {
                tick = 0;
                Dig();
            }
            else tick += Speed;
        }
        private void Dig()
        {
            if (ItemsOnTile.Any(item => item.rect.IntersectsWith(new RectangleF(vec.x, vec.y, Core.TSZ, Core.TSZ))) == false)
                Data.Instance.Items.Add(new Ore(GetRandomOre(), vec + Core.TSZ / 2F));
        }
    }
}
