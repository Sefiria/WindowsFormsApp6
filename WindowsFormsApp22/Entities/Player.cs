using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Tooling;
using static Tooling.KB;
using static WindowsFormsApp22.Core;

namespace WindowsFormsApp22.Entities
{
    public class Player : Entity
    {
        public float speed_move = 2F;
        public float speed_rotate => speed_move;
        public int cooldown = 0, cooldown_max = 50;

        public Player() : base("101", Color.Yellow, 32, 32)
        {
            weight = 60;
        }

        public override PointF CalculateLook()
        {
            return MouseStates.Position.Minus(hw - W / 2F - cam_ofs.x, hh - H / 2F - cam_ofs.y).norm();
        }

        public override void Update()
        {
            base.Update();
            Angle.Value += speed_rotate;
        }

        public override void Draw(Graphics g, PointF? position = null)
        {
            if (tex != null)
            {
                var path = TexMgr.Load(tex);
                var matrix = new Matrix();
                matrix.RotateAt(Angle.Value, new PointF(W/2F, H/2F), MatrixOrder.Prepend);
                matrix.Translate(hw - W / 2F - cam_ofs.x - W/2F, hh - H / 2F - cam_ofs.y - H / 2F, MatrixOrder.Append);
                matrix.Scale(W, H);
                path.Transform(matrix);
                g.FillPath(Brushes.Black, path);
                g.DrawPath(new Pen(color), path);
            }
        }

        public void Controls()
        {
            if (IsKeyDown(Key.Z))
            { Y -= speed_move; cam_ofs.y += speed_move / 2F; }
            else if (IsKeyDown(Key.S))
            { Y += speed_move; cam_ofs.y -= speed_move / 2F; }
            else cam_ofs.y = Maths.Round(cam_ofs.y * 0.98F, 4);
            if (IsKeyDown(Key.Q))
            { X -= speed_move; cam_ofs.x += speed_move / 2F; }
            else if (IsKeyDown(Key.D))
            { X += speed_move; cam_ofs.x -= speed_move / 2F; }
            else cam_ofs.x = Maths.Round(cam_ofs.x * 0.97F, 4);

            if (cooldown == 0)
            {
                if (MouseStates.ButtonDown == MouseButtons.Left)
                {
                    var bullet = new Behaviored("bullet", Color.Red, X, Y, 4, 4, Behaviored.Default_AddLook(CalculateLook(), 5F));
                    bullet.Parent = Core.Map.obj_colliders;
                    bullet.weight = 20;
                    cooldown += cooldown_max;
                }
            }
            else cooldown--;
        }
    }
}
