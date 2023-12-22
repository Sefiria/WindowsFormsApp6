using System.Drawing;
using Tooling;

namespace LayerPx
{
    internal class DATA
    {
        public byte[,,] data;
        public byte this[byte layer, byte x, byte y] { get => data[layer, x, y]; set => data[layer, x, y] = value; }
        public byte this[byte layer, Point xy] { get => data[layer, (byte)xy.X, (byte)xy.Y]; set => data[layer, (byte)xy.X, (byte)xy.Y] = value; }
        public DATA()
        {
            data = new byte[byte.MaxValue+1, byte.MaxValue+1, byte.MaxValue+1];
        }
        public bool check_fail(int x, int y) => x < 0 || y < 0 || x > byte.MaxValue || y > byte.MaxValue;
        public bool check_pass(int x, int y) => !check_fail(x, y);
        public (byte layer, byte x, byte y, byte index) PointedData(int x, int y)
        {
            if (check_fail(x,y))
                return (0,(byte)x, (byte)y,0);
            byte l = byte.MaxValue;
            while(l > 0)
            {
                if (data[l, x, y] > 0)
                    return (l, (byte)x, (byte)y, data[l, x, y]);
                l--;
            }
            return (0,(byte)x, (byte)y,0);
        }
        public (byte layer, byte x, byte y, byte index) PointedData(Point xy) => PointedData(xy.X, xy.Y);
        public byte PointedLayer(int x, int y) => PointedData(x, y).layer;
        public byte PointedLayer(Point xy) => PointedLayer(xy.X, xy.Y);
        public byte Pointedindex(int x, int y) => PointedData(x, y).index;
        public byte Pointedindex(Point xy) => Pointedindex(xy.X, xy.Y);
        public void Set(int l, int x, int y, int v) => data[(byte)l, (byte)x, (byte)y] = (byte)v;
        public void SetSquare(byte l, int x, int y, int v, int size)
        {
            for(int j= -size/2; j<size/2; j++)
            {
                for (int i = -size / 2; i < size / 2; i++)
                {
                    if(check_pass(x - i, y - j))
                        data[l, (byte)(x - i), (byte)(y - j)] = (byte)v;
                }
            }
        }
        public void SetCircle(byte l, int x, int y, int v, int diameter)
        {
            int radius = diameter / 2;
            for (int j = -radius; j < radius; j++)
            {
                for (int i = -radius; i < radius; i++)
                {
                    if (check_pass(x - i, y - j) && Maths.Distance((i, j).P()) <= radius)
                        data[l, (byte)(x - i), (byte)(y - j)] = (byte)v;
                }
            }
        }
        public void SetSquare(int direction, int x, int y, int v, int size)
        {
            if(size == 1)
            {
                set_single(direction, x, y, v);
                return;
            }

            for (int j = -size / 2; j < size / 2; j++)
                for (int i = -size / 2; i < size / 2; i++)
                    if (check_pass(x - i, y - j))
                        set_single(direction, x - i, y - j, v);
        }
        public void SetCircle(int direction, int x, int y, int v, int diameter)
        {
            if (diameter == 1)
            {
                set_single(direction, x, y, v);
                return;
            }

            int radius = diameter / 2;
            for (int j = -radius; j < radius; j++)
                for (int i = -radius; i < radius; i++)
                    if (check_pass(x - i, y - j) && Maths.Distance((i, j).P()) <= radius)
                        set_single(direction, x - i, y - j, v);
        }
        (byte old, byte @new) calc_layer(int direction, int x, int y)
        {
            byte old, layer;
            old = layer = PointedLayer(x, y);
            layer = layer == 0 ? (byte)128 : (byte)new RangeValue(layer + direction, byte.MinValue, byte.MaxValue).Value;
            return (old, layer);
        }
        void set_single(int direction, int x, int y, int v)
        {
            (byte old, byte @new) = calc_layer(direction, x, y);
            data[old, (byte)x, (byte)y] = 0;
            data[@new, (byte)x, (byte)y] = (byte)v;
        }
    }
}
