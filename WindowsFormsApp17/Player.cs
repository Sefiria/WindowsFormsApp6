using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using Tooling;
using WindowsFormsApp17.Properties;

namespace WindowsFormsApp17
{
    internal class Player : Entity
    {
        //public override PointF DrawPoint => base.DrawPoint.Minus(Core.TSZ - W, Core.TSZ - H).Minus(Core.TSZ / 2, Core.TSZ / 2).Minus(0,H/2);

        bool jumping = false;
        int jump_time = 0, jump_time_max = 10;

        public Player(int default_tsz = 0) : base("player", Resources.player_sheet, 6, 14, default_tsz > 0 ? Core.TSZ / (float)default_tsz : 0F)
        {
            while (Data.Instance.map[Tile.X, Tile.Y + 1] == 0)
                Pos = Pos.PlusF(0, Core.TSZ);
        }

        public override void Update()
        {
            base.Update();

            float mv_speed = 2F;

            //int z = KB.IsKeyDown(KB.Key.Z).ToInt();//debug
            //int s = KB.IsKeyDown(KB.Key.S).ToInt();//debug
            int q = (KB.IsKeyDown(KB.Key.Q) && Map.IsWorldAir(X -  mv_speed, Y)).ToInt();
            int d = (KB.IsKeyDown(KB.Key.D) && Map.IsWorldAir(X + mv_speed, Y)).ToInt();
            if (jumping == false && KB.IsKeyPressed(KB.Key.Space) && Map.IsWorldBlock(X, Y + H + 1))
                jumping = true;
            bool j = jumping && KB.IsKeyDown(KB.Key.Space) && jump_time < jump_time_max;
            jumping = j;
            //Console.WriteLine($"jumping: {jumping}  space_key: {KB.IsKeyDown(KB.Key.Space)}  on_ground: {Map.IsWorldBlock(X, Y + H + 1)}  under_jump_time_max: {jump_time < jump_time_max} (jump_time: {jump_time})");

            if (q!=0) Pos = Pos.Minus(mv_speed, 0F);
            if (d!=0) Pos = Pos.PlusF(mv_speed, 0F);
            //if (z != 0) Pos = Pos.Minus(0F, mv_speed);
            //if (s != 0) Pos = Pos.PlusF(0F, mv_speed);
            if (j) { jump_time++; Pos = Pos.Minus(0, mv_speed * 2F); }

            Animate(q*-1+d);

            if (!j)
            {
                if (Map.IsWorldAir(X, Y + H + 1))
                    Pos = Pos.PlusF(0, mv_speed * 1.5F);
                else
                    jump_time = 0;
            }
            if (Map.IsWorldBlock(X, Y + H - 2))
                Pos = Pos.Minus(0, mv_speed * 1.5F);
        }

        public override void Draw(Graphics g, PointF? position = null)
        {
            base.Draw(g, position);
            //g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Red)), DrawTile.X, DrawTile.Y, Core.TSZ, Core.TSZ);
        }
    }
}
