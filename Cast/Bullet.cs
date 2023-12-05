using System.Drawing;

namespace Cast
{
    public class Bullet : Entity
    {
        public static float Size = 12;

        public PointF Look = PointF.Empty;
        public float spdmv = 2F;

        public override RectangleF Bounds => new RectangleF(X - Size / 2F, Y - Size / 2F, Size / 2F, Size / 2F);

        public Bullet()
        {
            C = Color.Red;
        }
        public Bullet(PointF pos, PointF look)
        {
            C = Color.Red;
            X = pos.X;
            Y = pos.Y;
            Look = look;
        }
        public override void Update()
        {
            if (!Exists) return;

            base.Update();
            X += Look.X * spdmv;
            Y += Look.Y * spdmv;

            if (!Core.Map.InWorldLimits(Position))
                Exists = false;
        }
        public override void Draw(Graphics g, PointF? offset = null)
        {
            base.Draw(g);
            var rect = Bounds;
            rect.Offset(Core.CenterPoint.PlusF(offset ?? PointF.Empty));
            g.FillRectangle(new SolidBrush(C), rect);
        }
        public override void DrawMinimap(Graphics g)
        {
            base.DrawMinimap(g);
            var rect = Bounds;
            rect.Offset(Core.CenterPoint.Minus(Core.Cam.Position));
            g.FillRectangle(new SolidBrush(C), rect);
        }
    }
}
