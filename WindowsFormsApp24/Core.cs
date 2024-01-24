using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp24.Scenes;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24
{
    internal class Core
    {
        static Core m_Instance = null;

        public static Core Instance { get { if (m_Instance == null) m_Instance = new Core(); return m_Instance; } }

        public Dictionary<InputNames, int> KeysBinding;
        public const int MouseLeft = 1, MouseRight = 2, MouseMiddle = 3, MouseButton1 = 4, MouseButton2 = 5;
        public PictureBox Render;

        public Core()
        {
            KeysBinding = new Dictionary<InputNames, int>()
            {
                [InputNames.Up] = (int)Keys.Up,
                [InputNames.Left] = (int)Keys.Left,
                [InputNames.Down] = (int)Keys.Down,
                [InputNames.Right] = (int)Keys.Right,
                [InputNames.Primary] = MouseLeft,
                [InputNames.Secondary] = MouseRight,
                [InputNames.Ok] = MouseLeft,
                [InputNames.Cancel] = MouseRight,
                [InputNames.Menu] = (int)Keys.LControlKey,
            };
        }
        public void ResetGraphics()
        {
            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);
        }
        public void WriteGraphics()
        {
            Render.Image = RenderImage;
        }

        public SceneBase CurrentScene = new SceneMain();// debug temp set
        public Bitmap RenderImage;
        public Graphics g;
    }
}
