using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp27_comb
{
    internal abstract class UI
    {
        public Guid ID;
        public bool IsHover, IsDisabled;
        public int X, Y;
        public Point Position
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        protected UI()
        {
            ID = Guid.NewGuid();
        }
        public delegate void OnUpdate();
        public abstract void Draw(Graphics g);
    }
}
