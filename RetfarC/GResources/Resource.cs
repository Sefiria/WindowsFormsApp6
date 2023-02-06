using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetfarC
{
    public class Resource : Entity
    {
        private int m_HP;
        public int HP
        {
            get { return m_HP; }
            set { m_HP = value; if (m_HP <= 0) Drop(); }
        }
        public int HPMax;
        public List<Item> Items = new List<Item>();

        public Resource(float x, float y) : base(x, y)
        {
        }

        public override void Update()
        {
            base.Update();

            if(MouseHover)
            {
            }
        }

        public override void Draw()
        {
            base.Draw();
            DrawHP();
        }
        private void DrawHP()
        {
            if (!MouseHover) return;
            
            var str = $"{HP} / {HPMax}";
            var sz = Core.g.MeasureString(str, Core.Font);
            var strszx = sz.Width;
            var strszy = sz.Height;
            var x = Pos.X * Core.TSZ + Core.TSZ / 2 - strszx / 2;
            var y = (Pos.Y + (Pos.Y == Core.TW - 1 ? 0 : 2)) * Core.TSZ - strszy - 5;
            Core.g.DrawString(str, Core.Font, Brushes.White, x, y);
        }

        public void Drop()
        {
            Items.ForEach(i => i.ToVisible = true);
            Destroy = true;
        }

        public override void Click(MouseEventArgs e)
        {
            base.Click(e);
            if(MouseHover)
            {
                HP -= Data.DMG;
            }
        }
    }
}
