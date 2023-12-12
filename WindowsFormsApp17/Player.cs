using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp17.Properties;

namespace WindowsFormsApp17
{
    internal class Player : Entity
    {
        public Player(int default_tsz = 0) : base("player", Resources.player_sheet, 6, 14, default_tsz > 0 ? Core.TSZ / (float)default_tsz : 0F)
        {
            while (Data.Instance.map[Tile.X, Tile.Y + 1] == 0)
                Pos = Pos.PlusF(0, Core.TSZ);
        }

        public override void Update()
        {
            base.Update();

            float mv_speed = 2F;

            int q = KB.IsKeyDown(KB.Key.Q).ToInt();
            int d = KB.IsKeyDown(KB.Key.D).ToInt();
            
            if (q!=0) Pos = Pos.Minus(mv_speed, 0F);
            if (d!=0) Pos = Pos.PlusF(mv_speed, 0F);

            Animate(q*-1+d);

            if(Data.Instance.map[Tile.X, Tile.Y + 1] == 0)
                Pos = Pos.PlusF(0, mv_speed * 1.5F);
        }
    }
}
