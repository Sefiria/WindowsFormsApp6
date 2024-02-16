using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;
using WindowsFormsApp24.Events;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24
{
    internal class Map
    {
        static internal readonly int LAYERS = 3;
        static internal Map Current => (Core.Instance.CurrentScene as Scenes.SceneMain).Map;

        internal int ID;
        internal string Name;
        internal int W, H;
        internal TileArray<Tile> Tiles;
        internal Bitmap TilesRender;
        internal Graphics gTilesRender;
        internal List<Bitmap> Tileset;
        internal List<Event> Events = new List<Event>();
        internal RectangleF ScreenBounds => new RectangleF(Core.Cam.Position.MinusF(Core.TileSize), new SizeF((Core.Instance.RenderImage.Size.Width + Core.TileSize * 2F) / Core.Cam.Zoom, (Core.Instance.RenderImage.Size.Height + Core.TileSize * 2F) / Core.Cam.Zoom));
        internal bool IsOnScreen(float x, float y) => ScreenBounds.Contains(x, y);
        internal bool IsOnScreen(Event ev) => ScreenBounds.Contains(ev.X, ev.Y);
        internal bool IsOnScreen(Tile tile) => IsOnScreen(tile.x, tile.y);
        internal bool IsOnScreen(int tile_x, int tile_y) => IsOnScreen((float)tile_x * Core.TileSize, (float)tile_y * Core.TileSize);
        internal List<Event> GetOnScreenEvents()
        {
            var rect = ScreenBounds;
            return Events.Where(ev => rect.Contains(ev.Position)).ToList();
        }
        internal void DoOnScreenTiles(Action<int, int> func)
        {
            var ts = Core.TileSize;
            var rect = new Rectangle(Core.Cam.Position, Core.Instance.RenderImage.Size);
            for(int x= rect.X / ts; x - rect.X / ts < rect.Width / ts; x++)
                for(int y= rect.Y / ts; y - rect.Y / ts < rect.Height / ts; y++)
                    func(x, y);
        }
        internal DrawingPart DrawingPart;
        internal Queue<(int layer, int x, int y)> TilesToRefresh = new Queue<(int layer, int x, int y)>();
        internal bool complete_refresh = true;
        internal int SunOldTicks = -1;
        internal float SunTicks = 0F, SunTickRate = 0.002F;
        internal bool SunMoved => SunOldTicks != SunTicks;

        internal Guid AddEvent(Event ev) { Events.Add(ev); return Events.Last().Guid; }
        internal static Guid AddEventToCurrent(Event ev) { Current.Events.Add(ev); return Current.Events.Last().Guid; }
        internal static Event GetEvent(Guid guid) => Current.Events.FirstOrDefault(ev => ev.Guid == guid);
        internal static Event GetEvent(Type type) => Current.Events.FirstOrDefault(ev => ev.GetType() == type);
        internal static void SetEvent(Guid guid, Event ev) => Current.Events[Current.Events.IndexOf(GetEvent(guid))] = ev;

        public Map(int w, int h, Bitmap tileset, string name = "")
        {
            ID = (Core.Instance.CurrentScene as Scenes.SceneMain).Maps.Count;
            if (w <= 0) throw new ArgumentOutOfRangeException("w", "Map:ctor : Width has to be > 0");
            if (h <= 0) throw new ArgumentOutOfRangeException("h", "Map:ctor : Height has to be > 0");
            Name = name;
            W = w;
            H = h;
            tileset.MakeTransparent(Color.White);
            Tileset = tileset.Split(x:32, y:32);
            Tiles = new TileArray<Tile>(LAYERS, W, H);
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    Tiles[0, x, y] = new Tile(0, 0, x, y);
                    TilesToRefresh.Enqueue((0, x, y));
                }
        }

        public void Update()
        {
            ChunkerizedWork();
        
            var events = new List<Event>(Events);
            foreach (var ev in events)
            {
                if (!ev.Exists)
                    Events.Remove(ev);
                ev.Update();
            }

            Core.Instance.Ticks++;

            if (SunOldTicks != (int)SunTicks)
                SunOldTicks = (int)SunTicks;
            SunTicks += SunTickRate;
            while (SunTicks >= 32F) SunTicks -= 32F;
            while (SunTicks < 0F) SunTicks += 32F;

            UIMouseAssist.Update();
        }

        internal void ResetGraphics()
        {
            TilesRender = new Bitmap(Core.Instance.Render.Width, Core.Instance.Render.Height);
            gTilesRender = Graphics.FromImage(TilesRender);
            gTilesRender.Clear(Color.Black);
        }
        public void Draw()
        {
            var scene = Core.Instance.CurrentScene as Scenes.SceneMain;
            var cam = Core.Cam;
            int X, Y;
            Tile v;

            if (complete_refresh)
            {
                ResetGraphics();
                for (int layer = 0; layer < LAYERS; layer++)
                    for (int y = 0; y < H; y++)
                        for (int x = 0; x < W; x++)
                            TilesToRefresh.Enqueue((layer, x, y));
                complete_refresh = false;
            }

            bool dohighlight = Character.MainHandObjectDefined && Character.MainHandEvent is Seed;
            while (TilesToRefresh.Count > 0)
            {
                var t = TilesToRefresh.Dequeue();
                v = Tiles[t.layer, t.x, t.y];
                if (v != null)// first check to avoid an useless for loop
                {
                    for (int layer = t.layer; layer < LAYERS; layer++)
                    {
                        v = Tiles[layer, t.x, t.y];
                        if (v != null)
                        {
                            var img = v.wet > 0F ? Tileset[v.TilesetIndex].GetAdjusted(brightness: 1F - v.wet * 0.1F) : Tileset[v.TilesetIndex];
                            if (dohighlight && v.TilesetIndex == 1 && IsOnScreen(v) && !Events.Any(ev => ev.TilePosition == new Point(v.x, v.y) && ev is Crop))
                                img = img.GetAdjusted(brightness: 1F + 0.2F * ((Core.Instance.Ticks + 15) % 30) / 30F - 0.2F * (Core.Instance.Ticks % 30) / 30F);
                            gTilesRender.DrawImage(img, t.x * Core.TileSize - (int)cam.X, t.y * Core.TileSize - (int)cam.Y);
                        }
                    }
                }
            }

            Core.Instance.g.DrawImage(TilesRender, Point.Empty);

            {
                (int x, int y) = scene.MainCharacter.Position.Div(Core.TileSize).ToTupleInt();
                Core.Instance.g.FillRectangle(new SolidBrush(Color.FromArgb(50, 255, 255, 255)), x * Core.TileSize - Core.Cam.X, y * Core.TileSize - Core.Cam.Y, Core.TileSize, Core.TileSize);
            }


            var onscreenevents = GetOnScreenEvents();
            var listZ = onscreenevents.Select(ev => ev.Z).Distinct().OrderBy(z => z).ToList();
            if (listZ.Count == 0)
            {
                scene.MainCharacter.DrawShadow(SunMoved);
                UIMouseAssist.Draw();
                DrawingPart = DrawingPart.Bottom;
                scene.MainCharacter.Draw();
                DrawingPart = DrawingPart.Top;
                scene.MainCharacter.Draw();
            }
            else
            {
                for (int i = 0; i < listZ.Count; i++)
                {
                    onscreenevents.Where(ev => ev.Z == listZ[i]).ToList().ForEach(ev => ev.DrawShadow(SunMoved));
                    if (scene.MainCharacter.Z == listZ[i])
                        scene.MainCharacter.DrawShadow(SunMoved);
                }

                for (int i = 0; i < listZ.Count; i++)
                {
                    UIMouseAssist.Draw();
                    DrawingPart = DrawingPart.Bottom;
                    onscreenevents.Where(ev => ev.Z == listZ[i]).ToList().ForEach(ev => ev.Draw());
                    if (scene.MainCharacter.Z == listZ[i])
                        scene.MainCharacter.Draw();
                    DrawingPart = DrawingPart.Top;
                    if (scene.MainCharacter.Z == listZ[i])
                        scene.MainCharacter.Draw();
                    onscreenevents.Where(ev => ev.Z == listZ[i]).ToList().ForEach(ev => ev.Draw());
                }
            }

            //onscreenevents.ForEach(ev => Core.Instance.gUI.DrawRectangle(new Pen(Color.Red, 4F), ev.RealTimeBounds.ToIntRect()));//debug
            //Core.Instance.gUI.DrawRectangle(new Pen(Color.Red, 4F), scene.MainCharacter.RealTimeBounds.ToIntRect());//debug

            var a = (0.85F - Maths.Abs(Maths.Abs(180F - (Map.Current.SunTicks / 32F * 360F)) / 90F - 1F)) * 0.5F;
            Core.Instance.g.DrawImage(new Bitmap(Core.Instance.RenderImage.Width, Core.Instance.RenderImage.Height).CreateRectangle(Color.DarkBlue, a), Point.Empty);
        }

        public bool IsCrop(int tile_x, int tile_y) => Tiles.PointedTile(tile_x, tile_y)?.TilesetIndex == 1;
        public bool IsEventOnScreen(Event ev) => new RectangleF(Core.Cam.Position, new SizeF(Core.Instance.RenderImage.Size)).Contains(ev.Position);

        internal int chunk_x=0, chunk_y=0, chunk_size=8;
        public void ChunkerizedWork()
        {
            bool any = true;
            for(int l=0;l<LAYERS && any; l++)
            {
                any = false;
                for (int x = chunk_x * chunk_size; x - chunk_x * chunk_size < chunk_size; x++)
                {
                    for (int y = chunk_y * chunk_size; y - chunk_y * chunk_size < chunk_size; y++)
                    {
                        if (Tiles[l, x, y] == null)
                            continue;
                        any = true;
                        ChunkerizedWork_Execute(l, x, y);
                    }
                }
            }
            chunk_y++;
            if (chunk_y * chunk_size > H)
            {
                chunk_y = 0;
                chunk_x++;
                if (chunk_x * chunk_size > W)
                {
                    chunk_x = 0;
                }
            }
        }
        public void ChunkerizedWork_Execute(int l, int x, int y)
        {
            var tile = Tiles[l, x, y];
            tile.wet = Math.Max(0F, tile.wet - (1F - tile.liquid_absorption));
        }
    }
}