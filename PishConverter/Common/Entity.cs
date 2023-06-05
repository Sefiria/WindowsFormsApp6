using System.Numerics;

namespace PishConverter.Common
{
    internal class Entity
    {
        public bool Exists = true;
        public Vector2 Position;

        public virtual void Update()
        {
        }
    }
}
