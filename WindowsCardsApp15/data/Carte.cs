using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsCardsApp15.Utilities;

namespace WindowsCardsApp15.data
{
    internal class Carte : Box, ICarte
    {
        public bool Jouable = true;
        public List<Materia> RequisPourJouer = new List<Materia>();
        public byte ID;

        public void Draw()
        {
        }
    }
}
