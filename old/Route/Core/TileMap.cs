using Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Definitions;
using static Core.TileResMngr;
using static Core.Utils.Tools;

namespace Core
{
    public class TileMap
    {
        private Stack<Point> InvalidatedTiles = new Stack<Point>(1024);

        public int Size;
        public TileIndex[,] Map;

        public TileMap()
        {
        }
        public TileMap(int Size)
        {
            Initialize(Size);
        }
        private void Initialize(int Size = 0)
        {
            this.Size = Size;
            Map = new TileIndex[Size, Size];
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Map[x, y] = new TileIndex();
                    InvalidatedTiles.Push(new Point(x, y));
                }
            }
        }

        public TileIndex this[Point Location]
        {
            get => new TileIndex(Map[Location.X, Location.Y]);
            set
            {
                Map[Location.X, Location.Y] = new TileIndex(value);
                InvalidatedTiles.Push(Location);
            }
        }
        public TileIndex this[int X, int Y]
        {
            get => new TileIndex(Map[X, Y]);
            set => Map[X, Y] = value;
        }
        public TileIndex this[int X, int Y, object safe]
        {
            get
            {
                if (X < 0 || Y < 0 || X >= Size || Y >= Size)
                    return new TileIndex();
                else
                    return Map[X, Y];
            }
        }
        public TileIndex GetTileFromPixel(Point PixelLocation) => GetTileFromPixel(PixelLocation.X, PixelLocation.Y);
        public TileIndex GetTileFromPixel(int pixelX, int pixelY) => new TileIndex(Map[Tools.Snap(pixelX, SharedData.TileSize), Tools.Snap(pixelY, SharedData.TileSize)]);

        public void Render(Graphics g, float zoom = 1F, bool renderPathDots = false)
        {
            Bitmap img;
            while (InvalidatedTiles.Count > 0)
            {
                Point Location = InvalidatedTiles.Pop();
                TileIndex tileIndex = Map[Location.X, Location.Y];
                TileInfo tileInfo = TileResMngr.Instance.ResourcesTileInfo[tileIndex.Index];
                img = tileInfo.ImageWithSigns;
                img = Tools.ApplyRotation(img, tileIndex.Angle);
                if(zoom != 1F)
                    img = new Bitmap(img, (int)(img.Width * zoom) + 4, (int)(img.Height * zoom) + 4);
                g.DrawImage(
                        img,
                        Location.X * SharedData.TileSize * zoom,
                        Location.Y * SharedData.TileSize * zoom
                    );
                if (renderPathDots && tileIndex.Index > 0)
                    DrawPathDots(g, Location, tileIndex.GetRotatedPathDots(), zoom);
            }
        }
        private void DrawPathDots(Graphics g, Point Location, Dictionary<PathDots, PathDotType> pathDots, float zoom = 1F)
        {
            Location = new Point((int)(Location.X * zoom), (int)(Location.Y * zoom));

            Brush GetBrushFromPathDotType(PathDotType type)
            {
                switch (type)
                {
                    default:
                    case PathDotType.None: return Brushes.White;
                    case PathDotType.In: return Brushes.Red;
                    case PathDotType.Way: return Brushes.Lime;
                    case PathDotType.Out: return Brushes.Blue;
                }
            }

            g.Clip = new Region(new Rectangle(Location.X * SharedData.TileSize, Location.Y * SharedData.TileSize, (int)(SharedData.TileSize * zoom), (int)(SharedData.TileSize * zoom)));
            string[] listPathDotsNames = Enum.GetNames(typeof(PathDots));
            Rectangle rect = Rectangle.Empty;
            foreach (string PathDotsName in listPathDotsNames)
            {
                PathDots PathDotsValue = Tools.EnumParser<PathDots>(PathDotsName);
                rect = PathDotsRect[PathDotsValue];
                rect.Width = (int)((rect.Width / 2) * zoom) - 1;
                rect.Height = (int)((rect.Height / 2) * zoom) - 1;
                rect.X = (int)((rect.X / 4) * zoom);
                rect.Y = (int)((rect.Y / 4) * zoom);
                rect.Offset(Location.X * SharedData.TileSize, Location.Y * SharedData.TileSize);

                g.FillEllipse(GetBrushFromPathDotType(pathDots[PathDotsValue]), rect);
            }
            g.ResetClip();
        }
        public void Invalidate()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    InvalidatedTiles.Push(new Point(x, y));
                }
            }
        }

        public PathDots? GetRandomDot(List<PathDots> iPathDots)
        {
            if (iPathDots.Count == 0)
                return null;
            return iPathDots[rnd.Next(iPathDots.Count)];
        }
        public PathDots? GetRandomDotIn(Point tilePos) => GetRandomDot(GetListDotIn(tilePos));
        public PathDots? GetRandomDotWay(Point tilePos) => GetRandomDot(GetListDotWay(tilePos));
        public PathDots? GetRandomDotOut(Point tilePos) => GetRandomDot(GetListDotOut(tilePos));
        public PathDots? GetRandomDotNone(Point tilePos) => GetRandomDot(GetListDotNone(tilePos));
        public List<PathDots> GetListDot(Point tilePos, PathDotType type, bool OnlyAtLimit = true)
        {
            List<PathDots> iPathDots = new List<PathDots>();
            TileInfo info = TileResMngr.Instance.ResourcesTileInfo[Map[tilePos.X, tilePos.Y].Index];
            Dictionary<PathDots, PathDotType> list = Map[tilePos.X, tilePos.Y].GetRotatedPathDots();
            foreach (KeyValuePair<PathDots, PathDotType> pair in list)
                if (pair.Value == type)
                    iPathDots.Add(pair.Key);

            if(OnlyAtLimit)
            {
                for(int i=0; i<iPathDots.Count; i++)
                {
                    (int X, int Y, PathDots D) = Definitions.PathDotsLinkedCell[iPathDots[i]];
                    if (X != 0 || Y != 0)
                    {
                        TileIndex ti = this[tilePos.X + X, tilePos.Y + Y, null];
                        if (ti != null && ti.Index == 0)
                            continue;
                    }
                    iPathDots.RemoveAt(i--);
                }
            }

            return iPathDots;
        }
        public List<PathDots> GetListDotIn(Point tilePos) => GetListDot(tilePos, PathDotType.In);
        public List<PathDots> GetListDotWay(Point tilePos) => GetListDot(tilePos, PathDotType.Way);
        public List<PathDots> GetListDotOut(Point tilePos) => GetListDot(tilePos, PathDotType.Out);
        public List<PathDots> GetListDotNone(Point tilePos) => GetListDot(tilePos, PathDotType.None);
        public List<Point> GetListRouteTilePosition(bool OnlyAtLimit = true)
        {
            List<Point> routes = new List<Point>();
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                    if (Map[x, y].Index > 0 && (!OnlyAtLimit || 
                        (
                            this[x - 1, y - 1, null].Index == 0 ||
                            this[x - 1, y    , null].Index == 0 ||
                            this[x - 1, y + 1, null].Index == 0 ||
                            this[x    , y - 1, null].Index == 0 ||
                            this[x    , y + 1, null].Index == 0 ||
                            this[x + 1, y - 1, null].Index == 0 ||
                            this[x + 1, y    , null].Index == 0 ||
                            this[x + 1, y + 1, null].Index == 0
                        )
                        ))
                        routes.Add(new Point(x, y));

            if (routes.Count == 0)
                return null;

            return routes;
        }
        public Point? GetRandomRouteTilePosition(bool OnlyAtLimit = true, List<Point> routes = null)
        {
            if (routes == null)
                routes = GetListRouteTilePosition(OnlyAtLimit);
            if (routes.Count == 0)
                return null;
            return routes[Tools.rnd.Next(routes.Count)];
        }
    }
}