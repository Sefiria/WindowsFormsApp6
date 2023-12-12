using System;
using System.Collections.Generic;
using Tooling;
using WindowsFormsApp17.items;
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
        public struct fluid
        {
            public byte type;
            public float quantity;
        }

        public int w;
        public int h;
        public fluid[,] fluids;
        public byte[,] tiles;
        public byte[,] bg_tiles;

        public Map(int w = 64, int h = 64)
        {
            this.w = w;
            this.h = h;
            fluids = new fluid[w, h];
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
        public void reset(float _x, float _y)
        {
            int x = (int)_x;
            int y = (int)_y;
            if (check(x, y))
            {
                if(tiles[x, y] > (int)TileName.DirtyGrass)
                    bg_tiles[x, y] = tiles[x, y];
                tiles[x, y] = 0;
            }
        }

        float fluiddiff(int x, int y, int ofst_x, int ofst_y)
        {
            float diff = fluids[x, y].quantity - fluids[x + ofst_x, y + ofst_y].quantity;
            return diff > 0.01F ? diff : 0F;
        }
        public void FluidUpdate()
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (tiles[x, y] > 0)
                        continue;
                    if (fluids[x, y].quantity == 0F)
                        continue;

                    bool bottom = y < h - 1 && tiles[x, y + 1] == 0 && fluids[x, y + 1].quantity < 1F;
                    bool left = x > 0 && tiles[x - 1, y] == 0 && fluids[x - 1, y].quantity < 1F;
                    bool right = x < w-1 && tiles[x + 1, y] == 0 && fluids[x + 1, y].quantity < 1F;
                    bool top = y > 0 && tiles[x, y - 1] == 0 && fluids[x, y].quantity > 1F && fluids[x, y - 1].quantity < 1F;
                    float d, d2, q, spd = 0.1F;

                    void move(int ofst_x, int ofst_y, float _d)
                    {
                        if (_d <= 0F) return;
                        q = Math.Max(0.02F, _d);
                        fluids[x, y].quantity -= q;
                        if (fluids[x, y].quantity < 0F) fluids[x, y].quantity = 0F;
                        fluids[x + ofst_x, y + ofst_y].quantity += q;
                    }
                    void job(int ofst_x, int ofst_y, bool isdiff = true)
                    {
                        move(ofst_x, ofst_y, isdiff ? fluiddiff(x, y, ofst_x, ofst_y) * spd : fluids[x, y].quantity * 0.25F);
                    }

                    if (bottom) job(0, 1, false);
                    else if (left && right) { d = fluiddiff(x, y, -1, 0) * spd; d2 = fluiddiff(x, y, 1, 0) * spd; move(-1, 0, d); move(1, 0, d2); }
                    else if (left) job(-1, 0);
                    else if (right) job(1, 0);
                    else if (top) job(0, -1);
                    else if (fluids[x, y].quantity > 1F) fluids[x, y].quantity = 1F;
                }
            }
        }
    }
}
