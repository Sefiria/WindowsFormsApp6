using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsCardsApp15.Utilities;

namespace WindowsCardsApp15.data
{
    internal class CarteHistoire : Box, ICarte
    {
        public byte ID;
        public byte data1, data2;

        public void Draw()
        {
        }
    }
}
