using System.Drawing;

namespace Cast
{
    public class Wall : Structural
    {
        public Wall(PointF A, PointF B) { _A = A; _B = B; C = Color.White; }
    }
}
