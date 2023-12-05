using System;
using System.Drawing;
using System.Windows.Forms;
using Tooling;

namespace KubeLayers
{
    public class GCore : Core
    {
        private static GCore m_Instance = null;
        public static GCore Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new GCore();
                return m_Instance;
            }
        }

        public Bitmap ImageHUDUp, ImageHUDDown;
        public Graphics gHUDUp, gHUDDown;
        public int W, H;
        public Map Map = new Map();
        public Cam Cam = new Cam();

        public override PointF CenterPoint => (W/2, H/2).P();

        public override void Init(PictureBox render, Action update, Action draw)
        {
            base.Init(render, update, draw);

            ImageHUDUp = new Bitmap(iRW, 64);
            gHUDUp = Graphics.FromImage(ImageHUDUp);
            ImageHUDDown = new Bitmap(iRW, 64);
            gHUDDown = Graphics.FromImage(ImageHUDDown);

            W = render.Width;
            H = render.Height - 64 * 2;

            Image = new Bitmap(W, H);
            g = Graphics.FromImage(Image);

            Map.Gen();
        }
    }
}
