using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp24.Events;

namespace WindowsFormsApp24
{
    internal class Root
    {
        internal OldCrop Owner;
        internal Root Parent;
        internal int ParentDotIndex;
        internal PointF Look;
        internal List<Root> Roots = new List<Root>();
        internal List<Leaf> Leaves = new List<Leaf>();
        internal List<Fruit> Fruits = new List<Fruit>();
        private bool NeedGeneration = false;
        private long start_tick = 0;
        private int next_tick_gap, length = 0, max_length;
        List<PointF> dots = new List<PointF>();
        internal int get_legacy_degree()
        {
            int degree = 0;
            void up(Root cur)
            {
                if (cur.Parent != null) { degree++; up(cur.Parent); };
            }
            if (Parent != null) { degree++; up(Parent); };
            return degree;
        }

        public Root(OldCrop owner, Root parent)
        {
            Owner = owner;
            Parent = parent;
            NewCycle();
            max_length = (int)(5F / (get_legacy_degree() + 1F) * (Owner.Image.Height / 5));
            var d = get_legacy_degree();
            Look = (RandomThings.rnd(21)/10F-1F, -1F).P();
            if (Parent != null) Look = Parent.Look.Rotate(Look.ToAngle());
        }
        internal void Update()
        {
            GenerateUpdate();
            Roots.ForEach(root => root.Update());
            Leaves.ForEach(leaf => leaf.Update());
            Fruits.ForEach(fruit => fruit.Update());
        }
        internal void NewCycle()
        {
            start_tick = Core.Instance.Ticks;
            next_tick_gap = RandomThings.rnd(0, 10);
        }
        internal void GenerateUpdate()
        {
            if (length < max_length && Core.Instance.Ticks >= start_tick + next_tick_gap)
            {
                var ts = Core.TileSize;
                NewCycle();
                length++;
                PointF pt;
                if (dots.Count == 0)
                {
                    if (Parent == null)
                    {
                        pt = new PointF(ts, ts * 1.5F).Plus(new Point(RandomThings.rnd(0, ts), RandomThings.rnd(0, ts / 2)));
                    }
                    else
                    {
                        do { pt = new PointF(-1F + RandomThings.rnd(3), RandomThings.rnd(2)); } while (pt == PointF.Empty);
                        pt = Parent.dots[ParentDotIndex].Minus(pt);
                    }
                }
                else
                {
                    var d = get_legacy_degree();
                    pt = new PointF(Look .X - 1F + RandomThings.rnd(3), Look.Y + (d == 0?RandomThings.rnd(2):RandomThings.rnd(2*(10*d) / (10 * d))));
                    pt = dots.Last().Minus(pt);
                }
                dots.Add(pt);

                if (get_legacy_degree() < 4 && RandomThings.rnd((5-get_legacy_degree())*8) == 0)
                    Roots.Add(new Root(Owner, this) { ParentDotIndex = dots.Count - 1 });

                NeedGeneration = true;
            }
        }
        internal void GenerateDraw(Graphics g)
        {
            if(NeedGeneration)
            {
                NeedGeneration = false;
                var d = get_legacy_degree();
                int sz = Math.Max(1, 5 - d);
                var dot = dots.Last();
                var c = Color.FromArgb(107 + d * 10, 142 + d * 10, 35 + d * 10);
                var cs = c.Mod(-20,-20,-20);
                g.FillRectangle(new SolidBrush(c), dot.X, dot.Y, sz, sz);
                if(sz > 1) g.DrawLine(new Pen(cs), dot.X, dot.Y, dot.X, dot.Y+ sz);
                //g.DrawRectangle(Pens.Black, 0, 0, Owner.Image.Width - 1, Owner.Image.Height - 1);
                //var ts = Core.TileSize;
                //g.DrawRectangle(Pens.Black, ts, ts * 1.5F, ts, ts);
            }

            Roots.ForEach(root => root.GenerateDraw(g));
            Leaves.ForEach(leaf => leaf.GenerateDraw(g));
            Fruits.ForEach(fruit => fruit.GenerateDraw(g));
        }
    }
}
