using System.Drawing;
using WindowsFormsApp19.Map;
using WindowsFormsApp19.Utilities;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace WindowsFormsApp19
{
    internal class User
    {
        public bool Destroy = false;
        public vecf vec;
        public int HPMax = 1;
        public int HP;
        public float speedmove = 2F;
        public byte Room => Data.Instance.MapMgr.Rooms[vec.tile(Core.TSZ * MapMgr.ChunkSz).i.x, vec.tile(Core.TSZ * MapMgr.ChunkSz).i.y];
        public byte RoomOf(vecf v)
        {
            int x = v.tile(Core.TSZ * MapMgr.ChunkSz).i.x;
            int y = v.tile(Core.TSZ * MapMgr.ChunkSz).i.y;
            byte id = Data.Instance.MapMgr.Rooms[x, y];
            if (id > 0) return id;
            if(x - 1 >= 0)
            {
                id = Data.Instance.MapMgr.Rooms[x - 1, y];
                if (id > 0) return id;
            }
            if (y - 1 >= 0)
            {
                id = Data.Instance.MapMgr.Rooms[x, y - 1];
                if (id > 0) return id;
            }
            return 0;
        }
        public byte Tile => Rooms.Templates[Room][vec.tile(Core.TSZ).i.x % MapMgr.ChunkSz, vec.tile(Core.TSZ).i.y % MapMgr.ChunkSz];
        public byte TileOf(vecf v)
        {
            byte id = RoomOf(v);
            return (byte)(id == 0 ? 0 : Rooms.Templates[id][v.tile(MapMgr.ChunkSz).i.x % Core.TSZ, v.tile(MapMgr.ChunkSz).i.y % Core.TSZ]);
        }

        public User(float x, float y)
        {
            vec = new vecf(x, y);
            HP = HPMax;
        }

        public void Update()
        {
            if (Destroy) return;

            if (KB.IsKeyDown(KB.Key.Z) && TileOf(vec + new vecf(0, -speedmove)) == 0)
                vec.y -= speedmove;
            if (KB.IsKeyDown(KB.Key.S) && TileOf(vec  + new vecf(0, speedmove + Core.TSZ)) == 0)
                vec.y += speedmove;
            if (KB.IsKeyDown(KB.Key.Q) && TileOf(vec + new vecf(-speedmove, 0)) == 0)
                vec.x -= speedmove;
            if (KB.IsKeyDown(KB.Key.D) && TileOf(vec + new vecf(speedmove + Core.TSZ, 0)) == 0)
                vec.x += speedmove;
        }

        public void Draw()
        {
            if (Destroy) return;

            var pt = Core.MidScreen.ipt;
            var rect = new Rectangle(pt.X, pt.Y, Core.TSZ, Core.TSZ);
            Core.g.FillRectangle(new SolidBrush(HPColor), rect);
            Core.g.DrawRectangle(Pens.Black, rect);
        }

        public Color HPColor => Color.FromArgb((byte)((1F - HP) * 255), (byte)(HP * 255), 0);
    }
}
