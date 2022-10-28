using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp2.Entities.Enumerations;

namespace WindowsFormsApp2.Entities
{
    public abstract class Entity
    {
        public Guid ID;
        public bool Exists;
        public bool Traversable;
        public float X { get; set; }
        public float Y { get; set; }

        public int TX { get => (int)(X / SharedCore.TileSize); set => X = value * SharedCore.TileSize; }
        public int TY { get => (int)(Y / SharedCore.TileSize); set => Y = value * SharedCore.TileSize; }

        public VecF Look = new VecF();
        public Point TCoords { get => new Point(TX, TY); set { var ts = SharedCore.TileSize; X = value.X * ts + ts / 2; Y = value.Y * ts + ts / 2; } }


        public Entity(float X=0F, float Y=0F)
        {
            ID = Guid.NewGuid();
            Exists = true;
            Traversable = false;
            this.X = X;
            this.Y = Y;
        }

        public abstract void Update();
    }
}
