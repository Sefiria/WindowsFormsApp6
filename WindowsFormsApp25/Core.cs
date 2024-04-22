using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp25
{
    internal class Core
    {
        public enum Renders
        {
            Tiles, Entities, Particles, UI
        }

        private static Core m_Instance = null;
        public static Core Instance => m_Instance ?? (m_Instance = new Core());
        public static int ChunkSize = 10;
        public static int TileSize => Instance.ScreenWidth / ChunkSize;

        public Dictionary<Renders, RenderParam> RenderImages = new Dictionary<Renders, RenderParam>();
        public Bitmap FinalImage;
        public PictureBox Canvas;
        public long Ticks;
        public List<Entity> Entities;
        public List<Tile> Tiles;

        public int ScreenWidth => Canvas.Width;
        public int ScreenHeight => Canvas.Height;

        public Core()
        {
        }
        public void InitializeCore(ref PictureBox canvas)
        {
            Canvas = canvas;
            Ticks = 0;
            Entities = new List<Entity>();

            var list = Enum.GetValues(typeof(Renders));
            foreach (Renders name in list)
                RenderImages[name] = new RenderParam();
            RenderImages[Renders.Tiles].auto_clear = false;
            FinalImage = new Bitmap(ScreenWidth, ScreenHeight);

            GenerateScene();
        }
        public void ResetGraphics()
        {
            if (Canvas.Width == 0 || Canvas.Height == 0) return;
            var list = Enum.GetValues(typeof(Renders));
            foreach (Renders name in list)
            {
                if (RenderImages[name].g == null || RenderImages[name].auto_clear)
                {
                    RenderImages[name].img = new Bitmap(Canvas.Width, Canvas.Height);
                    RenderImages[name].g = Graphics.FromImage(RenderImages[name].img);
                    RenderImages[name].g.Clear(Color.Transparent);
                }
                else
                {
                    RenderImages[name].g = Graphics.FromImage(RenderImages[name].img);
                }
            }
        }
        public void GenerateScene()
        {
            //Tiles = new Tile[ChunkSize, ChunkSize];
            Tiles = new List<Tile>();
            for (int y = 0; y < ChunkSize; y++)
            {
                for (int x = 0; x < ChunkSize; x++)
                {
                    //Tiles[x, y] = new Tile(x, y, DB.Tex.Floor);
                    Tiles.Insert(x + y * ChunkSize, new Tile(x, y, DB.Tex.Floor));
                }
            }

            EntityFactory.CreateTable(1, 1);
            new Controllable(1, 1);
        }

        public void Update()
        {
            var entities = new List<Entity>(Entities);
            foreach (Entity entity in entities)
            {
                if (entity.Exists)
                    entity.Update();
                else
                    Entities.Remove(entity);
            }
            ParticlesManager.Update();
            Ticks++;
        }

        public void Draw()
        {
            ResetGraphics();
            DrawScene();
            ParticlesManager.Draw(RenderImages[Renders.Particles].g);

            // Merge renders
            var list = Enum.GetValues(typeof(Renders));
            using (var g = Graphics.FromImage(FinalImage))
                foreach (Renders name in list)
                    g.DrawImage(RenderImages[name].img, Point.Empty);
            Canvas.Image = FinalImage;
        }
        private void DrawScene()
        {
            //var img = DB.GetTex(DB.Tex.Floor);
            //for (int y = 0; y < ChunkSize; y++)
            //{
            //    for (int x = 0; x < ChunkSize; x++)
            //    {
            //        g.DrawImage(img, x * TileSize, y * TileSize);
            //    }
            //}
            Tiles.Where(t => t.Invalidated).ToList().ForEach(t => t.Draw(RenderImages[Renders.Tiles].g));
            Entities.Where(e => e.Exists).OrderBy(e => e.Y).ToList().ForEach(e => e.Draw(RenderImages[Renders.Entities].g));
        }
    }
}
