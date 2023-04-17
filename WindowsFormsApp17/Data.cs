using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using WindowsFormsApp17.items;
using WindowsFormsApp17.Utilities;
using static WindowsFormsApp17.enums;

namespace WindowsFormsApp17
{
    internal class Data
    {
        private static Data m_Instance = null;
        public static Data Instance
        {
            get
            {
                if(m_Instance == null)
                    m_Instance = new Data();
                return m_Instance;
            }
        }

        public Map map;
        public vecf cam;
        public List<mur> murs;
        public fluidmgr fluidmgr;

        public void Init()
        {
            map = new Map();
            MapGen.GenerateTiles();
            int x = 150, y = 200, ssz = 20, lsz = 200;
            murs = new List<mur>()
            {
                new mur(x, y, ssz, lsz),
                new mur(x + ssz, y + lsz - ssz, lsz, ssz),
                new mur(x + ssz + lsz, y, ssz, lsz),
            };
            fluidmgr = new fluidmgr();
        }
    }

    public class Map
    {
        public int w;
        public int h;
        public byte[,] tiles;
        public byte[,] bg_tiles;

        public Map(int w = 64, int h = 64)
        {
            this.w = w;
            this.h = h;
            tiles = new byte[w, h];
            bg_tiles = new byte[w, h];
        }

        public bool check(int x, int y) => !(x < 0 || y < 0 || x >= w || y >= h);
        public byte this[int x, int y] => check(x, y) ? tiles[x, y] : (byte)0;
        public byte GetBgTile(int x, int y) => check(x, y) ? bg_tiles[x, y] : (byte)0;
        public void set(int x, int y, byte v)
        {
            if (v == 0)
            {
                reset(x, y);
                return;
            }

            if (check(x, y)) tiles[x, y] = v;
        }
        public void reset(int x, int y)
        {
            if (check(x, y))
            {
                if(tiles[x, y] > (int)TileName.DirtyGrass)
                    bg_tiles[x, y] = tiles[x, y];
                tiles[x, y] = 0;
            }
        }
    }
}
