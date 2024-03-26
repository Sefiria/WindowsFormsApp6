using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace console_v2
{
    internal class Guy : Entity
    {
        private World ThisWorld = (Core.Instance.CurrentScene as SceneAdventure).World;

        public vec CurDimension = vec.Zero, CurChunk = vec.Zero;
        public float mv_speed = 0.05f;
        public vec PreviousPosition = vec.Zero;
        public bool HasMoved => Position.i != PreviousPosition;
        public vec Direction => Position.i - PreviousPosition;

        public Inventory Inventory;


        public Guy()
        {
            CharToDisplay = 'ῗ';
            Position = Chunk.ChunkSize.f  / 2f;
            Offset = (4f, 4f).Vf();
            Inventory = new Inventory();
        }

        public override void Update()
        {
            base.Update();

            ManageMovement();
        }

        private void ManageMovement()
        {
            PreviousPosition = Position.i;

            void BoundThis(ref vecf pos, ref vec chunk)
            {
                while (pos.x < 0) { pos.x += Chunk.ChunkSize.x; chunk.x--; }
                while (pos.x >= Chunk.ChunkSize.x) { pos.x -= Chunk.ChunkSize.x; chunk.x++; }
                while (pos.y < 0) { pos.y += Chunk.ChunkSize.y; chunk.y--; }
                while (pos.y >= Chunk.ChunkSize.y) { pos.y -= Chunk.ChunkSize.y; chunk.y++; }
            }

            var (z, q, s, d) = KB.ZQSD();
            void TryMove(vecf p)
            {
                vecf nxt_pos = Maths.Round(Position + p, 4);
                vec nxt_chunk = new vec(CurChunk);
                BoundThis(ref nxt_pos, ref nxt_chunk);
                if (nxt_pos.i == Position.i || ThisWorld.IsntBlocking(CurDimension, nxt_chunk, nxt_pos.i))
                {
                    Position = nxt_pos;
                    CurChunk = nxt_chunk;
                }
            }
            if (z) { var p = (0f, -mv_speed).Vf(); TryMove(p); }
            if (q) { var p = (-mv_speed, 0f).Vf(); TryMove(p); }
            if (s) { var p = (0f, mv_speed).Vf(); TryMove(p); }
            if (d) { var p = (mv_speed, 0f).Vf(); TryMove(p); }

            BoundThis(ref Position, ref CurChunk);
        }

        public override void Draw(Graphics g)
        {
            GraphicsManager.DrawString(g, string.Concat((char)CharToDisplay), CharBrush, Position.i.f * GraphicsManager.CharSize.Vf() + Offset);
            //for(int x=0;x<Chunk.ChunkSize.x;x++)
            //    GraphicsManager.DrawString(g, string.Concat((char)CharToDisplay), x == Chunk.ChunkSize.x-1 ? Brushes.Yellow : CharBrush, (x,7).Vf().i.f * GraphicsManager.CharSize.Vf());
            //GraphicsManager.DrawString(g, "ꬾ║¶₼Ƥ₶Шﷺﷺ#", Brushes.Yellow, SceneAdventure.DrawingRect.Location.vecf() + (0,8).Vf().i.f * GraphicsManager.CharSize.Vf());
            //GraphicsManager.DrawString(g, "##########", Brushes.Yellow, SceneAdventure.DrawingRect.Location.vecf() + (0,9).Vf().i.f * GraphicsManager.CharSize.Vf());
        }
    }
}
