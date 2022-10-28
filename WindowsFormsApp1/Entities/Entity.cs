using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp1.Entities.Enumerations;

namespace WindowsFormsApp1.Entities
{
    public abstract class Entity
    {
        public Guid ID;
        public bool Exists;
        public bool Traversable;
        public float X { get; set; }
        public float Y { get; set; }

        public int TX => (int)(X / SharedCore.TileSize);
        public int TY => (int)(Y / SharedCore.TileSize);


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
