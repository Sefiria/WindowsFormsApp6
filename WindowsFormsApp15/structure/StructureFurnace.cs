using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WindowsFormsApp15.items;
using WindowsFormsApp15.utilities.tiles;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;
using static WindowsFormsApp15.enums;
using static WindowsFormsApp15.structure.StructureFurnace;

namespace WindowsFormsApp15.structure
{
    internal class StructureFurnace : Structure, IOStructure
    {
        public class ItemJob
        {
            public Item Item;
            private int ticks = 0;
            private const int tickmax = 10;
            public ItemJob(Item Item)
            {
                this.Item = Item;
                Item.Unmovable = true;
            }
            public bool Tick()
            {
                if (ticks < tickmax)
                {
                    Item.Unmovable = false;
                    return true;
                }
                
                ticks++;
                return false;
            }
        }

        public List<Way> WaysIn { get; set; }
        public Way Way { get; set; } = Way.Down;
        public float Speed { get; set; } = 1F;

        public List<ItemJob> Queue = new List<ItemJob>();

        public StructureFurnace() : base(vecf.Zero)
        {
            anim = furnace;
        }
        public StructureFurnace(vecf vec) : base(vec)
        {
            anim = furnace;
        }
        public override void Update()
        {
            UpdateQueue();
            UpdateItems();
        }

        private void UpdateItems()
        {
            RectangleF rect = new RectangleF(vec.x, vec.y, Core.TSZ, Core.TSZ);
            var items = Data.Instance.Items.Where(i => i.rect.IntersectsWith(rect))
                                                            .Except(Queue.Select(qj => qj.Item))
                                                            .ToList();
            items.ForEach(i =>
            {
                if (Maths.Length(i.vec - vec) <= 2)
                    Queue.Add(new ItemJob(i));
            });
        }
        private void UpdateQueue()
        {
            new List<ItemJob>(Queue).ForEach(qj =>
            {
                if (qj.Tick())
                {
                    Transform(ref qj.Item);
                    Queue.Remove(qj);
                }
            });
        }

        private void Transform(ref Item item)
        {
            switch(item)
            {
                case Ore ore: item = new Plate(ore.OreType, item.vec); break; 
            }
        }
    }
}
