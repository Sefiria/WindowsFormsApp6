using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24.Events
{
    internal class Shovel : Event
    {
        internal Shovel(int x, int y, int z) : base(Core.NamedTextures[NamedObjects.Shovel], true, x, y, z){}
        internal Shovel(float x, float y, float z) : base(Core.NamedTextures[NamedObjects.Shovel], true, x, y, z){}

        internal override void SecondaryAction()
        {
            PredefinedAction_TakeDrop(this);
        }
        internal override void PrimaryAction()
        {
            if (Core.CurrentMainScene.MainCharacter.HandObject != Guid)
                return;
            int x = (int)((float)Data["X"]) / Core.TileSize;
            int y = (int)((float)Data["Y"]) / Core.TileSize;
            var tiledata = Map.Current.Tiles.PointedData(x, y);
            var index = tiledata.tile?.TilesetIndex;
            var layer = tiledata.layer;
            if ((index == null || index == 0) && Map.Current.Events.FirstOrDefault(ev => ev.TileX != x && ev.TileY != y) != null)
                Map.Current.Tiles.SetTilesetIndex(layer+1, x, y, 1, new Tile(1, layer + 1, x, y));
        }
    }
}
