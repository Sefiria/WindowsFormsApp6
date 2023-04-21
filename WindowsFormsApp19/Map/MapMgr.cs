using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsFormsApp19.Utilities;

namespace WindowsFormsApp19.Map
{
    internal class MapMgr
    {
        public static int ChunkSz = 12;
        public static int w => 16;
        public static int h => 16;

        public byte[,] Rooms; // value = Rooms.Templates index

        public MapMgr()
        {
            byte get(int i, int j) => (byte)(i < 0 || j < 0 || i >= w || j >= h ? -1 : Rooms[i, j]);

            Rooms = new byte[w, h];
            byte[,] r;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    r = Map.Rooms.GetTemplateIndex(get(x, y - 1));
                    if (r?.GetLength(1) / ChunkSz == 2)
                        continue;
                    r = Map.Rooms.GetTemplateIndex(get(x - 1, y + 1));
                    if (r?.GetLength(0) / ChunkSz == 2)
                        continue;
                    Rooms[x, y] = (byte)Core.RND.Next(1, Map.Rooms.Templates.Count);
                }
            }
        }

        public void Draw()
        {
            vecf v = Data.Instance.User.vec;
            vecf t = v.tile(Core.TSZ * ChunkSz);
            int startX = (int)t.x - 1;
            int startY = (int)t.y - 1;
            for (int i = 0; i < Core.rw / Core.TSZ / ChunkSz + 3; i++)
            {
                for (int j = 0; j < Core.rh / Core.TSZ / ChunkSz + 3; j++)
                {
                    if (startX + i < 0 || startY + j < 0 || startX + i >= w || startY + j >= h)
                        continue;

                    byte[,] room = Map.Rooms.Templates[Rooms[startX + i, startY + j]];
                    if(room == null)
                        continue;
                    int rw = room.GetLength(0);
                    int rh = room.GetLength(1);

                    for (int x = 0; x < rw; x++)
                    {
                        for (int y = 0; y < rh; y++)
                        {
                            if (room[x, y] == 1 || (startX + i) * ChunkSz + x * Core.TSZ == 0 || (startY + j) * ChunkSz + y * Core.TSZ == 0 || (startX + i) * ChunkSz + x == w * ChunkSz - 1 || (startY + j) * ChunkSz + y == h * ChunkSz - 1)
                            {
                                float _x = (startX + i) * ChunkSz * Core.TSZ + x * Core.TSZ - v.x + Core.MidScreen.x;
                                float _y = (startY + j) * ChunkSz * Core.TSZ + y * Core.TSZ - v.y + Core.MidScreen.y;
                                Core.g.FillRectangle(Brushes.White, _x, _y, Core.TSZ, Core.TSZ);
                            }
                        }
                    }
                }
            }
        }
    }
}
