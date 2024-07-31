using DOSBOX.Properties;
using DOSBOX.Suggestions.fusion.jsondata;
using DOSBOX.Suggestions.fusion.Triggerables;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
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
        public List<Warp> Warps = new List<Warp>();
        public List<Mob> Mobs = new List<Mob>();
        public List<PhysicalObject> PhysicalObjects = new List<PhysicalObject>();
        public byte[,] Pixels, PixelsFront;
        public int w => Pixels.GetLength(0);
        public int h => Pixels.GetLength(1);
        public bool HasFLies = false;
        public List<Fly> Flies = new List<Fly>();
        public const int max_flies = 3;

        public static Room Load(byte ID)
        {
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
            Doors.Clone().ForEach(d => d.Update());
            Mobs.Clone().ForEach(m => { if (!m.Exists) { Register.Write(m); Mobs.Remove(m); } else m.Update(); });
            PhysicalObjects.Clone().ForEach(po => { if (!po.Exists) { Register.Write(po); PhysicalObjects.Remove(po); } else po.Update(); });
            FliesMgmt();
        }
        public void FliesMgmt()
        {
            if (!HasFLies)
                return;
            if (Fusion.Instance.samus.afk_duration > 100)
            {
                if (Flies.Count < max_flies)
                {
                    if (RandomThings.rnd1() <= 0.075F)
                        Flies.Add(new Fly(RandomThings.rnd_vecf_in_screen(w, h)));
                }
                var flies = Flies.Clone();
                foreach (var fly in flies)
                {
                    fly.Update();
                    if (isout(fly))
                        Flies.Remove(fly);
                }
            }
            else
            {
                if (Flies.Count > 0)
                {
                    var flies = Flies.Clone();
                    foreach (var fly in flies)
                    {
                        fly.LeaveScreen();
                        if (isout(fly))
                            Flies.Remove(fly);
                    }
                }
            }
            
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
                    if(chunk.x * screen.w + x< w && chunk.y * screen.h + y < h)
                        Core.Layers[0][x, y] = Pixels[chunk.x * screen.w + x, chunk.y * screen.h + y];
                }
            }

            Doors.Clone().ForEach(d => d.Display(1, Core.Cam.i));
            Mobs.Clone().ForEach(m => m.Display(1, Core.Cam.i));
            PhysicalObjects.Clone().ForEach(po => po.Display(1, Core.Cam.i));
            Flies.Clone().ForEach(f => f.Display(1, Core.Cam.i));
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
                    if (chunk.x * screen.w + x < w && chunk.y * screen.h + y < h)
                    {
                        px = PixelsFront[chunk.x * screen.w + x, chunk.y * screen.h + y];
                        if (px != 0)
                            Core.Layers[1][x, y] = px;
                    }
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
                    if (RoomTiles.RefTiles[id].Type == Tile.TYPE.FRONT)
                        PixelsFront[_x * TSZ + x, _y * TSZ + y] = RoomTiles.RefTiles[id][x, y];
                    else if (RoomTiles.RefTiles[id][x, y] != 4)
                        Pixels[_x * TSZ + x, _y * TSZ + y] = RoomTiles.RefTiles[id][x, y];
                }
        }
        void LoadData()
        {
            var metaBytes = (byte[])Resources.ResourceManager.GetObject($"room_{ID}_meta");
            if (metaBytes == null)
                return;
            var meta = Encoding.UTF8.GetString(metaBytes);
            RoomData data = JsonSerializer.Deserialize<RoomData>(meta);
            if (data != null)
            {
                Doors.Clear();
                if(data.doors?.Length > 0)
                    Doors.AddRange(data.doors.Select(d => new Door(ID, d)));
                Warps.Clear();
                if(data.warps?.Length > 0)
                    Warps.AddRange(data.warps.Select(w => new Warp(w)));
                HasFLies = data.HasFlies;
                Flies.Clear();
                if (data.mobs?.Length > 0)
                {
                    foreach(var m in data.mobs)
                    {
                        if(!Register.mobs_killed.Contains(Mob.GenerateBaseHash(ID, m.vec.x, m.vec.y)))
                            Mobs.Add(new Mob(ID, m));
                    }
                }
                PhysicalObjects.Clear();
                if (data.objects?.Length > 0)
                    PhysicalObjects.AddRange(data.objects.Select(po => PhysicalObject.FactoryCreate(po)));
            }
        }
        public bool isout(Dispf d) => isout(d.vec.i.x, d.vec.i.y);
        public bool isout(vecf v) => isout(v.i.x, v.i.y);
        public bool isout(float x, float y) => isout((int)x, (int)y);
        public bool isout(int x, int y) => x < 0 || y < 0 || x >= w || y >= h;
        public bool isout(byte[,] g, float x, float y)
        {
            int w = g.GetLength(0);
            int h = g.GetLength(1);
            return x < 0 || y < 0 || x + w >= this.w || y + h >= this.h;
        }

        public static int TSZ => Tile.TSZ;
    }
}
