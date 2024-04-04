using console_v2.res.entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Schema;
using Tooling;

namespace console_v2
{
    internal class Core
    {
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
        public List<Shortcut> Shortcuts;

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
            var wooden_axe = new Tool("Hache en Bois", Outils.Hache, 3);
            var wooden_scythe = new Tool("Faux en Bois", Outils.Faux, 1);
            var wooden_shovel = new Tool("Pelle en Bois", Outils.Pelle, 1);
            var wooden_hammer = new Tool("Masse en Bois", Outils.Masse, 1);
            TheGuy.Inventory.Add(
                wooden_axe,
                wooden_scythe,
                wooden_shovel,
                wooden_hammer
                );
            Shortcuts = new List<Shortcut>
            {
                new Shortcut(0, KB.Key.C, wooden_shovel),
            };
            new EntityStructure((5, 5).V(), Structures.PetitAtelier);
        }
        public void ResetGraphics()
        {
            if (Canvas.Width == 0 || Canvas.Height == 0) return;
            bool first_init = g == null;
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

            if (first_init)
            {
                GraphicsManager.CharSize = g.MeasureString("A", GraphicsManager.Font, GraphicsManager.Font.Height, StringFormat.GenericTypographic);
            }

            g.Clear(Color.Black);
            gui.Clear(Color.Transparent);
            //g.Clip = new Region(SceneAdventure.DrawingRect);
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
