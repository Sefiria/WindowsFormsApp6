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

        public static List<Entity> CurrentEntities => Instance.CurrentScene.Entities;

        public SceneAdventure SceneAdventure;
        public Scene CurrentScene;
        public Bitmap RenderImage, UIImage;
        public vecf RenderCenter;
        public Graphics g, gui;
        public PictureBox Canvas;
        public Guy TheGuy;

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
            SceneAdventure = new SceneAdventure();
            CurrentScene = SceneAdventure;
            SceneAdventure.Initialize();
            TheGuy = new Guy();

            // temp debug
            for(int i=0;i<99;i++)
                TheGuy.Inventory.Items.Add(new Item(string.Concat(Enumerable.Repeat((char) RandomThings.rnd(48, 122), 10)), RandomThings.rnd(1, 99)));
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
        }

        public void Draw()
        {
            ResetGraphics();
            CurrentScene?.Draw(g);
            g.DrawImage(UIImage, 0f, 0f);
            Canvas.Image = RenderImage;
        }

        public enum Scenes { Adventure, Menu }
        internal void SwitchScene(Scenes scene)
        {
            switch(scene)
            {
                default:return;
                case Scenes.Adventure: CurrentScene = SceneAdventure; break;
                case Scenes.Menu: CurrentScene = new SceneMenu(); break;
            }
        }
    }
}
