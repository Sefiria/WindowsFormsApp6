using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using AssetMake.main;
using AssetMake.Properties;
using AssetMake.Utilities;

namespace AssetMake
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

        public vecf cam;
        public GlobalManager GlobalManager;

        public void Init()
        {
            cam = vecf.Zero;
            GlobalManager = new GlobalManager();
        }
    }
}
