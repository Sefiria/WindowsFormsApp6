using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    internal class MinimapManager
    {
        public static int sz = 200;
        public static int range = 3;
        
        public static void Draw(Graphics gui)
        {
            var tg = Core.Instance.TheGuy;
            var world = Core.Instance.SceneAdventure.World;
            var szmini = sz / (range * 2 + 1);
            Rectangle rect;
            vec c = tg.CurChunk;
            vec[,] chunks = new vec[range*2+1, range*2+1];
            chunks[range, range] = c;
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    if (x == 0 && y == 0) continue;
                    chunks[range + x, range + y] = c + (x, y).V();
                }
            }

            Bitmap minimap = new Bitmap(sz, sz);

            void draw_minichunk(Graphics g, int x, int y)
            {
                int _x = range + x;
                int _y = range + y;
                vec chunk_vec = chunks[_x, _y];
                Chunk chunk = world.GetChunk( chunk_vec);
                //if (chunk == null) return;
                //for (int i = 0; i < Chunk.ChunkSize.x; i++)
                //    for (int j = 0; j < Chunk.ChunkSize.y; j++)
                //        g.DrawRectangle(new Pen(DB.ResColor[chunk.Tiles[(x, y).V()].Sol != 0 ? (int)chunk.Tiles[(x, y).V()].Sol : (int)chunk.Tiles[(x, y).V()].Mur]), 
                if (chunk != null)
                {
                    rect = new Rectangle(szmini * _x, szmini* _y, szmini -1, szmini - 1);
                    g.FillRectangle(new SolidBrush(DB.ChunkLayerColor[chunk.Layer]), szmini * _x, szmini * _y, szmini, szmini);
                    var entities = new List<Entity>(chunk.Entities).Except(tg);
                    foreach (var e in entities)
                        g.DrawRectangle(Pens.White, rect.X + e.TileX, rect.Y + e.TileY, 1, 1);
                    if (x == 0 & y == 0)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(150, Core.Instance.Ticks % 20 < 10 ? Color.Yellow : Color.Red)), rect.X + tg.TilePositionF.x - 1, rect.Y + tg.TilePositionF.y - 1, 3, 3);
                        //g.DrawRectangle(Pens.White, rect.X + tg.TilePositionF.x, rect.Y + tg.TilePositionF.y, 1, 1);
                        g.DrawRectangle(Pens.LightGray, rect);
                    }
                }
            }

            using (Graphics g = Graphics.FromImage(minimap))
            {
                for (int x = -range; x <= range; x++)
                {
                    for (int y = -range; y <= range; y++)
                    {
                        draw_minichunk(g, x, y);
                    }
                }
            }

            gui.DrawImage(minimap, Core.Instance.ScreenWidth - 20 - sz, 20);
            gui.DrawRectangle(Pens.LightGray, Core.Instance.ScreenWidth - 20 - sz, 20, sz-1, sz-1);
        }
    }
}
