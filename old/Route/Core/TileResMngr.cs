using Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class TileResMngr
    {
        public class TileInfo
        {
            public Tile Tile;
            public int Index;
            public Bitmap Image;
            public Bitmap ImageWithSigns;

            public TileInfo(Tile Tile)
            {
                this.Tile = Tile;
                this.Image = (Bitmap)Tools.ByteArrayToObject(this.Tile.ImageBytes);
                this.Index = this.Tile.Index;

                this.ImageWithSigns = new Bitmap(this.Image);
                using (Graphics g = Graphics.FromImage(ImageWithSigns))
                {
                    foreach (TileSignObj sign in Tile.pathDotsSign.Values)
                    {
                        if (sign.Index == 0)
                            continue;

                        g.DrawImage(Tools.ApplyRotation(TileResMngr.Instance.ResourcesSignInfo[sign.Index].Image, sign.Angle), sign.Position);
                        if (sign.ShowBar)
                        {
                            Point pt = sign.Position;
                            g.DrawLine(new Pen(Color.LightGray, 3F), new Point(pt.X + 6, pt.Y + 16), new Point(pt.X + 6, pt.Y + 24));
                            g.DrawLine(new Pen(Color.DimGray, 3F), new Point(pt.X + 8, pt.Y + 16), new Point(pt.X + 8, pt.Y + 24));
                        }
                    }
                }
            }
        }
        public class SignInfo
        {
            public Sign Sign;
            public int Index;
            public Bitmap Image;
            public string Name;

            public SignInfo(string entry)
            {
                Sign = Tools.DeserializeJSONFromFile<Sign>(entry);
                Image = (Bitmap)Tools.ByteArrayToObject(this.Sign.ImageBytes);
                Index = Sign.Index;
                Name = Path.GetFileNameWithoutExtension(entry);
            }
        }

        private static TileResMngr m_Instance = null;
        private TileResMngr()
        {
            ResourcesTileInfo = new Dictionary<int, TileInfo>();
            ResourcesSignInfo = new Dictionary<int, SignInfo>();
        }

        public static TileResMngr Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new TileResMngr();
                return m_Instance;
            }
        }
        public void Kill()
        {
            m_Instance = null;
        }


        public Dictionary<int, TileInfo> ResourcesTileInfo;
        public Dictionary<int, SignInfo> ResourcesSignInfo;


        public void Initialize(string ResourcesPath)
        {
            List<string> entries = new List<string>();
            entries.AddRange(Directory.GetFiles(ResourcesPath, "*.sign", SearchOption.AllDirectories));
            SignInfo signInfo;
            foreach (string entry in entries)
            {
                signInfo = new SignInfo(entry);
                ResourcesSignInfo[signInfo.Index] = signInfo;
            }

            entries.Clear();
            entries.AddRange(Directory.GetFiles(ResourcesPath, "*.tile", SearchOption.AllDirectories));
            TileInfo tileInfo;
            foreach (string entry in entries)
            {
                tileInfo = new TileInfo(Tools.DeserializeJSONFromFile<Tile>(entry));
                ResourcesTileInfo[tileInfo.Index] = tileInfo;
            }
        }
    }
}
