using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ResourceMgr
{
    public class ResourceManager
    {
        static private ResourceManager m_Instance = null;
        static public ResourceManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new ResourceManager();
                return m_Instance;
            }
        }
        static public void KillInstance()
        {
            Dispose();
            m_Instance = null;
        }


        private Dictionary<string, Bitmap> Resources = null;


        private ResourceManager()
        {
            Resources = new Dictionary<string, Bitmap>()
            {
                ["PlayerShip"] = (Bitmap)Image.FromFile($"{Directory.GetCurrentDirectory()}\\Resources\\Ships\\PlayerShip.png"),
                ["ArmamentSimple"] = (Bitmap)Image.FromFile($"{Directory.GetCurrentDirectory()}\\Resources\\Armaments\\ArmamentSimple.png")
            };

            string[] keys = Resources.Keys.ToArray();
            foreach (string key in keys)
            {
                Bitmap source = Resources[key];
                source.MakeTransparent();
                Bitmap img = Resize(source, source.Width * 2, source.Height * 2);
                Resources[key] = img;
            }
        }
        static public void Dispose()
        {
            if (m_Instance == null)
                return;

            foreach (Bitmap resource in m_Instance.Resources.Values)
                resource.Dispose();
            m_Instance.Resources.Clear();
            m_Instance.Resources = null;
        }

        public Bitmap GetResource(string key) => Resources.ContainsKey(key) ? Resources[key] : null;

        private Bitmap Resize(Bitmap img, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);
            Graphics graph = Graphics.FromImage(bmp);
            graph.InterpolationMode = InterpolationMode.NearestNeighbor;
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias;
            graph.DrawImage(img, new Rectangle(0, 0, w, h));
            graph.Dispose();
            return bmp;
        }
    }
}
