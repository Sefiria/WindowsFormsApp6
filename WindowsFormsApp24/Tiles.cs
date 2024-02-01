using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp24
{
    internal class Tiles
    {
        public byte[] data;
        public int LAYERS, W, H;
        public int convert(int layer, int x, int y) => layer * (H+1) * (W+1) + y * (W+1) + x;
        public int convert(int layer, Point xy) => convert(layer, xy.X, xy.Y);
        public byte this[int layer, int x, int y] { get => data[convert(layer, x, y)]; set => data[convert(layer, x, y)] = value; }
        public byte this[int layer, Point xy] { get => data[convert(layer, xy)]; set => data[convert(layer, xy)] = value; }
        public Tiles(int LAYERS, int W, int H)
        {
            this.LAYERS = LAYERS;
            this.W = W;
            this.H = H;
            data = new byte[(LAYERS+1) * (H+1) * (W+1)];
        }
        public bool check_fail(int x, int y) => x < 0 || y < 0 || x > W || y > H;
        public bool check_pass(int x, int y) => !check_fail(x, y);
        public (int layer, int x, int y, int index) PointedData(int x, int y)
        {
            if (check_fail(x,y))
                return (0,x,y,0);
            int l = LAYERS - 1;
            while(l > 0)
            {
                if (this[l, x, y] > 0)
                    return (l, x, y, this[l, x, y]);
                l--;
            }
            return (0,x,y,0);
        }
        public (int layer, int x, int y, int index) PointedData(Point xy) => PointedData(xy.X, xy.Y);
        public int PointedLayer(int x, int y) => PointedData(x, y).layer;
        public int PointedLayer(Point xy) => PointedLayer(xy.X, xy.Y);
        public int PointedIndex(int x, int y) => PointedData(x, y).index;
        public int PointedIndex(Point xy) => PointedIndex(xy.X, xy.Y);
        public void Set(int l, int x, int y, int v) => this[l, x, y] = (byte)Maths.Range(0, byte.MaxValue, v);
    }
}
