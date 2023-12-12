using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;
using WindowsFormsApp17.items;
using WindowsFormsApp17.Properties;
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
        public Player player;
        public List<Entity> Entities = new List<Entity>();
        public List<Entity> OrphanEntities => Entities.Where(e => e.Parent == null).ToList();
        public List<Entity> VisibleEntities => Entities.Where(e => e.IsDrawable && Core.VisibleBounds.Contains(e.iPos)).ToList();

        public void Init(bool isrun = false)
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
            if(isrun)
            {
                Core.TSZ = 32;
                player = new Player(16);
            }
            ResMgr.Init();
        }

        public void UpdateRun()
        {
            player.Update();
            cam = player.Pos.vecf();
        }

        public void DrawRun()
        {
            DrawRunMap();
            //user.Display();
        }

        #region DrawMap internals
        Color[] colors_waters = new Color[3]{
                Color.Cyan,
                Color.DodgerBlue,
                Color.MidnightBlue
            };
        int[] water_level = new int[] { 15, 30, 50 };
        Brush calc_water_brush(int __y)
        {
            Color _c;
            if (__y < water_level[0]) _c = colors_waters[0];
            else if (__y < water_level[1]) { _c = colors_waters[0].ShadeWith(colors_waters[1], (__y - water_level[0]) / (water_level[1] - water_level[0] - 1F)); }
            else if (__y < water_level[2]) { _c = colors_waters[1].ShadeWith(colors_waters[2], (__y - water_level[1]) / (water_level[2] - water_level[1] - 1F)); }
            else _c = colors_waters[2];
            return new SolidBrush(_c);
        }
        #endregion
        private void DrawRunMap()
        {
            Core.g.Clear(map.bg);

            int tsz = Core.TSZ;
            var tcam = cam.tile(tsz);
            int hrtw = Core.w / tsz / 2;
            int hrth = Core.h / tsz / 2;

            int _x, _y, id;
            Bitmap tile;
            for (int x = -hrtw - 1; x < hrtw + 3; x++)
            {
                for (int y = -hrth - 1; y < hrth + 3; y++)
                {
                    _x = (int)(tcam.x + x);
                    _y = (int)(tcam.y + y);
                    id = map[_x, _y];
                    if (id == 0)
                    {
                        // draw bg instead if exists
                        int bg_id = map.GetBgTile(_x, _y);
                        if (bg_id > 0)
                            Core.g.DrawImage(ResMgr.GetBGTile(bg_id), (hrtw + x) * tsz - cam.x % tsz, (hrth + y) * tsz - cam.y % tsz);
                        if (map.check(_x, _y))
                        {
                            Map.fluid fluid = map.fluids[_x, _y];
                            float q = Math.Min(1F, fluid.quantity);
                            if (q > 0F) Core.g.FillRectangle(calc_water_brush(_y), (hrtw + x) * tsz - cam.x % tsz, (hrth + y) * tsz - cam.y % tsz + tsz * (1F - q), tsz, tsz * q);
                        }
                    }
                    else
                    {
                        tile = ResMgr.GetTile(id);
                        Core.g.DrawImage(tile, (hrtw + x) * tsz - cam.x % tsz, (hrth + y) * tsz - cam.y % tsz);
                    }
                }
            }

            float i = Core.MouseCamTile.x;
            float j = Core.MouseCamTile.y;
            Core.g.DrawRectangle(Pens.Cyan, (hrtw + i) * tsz - cam.x, (hrth + j) * tsz - cam.y, tsz, tsz);


            VisibleEntities.ForEach(e => e.Draw(Core.g));
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
        public Color bg;

        public Map(int w = 64, int h = 64)
        {
            this.w = w;
            this.h = h;
            fluids = new fluid[w, h];
            tiles = new byte[w, h];
            bg_tiles = new byte[w, h];
            bg = Color.LightCyan;
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
