using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp25
{
    internal class Controllable : Entity
    {
        public static float MoveSpeed = 2F;

        public vecf PreviousPosition = vecf.Zero;
        public vec PreviousTile = vec.Zero;
        public bool HasMoved => Position != PreviousPosition;
        public vecf Direction => ((((int)X - (int)PreviousPosition.x) == 0 ? 0 : ((int)X - (int)PreviousPosition.x).GetHashCode() >> 31) + 1, ((int)Y - (int)PreviousPosition.y) == 0 ? 0 : (((int)Y - (int)PreviousPosition.y).GetHashCode() >> 31) + 1).Vf();

        public Controllable(int tile_x, int tile_y) : base("You", DB.Tex.You, tile_x, tile_y)
        {
            Offset = (-28, -28).Vf();
        }

        public override void Update()
        {
            base.Update();

            Console.WriteLine(Tile);

            void TryMove(vecf direction)
            {
                vec nxt_pos = Maths.Round(Position + direction * MoveSpeed, 4).i;
                int bnd(int c) => Math.Max(0, Math.Min(c, Core.ChunkSize * Core.TileSize));
                nxt_pos = (bnd(nxt_pos.x), bnd(nxt_pos.y)).V();
                vec nxt_tile = nxt_pos / Core.TileSize;
                if (nxt_tile == Tile || (DB.IsntBlocking(Core.Instance.Tiles[nxt_tile.x + nxt_tile.y * Core.ChunkSize].Tex) && !Core.Instance.Entities.Any(e => e.Tile == nxt_tile)))
                    Position = nxt_pos.f;
            }
            var (z, q, s, d) = KB.ZQSD();
            if (z) TryMove((0f,-1f).Vf());
            if (q) TryMove((-1f,0f).Vf());
            if (s) TryMove((0f,1f).Vf());
            if (d) TryMove((1f,0f).Vf());

            if (KB.IsKeyDown(KB.Key.Space))
                Console.WriteLine(Direction);
                //Core.GetEntityAt(TilePositionF.i + DirectionPointed)?.Action(this);
        }
        public override void Tick()
        {
            base.Tick();
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }
    }
}
