using console_v3.res.entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Schema;
using Tooling;

namespace console_v3
{
    internal class Core
    {
        public static int TILE_SIZE = 32;
        private static Core m_Instance = null;
        public static Core Instance => m_Instance ?? (m_Instance = new Core());
        public static bool DEBUG = false;

        public static List<Entity> CurrentEntities => (Instance.CurrentScene as SceneAdventure).World.CurrentDimension.Chunks[Instance.TheGuy?.CurChunk ?? vec.Zero].Entities;
        public static void AddEntity(Entity e) => (Instance.CurrentScene as SceneAdventure).World.CurrentDimension.Chunks[Instance.TheGuy?.CurChunk ?? vec.Zero].Entities.Add(e);
        public static void AddEntity(Entity e, vec chunk) => (Instance.CurrentScene as SceneAdventure).World.CurrentDimension.Chunks[chunk].Entities.Add(e);
        public static void AddEntity(Entity e, vec dimension, vec chunk) => (Instance.CurrentScene as SceneAdventure).World.Dimensions[dimension].Chunks[chunk].Entities.Add(e);
        public static Entity GetEntityAt(vec tile) => CurrentEntities?.FirstOrDefault(e => e.Position.ToTile() == tile);

        public SceneAdventure SceneAdventure;
        public Scene CurrentScene;
        public Bitmap RenderImage, UIImage;
        public vecf RenderCenter;
        public Graphics g, gui;
        public PictureBox Canvas;
        public Guy TheGuy;
        public long Ticks;
        public List<Shortcut> Shortcuts = new List<Shortcut>();

        public int ScreenWidth => Canvas.Width;
        public int ScreenHeight => Canvas.Height;

        public Core()
        {
        }

        public void InitializeCore(ref PictureBox canvas)
        {
            Canvas = canvas;
            RenderCenter = ((Point)Canvas.Size).Div(2).vecf();
        }
        public void InitializeScenes()
        {
            ResetGraphics();

            Ticks = 0;
            SceneAdventure = new SceneAdventure();
            CurrentScene = SceneAdventure;
            SceneAdventure.Initialize();
            TheGuy = new Guy();
            new EntityStructure((10, 10).V(), (int)DB.Tex.TorchOn);
        }
        public void ResetGraphics()
        {
            if (Canvas.Width == 0 || Canvas.Height == 0) return;
            if (g == null)
            {
                RenderImage = new Bitmap(Canvas.Width, Canvas.Height);
                g = Graphics.FromImage(RenderImage);
            }
            if (gui == null)
            {
                UIImage = new Bitmap(Canvas.Width, Canvas.Height);
                gui = Graphics.FromImage(UIImage);
            }

            g.Clear(Color.Black);
            gui.Clear(Color.Transparent);
        }

        public void Update()
        {
            CurrentScene?.Update();
            ParticlesManager.Update();
            Ticks++;
        }

        public void TickSecond()
        {
            CurrentScene?.TickSecond();
        }


        public void Draw()
        {
            ResetGraphics();
            CurrentScene?.Draw(g, gui);
            ParticlesManager.Draw(g);
            g.DrawImage(UIImage, 0f, 0f);
            Canvas.Image = RenderImage;
        }

        public enum Scenes { Adventure, Menu, Craft }
        internal void SwitchScene(Scenes scene, params object[] args)
        {
            switch(scene)
            {
                default:return;
                case Scenes.Adventure: CurrentScene = SceneAdventure; break;
                case Scenes.Menu: CurrentScene = new SceneMenu(); break;
                case Scenes.Craft: CurrentScene = new SceneCraft((int)args[0]); break;
            }
        }
    }
}
