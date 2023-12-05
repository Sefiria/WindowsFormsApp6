using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Tooling;

namespace KubeLayers
{
    public class Tile
    {
        GCore core => GCore.Instance;
        public static int tsz => Core.TSZ;

        public byte Index;
        public vec TilePos;
        public Entity Entity = null;

        protected Bitmap Image;

        public Tile(byte Index, vec v)
        {
            this.Index = Index;
            TilePos = v;
            Image = new Bitmap(tsz, tsz);
        }

        public void Update()
        {
            Entity?.Update();
            if (!Entity?.Exists ?? false)
            {
                Entity.Dispose();
                Entity = null;
            }
        }

        public void Draw()
        {
            core.g.DrawImage(Image, core.CenterPoint.PlusF(TilePos * tsz).Minus(core.Cam.Position));
            Entity?.Draw();
        }

        public static Tile Random(vec v)
        {
            switch(Common.Rnd.Next(3))
            {
                default: return null;
                case 0: return new tiles.TileGrass(v);
                case 1: return new tiles.TileDirt(v);
                case 2: return new tiles.TileGravel(v);
            }
        }
    }
}
