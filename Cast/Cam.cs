using Cast.Utilities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using static Cast.Tools.KB;

namespace Cast
{
    public class Cam
    {
        public PointF Position = PointF.Empty;
        public float Angle = -90F;
        public float FOV = 30F;
        public float NearVal = 50F;
        public float Far = 400F;

        public static float SpeedMove = 2F;
        public static float SpeedTurn = 2F;
        public static SolidBrush BrushW = new SolidBrush(Color.FromArgb(40, 40, 40));
        public GraphicsPath PathW(float X, float Y, float S = 10F) => new GraphicsPath(
            new PointF[] { (X,          Y+4*S).P(),
                                  (X+6*S,   Y).P(),
                                  (X+16*S, Y+16*S).P(),
                                  (X+4*S,   Y+16*S).P(),
                                  (X,          Y+4*S).P(),
            },
            new byte[] { 0, 1, 1, 1, 1 } );

        public PointF Near => Position;
        public PointF Forward => Maths.Normalized(new PointF(Far, 0).Rotate(Angle));
        public PointF Left => Maths.Normalized(new PointF(Far, 0).Rotate(Angle - 90));
        public PointF Right => Maths.Normalized(new PointF(Far, 0).Rotate(Angle + 90));
        public PointF LookFovLeft => Core.CenterPoint.Plus(new PointF(Far, 0).Rotate(Angle - FOV / 2));
        public PointF LookFovRight => Core.CenterPoint.Plus(new PointF(Far, 0).Rotate(Angle + FOV / 2));
        public float X => Position.X;
        public float Y => Position.Y;


        float mvmW = 0F, shW = 0F;


        public Cam()
        {
        }

        public void Update()
        {
            bool Z = IsKeyDown(Key.Z);
            bool S = IsKeyDown(Key.S);
            bool Q = IsKeyDown(Key.Q);
            bool D = IsKeyDown(Key.D);
            bool A = IsKeyDown(Key.A);
            bool E = IsKeyDown(Key.E);
            bool Up = IsKeyPressed(Key.Up);

            if (Z) Position = Position.PlusF(Forward.x(SpeedMove));
            if (S) Position = Position.Minus(Forward.x(SpeedMove));
            if (Q) Angle -= SpeedTurn;
            if (D) Angle += SpeedTurn;
            if (A) Position = Position.PlusF(Left.x(SpeedMove));
            if (E) Position = Position.PlusF(Right.x(SpeedMove));
            if (Up) Shot();

            if(Z || S || A || E)
            {
                var spd = 0.05F;
                mvmW += spd;
                if (mvmW > 1F + spd) mvmW = 0F;
            }

            if (shW < 0F) shW += 0.1F;
            if (shW > 0F) shW = 0F;
        }

        public void DrawHUD()
        {
            var c = (float)Math.Cos((mvmW * 360F).ToRadians()) * 4F;
            var s = (float)Math.Sin((mvmW * 360F).ToRadians()) * 8F;
            Core.g.FillPath(BrushW, PathW(Core.RW - 128 + c - shW * 26, Core.RH - 128 + s - shW * 32));
        }

        public void Shot()
        {
            shW = -1F;
            var cam = Core.Cam;
            new Bullet(cam.Position.PlusF(cam.Forward.x(Bullet.Size)), cam.Forward);
        }
    }
}
