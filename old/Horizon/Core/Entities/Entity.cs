using static _Console.Core.Utils.Maths;

namespace Core.Entities
{
    public abstract class Entity
    {
        public Vec Position = Vec.Zero;
        public Vec2 Look = Vec2.Zero;
    }
}
