using System.Collections.Generic;
using System.Linq;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15
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

        public Dictionary<vecf, Structure> Structures = new Dictionary<vecf, Structure>();
        public List<Item> Items = new List<Item>();

        public void Init()
        {
        }

        public bool ThereStructureAt(vecf where) => Structures.Any(pair => pair.Key == where);
        public Structure GetStructureAt(vecf where)
        {
            return Structures.FirstOrDefault(pair => pair.Key == where).Value;
        }
    }
}
