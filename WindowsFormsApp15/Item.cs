using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp15.structure;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15
{
    internal class Item
    {
        public Items ItemType;
        public vecf vec;
        public anim anim;
        public bool Unmovable = false;
        public int _w, _h;
        public int hw => _w / 2;
        public int hh => _h / 2;
        public RectangleF rect => new RectangleF(vec.x - hw, vec.y - hh, _w, _h);
        public vecf TopLeft => vec + new vecf(-hw, -hh);
        public vecf TopRight => vec + new vecf(hw, -hh);
        public vecf BottomLeft => vec + new vecf(-hw, hh);
        public vecf BottomRight => vec + new vecf(hw, hh);

        public Item(Items itemtype, vecf vec)
        {
            ItemType = itemtype;
            this.vec = new vecf(vec);
            AnimRes.SetSize(itemtype, ref _w, ref _h);
        }

        public void Update()
        {
            if (Unmovable)
                return;

            vecf nextvec = new vecf(vec);

            bool checknotvoidOrOccupied(float x, float y)
            {
                var where = new vecf(x, y).snap();
                var @struct = Data.Instance.GetStructureAt(where);
                if(@struct is IMoveInfos)
                {
                    var rect = new RectangleF(x - hw, y - hh, _w, _h);
                    if (@struct.ItemsOnTile.Except(new List<Item>{this}).Any(item => item.rect.IntersectsWith(rect)) == false)
                    {
                        return true;
                    }
                }
                return false;
            }
            void move(IMoveInfos s)
            {
                var way = s.Way;
                var speed = s.Speed;
                switch (way)
                {
                    case Way.Down: if (checknotvoidOrOccupied(vec.x, vec.y + speed + hh)) nextvec.y += speed; break;
                    case Way.Up: if (checknotvoidOrOccupied(vec.x, vec.y - speed - hh)) nextvec.y -= speed; break;
                    case Way.Left: if (checknotvoidOrOccupied(vec.x - speed - hw, vec.y)) nextvec.x -= speed; break;
                    case Way.Right: if (checknotvoidOrOccupied(vec.x + speed + hw, vec.y)) nextvec.x += speed; break;
                }
            }

            var a = Data.Instance.GetStructureAt(TopLeft.snap()) as IMoveInfos;
            var e = Data.Instance.GetStructureAt(TopRight.snap()) as IMoveInfos;
            var w = Data.Instance.GetStructureAt(BottomLeft.snap()) as IMoveInfos;
            var c = Data.Instance.GetStructureAt(BottomRight.snap()) as IMoveInfos;

            if (a == e && e == w && w == c && a != null)
                move(a);

            if (a == e && e != null && a != w && e != c && (a.Way == Way.Down || a.Way == Way.Up))
                move(a);
            if (a == w && w != null && a != e && w != c && (a.Way == Way.Left || a.Way == Way.Right))
                move(a);
            if (c == e && e != null && c != w && e != a && (c.Way == Way.Left || c.Way == Way.Right))
                move(c);
            if (c == w && w != null && c != e && w != a && (c.Way == Way.Down || c.Way == Way.Up))
                move(c);

            vec = nextvec;
        }

        public void Display()
        {
            anim.Display(vec - Core.TSZ / 2F);
        }
    }
}
