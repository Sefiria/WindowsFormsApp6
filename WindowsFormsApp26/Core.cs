using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp26
{
    public class Core
    {
        static Core m_Instance = null;
        public static Core Instance => m_Instance ?? (m_Instance = new Core());
        public static bool DEBUG = false;

        public static List<Entity> CurrentEntities => Instance.CurrentScene.Entities;

        public Scene SceneMachinery;
        public Scene CurrentScene;
        public Bitmap RenderImage, UIImage;
        public Graphics g, gui;
        public PictureBox Canvas;

        public float Gravity = 0.225f;

        public Core()
        {
        }

        public void InitializeCore(ref PictureBox canvas)
        {
            Canvas = canvas;
        }
        public void InitializeScenes()
        {
            SceneMachinery = new Scene_Machinery();
            CurrentScene = SceneMachinery;
            SceneMachinery.Initialize();
        }
        public void ResetGraphics()
        {
            if (Canvas.Width == 0 || Canvas.Height == 0) return;
            RenderImage = new Bitmap(Canvas.Width, Canvas.Height);
            g = Graphics.FromImage(RenderImage);
            UIImage = new Bitmap(Canvas.Width, Canvas.Height);
            gui = Graphics.FromImage(UIImage);
        }

        public void Update()
        {
            CurrentScene?.Update();
        }

        public void Draw()
        {
            ResetGraphics();
            CurrentScene?.Draw(g);
            CurrentScene?.DrawForeground(g);
            g.DrawImage(UIImage, 0f, 0f);
            Canvas.Image = RenderImage;
        }
    }
}
