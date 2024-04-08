using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace console_v3
{
    internal class GraphicsManager
    {
        public struct RGB
        {
            public byte R, G, B;
            public RGB(int rgb)
            {
                var bytes = BitConverter.GetBytes(rgb);
                R = bytes[0];
                G = bytes[1];
                B = bytes[2];
            }
            public static implicit operator RGB(int d) => new RGB(d);
        }

        public static Font FontSQ = new Font("Courrier New", 24f, FontStyle.Regular);
        public static Font BigFont = new Font("Segoe UI", 14f);
        public static Font MidFont = new Font("Segoe UI", 12f);
        public static Font MiniFont = new Font("Segoe UI", 10f);

        public static Size ConsoleCharSize = new Size(8, 17);
        public static int TileSize => Core.TILE_SIZE;

        public static void DrawTile(Graphics g, Tile tile, vec coordinates)
        {
            var rw = Core.Instance.ScreenWidth;
            var rh = Core.Instance.ScreenHeight;
            var tw = TileSize;
            var th = TileSize;
            var bounds = new Rectangle(- rw / 2 - tw, - rh / 2 - th, rw + tw * 2, rh + th * 2);
            if (bounds.Contains(coordinates.ipt))
            {
                Draw(g, DB.GetTexture(tile.Value), coordinates.f);
            }
        }
        public static void Draw(Graphics g, Bitmap img, vecf v)
        {
            g.DrawImage(img, SceneAdventure.DrawingRect.Location.Plus(v.pt));
        }
    }
}
