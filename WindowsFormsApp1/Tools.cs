using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Entities;
using WindowsFormsApp1.Plants;

namespace WindowsFormsApp1
{
    public static class Tools
    {
        public static Random RND = new Random((int)DateTime.Now.Ticks);

        public static int CoordToTileCoord(int i) => i / SharedCore.TileSize;
        public static int CoordToTileCoord(float i) => CoordToTileCoord((int)i);
        public static Tile GetTile(Entity e) => new Tile(CoordToTileCoord(e.X), CoordToTileCoord(e.Y), e);
        public static List<Tile> GetTilesFromEntities()
        {
            var result = new List<Tile>();
            foreach (var e in SharedData.Entities)
            {
                if (e is Player) continue;
                if(e is Stand)
                    result.AddRange((e as Stand).GetTiles());
                else
                    result.Add(GetTile(e));
            }
            return result;
        }
        public static bool PositionTileIsOccupied(int X, int Y) => GetTilesFromEntities().Where(x => !x.LinkedEntity.Traversable).FirstOrDefault(x => x.X == CoordToTileCoord(X) && x.Y == CoordToTileCoord(Y)) != null;
        public static bool PositionTileIsOccupied(float X, float Y) => PositionTileIsOccupied((int)X, (int)Y);
        public static bool TileIsOccupied(int X, int Y) => GetTilesFromEntities().FirstOrDefault(i => i.X == X && i.Y == Y) != null;
        public static int SnapToGrid(float i) => CoordToTileCoord(i) * SharedCore.TileSize;
        public static int SnapToGrid(int i) => CoordToTileCoord(i) * SharedCore.TileSize;
        public static List<Bitmap> SplitImage(Bitmap b, int w, int h)
        {
            List<Bitmap> result = new List<Bitmap>();

            for(int x=0; x<b.Width; x += w)
            {
                Bitmap curimg = new Bitmap(w, h);
                var g = Graphics.FromImage(curimg);
                g.DrawImage(b, new Rectangle(0, 0, w, h), new Rectangle(x, 0, w, h), GraphicsUnit.Pixel);
                g.Dispose();
                curimg.MakeTransparent();
                result.Add(curimg);
            }

            return result;
        }
        public static List<Plant> GetAdjacentPlants(int TX, int TY)
        {
            var adj = new List<Pos>() { new Pos(TX - 1, TY - 1), new Pos(TX, TY - 1), new Pos(TX + 1, TY - 1),
                                        new Pos(TX - 1, TY),     new Pos(TX, TY),     new Pos(TX + 1, TY),
                                        new Pos(TX - 1, TY + 1), new Pos(TX, TY + 1), new Pos(TX + 1, TY + 1)};
            return SharedData.Entities.Where(e => e is Plant)
                                      .Where(p => adj.FirstOrDefault(a => a.X == p.TX && a.Y == p.TY) != null)
                                      .Cast<Plant>()
                                      .ToList();

        }
        public static List<Stand> GetAdjacentStands(int TX, int TY)
        {
            var adj = new List<Point>() { new Point(TX - 1, TY - 1), new Point(TX, TY - 1), new Point(TX + 1, TY - 1),
                                          new Point(TX - 1, TY),                            new Point(TX + 1, TY),
                                          new Point(TX - 1, TY + 1), new Point(TX, TY + 1), new Point(TX + 1, TY + 1)};
            return SharedData.Entities.Where(e => e is Stand)
                                      .Where(p => adj.FirstOrDefault(a => a.X == p.TX && a.Y == p.TY) != null)
                                      .Cast<Stand>()
                                      .ToList();

        }
        public static List<Plant> GetNearPlants(float X, float Y)
        {
            float h = SharedCore.TileSize / 2F;
            float d = h * 1.5F;
            return SharedData.Entities.Where(e => e is Plant)
                                      .Where(p => Sqrt(Sq(p.X - (X - h)) + Sq(p.Y - (Y - h))) < d)
                                      .Cast<Plant>()
                                      .ToList();
        }
        public static List<Stand> GetNearStands(float X, float Y)
        {
            float h = SharedCore.TileSize / 2F;
            float d = h * 3.5F;
            return SharedData.Entities.Where(e => e is Stand)
                                      .Where(p => Sqrt(Sq(p.X - (X - h)) + Sq(p.Y - (Y - h))) < d)
                                      .Cast<Stand>()
                                      .ToList();
        }
        public static bool NearToTrash(float X, float Y)
        {
            float h = SharedCore.TileSize / 2F;
            float d = h * 2F;
            return SharedData.Entities.Where(e => e is Trash).Any(p => Sqrt(Sq(p.X - (X - h)) + Sq(p.Y - (Y - h))) < d);
        }
        public static float Sq(float i) => i * i;
        public static unsafe float Sqrt(float number)
        {
            float y = number;
            long i = 0x5f3759df - ((*(long*)&y) >> 1);
            y = *(float*)&i;
            y = y * (1.5F - (number * 0.5F * y * y));
            return 1F / y * (1.5F - (number * 0.5F * y * y));
        }
    }
}