using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp28
{
    public class CellInfo
    {
        public static byte[] pixels;
        public static Fluid[] fluids;
        public static int w, h, BRICK, PIPE;

        public static void Init(byte[] pixels, Fluid[] fluids, int w, int h, int BRICK, int PIPE)
        {
            CellInfo.pixels = pixels;
            CellInfo.fluids = fluids;
            CellInfo.w = w;
            CellInfo.h = h;
            CellInfo.BRICK = BRICK;
            CellInfo.PIPE = PIPE;
        }

        public bool IsAir, IsBrick, IsPipe, IsPipeExtremity;
        public vec cell;
        public byte cell_px;
        public Fluid cell_fluid;
        public List<vec> linked_pipes = new List<vec>();
        public int pipesCount => linked_pipes.Count;

        public CellInfo(int x, int y)
        {
            cell = (x, y).V();
            cell_px = pixels[y * w + x];
            cell_fluid = fluids[y * w + x];
            if (x - 1 >= 0 && pixels[(y) * w + (x - 1)] == PIPE) linked_pipes.Add((x - 1, y).V());
            if (x + 1 < w && pixels[(y) * w + (x + 1)] == PIPE) linked_pipes.Add((x + 1, y).V());
            if (y - 1 >= 0 && pixels[(y - 1) * w + x] == PIPE) linked_pipes.Add((x, y - 1).V());
            if (y + 1 < h && pixels[(y + 1) * w + x] == PIPE) linked_pipes.Add((x, y + 1).V());
            IsAir = cell_px < BRICK;
            IsBrick = cell_px == BRICK;
            IsPipe = cell_px == PIPE;
            IsPipeExtremity = IsPipe ? linked_pipes.Count == 1 : false;
        }
        public CellInfo(vec pos) : this(pos.x, pos.y) { }
        public CellInfo(vecf pos) : this(pos.i) { }

    }
}
