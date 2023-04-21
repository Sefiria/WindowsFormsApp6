using System.Collections.Generic;
using System.Drawing;
using WindowsFormsApp19.Map;
using WindowsFormsApp19.Utilities;

namespace WindowsFormsApp19
{
    internal class Data
    {
        private static Data m_Instance = null;
        public static Data Instance
        {
            get
            {
                if(m_Instance == null)
                    m_Instance = new Data();
                return m_Instance;
            }
        }

        public MapMgr MapMgr;
        public vecf cam;
        public User User;

        public void Init()
        {
            MapMgr = new MapMgr();
            User = new User((MapMgr.w / 2 - 0.5F) * MapMgr.ChunkSz * Core.TSZ, (MapMgr.h / 2 - 0.5F) * MapMgr.ChunkSz * Core.TSZ);
        }
    }
}
