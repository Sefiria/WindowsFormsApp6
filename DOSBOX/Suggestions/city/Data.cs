using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.city
{
    public class Data
    {

        private static Data m_Instance = null;
        public static Data Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Data();
                    m_Instance.Init();
                }
                return m_Instance;
            }
        }
        public static void KillInstance()
        {
            m_Instance = null;
        }
        public static void LoadInstance(Data _instance) => m_Instance = _instance;


        public User user;
        public CityMap map;


        private void Init()
        {
            map = new CityMap();
            user = new User();
        }
    }
}
