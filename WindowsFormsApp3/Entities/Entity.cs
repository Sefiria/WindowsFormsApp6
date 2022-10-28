using System;
using System.Drawing;

namespace WindowsFormsApp3.Entities
{
    public abstract class Entity
    {
        public Guid ID;
        public bool Exists;
        public float X { get; set; }
        public float Y { get; set; }

        public Vecf Look = new Vecf();

        public Entity(float X = 0F, float Y = 0F)
        {
            ID = Guid.NewGuid();
            Exists = true;
            this.X = X;
            this.Y = Y;
        }

        public virtual void Update() { }
    }
}
