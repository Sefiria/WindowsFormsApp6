using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.ATiles;
using WindowsFormsApp2.Entities;

namespace WindowsFormsApp2
{
    public class Map
    {
        public static readonly int W = SharedCore.RenderW / SharedCore.TileSize, H = SharedCore.RenderH / SharedCore.TileSize;
        public Point WorldCoord;
        public int[,] Tiles;
        public List<Rectangle> Boxes = new List<Rectangle>();
        public Point DoorLeft, DoorRight, DoorTop, DoorBottom;
        public List<DrawableEntity> Entities = new List<DrawableEntity>();

        public Map()
        {
            Tiles = new int[W, H];
        }

        public void Update()
        {
            if (SharedData.Player != null)
            {
                var ptc = SharedData.Player.TCoords;
                var dl = SharedData.World.Current.DoorLeft;
                var dr = SharedData.World.Current.DoorRight;
                var dt = SharedData.World.Current.DoorTop;
                var db = SharedData.World.Current.DoorBottom;
                if (dl != Point.Empty && ptc == dl) SharedData.World.GoLeft();
                if (dr != Point.Empty && ptc == dr) SharedData.World.GoRight();
                if (dt != Point.Empty && ptc == dt) SharedData.World.GoTop();
                if (db != Point.Empty && ptc == db) SharedData.World.GoBottom();
            }
        }

        public void Draw()
        {
            int v;
            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    v = Tiles[x,y];
                    if (v == 4)
                    {
                        SharedCore.ATileWater.Draw(x * SharedCore.TileSize, y * SharedCore.TileSize);
                    }
                    else
                    {
                        SharedCore.g.DrawImage(MapResManager.GetTileImgFromValue(v), x * SharedCore.TileSize, y * SharedCore.TileSize);
                    }
                }
            }
        }

        public Point GetAvailableSpot()
        {
            var box = Boxes[0];
            List<Point> grounds = new List<Point>();
            for (int x = box.X; x < box.X + box.Width; x++)
            {
                for (int y = box.Y; y < box.Y + box.Height; y++)
                {
                    if(Tiles[x, y] == 1)
                        grounds.Add(new Point(x, y));
                }
            }
            var rndGround = grounds[Tools.RND.Next(0, grounds.Count)];
            return new Point(rndGround.X, rndGround.Y);
        }
    }
}
