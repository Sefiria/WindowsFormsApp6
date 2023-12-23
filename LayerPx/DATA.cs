using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Tooling;

namespace LayerPx
{
    internal class DATA
    {
        public byte[] data;
        public int LAYERS, W, H;
        public int convert(int layer, int x, int y) => layer * (H+1) * (W+1) + y * (W+1) + x;
        public int convert(int layer, Point xy) => convert(layer, xy.X, xy.Y);
        public byte this[int layer, int x, int y] { get => data[convert(layer, x, y)]; set => data[convert(layer, x, y)] = value; }
        public byte this[int layer, Point xy] { get => data[convert(layer, xy)]; set => data[convert(layer, xy)] = value; }
        public DATA(int LAYERS, int W, int H)
        {
            this.LAYERS = LAYERS;
            this.W = W;
            this.H = H;
            data = new byte[(LAYERS+1) * (H+1) * (W+1)];
        }
        public bool check_fail(int x, int y) => x < 0 || y < 0 || x >= W || y >= H;
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
        public int Pointedindex(int x, int y) => PointedData(x, y).index;
        public int Pointedindex(Point xy) => Pointedindex(xy.X, xy.Y);
        public void Set(int l, int x, int y, int v) => this[l, x, y] = (byte)Maths.Range(0, byte.MaxValue, v);
        public void SetSquareWithLayer(int l, int x, int y, int v, int size)
        {
            if (size == 1)
            {
                set_single_bylayer(l, x, y, v);
                return;
            }

            for (int j= -size/2; j<size/2; j++)
                for (int i = -size / 2; i < size / 2; i++)
                    if(check_pass(x - i, y - j))
                        set_single_bylayer(l, x-i, y-j, v);
        }
        public void SetCircleWithLayer(int l, int x, int y, int v, int diameter)
        {
            if (diameter == 1)
            {
                set_single_bylayer(l, x, y, v);
                return;
            }

            int radius = diameter / 2;
            for (int j = -radius; j < radius; j++)
                for (int i = -radius; i < radius; i++)
                    if (check_pass(x - i, y - j) && Maths.Distance((i, j).P()) <= radius)
                        set_single_bylayer(l, x-i, y-j, v);
        }
        public void SetSquareWithDirection(int direction, int x, int y, int v, int size)
        {
            if(size == 1)
            {
                set_single_bydir(direction, x, y, v);
                return;
            }

            for (int j = -size / 2; j < size / 2; j++)
                for (int i = -size / 2; i < size / 2; i++)
                    if (check_pass(x - i, y - j))
                        set_single_bydir(direction, x - i, y - j, v);
        }
        public void SetCircleWithDirection(int direction, int x, int y, int v, int diameter)
        {
            if (diameter == 1)
            {
                set_single_bydir(direction, x, y, v);
                return;
            }

            int radius = diameter / 2;
            for (int j = -radius; j < radius; j++)
                for (int i = -radius; i < radius; i++)
                    if (check_pass(x - i, y - j) && Maths.Distance((i, j).P()) <= radius)
                        set_single_bydir(direction, x - i, y - j, v);
        }
        public (int old, int @new) calc_layer(int direction, int x, int y)
        {
            int old, layer;
            old = layer = PointedLayer(x, y);
            layer = layer == 0 ? LAYERS / 2 : new RangeValue(layer + direction, 0, LAYERS - 1).Value;
            return (old, (int)Maths.Range(0, LAYERS-1, layer));
        }
        public void set_single_bydir(int direction, int x, int y, int v)
        {
            (int old, int @new) = calc_layer(direction, x, y);
            if (Form1.Instance.Mode != Form1.ToolModes.Normal && old != Form1.Instance.layer_at_first_press)
                return;
            this[old, x, y] = 0;
            this[@new, x, y] = (byte)v;
            Form1.Instance.draw_refresh_queue.Enqueue((x, y).iP());
        }
        public void set_single_bylayer(int @new, int x, int y, int v)
        {
            int old = PointedLayer(x, y);
            if (Form1.Instance.Mode != Form1.ToolModes.Normal && old != Form1.Instance.layer_at_first_press)
                return;
            this[old, x, y] = 0;
            this[@new, x, y] = (byte)v;
            Form1.Instance.draw_refresh_queue.Enqueue((x, y).iP());
        }
    }
}
