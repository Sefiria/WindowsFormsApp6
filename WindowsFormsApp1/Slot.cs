using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Slot
    {
        public Marchandise Marchandise = null;
        public bool IsFilled => Marchandise != null;
        /// <summary>
        /// Tile X
        /// </summary>
        public int TX;
        /// <summary>
        /// Tile Y
        /// </summary>
        public int TY;
        /// <summary>
        /// Relative X
        /// </summary>
        public int RX;
        /// <summary>
        /// Relative Y
        /// </summary>
        public int RY;

        public Slot(int TX, int TY, int RX, int RY, Marchandise Marchandise = null)
        {
            this.TX = TX;
            this.TY = TY;
            this.RX = RX;
            this.RY = RY;
            this.Marchandise = Marchandise;
        }

        public void Draw()
        {
            if(IsFilled)
            {
                var img = Marchandise.Images[0];
                SharedCore.g.DrawImage(img, TX * SharedCore.TileSize + RX - img.Width / 2, TY * SharedCore.TileSize + RY - img.Height / 2);
            }
        }
    }
}
