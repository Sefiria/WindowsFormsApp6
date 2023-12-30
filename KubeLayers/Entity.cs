using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace KubeLayers
{
    public class Entity : IDisposable
    {
        GCore core => GCore.Instance;
        public static int tsz => Core.TSZ;

        public bool Exists = true;
        public vec TileLocation;
        public bool IsBlockable = true;     // enable collisions
        public bool IsDroppable = false;    // enable placing it on a tile
        public bool IsLootable = false;     // enable taking it into inventory
        public Bitmap Image;

        public Entity()
        {
        }
        public virtual void Update()
        {
        }
        public virtual void Draw()
        {
            core.g.DrawImage(Image, core.CenterPoint.PlusF(TileLocation * tsz).MinusF(core.Cam.Position));
        }

        public virtual void Dispose()// for custom bitmaps
        {
        }
    }
}
