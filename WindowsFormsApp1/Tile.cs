using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Entities;

namespace WindowsFormsApp1
{
    public class Tile
    {
        public int X, Y;
        public Entity LinkedEntity;

        public Tile(int X, int Y, Entity LinkedEntity)
        {
            this.X = X;
            this.Y = Y;
            this.LinkedEntity = LinkedEntity;

        }
    }
}
