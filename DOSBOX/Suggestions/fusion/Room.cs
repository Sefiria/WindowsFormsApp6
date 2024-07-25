using DOSBOX.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Room
    {
        public byte ID;
        public byte[,] Tiles;
        public List<Door> Doors = new List<Door>();
        public byte[,] Pixels, PixelsFront;
        public int w => Pixels.GetLength(0);
        public int h => Pixels.GetLength(1);

        public static Room Load(byte ID)
        {
            if (RefTiles == null)
                DefineTiles();

            Room room = new Room();
            if (room.LoadPixels(ID))
            {
                room.ID = ID;
                room.LoadData();
                return room;
            }
            return null;
        }

        public void Update()
        {
            Doors.Clone().ForEach(b =>b.Update());
        }
        public void Display(vecf cam)
        {
            (int w, int h) screen = (64, 64);
            (int x, int y) chunk = ((int)cam.x / screen.w, (int)cam.y / screen.h);
            Core.Cam = new vecf(chunk.x * 64, chunk.y * 64);

            for (int x = 0; x < screen.w; x++)
            {
                for (int y = 0; y < screen.h; y++)
                {
                    Core.Layers[0][x, y] = Pixels[chunk.x * screen.w + x, chunk.y * screen.h + y];
                }
            }

            Doors.Clone().ForEach(b => b.Display(1, Core.Cam.i));
        }
        public void DisplayFront(vecf cam)
        {
            (int w, int h) screen = (64, 64);
            (int x, int y) chunk = ((int)cam.x / screen.w, (int)cam.y / screen.h);
            byte px;

            for (int x = 0; x < screen.w; x++)
            {
                for (int y = 0; y < screen.h; y++)
                {
                    px = PixelsFront[chunk.x * screen.w + x, chunk.y * screen.h + y];
                    if (px != 0)
                        Core.Layers[1][x, y] = px;
                }
            }
        }
        bool LoadPixels(byte ID)
        {
            Bitmap img = Resources.ResourceManager.GetObject($"room_{ID}") as Bitmap;
            if (img == null)
                return false;

            Pixels = new byte[img.Width * TSZ, img.Height * TSZ];
            PixelsFront = new byte[img.Width * TSZ, img.Height * TSZ];
            Tiles = new byte[img.Width, img.Height];
            byte b;
            for (int x = 0; x < w / TSZ; x++)
                for (int y = 0; y < h / TSZ; y++)
                {
                    b = GetByteFromColor(img.GetPixel(x, y).R);
                    Tiles[x, y] = b;
                    SetPixelsFromTileId(b, x, y);
                }
            return true;
        }
        byte GetByteFromColor(int r) => (byte)(r == 255 ? 0 : r + 1);
        void SetPixelsFromTileId(byte id, int _x, int _y)
        {
            for (int x = 0; x < TSZ; x++)
                for (int y = 0; y < TSZ; y++)
                {
                    if (RefTiles[id].Type == Tile.TYPE.FRONT)
                        PixelsFront[_x * TSZ + x, _y * TSZ + y] = RefTiles[id][x, y];
                    else if (RefTiles[id][x, y] != 4)
                        Pixels[_x * TSZ + x, _y * TSZ + y] = RefTiles[id][x, y];
                }
        }
        void LoadData()
        {
            var metaBytes = (byte[])Resources.ResourceManager.GetObject($"room_{ID}_meta");
            if (metaBytes == null)
                return;
            var meta = Encoding.UTF8.GetString(metaBytes);
            RoomData data = JsonSerializer.Deserialize<RoomData>(meta);
            if (data.doors?.Length > 0)
            {
                Doors.Clear();
                Doors.AddRange(data.doors.Select(d => new Door(d.x, d.y, d.w, d.h, (byte)d.state.ByteCut())));
            }
        }
        public bool isout(vecf v) => isout(v.i.x, v.i.y);
        public bool isout(float x, float y) => isout((int)x, (int)y);
        public bool isout(int x, int y) => x < 0 || y < 0 || x >= w || y >= h;
        public bool isout(byte[,] g, float x, float y)
        {
            int w = g.GetLength(0);
            int h = g.GetLength(1);
            return x < 0 || y < 0 || x + w >= this.w || y + h >= this.h;
        }

        public class RoomData
        {
            public RoomData_data[] doors { get; set; }
        }
        public class RoomData_data
        {
            public int state { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int w { get; set; }
            public int h { get; set; }
        }

        public static int TSZ => Tile.TSZ;
        public static List<Tile> RefTiles = null;
        private static void DefineTiles()
        {
            var humanreadableTiles = new List<Tile>
                {
                    new Tile() { Type = Tile.TYPE.EMPTY, Pixels = new byte[8, 8] },
                    new Tile() { Type = Tile.TYPE.SOLID, Pixels = new byte[8, 8]
                    {
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 3, 0, 0, 0, 0, 0, 0, 0 },
                        { 3, 1, 1, 1, 1, 1, 1, 0 },
                        { 3, 1, 1, 1, 1, 1, 1, 0 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 0, 0, 0, 0, 3, 0, 0, 0 },
                        { 1, 1, 1, 0, 3, 1, 1, 1 },
                        { 1, 1, 1, 0, 3, 1, 1, 1 },
                    } },
                    new Tile() { Type = Tile.TYPE.SOLID, Pixels = new byte[8, 8]
                    {
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 3, 2, 0, 0, 0, 0, 2, 3 },
                        { 3, 0, 2, 0, 0, 2, 0, 3 },
                        { 3, 0, 0, 2, 2, 0, 0, 3 },
                        { 3, 0, 0, 2, 2, 0, 0, 3 },
                        { 3, 0, 2, 0, 0, 2, 0, 3 },
                        { 3, 2, 0, 0, 0, 0, 2, 3 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                    } },
                    new Tile() { Type = Tile.TYPE.FRONT, Pixels = new byte[8, 8]
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 3, 3, 0, 0, 0 },
                        { 0, 3, 3, 3, 3, 3, 3, 0 },
                        { 3, 3, 4, 4, 4, 4, 3, 3 },
                        { 3, 4, 3, 3, 3, 3, 4, 3 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                    } },
                };

            RefTiles = new List<Tile>();
            foreach (var t in humanreadableTiles)
            {
                var tile = new Tile() { Type = t.Type, Pixels = new byte[8, 8] };
                for (int x = 0; x < 8; x++)
                    for (int y = 0; y < 8; y++)
                        tile.Pixels[x, y] = t[y, x];
                RefTiles.Add(tile);
            }
        }
    }
}
