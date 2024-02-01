using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
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
        internal Tiles Tiles;
        internal List<Bitmap> Tileset;
        internal List<Event> Events = new List<Event>();
        internal DrawingPart DrawingPart;

        public Map(int w, int h, Bitmap tileset, string name = "")
        {
            ID = (Core.Instance.CurrentScene as Scenes.SceneMain).Maps.Count;
            if (w <= 0) throw new ArgumentOutOfRangeException("w", "Map:ctor : Width has to be > 0");
            if (h <= 0) throw new ArgumentOutOfRangeException("h", "Map:ctor : Height has to be > 0");
            Name = name;
            W = w;
            H = h;
            Tiles = new Tiles(LAYERS, W, H);
            tileset.MakeTransparent(Color.White);
            Tileset = tileset.Split(x:32, y:32);
        }

        public void Update()
        {
            var events = new List<Event>(Events);
            foreach (var ev in events)
            {
                if (!ev.Exists)
                    Events.Remove(ev);
                ev.Update();
            }
            Core.Instance.Ticks++;

            UIMouseAssist.Update();
        }

        public void Draw()
        {
            var scene = Core.Instance.CurrentScene as Scenes.SceneMain;
            int v, X, Y;

            for (int layer=0; layer<LAYERS; layer++)
            {
                for (int y = 0; y < H; y++)
                {
                    for (int x = 0; x < W; x++)
                    {
                        v = Tiles[layer, x, y];
                        if (layer == 0 || v != 0)
                        {
                            Core.Instance.g.DrawImage(Tileset[v], x * Core.TileSize, y * Core.TileSize);
                            if(scene.MainCharacter.Position.Div(Core.TileSize) == (x, y).P())
                                Core.Instance.g.FillRectangle(new SolidBrush(Color.FromArgb(50, 255, 255, 255)), x * Core.TileSize, y * Core.TileSize, Core.TileSize, Core.TileSize);
                        }
                    }
                }
            }

            UIMouseAssist.Draw();
            DrawingPart = DrawingPart.Bottom;
            Events.ForEach(ev => ev.Draw());
            scene.MainCharacter.Draw();
            DrawingPart = DrawingPart.Top;
            Events.ForEach(ev => ev.Draw());
            scene.MainCharacter.Draw();

            //Core.Instance.g.FillRectangle(Brushes.Red, scene.MainCharacter.X, scene.MainCharacter.Y, 4, 4);

        }

        public bool IsCrop(int tile_x, int tile_y) => Tiles.PointedIndex(tile_x, tile_y) == 1;
    }
}