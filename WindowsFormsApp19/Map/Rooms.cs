using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp19.Map
{
    internal class Rooms
    {
        public static List<byte[,]> Templates = new List<byte[,]>(); // value = pixel type (0=air, 1=wall)
        public static byte[,] WALL;
        public static byte[,] GetTemplateIndex(int index) => index < 0 || index >= Templates.Count ? WALL : Templates[index];

        static int sz => MapMgr.ChunkSz;

        static Rooms()
        {
            WALL = new byte[sz, sz];
            for (int x = 0; x < sz; x++)
                for (int y = 0; y < sz; y++)
                    WALL[x, y] = 1;

            Templates.Add(null);
            AddRoom(1, 1, 2);
            AddRoom(2, 1, 1);
            AddRoom(2, 2, 2);
            AddRoom(1, 1, 1);
            AddRoom(1, 2, 2);
        }

        static void SetRectByte(ref byte[,] r, int w, int h)
        {
            for (int x = 0; x < sz * w; x++)
                r[x, 0] = 1;
            for (int y = 0; y < sz * h; y++)
                r[0, y] = 1;
        }
        static void SetDoors(ref byte[,] r, int w, int h, int doorcount)
        {
            if (doorcount < 1)
                doorcount = 1;
            if (doorcount > 2)
                doorcount = 2;
            List<int> donedoorside = new List<int>();
            List<int> l;
            int side;
            for (int d = 0; d < doorcount; d++)
            {
                l = new List<int> { 0, 1 }.Except(donedoorside).ToList();
                side = l[Core.RND.Next(l.Count)];
                donedoorside.Add(side);
                switch(side)
                {
                    case 0: var x = Core.RND.Next(1, w * sz - 2); r[x, 0] = 0; r[x + 1, 0] = 0; break;
                    case 1: var y = Core.RND.Next(1, h * sz - 2); r[0, y] = 0; r[0, y + 1] = 0; break;
                }
            }
        }
        private static void AddRoom(int w, int h, int doorcount)
        {
            var room = new byte[sz * w, sz * h];
            SetRectByte(ref room, w, h);
            SetDoors(ref room, w, h, doorcount);
            Templates.Add(room);
        }
    }
}
