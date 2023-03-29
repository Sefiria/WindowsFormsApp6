using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp15.utilities.tiles;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15.structure
{
    internal class StructureConveyor : Structure, IMoveStructure
    {
        public float Speed { get; set; } = 0.7F;
        private Way m_Way = (Way)2;
        public Way Way
        {
            get => m_Way;
            set
            {
                m_Way = value;
                anim = conveyors[Way];
            }
        }

        public StructureConveyor() : base(vecf.Zero)
        {
            Way = 0;
        }
        public StructureConveyor(vecf vec, int way) : base(vec)
        {
            Way = (Way)way;
        }
        public StructureConveyor(vecf vec, Way way) : base(vec)
        {
            Way = way;
        }
        public override void Update()
        {
            ItemsOnTile.ForEach(item =>
            {
                if (Way == Way.Up || Way == Way.Down)
                {
                    float x = vec.x + Core.TSZ / 2F;
                    var d = Maths.Distance(item.vec.x, 0F, x, 0F);
                    var amount = (x < item.vec.x ? -1F : 1F) * Maths.Normalize(d) * Speed;
                    if ((int)d == 0F || ItemsOnTile.Except(new List<Item> { item }).Any(o => o.rect.IntersectsWith(new RectangleF(item.vec.x + amount - item.hw, item.vec.y - item.hh, item._w, item._h))))
                        return;
                    item.vec.x += amount;
                }
                else
                {
                    float y = vec.y + Core.TSZ / 2F;
                    var d = Maths.Distance(0F, item.vec.y, 0F, y);
                    var amount = (y < item.vec.y ? -1F : 1F) * Maths.Normalize(d) * Speed;
                    if ((int)d == 0F || ItemsOnTile.Except(new List<Item> { item }).Any(o => o.rect.IntersectsWith(new RectangleF(item.vec.x - item.hw, item.vec.y + amount - item.hh, item._w, item._h))))
                        return;
                    item.vec.y += amount;
                }
            });
        }
    }
}
