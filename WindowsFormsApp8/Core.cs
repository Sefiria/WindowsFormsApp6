using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public class Core
    {
        public static int RW, RH;
        public static int TileSz = 32;
        public static Bitmap Image;
        public static Point MousePosition;
        public static Graphics g, gui;
        public static bool IsMouseDown = false;
        public static bool IsRightMouseDown = false;
        public static bool IsMiddleMouseDown = false;
        public static Palette Palette;
        public static ListBox ListTiles;
        public static PointF Cam;
        public static bool ControlKeyHelp = false;
        public static bool DisplayAllTiles = false;
        public static Point TileToManuallySet = Point.Empty;

        public static int CWT => RW / TileSz;
        public static int CHT => RH / TileSz;
        public static int WT = 32;
        public static int HT = 32;
        public static Point Mouse => new Point(MousePosition.X + (int)Cam.X, MousePosition.Y + (int)Cam.Y);
        public static Point MouseTile => new Point(Mouse.X / TileSz, Mouse.Y / TileSz);
        public static Point MouseSnap => new Point(MouseTile.X * TileSz - (int)Cam.X, MouseTile.Y * TileSz - (int)Cam.Y);
        public static Rectangle CamTile => new Rectangle((int)Cam.X / TileSz, (int)Cam.Y / TileSz, RW / TileSz, RH / TileSz);
        public static Point CamSnap => new Point(CamTile.X * TileSz, CamTile.Y * TileSz);


        public static Tile ListTilesTile(int tileX, int tileY) => ListTilesTile(RenderClass.Tiles[tileX, tileY].Index);
        public static Autotile.Autotile ListTilesAutotile(int tileX, int tileY) => ListTilesAutotile(RenderClass.Tiles[tileX, tileY].Index);
        public static Tile ListTilesTile(int i)
        {
            if (i < 0 || i >= ListTiles.Items.Count) return ListTiles.Items[0] as Tile;
            return ListTiles.Items[i] as Tile;
        }
        public static Autotile.Autotile ListTilesAutotile(int i)
        {
            if (i < 0 || i >= ListTiles.Items.Count) return ListTiles.Items[0] as Autotile.Autotile;
            return ListTiles.Items[i] as Autotile.Autotile;
        }
    }
}
