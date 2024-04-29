using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;
using WindowsFormsApp28_px.matters.organic;

namespace WindowsFormsApp28_px.matters.non_organic
{
    internal class Chest : Entity
    {
        public bool IsOpen = false;
        public List<Item> Content = new List<Item>();
        //public UIContent UI_Container = null;

        public Chest() : base()
        {
        }
        public Chest(Point position, Size size, float angle, int color, Item[] content) : base(position, size, angle, color)
        {
            Content = content.ToList();
        }
        public Chest(Point position, Size size, float angle, int color, (int id, int count)[] content) : base(position, size, angle, color)
        {
            Content = content.Select(i => new Item(i.id, i.count)).ToList();
        }
        public Chest(Point position, Size size, float angle, int color, (int id, int count) item) : base(position, size, angle, color)
        {
            Content = new Item[] { new Item(item) }.ToList();
        }
        public Chest(float X, float Y, int W, int H, float angle, int color, Item[] content) : base(X, Y, W, H, angle, color)
        {
            Content = content.ToList();
        }
        public Chest(float X, float Y, int W, int H, float angle, int color, (int id, int count)[] content) : base(X, Y, W, H, angle, color)
        {
            Content = content.Select(i => new Item(i.id, i.count)).ToList();
        }
        public Chest(float X, float Y, int W, int H, float angle, int color, (int id, int count) item) : base(X, Y, W, H, angle, color)
        {
            Content = new Item[] { new Item(item) }.ToList();
        }

        public override void Action(IMatter triggerer)
        {
            if (IsOpen)
                return;
            IsOpen = true;
            var c = Color.FromArgb(C);
            C = Color.FromArgb(c.R / 2, c.G / 2, c.B / 2).ToArgb();
            triggerer.Inventory.Merge(Content);
            Content = null;
        }

        public override void Update()
        {
            //bool old_IsOpen = IsOpen;
            //IsOpen = Common.Matters.OfType<Controllable>().FirstOrDefault(c => c.Point.vecf().Distance(Point.vecf()) < c.diameter * 4F) != null;
            //if(IsOpen == true && old_IsOpen == false)
            //    UI_Container = UIFactory.CreateItemContainer(Content, x - (DB.ItemSize + 8) * Maths.Sqrt(Content.Count) / 2, y - h * 2 - 8 - (DB.ItemSize + 8) * Maths.Sqrt(Content.Count));
            //if(IsOpen == false && old_IsOpen == true)
            //    UI_Container = null;
        }

        //public override void Draw(Graphics g)
        //{
        //    base.Draw(g);
        //}

        public override void DrawUI(Graphics g)
        {
            //base.Draw(g, Common.Cam.x(-1F));

            //if(IsOpen)
            //{
            //    UI_Container.Draw(g);
            //}
        }
    }
}
