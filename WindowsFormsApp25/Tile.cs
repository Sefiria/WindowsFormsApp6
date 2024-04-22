using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp25
{
    internal class Tile
    {
        public int X, Y;
        public vec Coords
        {
            get => (X, Y).V();
            set
            {
                X = value.x; Y = value.y;
            }
        }
        public Bitmap Image;
        public bool Invalidated = false;

        private DB.Tex m_Tex;
        public DB.Tex Tex
        {
            get => m_Tex;
            set
            {
                if (m_Tex == value)
                    return;
                m_Tex = value;
                Image = DB.GetTex(m_Tex);
                Invalidated = true;
            }
        }
        public Tile(int x, int y, DB.Tex tex)
        {
            X = x;
            Y = y;
            Tex = tex;
        }

        public void Invalidate() => Invalidated = true;

        public void Draw(Graphics g)
        {
            g.DrawImage(Image, (Coords * Core.TileSize).pt);
            g.DrawRectangle(Pens.Black, Coords.x * Core.TileSize, Coords.y * Core.TileSize, Core.TileSize, Core.TileSize);
            Invalidated = false;
        }
    }
}
