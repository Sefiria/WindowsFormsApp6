using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace ConfigureRoute.Obj
{
    public class Sign
    {
        public static readonly int type_count = 4;
        public static readonly int Aucun = 0, LigneContinue = 1, LigneDiscontinue = 2, Stop = 3, CederLePassage = 4;

        // sign has only top and left (bottom is the top of the bottom sign, right is the left of the right one)
        // t => type of sign at Top
        // l => type of sign at Left
        public int x, y;
        private int m_t, m_l;
        public int t
        {
            get => m_t;
            set
            {
                m_t = value;
                if (m_t < 0 || m_t > type_count) m_t = 0;
            }
        }
        public int l
        {
            get => m_l;
            set
            {
                m_l = value;
                if (m_l < 0 || m_l > type_count) m_l = 0;
            }
        }

        public vec Position => (x, y).V();
        public Sign()
        {
        }
    }
}
