using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tooling;
using WindowsFormsApp24.Events;
using WindowsFormsApp24.Properties;
using WindowsFormsApp24.Scenes;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24
{
    internal class Core
    {
        static Core m_Instance = null;

        static internal Core Instance { get { if (m_Instance == null) m_Instance = new Core(); return m_Instance; } }
        static internal readonly int TileSize = 32;
        internal static Bitmap[,] Textures;
        internal static Dictionary<NamedObjects, Bitmap> NamedTextures;

        internal Dictionary<InputNames, int> KeysBinding;
        internal const int MouseLeft = 100001, MouseRight = 100002, MouseMiddle = 100003, MouseButton1 = 100004, MouseButton2 = 100005;
        internal PictureBox Render;

        internal Core()
        {
            KeysBinding = new Dictionary<InputNames, int>()
            {
                [InputNames.Up] = (int)KB.Key.Z,
                [InputNames.Left] = (int)KB.Key.Q,
                [InputNames.Down] = (int)KB.Key.S,
                [InputNames.Right] = (int)KB.Key.D,
                [InputNames.Primary] = MouseLeft,
                [InputNames.Secondary] = MouseRight,
                [InputNames.Ok] = MouseLeft,
                [InputNames.Cancel] = MouseRight,
                [InputNames.Menu] = (int)KB.Key.LeftCtrl,
            };

            var objects = Resources.objects;
            objects.MakeTransparent(Color.White);
            Textures = objects.Split2D(32);

            NamedTextures = new Dictionary<NamedObjects, Bitmap>()
            {
                // Plants
                [NamedObjects.Carrot]               = Textures[1, 17],
                [NamedObjects.Onion]                = Textures[2, 17],
                [NamedObjects.Potatoe]              = Textures[3, 17],
                [NamedObjects.Healherb]            = Textures[6, 18],
                [NamedObjects.Salad]                 = Textures[7, 18],
                [NamedObjects.Pepper]               = Textures[8, 18],
                [NamedObjects.Tomatoe]           = Textures[9, 18],

                // Tools
                [NamedObjects.Shovel]               = Textures[10, 14],
                [NamedObjects.WateringCan]      = Textures[5, 12],

                // Miscs
                [NamedObjects.Bag]                    = Textures[1, 14],
                [NamedObjects.EventContainer]  = Resources.EventContainer,
                [NamedObjects.ClosetContainer]  = Resources.closet,
            };
        }
        internal void ResetGraphics()
        {
            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);

            ImageUI = new Bitmap(Render.Width, Render.Height);
            gUI = Graphics.FromImage(ImageUI);
        }
        internal void WriteGraphics()
        {
            g.DrawImage(ImageUI, 0, 0);
            Render.Image = RenderImage.Resize((int)(Cam.Zoom * RenderImage.Width), (int)(Cam.Zoom * RenderImage.Height));
        }

        internal SceneBase CurrentScene = new SceneMain();// debug temp set
        internal static SceneMain CurrentMainScene => Instance.CurrentScene as SceneMain;
        internal static Character MainCharacter => CurrentMainScene.MainCharacter;
        internal Bitmap RenderImage, ImageUI;
        internal Graphics g, gUI;
        private long m_Ticks = 0;
        internal long Ticks
        {
            get => m_Ticks;
            set
            {
                if (m_Ticks + value >= long.MaxValue - 100)
                    m_Ticks = 0;
                else
                    m_Ticks = value;
            }
        }

        internal static Cam Cam => CurrentMainScene?.Cam;

        internal static bool GetInput(InputNames input, bool pressedInsteadDown = false)
        {
            int v = Instance.KeysBinding[input];
            if (v == MouseLeft) return MouseStates.IsButtonDown(MouseButtons.Left, pressedInsteadDown);
            if (v == MouseRight) return MouseStates.IsButtonDown(MouseButtons.Right, pressedInsteadDown);
            if (v == MouseMiddle) return MouseStates.IsButtonDown(MouseButtons.Middle, pressedInsteadDown);
            if (v == MouseButton1) return MouseStates.IsButtonDown(MouseButtons.XButton1, pressedInsteadDown);
            if (v == MouseButton2) return MouseStates.IsButtonDown(MouseButtons.XButton2, pressedInsteadDown);
            return pressedInsteadDown ? KB.IsKeyPressed((KB.Key)v) : KB.IsKeyDown((KB.Key)v);
        }
    }
}
