using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class Card
    {
        public Point Coord;
        public Card Prev, Next;
        public Card(Point coord, Card prev, Card next)
        {
            Coord = coord;
            Prev = prev;
            Next = next;
        }
    }
}
