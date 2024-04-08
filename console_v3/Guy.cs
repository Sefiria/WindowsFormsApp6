using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace console_v3
{
    internal class Guy : Entity
    {
        private World ThisWorld = (Core.Instance.CurrentScene as SceneAdventure).World;

        public vecf TilePositionF;
        public vec CurDimension = vec.Zero, CurChunk = vec.Zero;
        public float mv_speed = 0.05f;
        public vec PreviousPosition = vec.Zero;
        public bool HasMoved => TilePositionF.i != PreviousPosition;
        public vec Direction => TilePositionF.i - PreviousPosition;
        public vec DirectionPointed = vec.Zero;
        public Bitmap Texture;


        public Guy() : base(vecf.Zero)
        {
            Texture = Properties.Resources.Guy;
            Texture.MakeTransparent(Color.FromArgb(0,255,0));
            Texture = new Bitmap(Texture, GraphicsManager.TileSize, GraphicsManager.TileSize);
            TilePositionF = Chunk.ChunkSize.f  / 2f;
            Stats = new Statistics(new Dictionary<Statistics.Stat, int>
            {
                [Statistics.Stat.HPMax] = 200,
                [Statistics.Stat.HP] = 200,
                [Statistics.Stat.MPMax] = 50,
                [Statistics.Stat.MP] = 50,
                [Statistics.Stat.STR] = 50,
                [Statistics.Stat.INT] = 75,
                [Statistics.Stat.DEF] = 0,
                [Statistics.Stat.MPDEF] = 0,
                [Statistics.Stat.MOVSPD] = 50,
            });
            Inventory = new Inventory(this);
        }

        public override void Update()
        {
            base.Update();

            ManageMovement();

            if(KB.IsKeyPressed(KB.Key.Space))
                Core.GetEntityAt(TilePositionF.i + DirectionPointed)?.Action(this);

            Position = TilePositionF.i.ToWorld();
        }

        private void ManageMovement()
        {
            PreviousPosition = TilePositionF.i;

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
                vecf nxt_pos = Maths.Round(TilePositionF + p, 4);
                vec nxt_chunk = new vec(CurChunk);
                BoundThis(ref nxt_pos, ref nxt_chunk);
                if (nxt_pos.i == TilePositionF.i || ThisWorld.IsntBlocking(CurDimension, nxt_chunk, nxt_pos.i))
                {
                    TilePositionF = nxt_pos;
                    if (CurChunk != nxt_chunk)
                    {
                        ThisWorld.Dimensions[CurDimension][CurChunk].Entities.Remove(this);
                        ThisWorld.Dimensions[CurDimension][nxt_chunk].Entities.Add(this);
                    }
                    CurChunk = nxt_chunk;
                    ThisWorld.GetChunk(CurChunk).AlreadyVisited = true;
                }
            }
            var mvspd = mv_speed + Stats._Get(Statistics.Stat.MOVSPD) * 0.001f;
            if (z) { var p = (0f, -mvspd).Vf(); TryMove(p); DirectionPointed = (0, -1).V(); }
            if (q) { var p = (-mvspd, 0f).Vf(); TryMove(p); DirectionPointed = (-1, 0).V(); }
            if (s) { var p = (0f, mvspd).Vf(); TryMove(p); DirectionPointed = (0, 1).V(); }
            if (d) { var p = (mvspd, 0f).Vf(); TryMove(p); DirectionPointed = (1, 0).V(); }

            BoundThis(ref TilePositionF, ref CurChunk);
        }

        public override void Draw(Graphics g)
        {
            GraphicsManager.Draw(g, Texture, TilePositionF.i.f * GraphicsManager.TileSize);
        }

        public override void TickSecond()
        {
            Stats.TickSecond();
        }
    }
}
