
using RetfarC.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetfarC
{
    public class Item : Entity
    {
        public int Count;

        public Item(int count) : base(0, 0)
        {
            Count = count;
            Visible = false;
        }
        public Item(float x, float y, int count) : base(x, y)
        {
            Count = count;
            Visible = false;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Click(MouseEventArgs e)
        {
            base.Click(e);
            if (MouseHover && Visible)
            {
                Data.AddItem(this);
                Visible = false;
            }
        }
    }
}
