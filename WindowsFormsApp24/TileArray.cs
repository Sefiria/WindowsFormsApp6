using System.Drawing;

namespace WindowsFormsApp24
{
    internal interface ITile
    {
        int TilesetIndex { get; set; }
        float wet { get; set; }
    }
    internal class TileArray<T> where T:ITile
    {
        public T[] data;
        public int LAYERS, W, H;
        public int convert(int layer, int x, int y) => layer * (H+1) * (W+1) + y * (W+1) + x;
        public int convert(int layer, Point xy) => convert(layer, xy.X, xy.Y);
        public T this[int layer, int x, int y] { get => data[convert(layer, x, y)]; set => data[convert(layer, x, y)] = value; }
        public T this[int layer, Point xy] { get => data[convert(layer, xy)]; set => data[convert(layer, xy)] = value; }
        public TileArray(int LAYERS, int W, int H)
        {
            this.LAYERS = LAYERS;
            this.W = W;
            this.H = H;
            data = new T[(LAYERS+1) * (H+1) * (W+1)];
        }
        public bool check_fail(int x, int y) => x < 0 || y < 0 || x > W || y > H;
        public bool check_pass(int x, int y) => !check_fail(x, y);
        public (int layer, int x, int y, T tile) PointedData(int x, int y)
        {
            if (check_fail(x,y))
                return (0,x,y,default(T));
            int l = LAYERS - 1;
            while(l >= 0)
            {
                if (this[l, x, y] != null)
                    return (l, x, y, this[l, x, y]);
                l--;
            }
            return (0,x,y, default(T));
        }
        public (int layer, int x, int y, T tile) PointedData(Point xy) => PointedData(xy.X, xy.Y);
        public int PointedLayer(int x, int y) => PointedData(x, y).layer;
        public int PointedLayer(Point xy) => PointedLayer(xy.X, xy.Y);
        public T PointedTile(int x, int y) => PointedData(x, y).tile;
        public T PointedTile(Point xy) => PointedTile(xy.X, xy.Y);
        public void Set(int l, int x, int y, T v)
        {
            this[l, x, y] = v;
            Map.Current.TilesToRefresh.Enqueue((l, x, y));
        }
        public void SetTilesetIndex(int l, int x, int y, int v, ITile default_create = null)
        {
            if (this[l, x, y] != null)
            {
                this[l, x, y].TilesetIndex = v;
                if (l>0 && this[l-1, x, y] != null)
                    this[l, x, y].wet = this[l - 1, x, y].wet;
            }
            else if (default_create != null)
            {
                this[l, x, y] = (T)default_create;
                this[l, x, y].TilesetIndex = v;
                if (l > 0 && this[l - 1, x, y] != null)
                    this[l, x, y].wet = this[l - 1, x, y].wet;
            }
            else
                return;
            Map.Current.TilesToRefresh.Enqueue((l, x, y));
        }
    }
}
