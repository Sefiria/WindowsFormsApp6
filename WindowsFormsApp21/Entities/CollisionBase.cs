using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp21.Utilities;

namespace WindowsFormsApp21.Entities
{
    public class CollisionBase : ICollision
    {
        public enum CollisionType
        {
            Circle, Square
        }

        public CollisionType Type { get; set; }

        public bool Collides(ICollision other)
        {
            if (Type == CollisionType.Circle)
            {
                var CircleA = (CollisionCircle)this;
                if (other.Type == CollisionType.Circle)
                {
                    var CircleB = (CollisionCircle)other;
                    return Maths.CollisionCercleCercle(CircleA, CircleB);
                }
                else if (other.Type == CollisionType.Square)
                {
                    var SquareB = (CollisionSquare)other;
                    return Maths.CollisionCercleBox(CircleA, SquareB) > 0;
                }
            }
            else if (Type == CollisionType.Square)
            {
                var SquareA = (CollisionSquare)this;
                if (other.Type == CollisionType.Circle)
                {
                    var CircleB = (CollisionCircle)other;
                    return Maths.CollisionCercleBox(CircleB, SquareA) > 0;
                }
                else if (other.Type == CollisionType.Square)
                {
                    var SquareB = (CollisionSquare)other;
                    return Maths.CollisionBoxBox(SquareA, SquareB);
                }
            }
            return false;
        }
    }
}
