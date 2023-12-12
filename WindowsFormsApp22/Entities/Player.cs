using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using static Tooling.KB;
using static WindowsFormsApp22.Core;

namespace WindowsFormsApp22.Entities
{
    public class Player : Entity
    {
        public bool IsBlind = false;
        public RangeValueF IsBlind_Visibility = new RangeValueF(0F, 1F, 1F);
        public bool IsUsingSpecial = false, IsSpecialShoting = false;


        public float speed_move = 2F;
        public float speed_rotate => speed_move;
        public int cooldownA = 0, cooldown_maxA = 50;
        public int cooldownB = 0, cooldown_maxB = 500;
        public float specialRaySizeBase = 20F, specialRaySize = 0F, specialAngle = 0F, specialTime = 0F;

        public Player() : base("101", Color.Yellow, 32, 32)
        {
            weight = 60;
        }

        public override PointF CalculateLook()
        {
            return MouseStates.Position.Minus(hw - cam_ofs.x, hh - cam_ofs.y).norm();
        }

        public override void Update()
        {
            base.Update();
            Angle.Value += speed_rotate;
        }

        //public override void Draw(Graphics g, PointF? position = null)
        //{
        //    if (tex != null)
        //    {
        //        var path = TexMgr.Load(tex);
        //        var matrix = new Matrix();
        //        matrix.RotateAt(Angle.Value, new PointF(W/2F, H/2F), MatrixOrder.Prepend);
        //        matrix.Translate(hw - W / 2F - cam_ofs.x - W/2F, hh - H / 2F - cam_ofs.y - H / 2F, MatrixOrder.Append);
        //        matrix.Scale(W, H);
        //        path.Transform(matrix);
        //        g.FillPath(Brushes.Black, path);
        //        g.DrawPath(new Pen(color), path);
        //    }
        //}

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
            if (IsKeyPressed(Key.B)) IsBlind = !IsBlind;// DEBUG

            if (cooldownA == 0)
            {
                if (MouseStates.ButtonDown == MouseButtons.Left)
                {
                    var bullet = new Behaviored("player_bullet", Color.Red, X, Y, 4, 4, Behaviored.Default_AddLook(CalculateLook(), 5F));
                    bullet.Parent = Core.Map.obj_colliders;
                    bullet.weight = 20;
                    cooldownA += cooldown_maxA;
                }
            }
            else cooldownA--;

            if (cooldownB == 0)
            {
                if (MouseStates.ButtonDown == MouseButtons.Right)
                {
                    IsUsingSpecial = true;
                    specialAngle = CalculateLook().GetAngle();
                    specialRaySize = 0F;
                    specialTime = 1F;
                    IsBlind = true;
                    cooldownB += cooldown_maxB;
                }
            }
            else cooldownB--;

            if(IsUsingSpecial)
            {
                if (IsBlind_Visibility.Value <= 0F)
                {
                    IsBlind = false;
                }
                else if (IsBlind == false && IsBlind_Visibility.Value >= 0.2F)
                {
                    IsSpecialShoting = true;
                    if (specialRaySize < 1F) specialRaySize += 0.05F;
                    if (specialRaySize > 1F) specialRaySize = 1F;
                    atk_circle.AtkEntities.Where(atk => { var a = this.GetAngleWith(atk); return a >= specialAngle - specialRaySize * specialRaySizeBase && a <= specialAngle + specialRaySize * specialRaySizeBase; })
                                                    .ToList()
                                                    .ForEach(atk => atk.Exist = false);

                    if (specialTime <= 0F)
                        IsUsingSpecial = false;
                    else specialTime -= 0.01F;
                }
            }
            else
            {
                if (specialRaySize > 0F)
                    specialRaySize -= 0.05F;
                else IsSpecialShoting = false;
            }

            IsBlind_Visibility.Value += (IsBlind ? -1F : 1F) * 0.01F;
        }
    }
}
