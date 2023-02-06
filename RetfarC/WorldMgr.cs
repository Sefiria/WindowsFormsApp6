using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC
{
    public class WorldMgr
    {
        public static TileTypes[,] Tiles;
        public static int ResourceCount => EntityMgr.Entities.Where(e => e is Resource).Count();

        private static int m_Ticks = 0;
        public static int Ticks
        {
            get => m_Ticks;
            private set
            {
                m_Ticks = value;
                if (m_Ticks < 0) m_Ticks = 0;
                while (m_Ticks > WorldConfig.MaxTicks)
                    m_Ticks -= WorldConfig.MaxTicks;
            }
        }
        public static int TimerResource = 0;

        public static void InitializeWorld()
        {
            GenWorldTiles();
            GenWorldResources();
        }
        private static void GenWorldTiles()
        {
            void setchunk(int _x, int _y, TileTypes _v)
            {
                Tiles[_x + 0, _y + 0] = _v;
                Tiles[_x + 1, _y + 0] = _v;
                Tiles[_x + 0, _y + 1] = _v;
                Tiles[_x + 1, _y + 1] = _v;
            }
            Tiles = new TileTypes[Core.TW, Core.TH];
            TileTypes v;
            int max = Enum.GetNames(typeof(TileTypes)).Length;
            for (int x = 0; x < Core.TW; x+=2)
            {
                for (int y = 0; y < Core.TH; y+=2)
                {
                    var rnd = Core.RND.Next(100);
                    v = 0;
                    if (rnd < WorldConfig.RatioGrass) v = TileTypes.Grass;
                    else if (rnd - WorldConfig.RatioGrass < WorldConfig.RatioStone) v = TileTypes.Stone;
                    else  v = TileTypes.Sand;
                    setchunk(x, y, v);
                }
            }
        }
        private static void GenWorldResources()
        {
            while (ResourceCount < WorldConfig.MaxResourceCount)
                AddRandomResource();
        }

        public static void Update()
        {
            Gen();
            EntityMgr.Update();

            Ticks++;
        }

        public static void Draw()
        {
            DrawTiles();
            EntityMgr.Draw();
        }
        public static void DrawTiles()
        {
            for(int x=0; x<Core.TW; x++)
            {
                for(int y=0; y< Core.TH; y++)
                {
                    DrawTile(x, y);
                }
            }
        }
        private static void DrawTile(int x, int y)
        {
            Brush b;
            switch(Tiles[x, y])
            {
                default: b = Brushes.Red; break;
                case TileTypes.Grass:  b = new SolidBrush(Color.FromArgb(PalConfig.TileGrassColor));  break;
                case TileTypes.Stone:  b = new SolidBrush(Color.FromArgb(PalConfig.TileStoneColor));  break;
                case TileTypes.Sand:  b = new SolidBrush(Color.FromArgb(PalConfig.TileSandColor));  break;
            }
            Core.g.FillRectangle(b, x * Core.TSZ, y * Core.TSZ, Core.TSZ, Core.TSZ);
        }

        private static void Gen()
        {
            GenResources();
        }
        private static void GenResources()
        {
            if(ResourceCount < WorldConfig.MaxResourceCount)
            {
                if (TimerResource >= WorldConfig.MaxTimerResource)
                {
                    TimerResource = 0;
                    AddRandomResource();
                }
                else TimerResource++;
            }
        }
        private static void AddRandomResource()
        {
            int x, y, t = 0;
            do
            {
                x = Core.RND.Next(Core.TW);
                y = Core.RND.Next(Core.TH);
                t++;
            }
            while (EntityMgr.GetEntitiesAt(x, y).Where(e => e is Resource).Count() > 0 || t > 100);

            if (t > 100) return; // timeout

            ResourceFactory.Create(x, y);
        }
    }
}
